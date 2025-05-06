using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GryKarciane.CzteryKarty
{
    public enum CardSuit
    {
        kier, karo, trefl, pik  
    }

    public enum CardValue
    {
        Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
    }

    internal class Card
    {
        public CardValue Value { get; }
        public CardSuit Suit { get; }

        public string ImagePath => $"avares://GryKarciane/Aski/Cards/{GetShortValue()}{Suit.ToString().ToLower()}.png";

        public Card(CardValue value, CardSuit suit)
        {
            Value = value;
            Suit = suit;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Card other)
                return false;

            return this.Value == other.Value && this.Suit == other.Suit;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Suit);
        }

        private string GetShortValue()
        {
            return Value switch
            {
                CardValue.Two => "2",
                CardValue.Three => "3",
                CardValue.Four => "4",
                CardValue.Five => "5",
                CardValue.Six => "6",
                CardValue.Seven => "7",
                CardValue.Eight => "8",
                CardValue.Nine => "9",
                CardValue.Ten => "10",
                CardValue.Jack => "J",
                CardValue.Queen => "Q",
                CardValue.King => "K",
                CardValue.Ace => "A",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
