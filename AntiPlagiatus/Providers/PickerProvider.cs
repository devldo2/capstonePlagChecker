using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace AntiPlagiatus.Providers
{
    public static class PickerProvider
    {
        private static bool isPickerOpened;

        public static event EventHandler PickerOpened;
        private static void RaisePickerOpened()
        {
            var pickerOpened = Volatile.Read(ref PickerOpened);
            pickerOpened?.Invoke(null, null);
        }
        public static async Task<StorageFile> PickSingleFileAsync(List<string> fileTypes, PickerLocationId startLocation= PickerLocationId.Desktop, PickerViewMode viewMode = PickerViewMode.Thumbnail)
        {
            StorageFile pickedFile = null;

            if (!isPickerOpened)
            {
                isPickerOpened = true;

                var filePicker = new FileOpenPicker();

                filePicker.SuggestedStartLocation = startLocation;
                filePicker.ViewMode = viewMode;

                foreach (var format in fileTypes)
                {
                    filePicker.FileTypeFilter.Add(format);
                }

                try
                {
                    pickedFile = await filePicker.PickSingleFileAsync();
                }
                catch (UnauthorizedAccessException) { }
                finally
                {
                    RaisePickerOpened();
                }

                isPickerOpened = false;
            }
            return pickedFile;
        }
        public static async Task<StorageFolder> PickSingleFolderAsync(List<string> fileTypes, PickerLocationId startLocation = PickerLocationId.Desktop, PickerViewMode viewMode = PickerViewMode.Thumbnail)
        {
            StorageFolder pickedFolder = null;

            if (!isPickerOpened)
            {
                isPickerOpened = true;

                FolderPicker folderPicker = new FolderPicker();
                folderPicker.SuggestedStartLocation = startLocation;
                folderPicker.ViewMode = viewMode;

                foreach (var format in fileTypes)
                {
                    folderPicker.FileTypeFilter.Add(format);
                }

                try
                {
                    pickedFolder = await folderPicker.PickSingleFolderAsync();
                }
                catch (UnauthorizedAccessException) { }
                finally
                {
                    RaisePickerOpened();
                }

                isPickerOpened = false;
            }
            return pickedFolder;
        }
        public static async Task<StorageFile> PickSaveFileAsync(StorageFile suggestedSaveFile, string fileTypesKey, List<string> fileTypes)
        {
            StorageFile savedFile = null;

            if (!isPickerOpened)
            {
                isPickerOpened = true;

                var savePicker = new FileSavePicker();

                savePicker.FileTypeChoices.Add(fileTypesKey, fileTypes);
                savePicker.SuggestedSaveFile = suggestedSaveFile;

                try
                {
                    savedFile = await savePicker.PickSaveFileAsync();
                }
                catch (UnauthorizedAccessException) { }
                finally
                {
                    RaisePickerOpened();
                }

                isPickerOpened = false;
            }
            return savedFile;
        }
        public static async Task<StorageFile> PickSaveFileAsync(string suggestedFileName, string fileTypesKey, List<string> fileTypes, PickerLocationId startLocation = PickerLocationId.Desktop)
        {
            StorageFile savedFile = null;

            if (!isPickerOpened)
            {
                isPickerOpened = true;

                var savePicker = new FileSavePicker();

                savePicker.SuggestedStartLocation = startLocation;
                savePicker.FileTypeChoices.Add(fileTypesKey, fileTypes);
                savePicker.SuggestedFileName = suggestedFileName;
                try
                {
                    savedFile = await savePicker.PickSaveFileAsync();
                }
                catch (UnauthorizedAccessException) { }
                finally
                {
                    RaisePickerOpened();
                }

                isPickerOpened = false;
            }
            return savedFile;
        }        
    }
}
