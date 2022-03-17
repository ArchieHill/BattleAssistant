using Battle_Assistant.Helpers;
using Battle_Assistant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle_Assistant.ViewModels
{
    public class GamesPageViewModel
    {
        private static GamesPageViewModel instance = null;
        private static readonly object padlock = new object();

        private const string GAMES_FILE_NAME = "games.json";
        public static GamesPageViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GamesPageViewModel();
                    }
                }

                return instance;
            }
        }
        public ObservableCollection<GameModel> Games { get; set; }
        GamesPageViewModel()
        {
            Games = StorageHelper.LoadModelsFromJSON<GameModel>(GAMES_FILE_NAME).Result;
        }
    }
}
