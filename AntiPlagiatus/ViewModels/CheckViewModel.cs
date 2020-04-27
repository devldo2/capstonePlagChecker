using AntiPlagiatus.Helpers;
using AntiPlagiatus.Helpers.Dialogs;
using AntiPlagiatus.Models;
using AntiPlagiatus.Models.Inferfaces;
using AntiPlagiatus.Models.UI;
using AntiPlagiatus.Providers;
using AntiPlagiatus.Views.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AntiPlagiatus.ViewModels
{
    class CheckViewModel : BaseViewModel
    {
        private readonly ISectionManager sectionManager;
        private readonly IHistoryProvider historyProvider;
        private readonly CurrentCheckProvider currentCheckProvider;
        private bool isCompleted = false;
        private bool isCheckedTextModified;
        private LayerByDomain selectedDomainLayer;
        public CheckViewModel(ISectionManager manager,IHistoryProvider hiProvider, CurrentCheckProvider currCheckProvider)
        {
            this.sectionManager = manager;
            this.historyProvider = hiProvider;
            this.currentCheckProvider = currCheckProvider;

            this.OpenFileCommand = new Command(OpenFileCommand_Execute);
            this.ShowAddFiltersDialogCommand = new Command(ShowAddFiltersDialogCommand_Execute);
            this.SaveFileCommand = new Command(SaveFileCommand_Execute);
            this.CheckCommand = new Command(CheckCommand_Execute);
            this.SelectDomainCommand = new Command<Domain>(SelectDomainCommand_Execute);
            this.SelectLayerCommand = new Command<LayerByDomain>(SelectLayerCommand_Execute);
            currentCheckProvider.ReportStatusChanged += Instance_ReportStatusChanged;
            var task = Refresh();
        }
        protected override string ViewTemplateKey => "ViewTemplate";
        protected override string ViewKey => "Check";
        public Status ReportStatus => currentCheckProvider.ReportStatus;
        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                if (this.isCompleted != value)
                {
                    this.isCompleted = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public ObservableCollection<Domain> Domains { get; private set; } = new ObservableCollection<Domain>();
        public int CharactersNumber { get; private set; }
        public int WordCount { get; private set; }
        public int UniquenessByPhrase { get; private set; }
        public int UniquenessByWord { get; private set; }
        public string Text
        {
            get => currentCheckProvider.Content;
            set
            {
                currentCheckProvider.Content = value;
                if (!isCheckedTextModified)
                {
                    isCheckedTextModified = true;
                    Task task = OnRefresh();
                }
            }
        }
        public List<IgnoreRule> IgnoreRules
        {
            get => currentCheckProvider.IgnoreRules;
            set { currentCheckProvider.IgnoreRules = value; }
        }
        public bool IsProcessing => currentCheckProvider.IsProcessing;
        public LayerByDomain SelectedDomainLayer
        {
            get => selectedDomainLayer;
            set
            {
                if (this.selectedDomainLayer != value)
                {
                    if (this.selectedDomainLayer != null)
                        this.selectedDomainLayer.IsSelected = false;
                    this.selectedDomainLayer = value;
                    if (this.selectedDomainLayer != null)
                        this.selectedDomainLayer.IsSelected = true;
                    this.RaisePropertyChanged();
                }
            }
        }
        public Command OpenFileCommand { get; set; }
        public Command SaveFileCommand { get; set; }
        public Command CheckCommand { get; set; }
        public Command ShowAddFiltersDialogCommand { get; set; }
        public Command<Domain> SelectDomainCommand { get; set; }
        public Command<LayerByDomain> SelectLayerCommand { get; set; }
        private void Instance_ReportStatusChanged(object sender, Status status)
        {
            this.RaisePropertyChanged(() => this.ReportStatus);
            this.RaisePropertyChanged(() => IsProcessing);
        }
        protected async override Task<Exception> OnRefresh(object parameter = null)
        {
            Exception exception = null;
            try
            {
                var lastReport = currentCheckProvider.GetLastReport ?? new ReportItem();
                if (this.Domains.Count > 0) this.Domains.Clear();
                if (lastReport.Domains.Count > 0)
                {
                    this.Domains = new ObservableCollection<Domain>(lastReport.Domains.ToList());
                    this.SelectedDomainLayer = this.Domains.First().Layers.First() ;
                }
                this.CharactersNumber = lastReport.CharactersNumber;
                this.WordCount = lastReport.WordCount;
                this.UniquenessByPhrase = 100 - lastReport.Equality;
                this.UniquenessByWord = 100 - lastReport.Rewrite;
                this.IsCompleted = this.Domains.Count > 0;

                this.RaisePropertyChanged(() => this.Domains);
                this.RaisePropertyChanged(() => this.CharactersNumber);
                this.RaisePropertyChanged(() => this.WordCount);
                this.RaisePropertyChanged(() => this.UniquenessByPhrase);
                this.RaisePropertyChanged(() => this.UniquenessByWord);
                this.RaisePropertyChanged(() => this.IsCompleted);
                this.RaisePropertyChanged(() => this.ReportStatus);
                this.RaisePropertyChanged(() => this.IsProcessing);

                this.ViewTemplate = ResourceHelper.GetViewModelTemplate(this.ViewKey, ViewTemplateKey);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            return exception;
        }

        #region CommandHandlers
        private async void OpenFileCommand_Execute()
        {
            var file = await PickerProvider.PickSingleFileAsync(new List<string>() { ".txt" });
            using (StreamReader streamReader = new StreamReader(await file.OpenStreamForReadAsync()))
            {
                this.Text = "";
                while (streamReader.Peek() > 0)
                {
                    this.Text += streamReader.ReadLine() + Environment.NewLine;
                }
            }
        }
        private void SaveFileCommand_Execute()
        {
        }
        private async void CheckCommand_Execute()
        {
            isCheckedTextModified = false;
            var existHistoryItem = this.historyProvider.GetItemByContentAndIgnores(this.Text, this.IgnoreRules);
            if (existHistoryItem == null)
            {
                var result = await currentCheckProvider.Check();
                if (result != null && !string.IsNullOrEmpty(result.result.error_msg))
                {
                    string error = result.result.error_msg;
                }
                else
                {
                    var report = await currentCheckProvider.ProcessReport();
                    if (report != null)
                    {
                        var history = new HistoryItem();
                        history.ReportId = Guid.NewGuid().ToString();
                        history.Content = new Content() { Text = this.Text };
                        history.Equality = report.Equality;
                        history.IgnoreRules = this.IgnoreRules.ToList();
                        history.Domains = report.Domains;
                        history.Status = report.Status;
                        history.ErrorMessage = report.ErrorMessage;
                        history.Rewrite = report.Rewrite;
                        history.CharactersNumber = report.CharactersNumber;
                        history.WordCount = report.WordCount;
                        history.Date = report.Date;
                        try
                        {
                            await historyProvider.AddItem(history);
                        }
                        catch (Exception) { }
                    }
                    else
                    {
                        // check was with an error, so there is no data for the last check
                    }
                    await OnRefresh();
                }
            }
            else
            {
                var existReport = new ReportItem();
                existReport.Status = existHistoryItem.Status;
                existReport.Date = existHistoryItem.Date;
                existReport.Equality = existHistoryItem.Equality;
                existReport.Rewrite = existHistoryItem.Rewrite;
                existReport.Domains.AddRange(existHistoryItem.Domains);
                existReport.CharactersNumber = existHistoryItem.CharactersNumber;
                existReport.ErrorMessage = existHistoryItem.ErrorMessage;
                existReport.WordCount = existHistoryItem.WordCount;
                this.currentCheckProvider.SetLastReport(existReport);
            }
        }
        private async void ShowAddFiltersDialogCommand_Execute()
        {
            var addFiltersDialog = new AddFiltersDialog(null);
            var updatedIgnoreRules = await addFiltersDialog.ShowAsync(this.IgnoreRules?.ToList());
            this.IgnoreRules.Clear();
            foreach (var rule in updatedIgnoreRules)
                this.IgnoreRules.Add(rule);

            this.RaisePropertyChanged(() => this.IgnoreRules);
        }
        private void SelectLayerCommand_Execute(LayerByDomain layer)
        {
            if (this.SelectedDomainLayer != layer)
                this.SelectedDomainLayer = layer;
            else this.SelectedDomainLayer = null;
        }
        private void SelectDomainCommand_Execute(Domain domain)
        {
            this.SelectedDomainLayer = domain.Layers.First();
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();
            if (this.SelectedDomainLayer != null)
            {
                this.SelectedDomainLayer.IsSelected = false;
                this.SelectedDomainLayer = null;
            }
            currentCheckProvider.ReportStatusChanged -= Instance_ReportStatusChanged;
        }
    }
}
