using AntiPlagiatus.Helpers.Extensions;
using AntiPlagiatus.Models;
using AntiPlagiatus.Models.UI;
using AntiPlagiatus.Providers;
using AntiPlagiatus.Views.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AntiPlagiatus.Helpers.Dialogs
{
    public class AddFiltersDialog : Control
    {
        private const string ParentRootKey = "ParentRoot";
        private const string ContentRootKey = "ContentRoot";

        private const string PopupStatesGroupName = "PopupStates";
        private const string OpenPopupStateName = "OpenPopupState";
        private const string ClosedPopupStateName = "ClosedPopupState";

        private Grid parentRoot;
        private Grid contentRoot;
        private Panel temporaryParentPanel;

        public ObservableCollection<IgnoreRule> FiltersList { get; set; } = new ObservableCollection<IgnoreRule>();

        public string VelidationMessage
        {
            get { return (string)GetValue(VelidationMessageProperty); }
            set { SetValue(VelidationMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VelidationMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VelidationMessageProperty =
            DependencyProperty.Register("VelidationMessage", typeof(string), typeof(AddFiltersDialog), new PropertyMetadata(string.Empty));

        public bool IsFiltersAdded
        {
            get { return (bool)GetValue(IsFiltersEmptyProperty); }
            set { SetValue(IsFiltersEmptyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VelidationMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFiltersEmptyProperty =
            DependencyProperty.Register("IsFiltersEmpty", typeof(string), typeof(AddFiltersDialog), new PropertyMetadata(false));

        public string FilterToAdd
        {
            get { return (string)GetValue(FilterToAddProperty); }
            set { SetValue(FilterToAddProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VelidationMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterToAddProperty =
            DependencyProperty.Register("FilterToAdd", typeof(string), typeof(AddFiltersDialog), new PropertyMetadata(string.Empty));

        private TaskCompletionSource<bool> resultSource;
        public bool Shown { get; private set; }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.parentRoot = (Grid)this.GetTemplateChild(ParentRootKey);
            this.contentRoot = (Grid)this.GetTemplateChild(ContentRootKey);
        }

        public AddFiltersDialog(string themeName)
        {
            DefaultStyleKey = typeof(AddFiltersDialog);
            //ResourceDictionary resourceDict = ResourceHelper.GetMainTheme(themeName);
           // this.Resources.MergedDictionaries.Add(resourceDict);
            this.Visibility = Visibility.Collapsed;

            this.AddFilterCommand = new Command(AddFilterCommand_Executed);
            this.DeleteFilterCommand = new Command<IgnoreRule>(DeleteFilterCommand_Executed);
            this.CloseCommand = new Command(CloseCommand_Execute);
        }

        public async Task<List<IgnoreRule>> ShowAsync(List<IgnoreRule> rules = null)
        {
            if (this.Shown)
            {
                throw new InvalidOperationException("The dialog is already shown.");
            }
            if (rules?.Count > 0)
            {
                this.FiltersList = new ObservableCollection<IgnoreRule>(rules);
                this.IsFiltersAdded = this.FiltersList?.Count > 0;
            }

            this.Visibility = Visibility.Visible;
            this.Shown = true;
            //this.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            resultSource = new TaskCompletionSource<bool>();
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

            this.ResizeLayoutRoot();
            await this.GoToVisualStateAsync(this.parentRoot, PopupStatesGroupName, OpenPopupStateName);
            await this.resultSource.Task;

#pragma warning disable 4014
            this.CloseAsync();
#pragma warning restore 4014
            return this.FiltersList.ToList();
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

        private void AddFilterCommand_Executed()
        {
            if (!string.IsNullOrEmpty(this.FilterToAdd))
            {
                var validRule = this.GetValidRule(this.FilterToAdd);
                if (validRule != null)
                {
                    var existContent = this.FiltersList.FirstOrDefault(item => item.Url == validRule.Url);
                    if (existContent != null)
                        this.FiltersList.Remove(existContent);
                    this.FiltersList.Add(validRule);
                    this.FilterToAdd = string.Empty;
                    this.IsFiltersAdded = this.FiltersList?.Count > 0;
                    this.VelidationMessage = string.Empty;
                }
                else
                    //invalid url
                    this.VelidationMessage = "Invalid url";
            }
            else
            {
                this.VelidationMessage = "Please provide a URL";
            }
        }

        private void DeleteFilterCommand_Executed(IgnoreRule rule)
        {
            this.FiltersList.Remove(rule);
            this.IsFiltersAdded = this.FiltersList?.Count > 0;
        }

        private async void CloseCommand_Execute()
        {
            this.VelidationMessage = string.Empty;
            this.FilterToAdd = string.Empty;
            await this.CloseAsync();
            this.resultSource?.TrySetResult(false);
        }

        public Command AddFilterCommand { get; private set; }
        public Command<IgnoreRule> DeleteFilterCommand { get; private set; }
        public Command CloseCommand { get; private set; }

        private IgnoreRule GetValidRule(string input)
        {
            IgnoreRule newRule = null;

            Uri uriResult;
            if (Uri.TryCreate(input, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                newRule = new IgnoreRule() { Type = IgnoreType.URL, Url = uriResult.AbsoluteUri };
                if (uriResult.Segments.Count() <= 1)
                {
                    newRule.Type = IgnoreType.Domain;
                    newRule.Url = uriResult.Host;
                }
            }

            return newRule;
        }
    }
}
