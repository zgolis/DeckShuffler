namespace DeckShuffler
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using static DeckShuffler.Information;

    public class Card : IEquatable<Card>
    {
        public Card(Suits suit, Values value)
        {
            this.Suit = suit;
            this.Value = value;
        }

        public Suits Suit { get; }

        public Values Value { get; }

        public Image Image => Image.FromFile(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + $"\\Resources\\{Enum.GetName(this.Value).ToLower()}_{Enum.GetName(this.Suit).ToLower()}.png");

        public static bool operator ==(Card card1, Card card2) => EqualityComparer<Card>.Default.Equals(card1, card2);

        public static bool operator !=(Card card1, Card card2) => !(card1 == card2);

        public Card Copy()
        {
            return this;
        }

        public override bool Equals(object obj) => this.Equals((Card)obj);

        public bool Equals(Card card)
        {
            return this.Suit == card.Suit && this.Value == card.Value;
        }

        public override int GetHashCode()
        {
            int hashCode = 1938119292;
            hashCode ^= this.Suit.GetHashCode();
            hashCode ^= this.Value.GetHashCode();
            return hashCode;
        }
    }
}
