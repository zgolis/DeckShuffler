namespace DeckShuffler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static DeckShuffler.Information;

    public class Deck : IEquatable<Deck>
    {
        private static Random random = new Random();

        public Deck(bool generateDeck = true)
        {
            if (generateDeck)
            {
                foreach (Suits suit in Enum.GetValues(typeof(Suits)))
                {
                    for (int count = 1; count < 14; count++)
                    {
                        this.Cards.Add(new Card(suit, (Values)count));
                    }
                }
            }
        }

        public Deck(List<Card> cards)
        {
            List<Card> newList = new List<Card>();
            foreach (Card card in cards)
            {
                newList.Add(card);
            }

            this.Cards = newList;
        }

        public List<Card> Cards { get; private set; } = new List<Card>();

        public static bool operator ==(Deck deck1, Deck deck2) => EqualityComparer<Deck>.Default.Equals(deck1, deck2);

        public static bool operator !=(Deck deck1, Deck deck2) => !(deck1 == deck2);

        public void LessEfficientShuffle()
        {
            List<Card> newList = new List<Card>();
            int newLocation;

            while (this.Cards.Count > 0)
            {
                newLocation = random.Next(0, this.Cards.Count);
                newList.Add(this.Cards[newLocation]);

                // Biased as it removes the old card.
                this.Cards.RemoveAt(newLocation);
            }

            this.Cards = newList;
        }

        public void UnbiasedShuffle()
        {
            Card[] array = this.Cards.ToArray();
            int length = array.Length;

            System.Diagnostics.Debug.WriteLine(length);

            for (int count = 0; count < length; count++)
            {
                int random = count + (int)(Deck.random.NextDouble() * (length - count));

                // Makes a copy of the card.
                Card tempCard = array[random];

                // Replaces the card with the current count card index.
                array[random] = array[count];

                // Replaces the current count card index with the copied card.
                array[count] = tempCard;
            }

            this.Cards = array.ToList();
        }

        public void PrintOrder()
        {
            System.Diagnostics.Debug.WriteLine("\n");
            foreach (Card card in this.Cards)
            {
                System.Diagnostics.Debug.WriteLine($"{Enum.GetName(card.Value)} of {Enum.GetName(card.Suit)}");
            }
        }

        public void PrintTotal()
        {
            int SuitCount(Suits suit)
            {
                return this.Cards.Where((item) => item.Suit == suit).Count();
            }

            int ValueCount(Values value)
            {
                return this.Cards.Where((item) => item.Value == value).Count();
            }

            System.Diagnostics.Debug.WriteLine($"{this.Cards.Count} cards in the deck.");
            System.Diagnostics.Debug.WriteLine($"Clubs: {SuitCount(Suits.Clubs)}");
            System.Diagnostics.Debug.WriteLine($"Diamonds: {SuitCount(Suits.Diamonds)}");
            System.Diagnostics.Debug.WriteLine($"Hearts: {SuitCount(Suits.Hearts)}");
            System.Diagnostics.Debug.WriteLine($"Spades: {SuitCount(Suits.Spades)}");

            System.Diagnostics.Debug.WriteLine($"Aces: {ValueCount(Values.Ace)}");
            System.Diagnostics.Debug.WriteLine($"Twos: {ValueCount(Values.Two)}");
            System.Diagnostics.Debug.WriteLine($"Threes: {ValueCount(Values.Three)}");
            System.Diagnostics.Debug.WriteLine($"Fours: {ValueCount(Values.Four)}");
            System.Diagnostics.Debug.WriteLine($"Fives: {ValueCount(Values.Five)}");
            System.Diagnostics.Debug.WriteLine($"Sixs: {ValueCount(Values.Six)}");
            System.Diagnostics.Debug.WriteLine($"Sevens: {ValueCount(Values.Seven)}");
            System.Diagnostics.Debug.WriteLine($"Eights: {ValueCount(Values.Eight)}");
            System.Diagnostics.Debug.WriteLine($"Nines: {ValueCount(Values.Nine)}");
            System.Diagnostics.Debug.WriteLine($"Tens: {ValueCount(Values.Ten)}");
            System.Diagnostics.Debug.WriteLine($"Jacks: {ValueCount(Values.Jack)}");
            System.Diagnostics.Debug.WriteLine($"Queens: {ValueCount(Values.Queen)}");
            System.Diagnostics.Debug.WriteLine($"Kings: {ValueCount(Values.King)}");
        }

        public Deck Copy()
        {
            return new Deck(this.Cards.Select(card => card.Copy()).ToList());
        }

        public override bool Equals(object obj) => this.Equals((Deck)obj);

        public bool Equals(Deck deck)
        {
            int count = 0;
            return this.Cards.All(card =>
            {
                var tempReturn = card.Equals(deck.Cards[count]);
                count++;
                return tempReturn;
            });
        }

        public override int GetHashCode()
        {
            int hashCode = 1938039292;
            if (this.Cards != null)
            {
                foreach (Card card in this.Cards)
                {
                    hashCode ^= card.GetHashCode();
                }
            }

            return hashCode;
        }
    }
}
