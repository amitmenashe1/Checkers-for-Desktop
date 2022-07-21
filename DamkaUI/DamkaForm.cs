using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Back;

namespace DamkaUI
{
    public partial class DamkaForm : Form
    {
        private Label labelPlayer1 = new Label();
        private Label labelPlayer2 = new Label();
        private Board board;
        private Button[,] buttonsBoard;
        private Button chosenButton;
        private gameSettingsForm gameSettingsForm;
        private Player player1;
        private Player player2;
        private Position nextMove = null;
        private Position fromMove = null;
        private bool firstClickDamka = false;
        private int turnChange = 1;

        public DamkaForm()
        {
            gameSettingsForm = new gameSettingsForm();

            if (this.gameSettingsForm.ShowDialog() == DialogResult.OK)
            {
                player1 = new Player(gameSettingsForm.Player1Name, eXorO.X);
                board = initBoard();
                buttonsBoard = new Button[board.Size, board.Size];
                player2 = initPlayer2();
                labelPlayer1.Text = gameSettingsForm.Player1Name + ": " + board.ScoreX.ToString();
            }

            this.Text = "Damka";
            initializeComponent();
            this.ShowDialog();
        }

        private Board initBoard()
        {
            int boardSize;

            if (gameSettingsForm.RadioButton6x6.Checked)
            {
                boardSize = 6;
            }
            else if (gameSettingsForm.RadioButton8x8.Checked)
            {
                boardSize = 8;
            }
            else
            {
                boardSize = 10;
            }

            return new Board(boardSize);
        }

        private Player initPlayer2()
        {
            string playerName;

            if (gameSettingsForm.CheckBoxPlayer2.Checked)
            {
                playerName = gameSettingsForm.Player2Name;
                labelPlayer2.Text = gameSettingsForm.Player2Name + ": " + board.ScoreO.ToString();
            }
            else
            {
                playerName = "Computer";
                labelPlayer2.Text = "Computer: " + board.ScoreO.ToString();
            }

            return new Player(playerName, eXorO.O);
        }

        private void initializeComponent()
        {
            int i = 0, j = 0;

            this.ClientSize = new Size(35 * board.Size + 17, 35 * board.Size + 40);

            for (i = 0; i < board.Size; i++)
            {
                for (j = 0; j < board.Size; j++)
                {
                    buttonsBoard[i, j] = new Button();
                    buttonsBoard[i, j].Size = new Size(35, 35);
                    buttonsBoard[i, j].Click += damkaForm_Click;
                    setButtonsLocation(i, j);
                    this.Controls.Add(buttonsBoard[i, j]);

                    if ((j % 2) == (i % 2))
                    {
                        buttonsBoard[i, j].Enabled = false;
                        buttonsBoard[i, j].BackColor = Color.Gray;
                    }
                }
            }

            for (i = board.Size - 1; i > board.Size / 2; i--)
            {
                for (j = 0; j < board.Size; j++)
                {
                    if ((j % 2) == ((i + 1) % 2))
                    {
                        buttonsBoard[i, j].Text = "X";
                    }
                }
            }

            for (i = 0; i < (board.Size / 2) - 1; i++)
            {
                for (j = board.Size - 1; j >= 0; j--)
                {
                    if ((j % 2) == ((i + 1) % 2))
                    {
                        buttonsBoard[i, j].Text = "O";
                    }
                }
            }

            this.labelPlayer1.Location = new Point(ClientSize.Width / 3 - labelPlayer1.Width / 2, 9);
            this.labelPlayer2.Location = new Point((ClientSize.Width - ClientSize.Width / 3) - labelPlayer1.Width / 2, 9);
            this.labelPlayer1.AutoSize = true;
            this.labelPlayer2.AutoSize = true;
            this.Controls.Add(labelPlayer1);
            this.Controls.Add(labelPlayer2);
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void setButtonsLocation(int i_Row, int i_Col)
        {
            int xValue = ((this.buttonsBoard[i_Row, i_Col].Width) * i_Col + 10);
            int yValue = ((this.buttonsBoard[i_Row, i_Col].Width) * i_Row) + 30;

            this.buttonsBoard[i_Row, i_Col].Location = new Point(xValue, yValue);
        }

        private void damkaForm_Click(object sender, EventArgs e)
        {
            Button theSender = sender as Button;
            int stepStatus;

            if (!firstClickDamka)
            {
                theSender.BackColor = Color.LightBlue;
                fromMove = findPositionOfButton(theSender);
                firstClickDamka = true;
            }
            else
            {
                firstClickDamka = false;
                chosenButton = getPreviousButtonLocation();

                if (theSender == chosenButton)
                {
                    fromMove = null;
                    nextMove = null;
                    theSender.BackColor = Color.White;
                }
                else
                {
                    nextMove = findPositionOfButton(theSender);
                    stepStatus = doNextStep();
                }
            }
        }

        private Button getPreviousButtonLocation()
        {
            Button prevButton = new Button();

            foreach (Button button in buttonsBoard)
            {
                if (button.BackColor == Color.LightBlue)
                {
                    prevButton = button;
                    break;
                }
            }

            return prevButton;
        }

        private void displayPlayer2()
        {
            if (gameSettingsForm.CheckBoxPlayer2.Checked)
            {
                labelPlayer2.Text = gameSettingsForm.Player2Name + ": " + board.ScoreO.ToString();
            }
            else
            {
                labelPlayer2.Text = "Computer: " + board.ScoreO.ToString();
            }
        }

        private Position findPositionOfButton(Button i_Button)
        {
            Position pressedPos = null;

            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    if (buttonsBoard[i, j].Equals(i_Button))
                    {
                        pressedPos = board.BoardArr[i, j];    
                    }
                }
            }

