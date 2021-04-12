using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public class Townie : Npc, ISpeak, IBattle
    {
        /// <summary>
        /// customize consts
        /// </summary>
        private const int DEFENDER_DAMAGE_ADJUSTMENT = 5;
        private const int MAXIMUM_RETREAT_DAMAGE = 5;

        public List<string> Messages { get; set; }
        public BattleModeEnum BattleMode { get; set; }
        public Weapon CurrentWeapons { get; set; }
        public Weapon CurrentWeapon { get; set; }

        /// <summary>
        /// Override string for abstract Npc class
        /// </summary>
        /// <returns></returns>
        protected override string InformationText()
        {
            return $"{Name} : {Description}";
        }
        #region CONSTRUCTORS
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Townie()
        {

        }
        public Townie(int id, string name, string description, List<string> messages, Weapon currentWeapons, Weapon currentWeapon)
        {
            Messages = messages;
            CurrentWeapon = currentWeapons;
            CurrentWeapon = currentWeapon;
        }

        #endregion
        #region INTERFACE METHODS
        /// <summary>
        /// Generates a message for Townie through ISpeak interface
        /// </summary>
        /// <returns></returns>
        public string Speak()
        {
            if (this.Messages != null)
            {
                return GetMessage();
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Selects message randomly
        /// </summary>
        /// <returns></returns>
        private string GetMessage()
        {
            Random r = new Random();
            int messageIndex = r.Next(0, Messages.Count());
            return Messages[messageIndex];
        }
        #endregion
        #region BATTLE METHODS
        /// <summary>
        /// Return hit points based on current weapon min/max damage
        /// </summary>
        /// <returns>hit points</returns>
        public int Attack()
        {
            int hitPoints = random.Next(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage);

            if(hitPoints <= 100)
            {
                return hitPoints;
            }
            else
            {
                return 100;
            }
        }
        /// <summary>
        /// Return hit points based on NPC weapon
        /// </summary>
        /// <returns>Hit points</returns>
        public int Defend()
        {
            int hitPoints = (random.Next(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage) - DEFENDER_DAMAGE_ADJUSTMENT);

            if (hitPoints >= 0 && hitPoints <= 100)
            {
                return hitPoints;
            }
            else if (hitPoints > 100)
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Return hit points based on retreat damage constant
        /// </summary>
        /// <returns>hit points</returns>
        public int Retreat()
        {
            int hitPoints = MAXIMUM_RETREAT_DAMAGE;

            if (hitPoints <= 100)
            {
                return hitPoints;
            }
            else
            {
                return 100;
            }
        }
        #endregion
    }
}
