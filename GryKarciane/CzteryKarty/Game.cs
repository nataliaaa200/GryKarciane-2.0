using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GryKarciane.CzteryKarty
{
    internal class Game
    {
        public List<Player> players;
        //private Deck deck;

        public Game()
        {
            players = new List<Player>();
            //deck = new Deck();
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public void DealCards(Deck deck)
        {
            foreach (var player in players)
            {
                for (int i = 0; i < 4; i++)
                {
                    player.Hand.Add(deck.DrawCard());
                }
            }
        }

        public bool CheckForWinner()
        {
            foreach (var player in players)
            {
                if (player.HasFourOfAKind())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
