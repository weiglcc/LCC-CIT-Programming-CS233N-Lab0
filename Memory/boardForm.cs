using System;
using System.Drawing;
using System.Windows.Forms;

namespace Memory
{
    public partial class boardForm : Form
    {
        public boardForm()
        {
            InitializeComponent();
        }

        #region Instance Variables

        private const int NOT_PICKED_YET = -1;

        private int firstCardNumber = NOT_PICKED_YET;
        private int secondCardNumber = NOT_PICKED_YET;
        private int matches = 0;

        #endregion Instance Variables

        #region Methods

        // This method finds a picture box on the board based on it's number (1 - 20)
        // It takes an integer as it's parameter and returns the picture box controls
        // that's name contains that number
        private PictureBox GetCard(int i)
        {
            PictureBox card = (PictureBox)this.Controls["card" + i];
            return card;
        }

        // This method gets the filename for the image displayed in a picture box given it's number
        // It takes an integer as it's parameter and returns a string containing the
        // filename for the image in the corresponding picture box
        private string GetCardFilename(int i)
        {
            return GetCard(i).Tag.ToString();
        }

        // This method changes the filename for a given picture box
        // It takes an integer and a string that represents a filename as it's parameters
        // It doesn't return a value but stores the filename for the image to be displayed
        // in the picture box.  It doesn't actually display the new image
        private void SetCardFilename(int i, string filename)
        {
            GetCard(i).Tag = filename;
        }

        // These 2 methods get the value (and suit) of the card in a given picturebox
        // Both methods take an integer as the parameter and return a string
        private string GetCardValue(int index)
        {
            return GetCardFilename(index).Substring(4, 1);
        }

        private string GetCardSuit(int index)
        {
            return GetCardFilename(index).Substring(5, 1);
        }

        // TODO:  students should write this one
        private bool IsMatch(int index1, int index2)
        {
            string firstCardValue = GetCardValue(index1);
            string secondCardalue = GetCardValue(index2);
            string firstCardSuit = GetCardSuit(index1);
            string secondCardSuit = GetCardSuit(index2);
            bool isSameValue = firstCardValue == secondCardalue;
            bool isSameColor =
                (firstCardSuit == "s" && secondCardSuit == "c") ||
                (firstCardSuit == "c" && secondCardSuit == "s") ||
                (firstCardSuit == "d" && secondCardSuit == "h") ||
                (firstCardSuit == "h" && secondCardSuit == "d");
            return isSameValue && isSameColor;
        }

        // This method fills each picture box with a filename
        private void FillCardFilenames()
        {
            string[] values = { "a", "2", "j", "q", "k" };
            string[] suits = { "c", "d", "h", "s" };
            int i = 1;

            for (int suit = 0; suit <= 3; suit++)
            {
                for (int value = 0; value <= 4; value++)
                {
                    SetCardFilename(i, "card" + values[value] + suits[suit] + ".jpg");
                    LoadCard(i);
                    i++;
                }
            }
        }

        // TODO:  students should write this one
        private void ShuffleCards()
        {
            Random random = new Random();
            for (int i = 1; i <= 20; i++)
            {
                PictureBox card = GetCard(i);
                Image tempImg = card.Image;
                object tempTag = card.Tag;
                int number = random.Next(i, 20);
                PictureBox target = GetCard(number);
                card.Image = target.Image;
                card.Tag = target.Tag;
                target.Image = tempImg;
                target.Tag = tempTag;
            }
        }

        // This method loads (shows) an image in a picture box.  Assumes that filenames
        // have been filled in an earlier call to FillCardFilenames
        private void LoadCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\Cards\\" + GetCardFilename(i));
        }

        // This method loads the image for the back of a card in a picture box
        private void LoadCardBack(int i)
        {
            PictureBox card = GetCard(i);
            card.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\Cards\\black_back.jpg");
        }

