using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLibrary.Models
{
    public class PlayerModel
    {
        public string FirstName { get; set; }
        public int PlayerNumber { get; set; }
        public List<string> ShipLocation = new List<string>();
        public List<string> OpponentGrid = new List<string>();
        public List<string> ShotsTaken = new List<string>();
        public double Stats { get; set; }
        private bool isWinner;

        public bool IsWinner
        {
            get { return isWinner; }
            set { isWinner = false; }
        }

    }
}
