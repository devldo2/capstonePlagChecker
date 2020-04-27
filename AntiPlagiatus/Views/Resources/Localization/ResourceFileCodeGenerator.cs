
namespace AntiPlagiatus.Views.Resources
{
	using System.Globalization;
    using AntiPlagiatus.Views.Resources.Localization.Core;
    using AntiPlagiatus.Models;
    using System;


	/// <summary>
    /// Provides access to resources from CommonResources.resw file.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Justification = "Yet it will be unstatic.")]
	public class CommonResources
	{
		/// <summary>
        /// Contains logic for accessing contsnt of resource file.
        /// </summary>
private static readonly ResourceProvider resourceProvider = new ResourceProvider("/CommonResources");		
		/// <summary>
        /// Overrides the current thread's CurrentUICulture property for all
        /// resource lookups using this strongly typed resource class.
        /// </summary>
        public static CultureInfo Culture
        {
            get
            {
                return resourceProvider.OverridedCulture;
            }
            set
            {
                resourceProvider.OverridedCulture = value;
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Account"
        /// </summary>
		public static string AccountLabel
        {
            get
            {
                return resourceProvider.GetString("AccountLabel");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Check Text"
        /// </summary>
		public static string CheckLabel
        {
            get
            {
                return resourceProvider.GetString("CheckLabel");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "History"
        /// </summary>
		public static string HistoryLabel
        {
            get
            {
                return resourceProvider.GetString("HistoryLabel");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Later"
        /// </summary>
		public static string Later
        {
            get
            {
                return resourceProvider.GetString("Later");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Login"
        /// </summary>
		public static string LoginAction
        {
            get
            {
                return resourceProvider.GetString("LoginAction");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Rate"
        /// </summary>
		public static string Rate
        {
            get
            {
                return resourceProvider.GetString("Rate");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Please, rate us. We are waiting for your review"
        /// </summary>
		public static string RateMessagePart1
        {
            get
            {
                return resourceProvider.GetString("RateMessagePart1");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Tell us what you think about our app"
        /// </summary>
		public static string RateMessagePart2
        {
            get
            {
                return resourceProvider.GetString("RateMessagePart2");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Register"
        /// </summary>
		public static string RegisterAction
        {
            get
            {
                return resourceProvider.GetString("RegisterAction");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Register an account"
        /// </summary>
		public static string RegisterOption
        {
            get
            {
                return resourceProvider.GetString("RegisterOption");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Report"
        /// </summary>
		public static string ReportLabel
        {
            get
            {
                return resourceProvider.GetString("ReportLabel");
            }
        }

		/// <summary>
        /// Gets a localized string similar to "Settings"
        /// </summary>
		public static string SettingsLabel
        {
            get
            {
                return resourceProvider.GetString("SettingsLabel");
            }
        }

	}


	public sealed class LocalizedStrings : Base
    {
		/// <summary>
        /// Initialize a new instance of <see cref="LocalizedStrings"/> class.
        /// </summary>
        public LocalizedStrings()
        {
            this.RefreshLanguageSettings();
        }

		public static event EventHandler LocalizedStringsRefreshedEvent;

		public void OnLocalizedStringsRefreshedEvent()
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            EventHandler handler = LocalizedStringsRefreshedEvent;

            // Event will be null if there are no subscribers 
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, new EventArgs());
            }
        }

		        /// <summary>
		/// Gets resources that are common across application.
		/// </summary>
		public CommonResources CommonResources { get; private set; }

	
		public void RefreshLanguageSettings()
        {
			            this.CommonResources = new CommonResources();
			this.RaisePropertyChanged("CommonResources");
		

			OnLocalizedStringsRefreshedEvent();
		}
	}
}
