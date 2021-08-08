namespace DeckShuffler
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using static DeckShuffler.Information;

    public class Card : IEquatable<Card>
    {
        /// <summary>
        /// Creates a new Card class.
        /// </summary>
        /// <param name="suit">The Suits value of the card.</param>
        /// <param name="value">The Value value of the card.</param>
        public Card(Suits suit, Values value)
        {
            this.Suit = suit;
            this.Value = value;
        }

        /// <summary>
        /// The Suite of the card.
        /// </summary>
        public Suits Suit { get; }

        /// <summary>
        /// The Value of the card.
        /// </summary>
        public Values Value { get; }

        /// <summary>
        /// The cards image.
        /// </summary>
        public Image Image => Image.FromFile(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + $"\\Resources\\{Enum.GetName(this.Value).ToLower()}_{Enum.GetName(this.Suit).ToLower()}.png");

        public static bool operator ==(Card card1, Card card2) => EqualityComparer<Card>.Default.Equals(card1, card2);

        public static bool operator !=(Card card1, Card card2) => !(card1 == card2);

        /// <summary>
        /// Copies the current card.
        /// </summary>
        /// <returns>The current card.</returns>
        public Card Copy()
        {
            return this;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this.Equals((Card)obj);

        /// <summary>
        /// Determines whether the specified card is equal to the current card.
        /// </summary>
        /// <param name="card">The card to compare against.</param>
        /// <returns>A bool if the values of a card compate.</returns>
        public bool Equals(Card card)
        {
            return this.Suit == card.Suit && this.Value == card.Value;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hashCode = 1938119292;
            hashCode ^= this.Suit.GetHashCode();
            hashCode ^= this.Value.GetHashCode();
            return hashCode;
        }
    }
}
