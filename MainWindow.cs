using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace O_Neillo
{
    public partial class MainWindow : Form
    {
        private string[] names = new string[] { "", "" };
        private readonly GameState gameState;
        private readonly Button[] buttons;
        private readonly EventHandler buttonEventHandler;
        private readonly AboutBox about = new AboutBox();

        public MainWindow()
        {
            InitializeComponent();

            buttons = new Button[64] {
                button1, button2, button3, button4, button5, button6, button7, button8,
                button9, button10, button11, button12, button13, button14, button15, button16,
                button17, button18, button19, button20, button21, button22, button23, button24,
                button25, button26, button27, button28, button29, button30, button31, button32,
                button33, button34, button35, button36, button37, button38, button39, button40,
                button41, button42, button43, button44, button45, button46, button47, button48,
                button49, button50, button51, button52, button53, button54, button55, button56,
                button57, button58, button59, button60, button61, button62, button63, button64
            };
            gameState = new GameState();

            buttonEventHandler = new EventHandler(GridClick);
        }

        private void AboutClick(object sender, EventArgs e)
        {
            about.Show();
        }

        private void ConfigureButtons()
        {
            foreach (var button in buttons)
            {
                button.Enabled = false;
                button.Click -= buttonEventHandler;
            }

            foreach (var button in buttons)
            {
                button.Enabled = true;
                button.Click += buttonEventHandler;
            }
        }

        private void StartClick(object sender, EventArgs e)
        {
            if (player1Name.TextLength + player1Name.TextLength == 0)
            {
                MessageBox.Show("You must provide a name for player 1 and player 2.");
                return;
            }

            names = new string[2] { player1Name.Text, player2Name.Text };

            ConfigureButtons();

            gameState.InitBoard();
            gameState.Turn = 1;
            PaintBoard(gameState.Board);
            SetTurn();
        }

        private void GridClick(object sender, EventArgs e)
        {
            var button = (sender as Button);
            var index = int.Parse(button.Name.Replace("button", "")) - 1;
            var canMove = gameState.ValidateMove(index);

            if (!canMove)
            {
                MessageBox.Show("Invalid move!");
                return;
            }
            
            gameState.MakeMove(index);

            PaintBoard(gameState.Board);
        }

        private void SetTurn()
        {
            var turnIndicators = new List<Label> { null, player1Next, player2Next };
            foreach (var indic in turnIndicators)
            {
                if (indic == null) continue;
                indic.Visible = false;
            }

            turnIndicators[gameState.Turn].Visible = true;
        }

        private void PaintBoard(int[] layout)
        {
            var images = new List<Bitmap> { null, Properties.Resources.black_2x, Properties.Resources.white_2x };

            var nextBadges = new List<Label> { null, player1Next, player2Next };
            nextBadges[gameState.Turn].Visible = true;
            nextBadges[gameState.Turn == 1 ? 2 : 1].Visible = false;

            // Reset the board
            for (var x = 0; x < 64; x += 8)
            {
                for (var y = 0; y < 8; y++)
                {
                    buttons[x + y].BackgroundImageLayout = ImageLayout.Stretch;
                    buttons[x + y].BackgroundImage = images[0];
                    buttons[x + y].Enabled = false;
                }
            }
            
            // Paint the board
            for (var x = 0; x < 64; x += 8)
            {
                for (var y = 0; y < 8; y++)
                {
                    if (layout[x + y] != 0)
                    {
                        buttons[x + y].BackgroundImageLayout = ImageLayout.Stretch;
                        buttons[x + y].BackgroundImage = images[layout[x + y]];
                        buttons[x + y].Enabled = false;
                    } else
                    {
                        buttons[x + y].Enabled = true;
                    }
                }
            }

            // Set the player names
            player1Name.Text = names[0];
            player1Name.Enabled = false;
            player2Name.Text = names[1];
            player2Name.Enabled = false;

            // Set the counters
            int p1 = (from counter in gameState.Board where counter == 1 select counter).Count();
            int p2 = (from counter in gameState.Board where counter == 2 select counter).Count();

            player1Tokens.Text = $"{p1}x";
            player2Tokens.Text = $"{p2}x";
        }

        private void ExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SaveGameClick(object sender, EventArgs e)
        {
            var data = SerializeSave(gameState.Board, gameState.Turn, names[0], names[1]);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Save files (*.sav)|*.sav";
            dialog.Title = "Save Game...";
            dialog.ShowDialog();

            if (dialog.FileName != "")
            {
                System.IO.FileStream fs = dialog.OpenFile() as System.IO.FileStream;

                var bytes = System.Text.Encoding.UTF8.GetBytes(data);

                fs.Write(bytes, 0, bytes.Length);

                fs.Close();
            }
        }

        private void LoadGameClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Save files(*.sav)|*.sav";
            dialog.Title = "Load Game...";
            dialog.ShowDialog();

            if (dialog.FileName != "")
            {
                System.IO.FileStream fs = dialog.OpenFile() as System.IO.FileStream;

                byte[] buffer = new byte[fs.Length];
                int cur;

                string sav = "";

                while ((cur = fs.Read(buffer, 0, (int)fs.Length)) > 0)
                {
                    var txt = System.Text.Encoding.UTF8.GetString(buffer, 0, cur);
                    sav += txt;
                }

                var data = sav.Split(';');
                names[0] = data[0] ?? "!!ERROR!!";
                names[1] = data[1] ?? "!!ERROR!!";

                var turn = data[2];

                var boardStr = data[3];
                int[] boardData = (boardStr ?? "").Split(',').Select(int.Parse).ToArray<int>();

                gameState.Board = boardData;
                gameState.Turn = int.Parse(turn);
                ConfigureButtons();
                PaintBoard(gameState.Board);
                SetTurn();
            }
        }

        public string SerializeSave(int[] board, int turn, string player1, string player2)
        {
            return $"{player1};{player2};{turn};{string.Join(",", board)}";
        }
    }
}