        // TODO:  students should write all of these
        // shows (loads) the backs of all of the cards
        private void LoadAllCardBacks()
        {
            for (int i = 1; i <= 20; i++)
            {
                LoadCardBack(i);
            }
        }

        // Hides a picture box
        private void HideCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Hide();
        }

        //private void HideAllCards()
        //{
        //    for (int i = 1; i <= 20; i++)
        //    {
        //        HideCard(i);
        //    }
        //}

        // shows a picture box
        private void ShowCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Show();
        }

        private void ShowAllCards()
        {
            for (int i = 1; i <= 20; i++)
            {
                ShowCard(i);
            }
        }

        // disables a picture box
        private void DisableCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Enabled = false;
        }

        private void DisableAllCards()
        {
            for (int i = 1; i <= 20; i++)
            {
                DisableCard(i);
            }
        }

        private void EnableCard(int i)
        {
            PictureBox card = GetCard(i);
            card.Enabled = true;
        }

        private void EnableAllCards()
        {
            for (int i = 1; i <= 20; i++)
            {
                EnableCard(i);
            }
        }

        private void EnableAllVisibleCards()
        {
            for (int i = 1; i <= 20; i++)
            {
                PictureBox card = GetCard(i);
                if (card.Visible)
                {
                    card.Enabled = true;
                }
            }
        }

        #endregion Methods

        #region EventHandlers

        private void boardForm_Load(object sender, EventArgs e)
        {
            /*
             * Fill the picture boxes with filenames
             * Shuffle the cards
             * Load all of the card backs -
             *      While you're testing you might want to load all of card faces
             *      to make sure that the cards are loaded successfully and that
             *      they're shuffled.  If you get all 2s, something is wrong.
             */
            FillCardFilenames();
            ShuffleCards();
            LoadAllCardBacks();
        }

        private void card_Click(object sender, EventArgs e)
        {
            PictureBox card = (PictureBox)sender;
            int cardNumber = int.Parse(card.Name.Substring(4));
            /*
             * if the first card isn't picked yet
             *      save the first card index
             *      load the card
             *      disable the card
             *  else (the user just picked the second card)
             *      save the second card index
             *      load the card
             *      disable all of the cards
             *      start the flip timer
             *  end if
             */
            if (firstCardNumber == NOT_PICKED_YET)
            {
                firstCardNumber = cardNumber;
                LoadCard(firstCardNumber);
                DisableCard(firstCardNumber);
                return;
            }
            else
            {
                secondCardNumber = cardNumber;
                LoadCard(secondCardNumber);
                DisableAllCards();
                flipTimer.Start();
            }
        }

        private void flipTimer_Tick(object sender, EventArgs e)
        {
            /*
             * stop the flip timer
             * if the first card and second card are a match
             *      increment the number of matches
             *      hide the first card
             *      hide the second card
             *      reset the first card number
             *      reset the second card number
             *      if the number of matches is 10
             *          show a message box
             *      else
             *          enable all of the cards left on the board
             *      end if
             * else
             *      flip the first card back over
             *      flip the second card back over
             *      reset the first card number
             *      reset the second card number
             *      enable all of the cards left on the board
             * end if
             */
            flipTimer.Stop();
            if (IsMatch(firstCardNumber, secondCardNumber))
            {
                matches++;
                HideCard(firstCardNumber);
                HideCard(secondCardNumber);
                if (matches == 10)
                {
                    MessageBox.Show("Done!");
                    matches = 0;
                    ShowAllCards();
                    EnableAllCards();
                    FillCardFilenames();
                    ShuffleCards();
                    LoadAllCardBacks();
                }
                else
                {
                    EnableAllVisibleCards();
                }
            }
            else
            {
                LoadCardBack(firstCardNumber);
                LoadCardBack(secondCardNumber);
                EnableAllVisibleCards();
            }
            firstCardNumber = NOT_PICKED_YET;
            secondCardNumber = NOT_PICKED_YET;
        }

        #endregion EventHandlers
    }
}