# DeckShuffler
## Description
This is a small application to shuffle a deck of cards in C# using .Net 5.0 and Windows Forms.
You can copy and compare decks and there are two implementations of shuffles.

### Prerequisites:
1. Download and install VS 2019 (Tested with Community Edition).
2. Clone the repository to your local machine.
3. Load the solution and project "DeckShuffler".
4. Run 'DeckShuffler' on the top bar.

# Features
### Top Right Buttons:
1. Clear Deck - Clears the current deck off the screen.
2. Copy Deck - Copies the current deck for use in comparing decks.
3. Load Deck - Loads the currently copied deck for use in comparing.
4. Compare Deck - Compares the current copied deck to the current deck in play.

### Bottom Buttons:
1. Draw Card - Draws a single card.
2. Draw 5 Cards - Draws 5 cards.
3. Draw 10 Cards - Draws 10 cards.
4. Flip All Cards - Flips all the cards. (They are still considered not drawn, so drawing more cards returns to the "Drawn" state.)
5. Shuffle - This does an optimal shuffle.
6. Alt Shuffle - This does an alternate less than optimal shuffle.

#### Center Board
This is the main view comprised of PictureBox's for the representation of the cards in the deck.

#### Top Left Board
This is the Copied Deck board view comprised of PictureBox's for the representation of the copied deck cards.
