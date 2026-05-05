using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.API.Model
{
    [System.Serializable]
    public class MPlayer
    {
        public int idPlayer; 
        public int id;       
        public string deck;

        public MPlayer(int idNew, string deckDef)
        {
            id = idNew;
            deck = deckDef;
        }

        public MPlayer(int idPlayerUp, int sameId,string deckUp)
        {
            idPlayer = idPlayerUp;
            id = sameId;
            deck = deckUp;
        }
    }
}
