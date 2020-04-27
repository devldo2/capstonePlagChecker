using AntiPlagiatus.Models;
using AntiPlagiatus.Views.Resources;
using System;
using System.Threading.Tasks;

namespace AntiPlagiatus.ViewModels
{
    public class ErrorViewModel : BaseViewModel, IVisible
    {
        private bool isVisible;

        protected override string ViewTemplateKey => "ViewTemplate";
        protected override string ViewKey => "Error";

        public ErrorViewModel()
        {
            var task = Refresh();
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

        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (value != this.isVisible)
                {
                    this.isVisible = value;
                    this.RaisePropertyChanged();
                }
            }
        }
    }
}
