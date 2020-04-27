namespace AntiPlagiatus.Providers
{
    using System;
    using System.Linq;
    using System.Threading;
    using Windows.Networking.Connectivity;

    public class InternetConnectionChangedEventArgs : EventArgs
    {
        public bool IsConnected { get; set; }
    }

    public static class InternetConnection
    {
        private static readonly int[] availableIanaNetworkInterfaces =
        {
            6, //ethernet
            71, //wifi
            216, //GTP (GPRS Tunneling Protocol)
            243, //3g
            244 //3GPP2 WWA
        };

        private static bool? isConnected;
        static InternetConnection()
        {
            NetworkInformation.NetworkStatusChanged += OnNetworkInformationNetworkStatusChanged;
        }
        public static bool IsConnected
        {
            get
            {
                if (!isConnected.HasValue)
                {
                    isConnected = CheckInternetConnection();
                }

                return isConnected.Value;
            }
        }

        public static event EventHandler<InternetConnectionChangedEventArgs> InternetConnectionChanged;
        private static void OnNetworkInformationNetworkStatusChanged(object sender)
        {
            var arg = new InternetConnectionChangedEventArgs
            {
                IsConnected = CheckInternetConnection()
            };

            isConnected = arg.IsConnected;

            var handler = Volatile.Read(ref InternetConnectionChanged);

            handler?.Invoke(null, arg);
        }
        private static bool CheckInternetConnection()
        {
            var result = false;

            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();

            if (connectionProfile != null)
            {
                var networkConnectivityLevel = connectionProfile.GetNetworkConnectivityLevel();
                var isIanaAvailable = availableIanaNetworkInterfaces.Any(a => a == connectionProfile.NetworkAdapter.IanaInterfaceType);

                if (connectionProfile.NetworkAdapter.InboundMaxBitsPerSecond > 0
                    && isIanaAvailable
                    && networkConnectivityLevel == NetworkConnectivityLevel.InternetAccess)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
