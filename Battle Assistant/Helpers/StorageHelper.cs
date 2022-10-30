// StorageHelper.cs
//
// Copyright (c) 2022 Archie Hill
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Battle_Assistant.Common;
using Battle_Assistant.Models;
using Newtonsoft.Json;
using Windows.Storage;

namespace Battle_Assistant.Helpers
{
    /// <summary>
    /// Storage helper methods
    /// </summary>
    public static class StorageHelper
    {
        static readonly StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        /// <summary>
        /// Updates all save files
        /// </summary>
        /// <returns></returns>
        public static async Task SaveAllAsync()
        {
            await SaveModels(App.Games, SaveFiles.GAMES);
            await SaveModels(App.Opponents, SaveFiles.OPPONENTS);
            await SaveModels(App.Battles, SaveFiles.BATTLES);
        }

        /// <summary>
        /// Loads all save files
        /// </summary>
        /// <returns></returns>
        public static async Task LoadAllAsync()
        {
            App.Games = await LoadModels<GameModel>(SaveFiles.GAMES);
            App.Opponents = await LoadModels<OpponentModel>(SaveFiles.OPPONENTS);
            App.Battles = await LoadModels<BattleModel>(SaveFiles.BATTLES);
        }

        /// <summary>
        /// Saves the models to their save file
        /// </summary>
        /// <typeparam name="T">The type of model to save</typeparam>
        /// <param name="models">The list of models</param>
        /// <param name="fileName">The save file, file name</param>
        /// <returns></returns>
        public static async Task SaveModels<T>(ObservableCollection<T> models, string fileName)
        {
            if (models != null)
            {
                StorageFile file = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(models, Formatting.Indented));
            }
        }

        /// <summary>
        /// Saves the battle models to the save file
        /// </summary>
        public static async void UpdateBattleFile()
        {
            await SaveModels(App.Battles, SaveFiles.BATTLES);
        }

        /// <summary>
        /// Saves the game models to the save file
        /// </summary>
        public static async void UpdateGameFile()
        {
            await SaveModels(App.Games, SaveFiles.GAMES);
        }

        /// <summary>
        /// Saves the opponent models to the save file
        /// </summary>
        public static async void UpdateOpponentFile()
        {
            await SaveModels(App.Opponents, SaveFiles.OPPONENTS);
        }

        /// <summary>
        /// Loads the models from the save file
        /// </summary>
        /// <typeparam name="T">The type of model thats being loaded</typeparam>
        /// <param name="fileName">The file name the data is being loaded from</param>
        /// <returns>A collection of the models loaded</returns>
        public static async Task<ObservableCollection<T>> LoadModels<T>(string fileName)
        {
            try
            {
                StorageFile file = await localFolder.GetFileAsync(fileName);
                string text = await FileIO.ReadTextAsync(file);
                ObservableCollection<T> models = JsonConvert.DeserializeObject<ObservableCollection<T>>(text);
                if (models != null)
                {
                    return models;
                }
                return new ObservableCollection<T>();
            }
            catch (FileNotFoundException e)
            {
                //With no file found, assume it doesn't exist and move on
                Debug.WriteLine("File not found: {0}", e.Message);
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
