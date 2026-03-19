using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckerZ
{
    public partial class GameEngine : Form
    {
        private ReplayDataDataContext DB = new ReplayDataDataContext();

        private int countDownTimer;

        private Piece selectedPiece;
        private Bitmap bmp;
        private BoardGrid grid;
        private Timer computerTimer;

        private enum GameState { PlayerTurn, ComputerTurn, Idle }
        private GameState currentState;

        private readonly Painter painter;
        private readonly GameData gameData;
        private readonly GameLogic gameLogic;
        private readonly PlayerController playerController;
        private readonly ComputerController computerController;

        // intialzing the game
        public GameEngine()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            countDownTimer = 10;
            selectedPiece = null;
            bmp = new Bitmap(Width, Height);
            grid = new BoardGrid();
            computerTimer = new Timer();
            currentState = GameState.Idle;

            painter = new Painter(this);

            gameData = new GameData();
            gameLogic = new GameLogic(gameData);
            playerController = new PlayerController(gameData, gameLogic);
            computerController = new ComputerController(gameData, gameLogic);

        }

        // --- ALL UI EVENTS STAY HERE ---

        //Displaying the game grid to the screen 

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // 1. Make the graphics butter-smooth
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 2. Draw the board
            grid.DrawGrid(e.Graphics);

            Piece movingPiece = null;

            // 3. Draw all stationary pieces first
            foreach (var piece in gameData.Board)
            {
                if (piece != null)
                {
                    // Calculate where this piece is SUPPOSED to be based on its matrix index
                    int expectedX = Piece.INITIALX + (piece.ColIndex * Piece.MOVEOFFSET);
                    int expectedY = Piece.INITIALY + (piece.RowIndex * Piece.MOVEOFFSET);

                    // If the piece's actual X/Y doesn't match its expected X/Y, it is currently animating!
                    if (piece.X != expectedX || piece.Y != expectedY)
                    {
                        movingPiece = piece; // Save it for later
                    }
                    else
                    {
                        piece.Draw(e.Graphics); // Draw normal pieces immediately
                    }
                }
            }

            // 4. Draw the moving piece LAST so it perfectly hovers over the board
            if (movingPiece != null)
            {
                movingPiece.Draw(e.Graphics);
            }

            e.Graphics.DrawImage(painter.Canvas, 0, 0);
        }


        private async Task AnimatePieceAsync(Piece pieceToMove, int targetRow, int targetCol)
        {
            // 1. Where are we starting? (Current X, Y)
            int startX = pieceToMove.X;
            int startY = pieceToMove.Y;

            // 2. Where are we going? (Calculate final pixels based on target matrix coordinates)
            int endX = Piece.INITIALX + (targetCol * Piece.MOVEOFFSET);
            int endY = Piece.INITIALY + (targetRow * Piece.MOVEOFFSET);

            int frames = 5; // Number of animation steps

            for (int i = 1; i <= frames; i++)
            {
                // 1. Move the math coordinates
                pieceToMove.X = startX + ((endX - startX) * i / frames);
                pieceToMove.Y = startY + ((endY - startY) * i / frames);

                // 2. Queue the paint request
                this.Invalidate();

                // 3. THE FIX: Force Windows to paint the frame RIGHT NOW
                this.Update();

                // 4. Pause for the next frame
                await Task.Delay(6);
            }

            // Ensure final snap and final paint
            pieceToMove.X = endX;
            pieceToMove.Y = endY;
            this.Invalidate();
            this.Update();
        }

        // clicking mouse on screen
        private void Matrix_MouseClick(object sender, MouseEventArgs e)
        {
            if (painter.Drawing)
            {
                return;
            }
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

        private void startgame_Click(object sender, EventArgs e)
        {
            if (painter.Drawing)
            {
                MessageBox.Show("Turn off drawing mode to start game!");
                return;
            }
            MessageBox.Show("Game Starts!");
            computerTimer.Interval = 500; // 1 second delay
            computerTimer.Tick += ComputerTimer_Tick;

            this.DoubleBuffered = true;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                  ControlStyles.UserPaint |
                  ControlStyles.OptimizedDoubleBuffer, true);
            countdownTimer.Start();
            comboBox1.Visible = false;
            GameIcon.Enabled = false;
            currentState = GameState.PlayerTurn;

            GameTable gameTable =new GameTable {PlayerName =  "Ofek",GameDate =  DateTime.Now, GameOutcome =  null, EndCondition = null };
            DB.GameTables.InsertOnSubmit(gameTable);
            DB.SubmitChanges();

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
            Piece movedPiece = null;
            int startX = 0;
            int startY = 0;
            // Check if player won before computer moves
            if (gameLogic.CheckWin())
            {
                PlayerWin();
                return;
            }

            // Execute the move and capture the outputs!
            if (computerController.ExecuteComputerMove(out movedPiece,out startX, out startY))
            {
                // --- PREPARE FOR ANIMATION ---
                // Force the visual coordinates back to the start so it can glide
                movedPiece.X = startX;
                movedPiece.Y = startY;

                // Run the smooth animation using the new logical coordinates
                await AnimatePieceAsync(movedPiece, movedPiece.RowIndex, movedPiece.ColIndex);
            }
            else
            {
                // If ExecuteComputerMove returns false, computer has no legal moves.
                PlayerWin();
                return;
            }

            // After computer is done animating, setup player's turn
            countdownTimer.Stop();
            countDownTimer = Convert.ToInt32(comboBox1.Text);
            currentState = GameState.PlayerTurn;

            if (gameLogic.CheckLose())
            {
                ComputerWin();
                return;
            }

            countdownTimer.Start();
            this.Refresh();
        }

        //private void SaveTurn()

        //right button
        private async void RightButtonClick(object sender, EventArgs e)
        {
            if (painter.Drawing)
            {
                return;
            }
            if (currentState == GameState.Idle) { MessageBox.Show("Press start"); return; }
            if (selectedPiece == null) { MessageBox.Show("Piece not selected"); return; }

            int targetCol = selectedPiece.ColIndex;
            int targetRow = selectedPiece.RowIndex;
            Piece targetPiece = selectedPiece;

            // --- GRAB STARTING PIXELS BEFORE THE LOGIC CHANGES THEM ---
            int startX = targetPiece.X;
            int startY = targetPiece.Y;

            int locationIndex = 0;
            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                if (gameData.playerLocations[i].Row == targetRow && gameData.playerLocations[i].Col == targetCol)
                    locationIndex = i;
            }

            // Update the matrix and run the logic!
            if (playerController.AttemptMoveRight(targetPiece, locationIndex))
            {

                // --- PREPARE FOR ANIMATION ---
                // Since playerController changed pieceToAnimate.X, we force it back to the start
                targetPiece.X = startX;
                targetPiece.Y = startY;

                // Run the smooth animation using the new logical coordinates
                await AnimatePieceAsync(targetPiece, targetPiece.RowIndex, targetPiece.ColIndex);

                selectedPiece = null;
                currentState = GameState.ComputerTurn;
                computerTimer.Start();
                return;
            }
            else
            {
                MessageBox.Show("Cant Move in this direction.");
                selectedPiece = null;
            }
        }

        //left button
        private async void LeftButtonClick(object sender, EventArgs e)
        {
            if (painter.Drawing)
            {
                return;
            }
            if (currentState == GameState.Idle) { MessageBox.Show("Press start"); return; }
            if (selectedPiece == null) { MessageBox.Show("Piece not selected"); return; }

            int targetCol = selectedPiece.ColIndex;
            int targetRow = selectedPiece.RowIndex;
            Piece targetPiece = selectedPiece;

            // --- GRAB STARTING PIXELS BEFORE THE LOGIC CHANGES THEM ---
            int startX = targetPiece.X;
            int startY = targetPiece.Y;

            int locationIndex = 0;
            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                if (gameData.playerLocations[i].Row == targetRow && gameData.playerLocations[i].Col == targetCol)
                    locationIndex = i;
            }

            // Update the matrix and run the logic!
            if (playerController.AttemptMoveLeft(targetPiece, locationIndex))
            {

                // --- PREPARE FOR ANIMATION ---
                // Since playerController changed pieceToAnimate.X, we force it back to the start
                targetPiece.X = startX;
                targetPiece.Y = startY;

                // Run the smooth animation using the new logical coordinates
                await AnimatePieceAsync(targetPiece, targetPiece.RowIndex, targetPiece.ColIndex);

                selectedPiece = null;
                currentState = GameState.ComputerTurn;
                computerTimer.Start();
                return;
            }
            else
            {
                MessageBox.Show("Cant Move in this direction.");
                selectedPiece = null;
            }
        }

        private async void ReverseRightClick(object sender, EventArgs e)
        {
            if (painter.Drawing)
            {
                return;
            }
            if (currentState == GameState.Idle) { MessageBox.Show("Press start"); return; }
            if (selectedPiece == null) { MessageBox.Show("Piece not selected"); return; }

            int targetCol = selectedPiece.ColIndex;
            int targetRow = selectedPiece.RowIndex;
            Piece targetPiece = selectedPiece;

            if (targetPiece.Reversed)
            {
                MessageBox.Show("Already reversed. move in another direction or select another piece");
                return;
            }

            // --- GRAB STARTING PIXELS BEFORE THE LOGIC CHANGES THEM ---
            int startX = targetPiece.X;
            int startY = targetPiece.Y;

            int locationIndex = 0;
            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                if (gameData.playerLocations[i].Row == targetRow && gameData.playerLocations[i].Col == targetCol)
                    locationIndex = i;
            }

            // Update the matrix and run the logic!
            if (playerController.TryMoveDownRight(gameData.playerLocations, locationIndex, targetRow, targetCol, targetPiece))
            {

                // --- PREPARE FOR ANIMATION ---
                // Since playerController changed pieceToAnimate.X, we force it back to the start
                targetPiece.X = startX;
                targetPiece.Y = startY;

                // Run the smooth animation using the new logical coordinates
                await AnimatePieceAsync(targetPiece, targetPiece.RowIndex, targetPiece.ColIndex);

                selectedPiece = null;
                currentState = GameState.ComputerTurn;
                computerTimer.Start();
                return;
            }
            else
            {
                MessageBox.Show("Cant Move in this direction.");
                selectedPiece = null;
            }
        }

        private async void ReverseLeftClick(object sender, EventArgs e)
        {
            if (painter.Drawing)
            {
                return;
            }
            if (currentState == GameState.Idle) { MessageBox.Show("Press start"); return; }
            if (selectedPiece == null) { MessageBox.Show("Piece not selected"); return; }

            int targetCol = selectedPiece.ColIndex;
            int targetRow = selectedPiece.RowIndex;
            Piece targetPiece = selectedPiece;

            if (targetPiece.Reversed)
            {
                MessageBox.Show("Already reversed. move in another direction or select another piece");
                return;
            }

            // --- GRAB STARTING PIXELS BEFORE THE LOGIC CHANGES THEM ---
            int startX = targetPiece.X;
            int startY = targetPiece.Y;

            int locationIndex = 0;
            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                if (gameData.playerLocations[i].Row == targetRow && gameData.playerLocations[i].Col == targetCol)
                    locationIndex = i; 
            }

            // Update the matrix and run the logic!
            if (playerController.TryMoveDownLeft(gameData.playerLocations, locationIndex, targetRow, targetCol, targetPiece))
            {

                // --- PREPARE FOR ANIMATION ---
                // Since playerController changed pieceToAnimate.X, we force it back to the start
                targetPiece.X = startX;
                targetPiece.Y = startY;

                // Run the smooth animation using the new logical coordinates
                await AnimatePieceAsync(targetPiece, targetPiece.RowIndex, targetPiece.ColIndex);

                selectedPiece = null;
                currentState = GameState.ComputerTurn;
                computerTimer.Start();
                return;
            }
            else
            {
                MessageBox.Show("Cant Move in this direction.");
                selectedPiece = null;
            }
        }
        private void DrawOnScreen_Click(object sender, EventArgs e)
        {
            if (!painter.Drawing)
            {
                painter.InitializePen();
                computerTimer.Stop();
                countdownTimer.Stop();
                MessageBox.Show("Drawing mode");
            }
            else
            {
                painter.Drawing = false;
                MessageBox.Show("Drawing mode disabled");
                if (currentState == GameState.ComputerTurn)
                {
                    computerTimer.Start();
                }
                else
                {
                    if (currentState != GameState.Idle)
                    {
                        countdownTimer.Start();
                    }
                }
            }
        }
        private void ClearDraws_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(painter.Canvas);
            g.Clear(Color.Transparent);
            this.Refresh();
        }

        private void GameEngine_MouseMove(object sender, MouseEventArgs e)
        {
            if (painter.Drawing && e.Button == MouseButtons.Left)
            {
                painter.DrawOnScreen(this,e);
            }
        }

        private void GameEngine_MouseDown(object sender, MouseEventArgs e)
        {
            if (painter.Drawing && e.Button == MouseButtons.Left)
            {
                painter.PenX = e.X; painter.PenY = e.Y;
            }
        }

        private void GameEngine_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (painter.Canvas != null)
            {
                painter.Canvas.Dispose();
                painter.Canvas = null;
            }
            if(bmp != null)
            {
                bmp.Dispose();
                painter.Canvas = null;
            }
            countdownTimer.Dispose();
            computerTimer.Dispose();
            animationTimer.Dispose();
            if(painter.Pen != null)
                painter.Pen.Dispose();
        }
    }
}
