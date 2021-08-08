namespace DeckShuffler
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        /// <summary>
        /// Determines whether a board has been flipped.
        /// </summary>
        private bool cardsFlipped = false;

        /// <summary>
        /// Creates a new MainForm class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
            this.ResetDeck();

            for (int count = 0; count < 52; count++)
            {
                PictureBox dynamicPictureBox = this.DynamicPictureBox(count);
                PictureBox dynamicCopiedPictureBox = this.DynamicPictureBox(count, "Copied");
                dynamicPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                dynamicCopiedPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            this.AllCardBackBackgrounds();
        }

        /// <summary>
        /// The root deck.
        /// </summary>
        public Deck DeckRoot { get; private set; }

        /// <summary>
        /// The draw pile.
        /// </summary>
        public Deck DrawPile { get; private set; }

        /// <summary>
        /// The discard pile.
        /// </summary>
        public Deck DiscardPile { get; private set; }

        /// <summary>
        /// The copied deck.
        /// </summary>
        public Deck DeckCopy { get; private set; }

        /// <summary>
        /// General card back image.
        /// </summary>
        private Image Background => Image.FromFile(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + $"\\Resources\\back.png");

        // * Button Clicks * //
        private void ButtonShuffle_Click(object sender, EventArgs e)
        {
            this.Shuffle(() => this.DeckRoot.UnbiasedShuffle());
        }

        private void ButtonAltShuffle_Click(object sender, EventArgs e)
        {
            this.Shuffle(() => this.DeckRoot.LessEfficientShuffle());
        }

        private void ButtonClearDeck_Click(object sender, EventArgs e)
        {
            this.ResetDeck();
        }

        private void ButtonCopyDeck_Click(object sender, EventArgs e)
        {
            this.CopyDeck();
        }

        private void ButtonDrawCard_Click(object sender, EventArgs e)
        {
            this.DrawCard();
        }

        private void ButtonDrawFiveCards_Click(object sender, EventArgs e)
        {
            this.DrawCard(5);
        }

        private void ButtonDrawTenCards_Click(object sender, EventArgs e)
        {
            this.DrawCard(10);
        }

        private void ButtonFlipAllCards_Click(object sender, EventArgs e)
        {
            this.FlipCards((card) => this.cardsFlipped ? this.Background : card.Image);
            this.cardsFlipped ^= true;
        }

        private void ButtonLoadDeck_Click(object sender, EventArgs e)
        {
            this.AllCardBackBackgrounds();
            this.DeckRoot = this.DeckCopy.Copy();
            this.DrawPile = this.DeckCopy.Copy();
            this.DiscardPile = new Deck(generateDeck: false);
        }

        private void ButtonCompareDecks_Click(object sender, EventArgs e)
        {
            if (this.DeckCopy == null)
            {
                this.CopyDeck();
            }
            else
            {
                this.LabelOutput.Text = string.Empty;
                this.LabelOutput.Text = this.DeckCopy.Equals(this.DeckRoot) ? "Yes, the decks are the same." : "No, the decks are different";
            }
        }

        // * End Button Clicks * //

        /// <summary>
        /// Shuffles the deck.
        /// </summary>
        /// <param name="action">The type of shuffle to perform.</param>
        private void Shuffle(Action action)
        {
            this.AllCardBackBackgrounds();
            action();
            this.DrawPile = this.DeckRoot.Copy();
            this.DiscardPile = new Deck(generateDeck: false);

            if (this.cardsFlipped)
            {
                this.FlipCards((card) => card.Image);
            }

            GC.Collect();
        }

        /// <summary>
        /// Flips all cards.
        /// </summary>
        /// <param name="cardImage">Image to flip the cards to.</param>
        private void FlipCards(Func<Card, Image> cardImage)
        {
            for (int count = this.DiscardPile.Cards.Count(); count < this.DeckRoot.Cards.Count; count++)
            {
                Card card = this.DeckRoot.Cards[count];
                this.SetCardImage(count, cardImage(card));
            }
        }

        /// <summary>
        /// Copies the deck.
        /// </summary>
        private void CopyDeck()
        {
            this.DeckCopy = this.DeckRoot.Copy();

            for (int count = 0; count < this.DeckRoot.Cards.Count; count++)
            {
                Card card = this.DeckCopy.Cards[count];
                this.SetCopiedCardImage(count, card.Image);
            }
        }

        /// <summary>
        /// Resets the deck.
        /// </summary>
        /// <param name="action">Action to perform such as shuffles.</param>
        private void ResetDeck()
        {
            this.DeckRoot = new Deck();
            this.ClearAllBackgrounds();
            this.DrawPile = this.DeckRoot.Copy();
            this.DiscardPile = new Deck(generateDeck: false);
        }

        /// <summary>
        /// Draws a set number of cards.
        /// </summary>
        /// <param name="cardsToDraw">The number of cards to draw. Default is 1.</param>
        private void DrawCard(int cardsToDraw = 1)
        {
            this.LabelOutput.Text = string.Empty;
            int counter = 0;

            if (this.DiscardPile.Cards.Count() == 0)
            {
                this.AllCardBackBackgrounds();
            }

            if (this.cardsFlipped)
            {
                this.cardsFlipped = false;
                this.AllCardBackBackgrounds();

                for (int count = 0; count < this.DiscardPile.Cards.Count; count++)
                {
                    this.SetCardImage(count, this.DiscardPile.Cards[count].Image);
                }
            }

            while (cardsToDraw > 0)
            {
                if (this.DrawPile.Cards.Count > 0)
                {
                    Card card = this.DrawPile.Cards[0];
                    this.DiscardPile.Cards.Add(card);
                    this.SetCardImage(this.DiscardPile.Cards.Count() - 1, card.Image);
                    this.DrawPile.Cards.Remove(card);
                }
                else
                {
                    counter++;
                    this.LabelOutput.Text = $"No more cards to draw! x{counter}";
                }

                cardsToDraw--;
            }
        }

        /// <summary>
        /// Sets all cards in the main deck to card back backgrounds.
        /// </summary>
        private void AllCardBackBackgrounds() => this.SetBackgrounds((count) => this.SetCardImage(count, this.Background));

        /// <summary>
        /// Sets all backgrounds to nothing.
        /// </summary>
        private void ClearAllBackgrounds() => this.SetBackgrounds((count) => this.SetCardImage(count));

        /// <summary>
        /// Helper method to set the backgrounds of card cells.
        /// </summary>
        /// <param name="action">The action to perform which takes an counter.</param>
        private void SetBackgrounds(Action<int> action)
        {
            for (int count = 0; count < this.DeckRoot.Cards.Count; count++)
            {
                action(count);
            }
        }

        /// <summary>
        /// Helper method to set the card image of a cell.
        /// </summary>
        /// <param name="count">The number to identitify the picturebox.</param>
        /// <param name="image">The image to set the picturebox to.</param>
        private void SetCardImage(int count, Image image = null)
        {
            PictureBox dynamicPictureBox = this.DynamicPictureBox(count);
            dynamicPictureBox.Image = image;
        }

        /// <summary>
        /// Helper method to set the copied card image, which is reversed.
        /// </summary>
        /// <param name="count">The number to identitify the picturebox.</param>
        /// <param name="image">The image to set the picturebox to.</param>
        private void SetCopiedCardImage(int count, Image image = null)
        {
            PictureBox dynamicPictureBox = this.DynamicPictureBox(51 - count, "Copied");
            dynamicPictureBox.Image = image;
        }

        /// <summary>
        /// Helper method to return a picturebox.
        /// </summary>
        /// <param name="count">The int ending of the picturebox's name.</param>
        /// <param name="copied">Whether or not the picturebox is for the copied table or main.</param>
        /// <returns></returns>
        private PictureBox DynamicPictureBox(int count, string copied = null) => (PictureBox)typeof(MainForm).GetField($"{copied}PictureBox{count + 1}", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
    }
}
