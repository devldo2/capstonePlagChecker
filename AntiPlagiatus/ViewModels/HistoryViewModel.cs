using AntiPlagiatus.Helpers;
using AntiPlagiatus.Helpers.Dialogs;
using AntiPlagiatus.Models;
using AntiPlagiatus.Models.Inferfaces;
using AntiPlagiatus.Models.UI;
using AntiPlagiatus.Providers;
using AntiPlagiatus.Views.Resources;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AntiPlagiatus.ViewModels
{
    class HistoryViewModel : BaseViewModel
    {
        private readonly ISectionManager sectionManager;
        private readonly IHistoryProvider historyProvider;
        private readonly CurrentCheckProvider currentCheckProvider;
        public HistoryViewModel(ISectionManager manager, IHistoryProvider hiProvider, CurrentCheckProvider currCheckProvider)
        {
            this.sectionManager = manager;
            this.historyProvider = hiProvider;
            this.currentCheckProvider = currCheckProvider;

            this.ClearCommand = new Command(ClearCommand_Execute);
            this.OpenReportPreviewCommand = new Command<HistoryItem>(OpenReportPreviewCommand_Execute);
            this.DeleteItemCommand = new Command<HistoryItem>(DeleteItemCommand_Execute);
            this.GoToCheckViewCommand = new Command(GoToCheckViewCommand_Execute);
            this.GoToAccountViewCommand = new Command(GoToAccountViewCommand_Execute);
            var task = Refresh();
        }
        protected override string ViewTemplateKey => "ViewTemplate";
        protected override string ViewKey => "History";
        public bool IsLogged => true;//AccountProvider.Instance.IsLogged;
        public ObservableCollection<HistoryItem> Operations => historyProvider.GetHistory();
        public Command GoToAccountViewCommand { get; private set; }
        public Command GoToCheckViewCommand { get; private set; }
        public Command ClearCommand { get; private set; }
        public Command<HistoryItem> DeleteItemCommand { get; private set; }
        public Command<HistoryItem> OpenReportPreviewCommand { get; private set; }
        public override void Dispose()
        {
            base.Dispose();
        }
        protected async override Task<Exception> OnRefresh(object parameter = null)
        {
            Exception exception = null;
            try
            {
                this.ViewTemplate = ResourceHelper.GetViewModelTemplate(this.ViewKey, ViewTemplateKey);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            return exception;
        }

        #region CommandHandlers
        private async void ClearCommand_Execute()
        {
            await historyProvider.Clear();
        }
        private async void OpenReportPreviewCommand_Execute(HistoryItem report)
        {
            var previewDialog = new ReportPreviewDialog();
            var result = await previewDialog.ShowAsync(report);
            if (result == OperationPreviewDialogResult.CheckAgain)
            {
                if (!currentCheckProvider.IsProcessing)
                {
                    currentCheckProvider.ReuseHistoryItem(report);
                    sectionManager?.GoTo(SectionType.Check);
                }
            }
        }
        private async void DeleteItemCommand_Execute(HistoryItem item)
        {
            try
            {
                await historyProvider.RemoveItem(item);

            }
            catch (Exception) { }
        }
        private void GoToCheckViewCommand_Execute()
        {
            sectionManager.GoTo(SectionType.Check);
        }
        private void GoToAccountViewCommand_Execute()
        {
            sectionManager.GoTo(SectionType.Account);
        }
        #endregion
    }
}
