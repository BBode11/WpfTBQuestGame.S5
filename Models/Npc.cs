using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public abstract class Npc : Character
    {
        public string Description { get; set; }
        public string Information
        {
            get
            {
                return InformationText();
            }
            set
            {

            }
        }

        protected abstract string InformationText();

        /// <summary>
        ///Default Constructor
        /// </summary>
        public Npc()
        {

        }
        public Npc(int id, string name, string description, int _health)
        {
            ID = id;
            Name = name;
            Description = description;
            Health = _health;
        }
    }
}
