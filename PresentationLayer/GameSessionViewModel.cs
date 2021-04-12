using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TBQuestGame.DataLayer;
using TBQuestGame.Models;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace TBQuestGame.PresentationLayer
{
    public class GameSessionViewModel : ObservableObject
    {
        #region FIELDS

        //private DateTime _gameStartTime;
        //private string _gameTimeDisplay;
        //private TimeSpan _gameTime;

        private Player _player;
        private List<string> _messages;
        private Map _map;
        private Location _currentLocation;
        private string _currentLocationInformation;

        

        private GameItem _currentGameItem;
        private Npc _currentNpc;

        private Random random = new Random();
        

        


        #endregion
        #region PROPERTIES
        
        public Map Map
        {
            get { return _map; }
            set
            {
                _map = value;
                OnPropertyChanged(nameof(Map));
            }
        }
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                _currentLocationInformation = _currentLocation.Description;
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(CurrentLocationInformation));
            }
        }
        public string CurrentLocationInformation
        {
            get { return _currentLocationInformation; }
            set 
            {
                _currentLocationInformation = value;
                OnPropertyChanged(nameof(CurrentLocationInformation));
            }
        }
        /// <summary>
        /// return the list of strings with new lines between each message
        /// </summary>
        public string MessageDisplay
        {
            get { return string.Join("\n\n", _messages); }
        }

        public Player Player
        {
            
            get { return _player; }
            set 
            { 
                _player = value;
                OnPropertyChanged(nameof(Player));            
            }
        }
        public GameItem CurrentGameItem
        {
            get { return _currentGameItem; }
            set 
            {
                
                _currentGameItem = value;
                OnPropertyChanged(nameof(CurrentGameItem));
                if (_currentGameItem != null && _currentGameItem is Weapon)
                {
                    _player.CurrentWeapon = _currentGameItem as Weapon;
                }
            }
        }

        public Npc CurrentNpc
        {
            get { return _currentNpc; }
            set
            {
                _currentNpc = value;
                OnPropertyChanged(nameof(CurrentNpc));
            }
        }
        //public string MissionTimeDisplay
        //{
        //    get { return _gameTimeDisplay; }
        //    set
        //    {
        //        _gameTimeDisplay = value;
        //        OnPropertyChanged(nameof(MissionTimeDisplay));
        //    }
        //}
        #endregion
        #region CONSTRUCTORS

        public GameSessionViewModel()
        {

        }

        public GameSessionViewModel(Player player, List<string> startUpMessage, Map map)
        {
            Player = GameData.PlayerData();
            Player.Name = player.Name;
            Player.Age = player.Age;
            _messages = startUpMessage;

            Map = map;
            Map.CurrentLocation = _map.MapLocations.FirstOrDefault(l => l.Id == _map.CurrentLocationId);

            //GameTimer();
        }

        #endregion
        #region METHODS
        /// <summary>
        /// Method to move player back one in map
        /// </summary>
        internal void MoveToPreviousIsle()
        {
            if (Map.CurrentLocationId > 0)
            {
                _map.CurrentLocationId -= 1;
                Map.CurrentLocation = _map.MapLocations.FirstOrDefault(l => l.Id == _map.CurrentLocationId);
                InitializeView();
                ModifyHealth();
                //OnPlayerMove();
            }
        }
        /// <summary>
        /// Method to move player to the next map location
        /// </summary>
        public void MoveToNextIsle()
        {
            //take the maps current location Id and compare it to the location array length
            if (Map.CurrentLocationId < _map.MapLocations.Length - 1)
            {
                _map.CurrentLocationId += 1;
                Map.CurrentLocation = _map.MapLocations.FirstOrDefault(l => l.Id == _map.CurrentLocationId);
                //OnPlayerMove();
                ModifyHealth();
            }
        }
        /// <summary>
        /// Method to modify health based on certain locations
        /// </summary>
        public void ModifyHealth()
        {
            if (Map.CurrentLocationId == 5)
            {
                Player.Health = Player.Health - 50;
                if (Player.Health <= 0)
                {
                    Player.Lives--;
                    Player.Health = 100;
                }
            }

        }
        private void InitializeView()
        {
            //_gameStartTime = DateTime.Now;
            _player.UpdateInventoryCategories();
        }
       

        /// <summary>
        /// Method to add to player inventory by taking item from current game map location
        /// </summary>
        public void AddItemToInventory()
        {
            //check to see if a game item is selected and is in the current location
            if (_currentGameItem != null && _map.CurrentLocation.GameItems.Contains(_currentGameItem))
            {
                // Cast selected game item
                GameItem selectedGameItem = _currentGameItem as GameItem;

                Map.CurrentLocation.RemoveGameItemFromLocation(selectedGameItem);
                _player.AddGameItemToInventory(selectedGameItem);
                _player.Currency = Player.Currency - selectedGameItem.Price;
            }
        }


        /// <summary>
        /// method to remove items from player inventory and place back into location
        /// </summary>
        public void RemoveItemFromInventory()
        {
            //check to see if a game item is selected and is in the current location
            //subtract from inventory and add to current location
            if (_currentGameItem != null)
            {
                //cast selected game item
                GameItem selectedGameItem = _currentGameItem as GameItem;

                Map.CurrentLocation.AddGameItemToLocation(selectedGameItem);
                _player.RemoveGameItemFromInventory(selectedGameItem);
                _player.Currency = Player.Currency + selectedGameItem.Price;
            }
        }


        /// <summary>
        /// A method for player death   ***USED IN LATER VERSION***
        /// </summary>
        /// <param name="message"></param>
        private void OnPlayerDies(string message)
        {
            string messageText = message + "\nWould you like to play again?";

            string titleText = "Death";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(messageText, titleText, button);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    ResetPlayer();
                    break;
                case MessageBoxResult.No:
                    QuitApplication();
                    break;
            }
        }
        /// <summary>
        /// Method to quit the game
        /// </summary>
        private void QuitApplication()
        {
            Environment.Exit(0);
        }
        /// <summary>
        /// Reset the game with the same player info
        /// </summary>
        private void ResetPlayer()
        {
            //update to reset game with same player info
            Environment.Exit(0);
        }
        /// <summary>
        /// Creates the speak to event in the view
        /// </summary>
        public void OnPlayerTalkTo()
        {
            if (CurrentNpc != null && CurrentNpc is ISpeak)
            {
                ISpeak speakingNpc = CurrentNpc as ISpeak;
                CurrentLocationInformation = speakingNpc.Speak();
            }
        }
        /// <summary>
        /// Method for player attack
        /// </summary>
        public void OnPlayerAttack()
        {
            _player.BattleMode = BattleModeEnum.ATTACK;
            Battle();
        }
        /// <summary>
        /// Method for player to defend
        /// </summary>
        public void OnPlayerDefend()
        {
            _player.BattleMode = BattleModeEnum.DEFEND;
            Battle();
        }
        /// <summary>
        /// Method for the player to retreat from battle
        /// </summary>
        public void OnPlayerRetreat()
        {
            _player.BattleMode = BattleModeEnum.RETREAT;
            Battle();

        }

        #endregion
        #region BATTLE METHODS

        /// <summary>
        /// Identify the outcome of the battle based on NPC
        /// </summary>
        private void Battle()
        {
            //
            // Check to see if the NPC is a townie and able to battle
            //
            if (_currentNpc is IBattle)
            {
                IBattle battleNpc = _currentNpc as IBattle;
                int playerHitPoints = 0;
                int battleNpcHitPoints = 0;
                string battleInformation = "";

                //
                // Show weapon and hit points
                //
                if (_player.CurrentWeapon != null)
                {
                    playerHitPoints = CalculatePlayerHitPoints();
                }
                else
                {
                    battleInformation = "Oh no! You do not have a weapon to defend yourself." + Environment.NewLine;
                }

                if (battleNpc.CurrentWeapon != null)
                {
                    battleNpcHitPoints = CalculateNpcHitPoints(battleNpc);
                }
                else
                {
                    battleInformation = $"You are about to fight {_currentNpc.Name} who has no weapon." + Environment.NewLine;
                }

                //
                // create and show text for current location information
                //
                battleInformation += $"Player: {_player.BattleMode}  Hit Points: {playerHitPoints}" + Environment.NewLine + $"NPC: {battleNpc.BattleMode}  Hit Points: {battleNpcHitPoints}" + Environment.NewLine;

                //
                // calculate results of battle
                //
                if (playerHitPoints >= battleNpcHitPoints)
                {
                    battleInformation += $"You have beat {_currentNpc.Name} to a pulp. \n{_currentNpc.Name} Health = {_currentNpc.Health}";
                    _currentNpc.Health -= playerHitPoints - battleNpcHitPoints;

                    if (_currentNpc.Health <= 0)
                    {
                    Map.CurrentLocation.Npcs.Remove(_currentNpc);
                    }
                }
                else
                {
                    battleInformation += $"You have been beat to a pulp by {_currentNpc.Name}.";
                    Player.Health -= battleNpcHitPoints - playerHitPoints;
                    if (_player.Health <= 0) 
                    {
                        _player.Lives--;

                    }
                }

                CurrentLocationInformation = battleInformation;
                if (_player.Lives <= 0) OnPlayerDies("You have died and have no lives left.");
            }
            else
            {
                CurrentLocationInformation = "The worker does not battle and ran off.";
                Map.CurrentLocation.Npcs.Remove(_currentNpc);
            }

        }

        /// <summary>
        /// Method used to determine Npcs battle response
        /// </summary>
        /// <returns>battle response</returns>
        private BattleModeEnum NpcBattleResponse()
        {
            BattleModeEnum npcBattleResponse = BattleModeEnum.RETREAT;

            switch (DieRoll(3))
            {
                case 1:
                    npcBattleResponse = BattleModeEnum.ATTACK;
                    break;
                case 2:
                    npcBattleResponse = BattleModeEnum.DEFEND;
                    break;
                case 3:
                    npcBattleResponse = BattleModeEnum.RETREAT;
                    break;
            }
            return npcBattleResponse;
        }

        /// <summary>
        /// Player hit points based on battle mode enum
        /// </summary>
        /// <returns>player hit points</returns>
        private int CalculatePlayerHitPoints()
        {
            int playerHitPoints = 0;

            switch (_player.BattleMode)
            {
                case BattleModeEnum.ATTACK:
                    playerHitPoints = _player.Attack();
                    break;
                case BattleModeEnum.DEFEND:
                    playerHitPoints = _player.Defend();
                    break;
                case BattleModeEnum.RETREAT:
                    playerHitPoints = _player.Retreat();
                    break;
            }

            return playerHitPoints;
        }

        /// <summary>
        /// NPC hit points based on battle mode enum
        /// </summary>
        /// <returns>NPC hit points</returns>
        private int CalculateNpcHitPoints(IBattle battleNpc)
        {
            int battleNpcHitPoints = 0;

            switch (NpcBattleResponse())
            {
                case BattleModeEnum.ATTACK:
                    battleNpcHitPoints = battleNpc.Attack();
                    break;
                case BattleModeEnum.DEFEND:
                    battleNpcHitPoints = battleNpc.Defend();
                    break;
                case BattleModeEnum.RETREAT:
                    battleNpcHitPoints = battleNpc.Retreat();
                    break;
            }

            return battleNpcHitPoints;
        }
        #endregion
        #region HELPER METHODS

        private int DieRoll(int sides)
        {
            return random.Next(1, sides + 1);
        }

        #endregion
        #region GAME TIME METHODS

        ///// <summary>
        ///// running time of game
        ///// </summary>
        ///// <returns></returns>
        //private TimeSpan GameTime()
        //{
        //    return DateTime.Now - _gameStartTime;
        //}

        ///// <summary>
        ///// game time event, publishes every 1 second
        ///// </summary>
        //public void GameTimer()
        //{
        //    DispatcherTimer timer = new DispatcherTimer();
        //    timer.Interval = TimeSpan.FromMilliseconds(1000);
        //    timer.Tick += OnGameTimerTick;
        //    timer.Start();
        //}

        ///// <summary>
        ///// game timer event handler
        ///// 1) update mission time on window
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void OnGameTimerTick(object sender, EventArgs e)
        //{
        //    _gameTime = DateTime.Now - _gameStartTime;
        //    MissionTimeDisplay = "Mission Time " + _gameTime.ToString(@"hh\:mm\:ss");
        //}
        #endregion
    }
}
