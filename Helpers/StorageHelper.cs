using Battle_Assistant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace Battle_Assistant.Helpers
{
    public static class StorageHelper
    {
        static readonly StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        static readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public static async Task SaveModelsToJSONAsync<T>(ObservableCollection<T> models, string fileName)
        {
            StorageFile file = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            Stream fileStream = await file.OpenStreamForReadAsync();
            await JsonSerializer.SerializeAsync(fileStream, models);
            await fileStream.DisposeAsync();
        }

        //C:\Users\Admin\AppData\Local\Packages\c8e8831d-4222-4ea8-ae83-43ec81e66df7_5gyrq6psz227t\LocalState
        public static async Task<ObservableCollection<T>> LoadModelsFromJSON<T>(string fileName)
        {
            ObservableCollection<T> models = new ObservableCollection<T>();
            try
            {
                StorageFile file = await localFolder.GetFileAsync(fileName);
                Stream fileStream = await file.OpenStreamForReadAsync();
                models = await JsonSerializer.DeserializeAsync<ObservableCollection<T>>(fileStream);        
            }
            catch (FileNotFoundException)
            {
                //With no file found, assume it doesn't exist and move on
            }
            catch (IOException e)
            {
                if (e.Source != null)
                {
                    Debug.WriteLine("IOException source: {0}", e.Source);
                }
            }
            return models;
        }
    }
}
