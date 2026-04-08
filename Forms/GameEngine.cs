using CheckerZ.Data.DB;
using CheckerZ.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace CheckerZ
{
    // class that handles the entire operation of the game
    public partial class GameEngine : Form
    {
        //The context of replay data base
        private ReplayDataDataContext DB = new ReplayDataDataContext();

        //Timer for each turn the player is taking.
        private int countDownTimer;

        //Object for the piece the player clicked on to be used in game logic and when moving from place to place
        private Piece selectedPiece;

        // Bitmap to paint the grid of the game
        private Bitmap bmp;

        //The actual grid displayed on screen
        private BoardGrid grid;

        //Timer to handle the state machine of the game
        private Timer computerTimer;

        // Enum to handle the state the game switches from
        private enum GameState { PlayerTurn, ComputerTurn, Idle }

        //Changes according to current state
        private GameState currentState;

        //Object to handle the paintings displayed on the screen
        private readonly Painter painter;

        //Holds the gamedata
        private GameData gameData;

        //Holds the game logic
        private GameLogic gameLogic;

        //Object to handle player actions
        private PlayerController playerController;

        //Object to handle computer actions
        private ComputerController computerController;

        //Object to save each snapshot of the game state
        private MoveSnapshot snapshot;

        //Variable to be used to seperate games and allows saving in replay database
        private int currentGameID;

        //Stop watch to count the actual duration of the game
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

        //resets the game to intial state so that it can be played again
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

        //Displaying the game grid to the screen and placing the pieces

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            grid.DrawGrid(e.Graphics);

            Piece movingPiece = null;

            foreach (var piece in gameData.Board)
            {
                if (piece != null)
                {
                  
                    int expectedX = Piece.INITIALX + (piece.ColIndex * Piece.MOVEOFFSET);
                    int expectedY = Piece.INITIALY + (piece.RowIndex * Piece.MOVEOFFSET);

                   
                    if (piece.X != expectedX || piece.Y != expectedY)
                    {
                        movingPiece = piece; 
                    }
                    else
                    {
                        piece.Draw(e.Graphics); 
                    }
                }
            }

            if (movingPiece != null)
            {
                movingPiece.Draw(e.Graphics);
            }

            e.Graphics.DrawImage(painter.Canvas, 0, 0);
        }

        // movement animation of a piece that has moved in a turn
        private async Task AnimatePieceAsync(Piece pieceToMove, int targetRow, int targetCol)
        {
            int startX = pieceToMove.X;
            int startY = pieceToMove.Y;

           
            int endX = Piece.INITIALX + (targetCol * Piece.MOVEOFFSET);
            int endY = Piece.INITIALY + (targetRow * Piece.MOVEOFFSET);

            int frames = 5; 

            for (int i = 1; i <= frames; i++)
            {

                pieceToMove.X = startX + ((endX - startX) * i / frames);
                pieceToMove.Y = startY + ((endY - startY) * i / frames);

                this.Invalidate();

                this.Update();

                await Task.Delay(6);
            }

            pieceToMove.X = endX;
            pieceToMove.Y = endY;
            this.Invalidate();
            this.Update();
        }

        // clicking mouse on screen and allows choosing a piece to move
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

            //if the player choosen a piece already change back to player color
            if (selectedPiece != null)
            {
                selectedPiece.PieceColor = Color.Blue;
                selectedPiece = null;
                this.Refresh();
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
                            if (selectedPiece.IsPlayer)
                            {
                                selectedPiece.PieceColor = Color.Yellow;
                                this.Refresh();
                            }

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

        //starting the current game and sets up a new replay to be recorded 
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
                    //intializing new game in the replay data base and saves it
                    GameTable currentGame = new GameTable { PlayerID = selectPlayer.selectedID, PlayerName = selectPlayer.selectedName, GameDate = DateTime.Now, GameOutcome = null, EndCondition = null };
                    DB.GameTables.InsertOnSubmit(currentGame);
                    DB.SubmitChanges();

                    //save player move to currently recorded replay
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

                    // disable the ui elements used to start the game so that the game wont reset

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

        //Handles the countdown timer of the player turn
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
            if (selectedPiece != null)
            {
                selectedPiece.PieceColor = Color.Blue;
                selectedPiece = null;
            }
            computerTimer.Stop(); // Stop so it only runs once per turn
            countdownTimer.Stop();
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
            int startRow;
            int startCol;

            // Try to execute computer move
            MoveCommand serverCommand = await ApiManager.GetComputerMove(gameData.playerLocations, gameData.computerLocations);
            if (computerController.ComputerMove(serverCommand,out movedPiece, out startRow, out startCol) )
            {

                // Run animation 
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
                await AnimatePieceAsync(targetPiece, targetPiece.RowIndex, targetPiece.ColIndex);

                snapshot.UpdateSnapshot(gameData.playerLocations, gameData.computerLocations, startRow, startCol, targetPiece.RowIndex, targetPiece.ColIndex);
                SaveMove();
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


            int locationIndex = 0;
            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                if (gameData.playerLocations[i].Row == startRow && gameData.playerLocations[i].Col == startCol)
                    locationIndex = i;
            }

            // Update the matrix and run the logic!
            if (playerController.AttemptMoveLeft(targetPiece, locationIndex))
            {
                await AnimatePieceAsync(targetPiece, targetPiece.RowIndex, targetPiece.ColIndex);

                snapshot.UpdateSnapshot(gameData.playerLocations, gameData.computerLocations, startRow, startCol, targetPiece.RowIndex, targetPiece.ColIndex);
                SaveMove();
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
        //reverse right button
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

            int locationIndex = 0;
            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                if (gameData.playerLocations[i].Row == startRow && gameData.playerLocations[i].Col == startCol)
                    locationIndex = i;
            }

            if (gameData.playerLocations[locationIndex].isReversed)
            {
                MessageBox.Show("Already reversed. move in another direction or select another piece");
                return;
            }


            // Update the matrix and run the logic!
            if (playerController.TryMoveDownRight( locationIndex, startRow, startCol, targetPiece))
            {
                await AnimatePieceAsync(targetPiece, targetPiece.RowIndex, targetPiece.ColIndex);

                snapshot.UpdateSnapshot(gameData.playerLocations, gameData.computerLocations, startRow, startCol, targetPiece.RowIndex, targetPiece.ColIndex);
                SaveMove();

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
        //reverse left button
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

            int locationIndex = 0;
            for (int i = 0; i < gameData.playerLocations.Count; i++)
            {
                if (gameData.playerLocations[i].Row == startRow && gameData.playerLocations[i].Col == startCol)
                    locationIndex = i;
            }

            if (gameData.playerLocations[locationIndex].isReversed)
            {
                MessageBox.Show("Already reversed. move in another direction or select another piece");
                return;
            }

            // Update the matrix and run the logic!
            if (playerController.TryMoveDownLeft(locationIndex, startRow, startCol, targetPiece))
            {
                await AnimatePieceAsync(targetPiece, targetPiece.RowIndex, targetPiece.ColIndex);

                snapshot.UpdateSnapshot(gameData.playerLocations, gameData.computerLocations, startRow, startCol, targetPiece.RowIndex, targetPiece.ColIndex);
                SaveMove();

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

        //drawing mode button
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

        // clear drawing button
        private void ClearDraws_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(painter.Canvas);
            g.Clear(Color.Transparent);
            this.Refresh();
        }

        //gets the point you clicked to be used for painting
        private void GameEngine_MouseDown(object sender, MouseEventArgs e)
        {
            if (painter.Drawing && e.Button == MouseButtons.Left)
            {
                painter.PenX = e.X; painter.PenY = e.Y;
            }
        }

        //while pressing right click it starts painting on the screen
        private void GameEngine_MouseMove(object sender, MouseEventArgs e)
        {
            if (painter.Drawing && e.Button == MouseButtons.Left)
            {
                painter.DrawOnScreen(this, e);
            }
        }

        //When user closes the game,
        //safely dispose all graphical elements in timers to avoid resource waste in memory
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


        //uploads the current snapshot to the player moves data base
        private void SaveMove()
        {
            GameMoveTable currentMove = new GameMoveTable
            {
                GameID = snapshot.GameID,
                MoveNumber = snapshot.MoveNumber,

                // Use LINQ .Select() to explicitly grab only Row and Col!
                PlayerLocations = string.Join(",", snapshot.PlayerLocations.Select(loc => $"[{loc.Row},{loc.Col}]")),
                ComputerLocations = string.Join(",", snapshot.ComputerLocations.Select(loc => $"[{loc.Row},{loc.Col}]")),

                StartRow = snapshot.StartRow,
                StartCol = snapshot.StartCol,
                TargetRow = snapshot.TargetRow,
                TargetCol = snapshot.TargetCol
            };

            DB.GameMoveTables.InsertOnSubmit(currentMove);
            DB.SubmitChanges();
        }

        // Upon a game end,
        // The current game is updated with game outcome and end condition,
        // Updates the current game in games data base
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

        // Button for opening the replay menu
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

        // Decoding player location list from the data base upon extraction
        private List<BoardLocation> DecodeLocations(string dbString)
        {
            List<BoardLocation> list = new List<BoardLocation>();

            if (string.IsNullOrWhiteSpace(dbString))
            {
                return list;
            }

            string cleanedString = dbString.Replace("[", "").Replace("]", "");

            string[] numbers = cleanedString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

       
            for (int i = 0; i < numbers.Length; i += 2)
            {
                if (i + 1 < numbers.Length)
                {
                    int row = int.Parse(numbers[i]);
                    int col = int.Parse(numbers[i + 1]);

                    list.Add(new BoardLocation(row, col));
                }
            }

            return list;
        }

        //Executing replay choosen from the replay menu
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
                
                Piece visualPiece = gameData.Board[move.StartRow, move.StartCol];
                if (visualPiece != null)
                {
                    
                    await AnimatePieceAsync(visualPiece, move.TargetRow, move.TargetCol);
                }
 
                // using the lists you decoded from the database for this specific move.
                UpdateBoardState(move.PlayerLocations, move.ComputerLocations);

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

        //Updating the board state by executing the move saved from the snapshot data base
        private void UpdateBoardState(List<BoardLocation> newPlayerLocs, List<BoardLocation> newComputerLocs)
        {
            Array.Clear(gameData.Board, 0, gameData.Board.Length);

            gameData.playerLocations.Clear();
            gameData.playerLocations.AddRange(newPlayerLocs);

            gameData.computerLocations.Clear();
            gameData.computerLocations.AddRange(newComputerLocs);

            foreach (var loc in gameData.playerLocations)
            {
                
                gameData.Board[loc.Row, loc.Col] = new Piece(loc.Row, loc.Col, true);
            }

            foreach (var loc in gameData.computerLocations)
            {
                gameData.Board[loc.Row, loc.Col] = new Piece(loc.Row, loc.Col, false);
            }
        }

        //Hide Ui while replay is played
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

        //Showing Ui when replay end
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
    }
}