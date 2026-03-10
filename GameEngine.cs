using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckerZ
{
    public partial class GameEngine : Form
    {
        private Bitmap bmp = null;

        private readonly GameData gameData;
        private readonly GameLogic gameLogic;
        private readonly PlayerController playerController;
        private readonly ComputerController computerController;

        Timer computerTimer = new Timer();

        private enum Direction { Upright, Upleft, Downright, Downleft }

        private Direction animationDirection;

        private bool runAnimation = false;

        private enum GameState { PlayerTurn, ComputerTurn, Idle }

        private GameState currentState = GameState.Idle;
        private Piece selectedPiece = null;
        BoardGrid grid = new BoardGrid();
        int countDownTimer = 10;


        // intialzing the game

        public GameEngine()
        {
            InitializeComponent();
            gameData = new GameData();
            gameLogic = new GameLogic(gameData);
            playerController = new PlayerController(gameData, gameLogic);
            computerController = new ComputerController(gameData, gameLogic);
            this.DoubleBuffered = true;
        }

        // --- ALL UI EVENTS STAY HERE ---

        //Displaying the game grid to the screen 
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            grid.DrawGrid(this.CreateGraphics());
        }
        //constantly displaying the pieces on board

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            if (bmp != null)
            {
                e.Graphics.DrawImage(bmp,this.Width,this.Height);
            }

            foreach (var piece in gameData.Board)
            {
                if (piece != null)
                {
                    piece.Draw(e.Graphics);
                }
            }
        }


        // clicking mouse on screen
        private void Matrix_MouseClick(object sender, MouseEventArgs e)
        {
            if (currentState == GameState.Idle)
            {
                MessageBox.Show("Press the start game button");
                return;
            }
            // Get the coordinates relative to the formsPlot1 control
            int x = e.X;
            int y = e.Y;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (gameData.Board[i, j] != null)
                    {
                        Piece p = gameData.Board[i, j];
                        if (x >= p.X && x <= p.X + Piece.SIZE && y >= p.Y && y <= p.Y + Piece.SIZE)
                        {
                            selectedPiece = p;

                            // check if player select his piece

                            if (!selectedPiece.IsPlayer)
                            {
                                MessageBox.Show("Select player piece only!");
                                selectedPiece = null;
                            }
                            return;
                        }
                    }
                }
            }
        }

        // game starts after choosing time for player and pressing the startgame button
        private void startgame_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Game Starts!");
            computerTimer.Interval = 500; // 1 second delay
            computerTimer.Tick += ComputerTimer_Tick;

            this.DoubleBuffered = true;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                  ControlStyles.UserPaint |
                  ControlStyles.OptimizedDoubleBuffer, true);
            countdownTimer.Start();
            comboBox1.Visible = false;
            startgame.Dispose();
            currentState = GameState.PlayerTurn;
        }

        // logic for combo box options
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerlabel.Text = comboBox1.SelectedIndex.ToString();
            if (int.TryParse(comboBox1.Text, out int result))
            {
                countDownTimer = result;
                timerlabel.Text = countDownTimer + "";
            }
        }

        // Timers and Blinking stay here because they control the screen!
        private void countdownTimer_Tick(object sender, EventArgs e)
        {
            if (countDownTimer > 0)
            {
                countDownTimer -= 1;
                timerlabel.Text = countDownTimer + "";
                if (countDownTimer <= 5)
                    timerlabel.ForeColor = Color.Red;
                else
                    timerlabel.ForeColor = Color.Black;
            }
            else
            {
                countdownTimer.Stop();
                MessageBox.Show("time is up!");
                ComputerWin();
            }
        }

        // victory celebration for the winning side
        private async Task blinkingPieces(List<BoardLocation> locations)
        {
            int countDown = 5;
            while (countDown > 0)
            {
                for (int i = 0; i < locations.Count; i++)
                {
                    int targetRow = locations[i].Row;
                    int targetCol = locations[i].Col;
                    gameData.Board[targetRow, targetCol].PieceColor = Color.LightGreen;
                }
                this.Refresh();
                await Task.Delay(100);
                for (int i = 0; i < locations.Count; i++)
                {
                    int targetRow = locations[i].Row;
                    int targetCol = locations[i].Col;
                    gameData.Board[targetRow, targetCol].PieceColor = Color.DarkGreen;
                }
                this.Refresh();

                await Task.Delay(100);
                countDown--;

            }
        }

        //event for computer win
        private async void ComputerWin()
        {
            countdownTimer.Stop();
            await blinkingPieces(gameData.computerLocations);
            MessageBox.Show("Computer Won !");
            MessageBox.Show("Thank you for playing my game!");
            this.Close();
        }
        //event for player win
        private async void PlayerWin()
        {
            countdownTimer.Stop();
            await blinkingPieces(gameData.playerLocations);
            MessageBox.Show("Player Won !");
            MessageBox.Show("Thank you for playing my game!");
            this.Close();
        }



        //State machine that handles computer and player taking turn while playing
        private async void ComputerTimer_Tick(object sender, EventArgs e)
        {
            computerTimer.Stop(); // Stop so it only runs once per turn

            if (gameLogic.CheckWin() || !computerController.ExecuteComputerMove())
            {
                PlayerWin();
                return;
            }
            else
            {
                countdownTimer.Stop();
                countDownTimer = Convert.ToInt32(comboBox1.Text);
                currentState = GameState.ComputerTurn;
            }

            // After computer is done, give control back to player
            currentState = GameState.PlayerTurn;

            if (gameLogic.CheckLose())
            {
                ComputerWin();
                return;
            }
            countdownTimer.Start();
            this.Refresh();

        }


        //right button

        private void RightButtonClick(object sender, EventArgs e)
        {
            if (currentState == GameState.Idle)
            {
                MessageBox.Show("Press the start game button");
                return;
            }
            if (selectedPiece == null)
            {
                MessageBox.Show("Piece not selected");
                return;
            }
            int targetCol = selectedPiece.ColIndex;
            int targetRow = selectedPiece.RowIndex;
            Piece targetPiece = gameData.Board[targetRow, targetCol];
            int locationIndex = 0;
            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                if (gameData.playerLocations[i].Row == targetRow && gameData.playerLocations[i].Col == targetCol)
                    locationIndex = i;
            }
            if (playerController.AttemptMoveRight(targetPiece, locationIndex))
            {
                selectedPiece = null;
                this.Refresh();
                currentState = GameState.ComputerTurn;
                computerTimer.Start();
                return;
            }
            else
            {
                MessageBox.Show("Cant Move in this direction. Choose another direction or select another piece");
                selectedPiece = null;
            }
        }

        //left button
        private void LeftButtonClick(object sender, EventArgs e)
        {
            if (currentState == GameState.Idle)
            {
                MessageBox.Show("Press the start game button");
                return;
            }
            if (selectedPiece == null)
            {
                MessageBox.Show("Piece not selected");
                return;
            }

            int targetCol = selectedPiece.ColIndex;
            int targetRow = selectedPiece.RowIndex;
            Piece targetPiece = gameData.Board[targetRow, targetCol];
            int locationIndex = 0;
            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                if (gameData.playerLocations[i].Row == targetRow && gameData.playerLocations[i].Col == targetCol)
                    locationIndex = i;
            }
            if (playerController.AttemptMoveLeft(targetPiece, locationIndex))
            {
                selectedPiece = null;
                this.Refresh();
                currentState = GameState.ComputerTurn;
                computerTimer.Start();
                return;
            }
            else
            {
                MessageBox.Show("Cant Move in this direction. Choose another direction or select another piece");
                selectedPiece = null;
            }
        }

        //reverseright button
        private void ReverseRightClick(object sender, EventArgs e)
        {
            if (currentState == GameState.Idle)
            {
                MessageBox.Show("Press the start game button");
                return;
            }
            if (selectedPiece == null)
            {
                MessageBox.Show("Piece not selected");
                return;
            }
            if (selectedPiece.Reversed)
            {
                MessageBox.Show("Can only reverse a piece once. Select another direction or move another piece");
                selectedPiece = null;
                return;
            }

            int targetCol = selectedPiece.ColIndex;
            int targetRow = selectedPiece.RowIndex;
            Piece targetPiece = gameData.Board[targetRow, targetCol];
            int locationIndex = 0;
            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                if (gameData.playerLocations[i].Row == targetRow && gameData.playerLocations[i].Col == targetCol)
                    locationIndex = i;
            }
            if (playerController.TryMoveDownRight(gameData.playerLocations, locationIndex, targetRow, targetCol, targetPiece))
            {
                selectedPiece = null;
                this.Refresh();
                currentState = GameState.ComputerTurn;
                computerTimer.Start();
                return;
            }
            else
            {
                MessageBox.Show("Cant Move in this direction. Choose another direction or select another piece");
                selectedPiece = null;
            }
        }

        //reverseleft button
        private void ReverseLeftClick(object sender, EventArgs e)
        {
            if (currentState == GameState.Idle)
            {
                MessageBox.Show("Press the start game button");
                return;
            }
            if (selectedPiece == null)
            {
                MessageBox.Show("Piece not selected");
                return;
            }
            if (selectedPiece.Reversed)
            {
                MessageBox.Show("Can only reverse a piece once. Select another direction or move another piece");
                selectedPiece = null;
                return;
            }

            int targetCol = selectedPiece.ColIndex;
            int targetRow = selectedPiece.RowIndex;
            Piece targetPiece = gameData.Board[targetRow, targetCol];
            int locationIndex = 0;

            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                if (gameData.playerLocations[i].Row == targetRow && gameData.playerLocations[i].Col == targetCol)
                    locationIndex = i;
            }

            if (playerController.TryMoveDownLeft(gameData.playerLocations, locationIndex, targetRow, targetCol, targetPiece))
            {
                selectedPiece = null;
                this.Refresh();
                currentState = GameState.ComputerTurn;
                computerTimer.Start();
                return;
            }
            else
            {
                MessageBox.Show("Cant Move in this direction. Choose another direction or select another piece");
                selectedPiece = null;
            }
        }

        private void GameEngine_Load(object sender, EventArgs e)
        {
            bmp = new Bitmap(Width, Height);

        }

        private void animationTimer_Tick(object sender, EventArgs e)
        {
        //    if (runAnimation)
        //    {
        //        int animationCountDown = 60;
        //        switch (animationDirection)
        //        {
        //            case Direction.Upright:
        //                while()
        //                {
        //                    selectedPiece.MoveUpRight(false);
        //                    selectedPiece = null;
        //                    this.Invalidate();
        //                    animationCountDown--;
        //                }
        //                break;
        //            case Direction.Upleft:
        //                selectedPiece.MoveUpLeft(false);
        //                selectedPiece = null;
        //                this.Invalidate();
        //                break;
        //            case Direction.Downright:
        //                selectedPiece.MoveDownRight(false);
        //                selectedPiece = null;
        //                this.Invalidate();
        //                break;
        //            case Direction.Downleft:
        //                selectedPiece.MoveDownLeft(false);
        //                selectedPiece = null;
        //                this.Invalidate();
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        }
    }
}
