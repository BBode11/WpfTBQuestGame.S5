using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public interface IBattle
    {
        //possible reference error for weapon****
        Weapon CurrentWeapon { get; set; }
        BattleModeEnum BattleMode{ get; set; }

        ///Methods for battle hit points
        int Attack();
        int Defend();
        int Retreat();
    }
}
