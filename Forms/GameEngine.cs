using CheckerZ.Data.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        private GameData gameData;
        private GameLogic gameLogic;
        private PlayerController playerController;
        private ComputerController computerController;

        private MoveSnapshot snapshot;
        private int currentGameID;

        private Stopwatch stopwatch = new Stopwatch();

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
        private void ResetGame()
        {
            this.DoubleBuffered = true;

            comboBox1.Visible = true;
            GameIcon.Enabled = true;
            timerlabel.ForeColor = Color.Black;

            countDownTimer = 10;
            timerlabel.Text = Convert.ToString(countDownTimer);

            selectedPiece = null;
            bmp = new Bitmap(Width, Height);
            computerTimer = new Timer();
            currentState = GameState.Idle;

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
            using (SelectPlayer selectPlayer = new SelectPlayer())
            {
                if (selectPlayer.ShowDialog() == DialogResult.OK)
                {
                    GameTable currentGame = new GameTable { PlayerID = selectPlayer.selectedID, PlayerName = selectPlayer.selectedName, GameDate = DateTime.Now, GameOutcome = null, EndCondition = null };
                    DB.GameTables.InsertOnSubmit(currentGame);
                    DB.SubmitChanges();
                    currentGameID = currentGame.GameID;
                    snapshot = new MoveSnapshot(currentGameID, 1, gameData.playerLocations, gameData.computerLocations, 0, 0, 0, 0);
                    SaveMove();

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
                    stopwatch.Start();
                }
            }
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
                string endCondition = EndCondition.TimerRanOut.ToString();
                string outcome = GameOutcome.Loss.ToString();
                SaveGame(outcome, endCondition);
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
            stopwatch.Stop();
            await blinkingPieces(gameData.computerLocations);
            MessageBox.Show("Computer Won !");
            MessageBox.Show("Thank you for playing my game!");
            ResetGame();
            this.Refresh();
        }

        //event for player win
        private async void PlayerWin()
        {
            countdownTimer.Stop();
            stopwatch.Stop();
            await blinkingPieces(gameData.playerLocations);
            MessageBox.Show("Player Won !");
            MessageBox.Show("Thank you for playing my game!");
            ResetGame();
            this.Refresh();
        }

        //State machine that handles computer and player taking turn while playing
        private async void ComputerTimer_Tick(object sender, EventArgs e)
        {
            computerTimer.Stop(); // Stop so it only runs once per turn
            Piece movedPiece = null;
            string outcome;
            string endCondition;
            // Check if player won before computer moves
            if (gameLogic.CheckWin(out outcome, out endCondition))
            {
                PlayerWin();
                SaveGame(outcome, endCondition);
                return;
            }
            int startX = 0;
            int startY = 0;
            int startRow;
            int startCol;

            // Try to execute computer move 
            if (computerController.ExecuteComputerMove(out movedPiece, out startX, out startY, out startRow, out startCol))
            {
                // --- PREPARE FOR ANIMATION ---
                // Force the visual coordinates back to the start so it can glide
                movedPiece.X = startX;
                movedPiece.Y = startY;

                // Run the smooth animation using the new logical coordinates
                await AnimatePieceAsync(movedPiece, movedPiece.RowIndex, movedPiece.ColIndex);

                snapshot.UpdateSnapshot(gameData.playerLocations, gameData.computerLocations, startRow, startCol, movedPiece.RowIndex, movedPiece.ColIndex);
                SaveMove();
            }
            // If ExecuteComputerMove returns false, computer has no legal moves.
            else
            {
                outcome = GameOutcome.Win.ToString();
                endCondition = EndCondition.ComputerBlocked.ToString();
                PlayerWin();
                SaveGame(outcome, endCondition);
                return;
            }

            // After computer is done animating, setup player's turn
            countdownTimer.Stop();
            countDownTimer = Convert.ToInt32(comboBox1.Text);
            currentState = GameState.PlayerTurn;

            if (gameLogic.CheckLose(out outcome, out endCondition))
            {
                ComputerWin();
                SaveGame(outcome, endCondition);
                return;
            }

            countdownTimer.Start();
            this.Refresh();
        }

        //right button
        private async void RightButtonClick(object sender, EventArgs e)
        {
            if (painter.Drawing)
            {
                return;
            }
            if (currentState == GameState.Idle) { MessageBox.Show("Press start"); return; }
            if (selectedPiece == null) { MessageBox.Show("Piece not selected"); return; }

            int startCol = selectedPiece.ColIndex;
            int startRow = selectedPiece.RowIndex;
            Piece targetPiece = selectedPiece;

            // --- GRAB STARTING PIXELS BEFORE THE LOGIC CHANGES THEM ---
            int startX = targetPiece.X;
            int startY = targetPiece.Y;

            int locationIndex = 0;
            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                if (gameData.playerLocations[i].Row == startRow && gameData.playerLocations[i].Col == startCol)
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

                snapshot.UpdateSnapshot(gameData.playerLocations, gameData.computerLocations, startRow, startCol, targetPiece.RowIndex, targetPiece.ColIndex);
                SaveMove();
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

            int startCol = selectedPiece.ColIndex;
            int startRow = selectedPiece.RowIndex;
            Piece targetPiece = selectedPiece;

            // --- GRAB STARTING PIXELS BEFORE THE LOGIC CHANGES THEM ---
            int startX = targetPiece.X;
            int startY = targetPiece.Y;

            int locationIndex = 0;
            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                if (gameData.playerLocations[i].Row == startRow && gameData.playerLocations[i].Col == startCol)
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

                snapshot.UpdateSnapshot(gameData.playerLocations, gameData.computerLocations, startRow, startCol, targetPiece.RowIndex, targetPiece.ColIndex);
                SaveMove();
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

            int startCol = selectedPiece.ColIndex;
            int startRow = selectedPiece.RowIndex;
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
                if (gameData.playerLocations[i].Row == startRow && gameData.playerLocations[i].Col == startCol)
                    locationIndex = i;
            }

            // Update the matrix and run the logic!
            if (playerController.TryMoveDownRight(gameData.playerLocations, locationIndex, startRow, startCol, targetPiece))
            {

                // --- PREPARE FOR ANIMATION ---
                // Since playerController changed pieceToAnimate.X, we force it back to the start
                targetPiece.X = startX;
                targetPiece.Y = startY;

                // Run the smooth animation using the new logical coordinates
                await AnimatePieceAsync(targetPiece, targetPiece.RowIndex, targetPiece.ColIndex);

                snapshot.UpdateSnapshot(gameData.playerLocations, gameData.computerLocations, startRow, startCol, targetPiece.RowIndex, targetPiece.ColIndex);
                SaveMove();
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

            int startCol = selectedPiece.ColIndex;
            int startRow = selectedPiece.RowIndex;
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
                if (gameData.playerLocations[i].Row == startRow && gameData.playerLocations[i].Col == startCol)
                    locationIndex = i;
            }

            // Update the matrix and run the logic!
            if (playerController.TryMoveDownLeft(gameData.playerLocations, locationIndex, startRow, startCol, targetPiece))
            {

                // --- PREPARE FOR ANIMATION ---
                // Since playerController changed pieceToAnimate.X, we force it back to the start
                targetPiece.X = startX;
                targetPiece.Y = startY;

                // Run the smooth animation using the new logical coordinates
                await AnimatePieceAsync(targetPiece, targetPiece.RowIndex, targetPiece.ColIndex);

                snapshot.UpdateSnapshot(gameData.playerLocations, gameData.computerLocations, startRow, startCol, targetPiece.RowIndex, targetPiece.ColIndex);
                SaveMove();
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
                painter.DrawOnScreen(this, e);
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
            if (bmp != null)
            {
                bmp.Dispose();
                painter.Canvas = null;
            }
            countdownTimer.Dispose();
            computerTimer.Dispose();
            animationTimer.Dispose();
            if (painter.Pen != null)
                painter.Pen.Dispose();
        }


        //uploads the current snapshot to data base
        private void SaveMove()
        {
            GameMoveTable currentMove = new GameMoveTable
            {
                GameID = snapshot.GameID,
                MoveNumber = snapshot.MoveNumber,
                PlayerLocations = string.Join(",", snapshot.PlayerLocations),
                ComputerLocations = string.Join(",", snapshot.ComputerLocations),
                StartRow = snapshot.StartRow,
                StartCol = snapshot.StartCol,
                TargetRow = snapshot.TargetRow,
                TargetCol = snapshot.TargetCol
            };
            DB.GameMoveTables.InsertOnSubmit(currentMove);
            DB.SubmitChanges();
        }

        private async void SaveGame(string outcome, string endCondition)
        {
            var game = DB.GameTables.First(Game => Game.GameID == currentGameID);
            if (game != null)
            {
                game.GameOutcome = outcome;
                game.EndCondition = endCondition;
            }
            DB.SubmitChanges();
            var savedGame = new { PlayerID = game.PlayerID, PlayerName = game.PlayerName, GameDate = game.GameDate, GameOutcome = game.GameOutcome, Duration = (int)stopwatch.Elapsed.TotalSeconds };
            await ApiManager.SaveGameToServer(savedGame);
            stopwatch.Reset();
        }

        private void RunReplay_Click(object sender, EventArgs e)
        {
            using (ReplayMenu replay = new ReplayMenu())
            {
                if (replay.ShowDialog() == DialogResult.OK)
                {
                    currentGameID = replay.selectedID;
                    ExecuteReplay();
                }
            }
        }

        private List<BoardLocation> DecodeLocations(string dbString)
        {
            List<BoardLocation> list = new List<BoardLocation>();

            if (string.IsNullOrWhiteSpace(dbString))
            {
                return list;
            }

            // 1. Strip away all the brackets. 
            // "[6,1],[6,3]" becomes "6,1,6,3"
            string cleanedString = dbString.Replace("[", "").Replace("]", "");

            // 2. Split what is left by the commas
            string[] numbers = cleanedString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // 3. Loop through the array two steps at a time to grab the Row/Col pairs
            for (int i = 0; i < numbers.Length; i += 2)
            {
                // Ensure we don't go out of bounds if the string is malformed
                if (i + 1 < numbers.Length)
                {
                    int row = int.Parse(numbers[i]);
                    int col = int.Parse(numbers[i + 1]);

                    // Rebuild the object and add it to the list
                    list.Add(new BoardLocation(row, col));
                }
            }

            return list;
        }

        private async void ExecuteReplay()
        {
            HideUI();

            var moves = from move in DB.GameMoveTables
                        where move.GameID == currentGameID
                        orderby move.MoveNumber
                        select new MoveSnapshot(
                        move.GameID,
                        move.MoveNumber,
                        DecodeLocations(move.PlayerLocations),
                        DecodeLocations(move.ComputerLocations),
                        move.StartRow,
                        move.StartCol,
                        move.TargetRow,
                        move.TargetCol);
            foreach (var move in moves)
            {
                if (move.MoveNumber == 1)
                {
                    UpdateBoardState(move.PlayerLocations, move.ComputerLocations);
                    this.Refresh();
                    continue;
                }
                while (painter.Drawing)
                {
                    await Task.Delay(1000);
                }

                await Task.Delay(1000);
                // 1. Find the piece that needs to move on the CURRENT board
                Piece visualPiece = gameData.Board[move.StartRow, move.StartCol];
                if (visualPiece != null)
                {
                    // 2. Visually glide it across the screen to the target
                    await AnimatePieceAsync(visualPiece, move.TargetRow, move.TargetCol);
                }

                // 3. Now that the animation is done, officially update the GameData logic 
                // using the lists you decoded from the database for this specific move.
                UpdateBoardState(move.PlayerLocations, move.ComputerLocations);

                // 4. Force a final repaint to clear away any captured pieces
                this.Refresh();
            }
            var game = DB.GameTables.First(g => g.GameID == currentGameID);

            if (game.GameOutcome == GameOutcome.Win.ToString())
            {
                await blinkingPieces(gameData.playerLocations);
                MessageBox.Show($"Player won by {game.EndCondition} ");
            }
            else
            {
                await blinkingPieces(gameData.computerLocations);
                MessageBox.Show($"Computer won by {game.EndCondition}");
            }
            ShowUI();

            ResetGame();
            this.Refresh();

        }

        private void UpdateBoardState(List<BoardLocation> newPlayerLocs, List<BoardLocation> newComputerLocs)
        {
            // 1. Wipe the logical matrix clean
            Array.Clear(gameData.Board, 0, gameData.Board.Length);

            // 2. Overwrite the tracking lists in GameData
            gameData.playerLocations.Clear();
            gameData.playerLocations.AddRange(newPlayerLocs);

            gameData.computerLocations.Clear();
            gameData.computerLocations.AddRange(newComputerLocs);

            // 3. Rebuild the Player pieces on the matrix
            foreach (var loc in gameData.playerLocations)
            {
                // Parameters: row, col, isPlayer (true), isReversed (false default based on db save)
                gameData.Board[loc.Row, loc.Col] = new Piece(loc.Row, loc.Col, true, false);
            }

            // 4. Rebuild the Computer pieces on the matrix
            foreach (var loc in gameData.computerLocations)
            {
                // Parameters: row, col, isPlayer (false), isReversed (false default)
                gameData.Board[loc.Row, loc.Col] = new Piece(loc.Row, loc.Col, false, false);
            }
        }

        private void HideUI()
        {
            RightButton.Visible = false;
            LeftButton.Visible = false;
            ReverseLeftButton.Visible = false;
            ReverseRightButton.Visible = false;
            comboBox1.Visible = false;
            timerlabel.Visible = false;
            GameIcon.Visible = false;
        }
        private void ShowUI()
        {
            RightButton.Visible = true;
            LeftButton.Visible = true;
            ReverseLeftButton.Visible = true;
            ReverseRightButton.Visible = true;

            comboBox1.Visible = true;
            timerlabel.Visible = true;
            GameIcon.Visible = true;
        }

        private void GameDuration_Tick(object sender, EventArgs e)
        {

        }
    }
}