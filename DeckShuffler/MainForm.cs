namespace DeckShuffler
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        private bool cardsFlipped = false;

        public MainForm()
        {
            this.InitializeComponent();
            this.DeckRoot = new Deck();
            this.NewPile = this.DeckRoot.Copy();
            this.OldPile = new Deck(generateDeck: false);

            for (int count = 0; count < 52; count++)
            {
                PictureBox dynamicPictureBox = (PictureBox)typeof(MainForm).GetField($"PictureBox{count + 1}", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
                dynamicPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            this.AllCardBackBackgrounds();
        }

        public Deck NewPile { get; private set; }

        public Deck OldPile { get; private set; }

        public Deck DeckRoot { get; private set; }

        public Deck DeckCopy { get; private set; }

        private Image Background => Image.FromFile(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + $"\\Resources\\back.png");

        private void ResetDeck(Action action = null)
        {
            this.ClearAllBackgrounds();
            this.DeckRoot = new Deck();

            if (action != null)
            {
                action();
            }

            this.NewPile = this.DeckRoot.Copy();
            this.OldPile = new Deck(generateDeck: false);
        }

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
        }

        private void ButtonShuffle_Click(object sender, EventArgs e)
        {
            this.AllCardBackBackgrounds();
            this.DeckRoot = new Deck();
            this.DeckRoot.UnbiasedShuffle();
            this.NewPile = this.DeckRoot.Copy();
            this.OldPile = new Deck(generateDeck: false);
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
            int newPileLength = this.NewPile.Cards.Count();
            int oldPileLength = this.OldPile.Cards.Count();

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
            this.NewPile = this.DeckCopy.Copy();
            this.OldPile = new Deck(generateDeck: false);
        }

        private void ButtonCompareDecks_Click(object sender, EventArgs e)
        {
            this.LabelOutput.Text = string.Empty;
            this.LabelOutput.Text = this.DeckCopy.Equals(this.DeckRoot) ? "Yes, the decks are the same." : "No, the decks are different";
        }

        private void DrawCard(int cardsToDraw = 1)
        {
            this.LabelOutput.Text = string.Empty;

            if (this.cardsFlipped)
            {
                this.AllCardBackBackgrounds();
                this.cardsFlipped = false;
                this.DeckRoot = new Deck();
                this.DeckRoot.LessEfficientShuffle();
                this.NewPile = this.DeckRoot.Copy();
            }

            while (cardsToDraw > 0)
            {
                if (this.NewPile.Cards.Count > 0)
                {
                    Card card = this.NewPile.Cards[0];
                    this.OldPile.Cards.Add(card);
                    this.SetCardImage(this.OldPile.Cards.Count() - 1, card.Image);
                    this.NewPile.Cards.Remove(card);
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

        private PictureBox DynamicPictureBox(int count) => (PictureBox)typeof(MainForm).GetField($"PictureBox{count + 1}", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
    }
}
