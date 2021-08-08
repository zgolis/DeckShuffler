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
        private bool cardsFlipped = false;

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

        public Deck DrawPile { get; private set; }

        public Deck DiscardPile { get; private set; }

        public Deck DeckRoot { get; private set; }

        public Deck DeckCopy { get; private set; }

        private Image Background => Image.FromFile(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + $"\\Resources\\back.png");

        private void ButtonRealShuffle_Click(object sender, EventArgs e)
        {
            this.ResetDeck(() => this.DeckRoot.LessEfficientShuffle());
            this.AllCardBackBackgrounds();
        }

        private void ButtonClearDeck_Click(object sender, EventArgs e)
        {
            this.ResetDeck();
        }

        private void ButtonCopyDeck_Click(object sender, EventArgs e)
        {
            this.DeckCopy = this.DeckRoot.Copy();

            for (int count = 0; count < 52; count++)
            {
                Card card = this.DeckCopy.Cards[count];
                this.SetCopiedCardImage(count, card.Image);
            }
        }

        private void ButtonShuffle_Click(object sender, EventArgs e)
        {
            this.AllCardBackBackgrounds();
            this.ResetDeck(() => this.DeckRoot.UnbiasedShuffle());
        }

        private void ButtonDrawCard_Click(object sender, EventArgs e)
        {
            this.DrawCard();
        }

        private void ButtonDrawFiveCards_Click(object sender, EventArgs e)
        {
            this.DrawCard(5);
        }

        private void ButtonFlipAllCards_Click(object sender, EventArgs e)
        {
            int oldPileLength = this.DiscardPile.Cards.Count();

            for (int count = oldPileLength; count < 52; count++)
            {
                Card card = this.DeckRoot.Cards[count];
                this.SetCardImage(count, card.Image);
            }

            this.cardsFlipped = true;
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
            this.LabelOutput.Text = string.Empty;
            this.LabelOutput.Text = this.DeckCopy.Equals(this.DeckRoot) ? "Yes, the decks are the same." : "No, the decks are different";
        }

        private void ResetDeck(Action action = null)
        {
            this.ClearAllBackgrounds();
            this.DeckRoot = new Deck();

            if (action != null)
            {
                action();
            }

            this.DrawPile = this.DeckRoot.Copy();
            this.DiscardPile = new Deck(generateDeck: false);
        }

        private void DrawCard(int cardsToDraw = 1)
        {
            this.LabelOutput.Text = string.Empty;

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
                    this.LabelOutput.Text += "No more cards to draw!\n";
                }

                cardsToDraw--;
            }
        }

        private void AllCardBackBackgrounds()
        {
            for (int count = 0; count < 52; count++)
            {
                this.SetCardImage(count, this.Background);
            }
        }

        private void ClearAllBackgrounds()
        {
            for (int count = 0; count < 52; count++)
            {
                this.SetCardImage(count);
            }
        }

        private void SetCardImage(int count, Image image = null)
        {
            PictureBox dynamicPictureBox = this.DynamicPictureBox(count);
            dynamicPictureBox.Image = image;
        }

        private void SetCopiedCardImage(int count, Image image = null)
        {
            PictureBox dynamicPictureBox = this.DynamicPictureBox(51 - count, "Copied");
            dynamicPictureBox.Image = image;
        }

        private PictureBox DynamicPictureBox(int count, string copied = null) => (PictureBox)typeof(MainForm).GetField($"{copied}PictureBox{count + 1}", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
    }
}
