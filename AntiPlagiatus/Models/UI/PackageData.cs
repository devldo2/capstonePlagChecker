using System.Text;

namespace AntiPlagiatus.Models.UI
{
    public class PackageData
    {
        public string Version { get; set; }

        public string Publisher { get; set; }

        public string Architecture { get; set; }

        public string Culture { get; set; }

        public string Region { get; set; }

        public string Device { get; set; }

        public string Model { get; set; }

        public string WindowsVersion { get; set; }

        public string AppName { get; set; }

        public string UserAgent { get; set; }

        public int RatingValue { get; set; }

        public string UserEmail { get; set; }

        public string FeedbackMessage { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (!string.IsNullOrEmpty(this.UserEmail))
            {
                builder.AppendLine(string.Format("From: {0}", this.UserEmail));
            }

            if (!string.IsNullOrEmpty(this.FeedbackMessage))
            {
                builder.AppendLine(string.Format("Message: {0}", this.FeedbackMessage));
            }

            if (RatingValue != 0)
            {
                builder.AppendLine(string.Format("Rating: {0}", this.RatingValue));
            }

            builder.AppendLine("\n");

            builder.AppendLine(string.Format("AppName: {0}", this.AppName));
            builder.AppendLine(string.Format("Architecture: {0}", this.Architecture));
            builder.AppendLine(string.Format("Culture: {0}", this.Culture));
            builder.AppendLine(string.Format("Device: {0}", this.Device));
            builder.AppendLine(string.Format("Model: {0}", this.Model));
            builder.AppendLine(string.Format("Publisher: {0}", this.Publisher));
            builder.AppendLine(string.Format("Region: {0}", this.Region));
            builder.AppendLine(string.Format("Version: {0}", this.Version));
            builder.AppendLine(string.Format("WindowsVersion: {0}", this.WindowsVersion));
            builder.AppendLine(string.Format("SystemUserAgent: {0}", this.UserAgent));

            return builder.ToString();
        }
    }
}
