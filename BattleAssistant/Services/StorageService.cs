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
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Markup;
using BattleAssistant.Common;
using BattleAssistant.Helpers;
using BattleAssistant.Interfaces;
using BattleAssistant.Models;
using Newtonsoft.Json;
using Serilog;
using Windows.Storage;

namespace BattleAssistant.Services
{
    /// <summary>
    /// Storage helper methods
    /// </summary>
    public class StorageService : IStorageService
    {
        private readonly ILogger Logger;

        private static readonly StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public ObservableCollection<BattleModel> Battles { get; private set; }
        public ObservableCollection<GameModel> Games { get; private set; }
        public ObservableCollection<OpponentModel> Opponents { get; private set; }

        public StorageService(ILogger logger)
        {
            Logger = logger;

            Games = Load<GameModel>(SaveFiles.Games).Result;
            Opponents = Load<OpponentModel>(SaveFiles.Opponents).Result;
            Battles = Load<BattleModel>(SaveFiles.Battles).Result;
            Logger.Information("Loaded save files");
        }

        public async Task UpdateBattleFile()
        {
            await Save(Battles, SaveFiles.Battles);
        }

        public async Task UpdateGameFile()
        {
            await Save(Games, SaveFiles.Games);
        }

        public async Task UpdateOpponentFile()
        {
            await Save(Opponents, SaveFiles.Opponents);
        }

        /// <summary>
        /// Saves the models to their save file
        /// </summary>
        /// <typeparam name="T">The type of model to save</typeparam>
        /// <param name="models">The list of models</param>
        /// <param name="fileName">The save file, file name</param>
        /// <returns></returns>
        private static async Task Save<T>(ObservableCollection<T> models, string fileName)
        {
            while (FileHelper.FileIsLocked(new FileInfo(fileName)))
            {
                var file = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(models, Formatting.Indented));
            }
        }

        /// <summary>
        /// Loads the models from the save file
        /// </summary>
        /// <typeparam name="T">The type of model thats being loaded</typeparam>
        /// <param name="fileName">The file name the data is being loaded from</param>
        /// <returns>A collection of the models loaded</returns>
        private async Task<ObservableCollection<T>> Load<T>(string fileName)
        {
            try
            {
                var file = await localFolder.GetFileAsync(fileName);
                var text = await FileIO.ReadTextAsync(file);
                var models = JsonConvert.DeserializeObject<ObservableCollection<T>>(text);
                if (models != null)
                {
                    return models;
                }
                return new ObservableCollection<T>();
            }
            catch (FileNotFoundException ex)
            {
                //With no file found, assume it doesn't exist and move on
                Logger.Warning(ex, "Save file not found, creating empty list");
                return new ObservableCollection<T>();
            }
            catch (Exception ex)
            {
                if (ex.Source != null)
                {
                    Logger.Error("IOException source: {0}", ex.Source);
                }
                return new ObservableCollection<T>();
            }
        }
    }
}
