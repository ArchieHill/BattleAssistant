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
using BattleAssistant.Common;
using BattleAssistant.Models;
using Newtonsoft.Json;
using Serilog;
using Windows.Storage;

namespace BattleAssistant.Helpers
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
            await SaveModels(App.Games, SaveFiles.Games);
            await SaveModels(App.Opponents, SaveFiles.Opponents);
            await SaveModels(App.Battles, SaveFiles.Battles);
        }

        /// <summary>
        /// Loads all save files
        /// </summary>
        /// <returns></returns>
        public static async Task LoadAllAsync()
        {
            App.Games = await LoadModels<GameModel>(SaveFiles.Games);
            App.Opponents = await LoadModels<OpponentModel>(SaveFiles.Opponents);
            App.Battles = await LoadModels<BattleModel>(SaveFiles.Battles);
            Log.Information("Loaded all save data");
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
            await SaveModels(App.Battles, SaveFiles.Battles);
        }

        /// <summary>
        /// Saves the game models to the save file
        /// </summary>
        public static async void UpdateGameFile()
        {
            await SaveModels(App.Games, SaveFiles.Games);
        }

        /// <summary>
        /// Saves the opponent models to the save file
        /// </summary>
        public static async void UpdateOpponentFile()
        {
            await SaveModels(App.Opponents, SaveFiles.Opponents);
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
            catch (FileNotFoundException ex)
            {
                //With no file found, assume it doesn't exist and move on
                Log.Warning(ex, "Save file not found, creating empty list");
                return new ObservableCollection<T>();
            }
            catch (Exception ex)
            {
                if (ex.Source != null)
                {
                    Log.Error("IOException source: {0}", ex.Source);
                }
                return new ObservableCollection<T>();
            }
        }
    }
}
