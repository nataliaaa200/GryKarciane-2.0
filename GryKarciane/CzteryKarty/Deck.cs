using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GryKarciane.CzteryKarty
{
    internal class Deck
    {
        private List<Card> cards;
        public int RemainingCards => cards.Count;

        public Deck()
        {
            cards = new List<Card>();
            InitializeDeck();
        }

        private void InitializeDeck()
        {
            var suits = Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>().ToList();
            var values = Enum.GetValues(typeof(CardValue)).Cast<CardValue>().ToList();

            foreach (var suit in suits)
            {
                foreach (var value in values)
                {
                    cards.Add(new Card(value, suit));
                }
            }

            Shuffle();
            Console.WriteLine(cards.Count);
        }

        public void Shuffle()
        {
            var rand = new Random();
            cards = cards.OrderBy(card => rand.Next()).ToList();
        }

        public Card DrawCard()
        {
            if (cards.Count == 0) return null;
            var card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public void RemoveCard(Card card)
        {
            cards.Remove(card);
        }
    }
}