            return pressedPos;
        }

        private void updateLocationOfEatenButton(Button i_CurrentButton, Button i_DestButton)
        {
            int currRow = 0, currCol = 0, destRow = 0, destCol = 0;

            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    if (buttonsBoard[i, j].Equals(i_CurrentButton))
                    {
                        currRow = i;
                        currCol = j;
                    }
                    if (buttonsBoard[i, j].Equals(i_DestButton))
                    {
                        destRow = i;
                        destCol = j;
                    }
                }
            }

            buttonsBoard[(currRow + destRow) / 2, (currCol + destCol) / 2].Text = string.Empty;
        }

        private int doNextStep()
        {
            int nextStepStatus = 0;
            bool validStep = true;

            if (gameSettingsForm.CheckBoxPlayer2.Checked)
            {
                if (GameManager.ChangeTurn(ref turnChange))
                {
                    nextStepStatus = GameManager.MakeStep(fromMove, nextMove, board, player1);
                }
                else
                {
                    nextStepStatus = GameManager.MakeStep(fromMove, nextMove, board, player2);
                }

                GameManager.ChangeTurn(ref turnChange);
                if (nextStepStatus == 0 || !GameManager.PlayWitnYourChecker(fromMove, turnChange, board))
                {
                    MessageBox.Show("Invalid move. Please try again", "Invalid move",
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    validStep = !validStep;
                }

                GameManager.ChangeTurn(ref turnChange);
                if (validStep)
                {
                    updateButtonsBoard(nextStepStatus);
                }

                gameOver();
            }
            else if (!gameSettingsForm.CheckBoxPlayer2.Checked)
            {
                if (GameManager.ChangeTurn(ref turnChange))
                {
                    nextStepStatus = GameManager.MakeStep(fromMove, nextMove, board, player1);
                }

                GameManager.ChangeTurn(ref turnChange);
                if (nextStepStatus == 0 || !GameManager.PlayWitnYourChecker(fromMove, turnChange, board))
                {
                    MessageBox.Show("Invalid move. Please try again", "Invalid move",
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    validStep = !validStep;
                }

                if (validStep)
                {
                    updateButtonsBoard(nextStepStatus);
                }
                gameOver();
                Position[] computerMove = GameManager.ComputerMove(board);
                fromMove = computerMove[0];
                nextMove = computerMove[1];
                nextStepStatus = GameManager.MakeStep(computerMove[0],computerMove[1], board, player2);
                updateButtonsBoard(nextStepStatus);
                gameOver();
            }

            return nextStepStatus;
        }

        private void updateButtonsBoard(int i_NextStepStatus)
        {
            Button buttonStart = buttonsBoard[fromMove.Row, fromMove.Col];
            Button buttonDest = buttonsBoard[nextMove.Row, nextMove.Col];

            if (i_NextStepStatus == 1)
            {
                buttonStart.BackColor = Color.White;
                updateButtonsText(buttonStart, buttonDest);
                buttonDest.BackColor = Color.White;
            }
            else if (i_NextStepStatus == 2)
            {
                buttonStart.BackColor = Color.White;
                updateButtonsText(buttonStart, buttonDest);
                buttonDest.BackColor = Color.White;
                updateLocationOfEatenButton(buttonStart, buttonDest);
                labelPlayer1.Text = gameSettingsForm.Player1Name + ": " + board.ScoreX.ToString();
                displayPlayer2();
            }

            nextMove = null;
            fromMove = null;
        }

        private void gameOver()
        {
            string winner;

            if (GameManager.IfStuck(player1, board) && GameManager.IfStuck(player2, board))
            {
                if (MessageBox.Show(String.Format(@"Tie!
AnotherRound?"), "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    resetGame();
                }
                else
                {
                    this.Close();
                }
            }
            else if ((winner = board.Winner()) != null)
            {
                if (MessageBox.Show(String.Format(@"Player {0} Won!
AnotherRound?", winner), "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    resetGame();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void resetGame()
        {
            board = initBoard();
            resetBoard();
        }

        private void resetBoard()
        {
            int i = 0, j = 0; 

            for (i = 0; i < board.Size; i++)
            {
                for (j = 0; j < board.Size; j++)
                {
                    if ((j % 2) == (i % 2))
                    {
                        buttonsBoard[i, j].Enabled = false;
                        buttonsBoard[i, j].BackColor = Color.Gray;
                    }

                    if (buttonsBoard[i, j].Text != string.Empty)
                    {
                        buttonsBoard[i, j].Text = string.Empty;
                    }
                }
            }

            for (i = board.Size - 1; i > board.Size / 2; i--)
            {
                for (j = 0; j < board.Size; j++)
                {
                    if ((j % 2) == ((i + 1) % 2))
                    {
                        buttonsBoard[i, j].Text = "X";
                    }
                    else
                    {
                        buttonsBoard[i, j].Text = string.Empty;
                    }
                }
            }

            for (i = 0; i < (board.Size / 2) - 1; i++)
            {
                for (j = board.Size - 1; j >= 0; j--)
                {
                    if ((j % 2) == ((i + 1) % 2))
                    {
                        buttonsBoard[i, j].Text = "O";
                    }
                    else
                    {
                        buttonsBoard[i, j].Text = string.Empty;
                    }
                }
            }
        }

        private void DamkaForm_Load(object sender, EventArgs e)
        {

        }

        private void updateButtonsText(Button i_CurrButton, Button i_DestButton)
        {
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    if (buttonsBoard[i, j].Equals(i_DestButton))
                    {
                        if (board.BoardArr[i, j].IsKing && board.BoardArr[i, j].XorO.Equals(eXorO.X))
                        {
                            i_DestButton.Text = "Z";
                        }
                        else if (board.BoardArr[i, j].IsKing && board.BoardArr[i, j].XorO.Equals(eXorO.O))
                        {
                            i_DestButton.Text = "Q";
                        }
                        else
                        {
                            i_DestButton.Text = i_CurrButton.Text;
                        }

                        i_CurrButton.Text = string.Empty;
                    }
                }
            }
        }
    }
}      
    

