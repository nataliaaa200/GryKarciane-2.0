using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GryKarciane.CzteryKarty
{
    internal class Player
    {
        public string Name { get; }
        public List<Card> Hand { get; } = new List<Card>();

        public bool HasFourOfAKind()
        {
            return Hand.GroupBy(card => card.Value).Any(group => group.Count() == 4);
        }

        public Player(string name)
        {
            Name = name;
        }

        public void MakeMove(Deck deck)
        {
            // Wylosuj jedną kartę do wymiany (np. pierwszą, która nie ma pary)
            var grouped = Hand.GroupBy(c => c.Value).ToList();
            var cardToReplace = Hand.FirstOrDefault(card => grouped.First(g => g.Key == card.Value).Count() == 1);

            if (cardToReplace != null)
            {
                Hand.Remove(cardToReplace);
                var newCard = deck.DrawCard();
                if (newCard != null)
                {
                    Hand.Add(newCard);
                }
            }
        }
    }
}
