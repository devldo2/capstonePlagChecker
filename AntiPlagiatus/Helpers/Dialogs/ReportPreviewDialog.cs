using AntiPlagiatus.Helpers.Extensions;
using AntiPlagiatus.Models;
using AntiPlagiatus.Models.UI;
using AntiPlagiatus.Providers;
using AntiPlagiatus.Views.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AntiPlagiatus.Helpers.Dialogs
{
    public enum OperationPreviewDialogResult
    {
        None,
        CheckAgain
    }
    public class ReportPreviewDialog : Control
    {
        private const string ParentRootKey = "ParentRoot";
        private const string ContentRootKey = "ContentRoot";

        private const string PopupStatesGroupName = "PopupStates";
        private const string OpenPopupStateName = "OpenPopupState";
        private const string ClosedPopupStateName = "ClosedPopupState";

        private TaskCompletionSource<OperationPreviewDialogResult> resultSource;
        public bool Shown { get; private set; }

        private Grid parentRoot;
        private Grid contentRoot;
        private Panel temporaryParentPanel;

        public ReportPreviewDialog()
        {
            DefaultStyleKey = typeof(ReportPreviewDialog);
            
            //ResourceDictionary resourceDict = ResourceHelper.GetMainTheme(LocalAcountProvider.Instance.DefaultTheme);
            //this.Resources.MergedDictionaries.Add(resourceDict);

            Visibility = Visibility.Collapsed;

            this.CheckAgainCommand = new Command(CheckAgainCommand_Executed);
            this.CloseCommand = new Command(CloseCommand_Executed);
            this.SelectDomainCommand = new Command<Domain>(SelectDomainCommand_Execute);
            this.SelectLayerCommand = new Command<LayerByDomain>(SelectLayerCommand_Execute);
        }

        public Command CheckAgainCommand { get; private set; }
        public Command CloseCommand { get; private set; }
        public Command<Domain> SelectDomainCommand { get; set; }
        public Command<LayerByDomain> SelectLayerCommand { get; set; }

        public HistoryItem Report { get; private set; }
        public string UniquenessByPhrase { get; private set; }
        public string UniquenessByWord { get; private set; }

        public LayerByDomain SelectedDomainLayer
        {
            get { return (LayerByDomain)GetValue(SelectedDomainLayerProperty); }
            set { SetValue(SelectedDomainLayerProperty, value); }
        }

        public static readonly DependencyProperty SelectedDomainLayerProperty =
            DependencyProperty.Register("SelectedDomainLayer", typeof(LayerByDomain), typeof(ReportPreviewDialog), new PropertyMetadata(null, SelectedLayerChnaged));

        private static void SelectedLayerChnaged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = d as ReportPreviewDialog;
            if (dialog != null)
            {
                if (e.OldValue != null)
                    ((LayerByDomain)e.OldValue).IsSelected = false;
                if (e.NewValue != null)
                    ((LayerByDomain)e.NewValue).IsSelected = true;
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.parentRoot = (Grid)this.GetTemplateChild(ParentRootKey);
            this.contentRoot = (Grid)this.GetTemplateChild(ContentRootKey);
        }

        public async Task<OperationPreviewDialogResult> ShowAsync(HistoryItem report)
        {
            if (this.Shown)
            {
                throw new InvalidOperationException("The dialog is already shown.");
            }

            this.Report = report;

            this.UniquenessByPhrase = $"{100 - report.Equality}";
            this.UniquenessByWord = $"{100 - report.Rewrite}";
            this.Visibility = Visibility.Visible;
            this.Shown = true;
            resultSource = new TaskCompletionSource<OperationPreviewDialogResult>();
            this.temporaryParentPanel = Window.Current.Content.GetFirstDescendantOfType<Panel>();

            if (temporaryParentPanel != null)
            {
                temporaryParentPanel.Children.Add(this);
                temporaryParentPanel.SizeChanged += this.OnParentSizeChanged;
            }

            var inputPane = InputPane.GetForCurrentView();
            inputPane.Showing -= this.OnInputPaneShowing;
            inputPane.Showing += this.OnInputPaneShowing;
            inputPane.Hiding -= this.OnInputPaneHiding;
            inputPane.Hiding += this.OnInputPaneHiding;

            //this.dialogPopup.IsOpen = true;
            await this.WaitForLayoutUpdateAsync();
            if (this.Report != null && this.Report.Domains.Count > 0)
                this.SelectedDomainLayer = this.Report.Domains.First().Layers.First();

            this.ResizeLayoutRoot();
            await this.GoToVisualStateAsync(this.parentRoot, PopupStatesGroupName, OpenPopupStateName);
            var result = await this.resultSource.Task;
            if (this.SelectedDomainLayer != null)
            {
                this.SelectedDomainLayer.IsSelected = false;
                this.SelectedDomainLayer = null;
            }
#pragma warning disable 4014
            this.CloseAsync();
#pragma warning restore 4014

            return result;
        }

        private void OnInputPaneShowing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            parentRoot.VerticalAlignment = VerticalAlignment.Top;
        }

        private void OnInputPaneHiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            parentRoot.VerticalAlignment = VerticalAlignment.Center;
        }

        private void ResizeLayoutRoot()
        {
            FrameworkElement root = this.temporaryParentPanel as FrameworkElement;
            this.parentRoot.Width = root.ActualWidth;
            this.parentRoot.Height = root.ActualHeight;
        }

        private void OnParentSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            this.ResizeLayoutRoot();
        }

        private async Task CloseAsync()
        {
            if (!this.Shown)
            {
                throw new InvalidOperationException("The dialog isn't shown, so it can't be closed.");
            }

            await this.GoToVisualStateAsync(this.parentRoot, PopupStatesGroupName, ClosedPopupStateName);

            if (this.temporaryParentPanel != null)
            {
                this.temporaryParentPanel.Children.Remove(this);
                this.temporaryParentPanel.SizeChanged -= this.OnParentSizeChanged;
                this.temporaryParentPanel = null;
            }

            this.Visibility = Visibility.Collapsed;
            this.Shown = false;
        }
        private void CheckAgainCommand_Executed()
        {
            this.resultSource?.TrySetResult(OperationPreviewDialogResult.CheckAgain);
        }
        private void CloseCommand_Executed()
        {
            this.resultSource?.TrySetResult(OperationPreviewDialogResult.None);
        }
        private void SelectLayerCommand_Execute(LayerByDomain layer)
        {
            if (this.SelectedDomainLayer != layer)
                this.SelectedDomainLayer = layer;
        }

        private void SelectDomainCommand_Execute(Domain domain)
        {
            this.SelectedDomainLayer = domain.Layers.First();
        }
    }
}
