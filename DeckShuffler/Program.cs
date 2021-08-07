namespace DeckShuffler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Deck deck = new Deck();
            // Deck deckOld = deck.Copy();
            deck.UnbiasedShuffle();
            // deckOld.UnbiasedShuffle();
            // deckOld.PrintOrder();
            deck.PrintOrder();

            /*
            System.Diagnostics.Debug.WriteLine(deckOld.Equals(deck));
            System.Diagnostics.Debug.WriteLine(deckOld.GetHashCode());
            System.Diagnostics.Debug.WriteLine(deck.GetHashCode());
            System.Diagnostics.Debug.WriteLine(deckOld == deck);
            System.Diagnostics.Debug.WriteLine(deckOld != deck);
            */
            /*
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            */
        }
    }
}
