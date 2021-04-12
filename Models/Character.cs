using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    /// <summary>
    /// Base class for all game characters
    /// </summary>
    public class Character : ObservableObject
    {
        #region FIELDS
        //attributes of character
        protected int _id;
        protected string _name;
        protected int _age;
        protected int _locationId;
        protected int _health;

        protected Random random = new Random();

        #endregion

        #region PROPERTIES
        public int LocationId
        {
            get { return _locationId; }
            set { _locationId = value; }
        }


        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;
                OnPropertyChanged(nameof(Health));
            }
        }
        #endregion

        #region CONSTRUCTORS

        public Character()
        {

        }

        public Character(string name, int age, int locationId, int _health)
        {
            _name = name;
            _age = age;
            _locationId = locationId;
            _health = Health ;
        }

        #endregion

        #region METHODS

        /// <summary>
        /// ***ADD ABSTRACT AND VIRTUAL METHOD
        /// </summary>
        /// <returns></returns>
        public virtual string DefaultPlayerGreeting()
        {
            return $"Hello my name is {_name}!";
        }

        #endregion
    }
}
