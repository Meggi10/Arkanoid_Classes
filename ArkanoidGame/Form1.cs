using System.Drawing.Text;
using System.Security.Cryptography;

namespace ArkanoidGame
{
    public partial class Arkanoid : Form
    {
        GameManager gameManager;
        BrickManager brickManager;
        BallController ballController;
        PlayerController playerController;

        public Arkanoid()
        {
            InitializeComponent();

            gameManager = new GameManager();
            brickManager = new BrickManager(this);
            ballController = new BallController(ball);
            playerController = new PlayerController(player);

            StartGame();
        }

        private void StartGame()
        {
            brickManager.PlaceBricks();
            gameManager.Reset();
            ballController.ResetPosition();
            playerController.ResetPosition();
            timer.Start();
            txtScore.Text = "Score: 0";
        }

        private void mainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + gameManager.Score;

            playerController.Move();
            ballController.Move();
            ballController.BounceOffPlayer(player);

            var bricksToRemove = new List<Control>();

            foreach (var brick in brickManager.Bricks.ToList()) 
            {
                if (ballController.BounceOffBrick(brick))
                {
                    gameManager.AddScore(1);
                    this.Controls.Remove(brick);
                    brickManager.Bricks.Remove(brick); 
                    break; 
                }
            }

            foreach (var brick in bricksToRemove)
            {
                this.Controls.Remove(brick);
            }

            if (gameManager.Score == 15)
            {
                gameManager.EndGame("You win! Press Enter to play again!", timer, txtScore);
            }

            if (ballController.IsOutOfBounds(489))
            {
                gameManager.EndGame("You lose! Press Enter to play again!", timer, txtScore);
            }
        }


        private void keyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                playerController.GoLeft = true;

            if (e.KeyCode == Keys.Right)
                playerController.GoRight = true;
        }

        private void keyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                playerController.GoLeft = false;

            if (e.KeyCode == Keys.Right)
                playerController.GoRight = false;

            if (e.KeyCode == Keys.Enter && gameManager.IsGameOver)
            {
                brickManager.RemoveBricks();
                StartGame();
            }
        }
    }

}
