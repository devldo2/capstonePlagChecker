using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace AntiPlagiatus.Providers
{
    public static class SettingsProvider
    {
        public static bool IsExistsInAppData(string key) => ApplicationData.Current.LocalSettings.Values.ContainsKey(key);
        public static T ReadValueFromAppData<T>(string key)
        {
            var result = default(T);

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                var compositeValue = ApplicationData.Current.LocalSettings.Values[key] as ApplicationDataCompositeValue;
                var streamString = compositeValue != null ? (string)compositeValue[key] : string.Empty;
                result = DataContractSerializer.DeserializeObject<T>(streamString);
            }
            return result;
        }
        public static void WriteValueIntoAppData<T>(string key, T value)
        {
            using (var stream = new MemoryStream())
            {
                try
                {
                    var compositeValue = new ApplicationDataCompositeValue();
                    compositeValue[key] = DataContractSerializer.SerializeObject(value);
                    ApplicationData.Current.LocalSettings.Values[key] = compositeValue;
                }
                catch (Exception)
                {
                    // Data in file can be broken.
                }
            }
        }
        public async static Task<T> ReadValueFromFile<T>(string fileName)
        {
            var result = default(T);

            try
            {
                var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);

                if (file != null)
                {
                    var servicesJson = await FileIO.ReadTextAsync((StorageFile)file);
                    result = DataContractSerializer.DeserializeObject<T>(servicesJson);
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async static Task WriteValueToFile<T>(string fileName, T value)
        {
            try
            {
                var json = DataContractSerializer.SerializeObject(value);
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, json);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
