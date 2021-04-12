using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public class Worker : Npc, ISpeak
    {
        Random r = new Random();

        public List<string> Messages { get; set; }

        protected override string InformationText()
        {
            return $"{Name} : {Description}";
        }
        #region CONSTRUCTORS
        /// <summary>
        /// Default constructor
        /// </summary>
        public Worker()
        {

        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="messages"></param>
        public Worker(int id, string name, string description, List<string> messages)
        {
            Messages = messages;
        }
        #endregion
        #region INTERFACE METHODS
        /// <summary>
        /// Generates a message for worker through ISpeak interface
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

        private string GetMessage()
        {
            int messageIndex = r.Next(0, Messages.Count());
            return Messages[messageIndex];
        }
        #endregion
    }
}
