using Battle_Assistant.Common;
using Battle_Assistant.Models;
using Newtonsoft.Json;
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

        public static async Task SaveAllAsync()
        {
            await SaveModels(App.Games, SaveFiles.GAMES);
            await SaveModels(App.Opponents, SaveFiles.OPPONENTS);
            await SaveModels(App.Battles, SaveFiles.BATTLES);
        }

        public static async Task LoadAllAsync()
        {
            App.Games = await LoadModels<GameModel>(SaveFiles.GAMES);
            App.Opponents = await LoadModels<OpponentModel>(SaveFiles.OPPONENTS);
            App.Battles = await LoadModels<BattleModel>(SaveFiles.BATTLES);
        }

        public static async Task SaveModels<T>(ObservableCollection<T> models, string fileName)
        {
            StorageFile file = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(models, Formatting.Indented));
        }

        public static async void UpdateBattleFile()
        {
            await SaveModels(App.Battles, SaveFiles.BATTLES);
        }

        public static async void UpdateGameFile()
        {
            await SaveModels(App.Games, SaveFiles.GAMES);
        }

        public static async void UpdateOpponentFile()
        {
            await SaveModels(App.Opponents, SaveFiles.OPPONENTS);
        }
        //C:\Users\Admin\AppData\Local\Packages\c8e8831d-4222-4ea8-ae83-43ec81e66df7_5gyrq6psz227t\LocalState
        public static async Task<ObservableCollection<T>> LoadModels<T>(string fileName)
        {
            try
            {
                StorageFile file = await localFolder.GetFileAsync(fileName);
                string text = await FileIO.ReadTextAsync(file);
                return JsonConvert.DeserializeObject<ObservableCollection<T>>(text);
            }
            catch (FileNotFoundException)
            {
                //With no file found, assume it doesn't exist and move on
                Debug.WriteLine("File not found");
                return new ObservableCollection<T>();
            }
            catch (Exception e)
            {
                if (e.Source != null)
                {
                    Debug.WriteLine("IOException source: {0}", e.Source);
                }
                return new ObservableCollection<T>();
            }
        }
    }
}
