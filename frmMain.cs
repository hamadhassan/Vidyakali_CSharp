using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EZInput;

namespace Vidyakali
{
    public partial class frmMain : Form
    {
        #region Level2 
        PictureBox playerMovement;
        ProgressBar playerHealth;
        private int playerSpeed;
        private int playerBulletSpeed;
        PictureBox enemyIdel;
        Random random;
        private string enemyDirection = "left";
        private int enemySpeed = 0;
        List<PictureBox> playerFireRight = new List<PictureBox>();
        List<PictureBox> playerFireLeft = new List<PictureBox>();
        List<PictureBox> playerFireUp = new List<PictureBox>();
        List<PictureBox> playerFireDown = new List<PictureBox>();



        List<PictureBox> enemyFire = new List<PictureBox>();
        private int enemyFireGenerationTime;
        private int enemyFireCurrentGeneration;
        #endregion
        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            StartLevel2();
        }
        private void gameLoop_Tick(object sender, EventArgs e)
        {
            #region Level 2
            movePlayer();
            //Bullets Fire 
            firePlayerBullet();
            movePlayerBulletRight();
            movePlayerBulletLeft();
            movePlayerBulletUp();
            movePlayerBulletDown();
            removePlayerBulletRight();
            removePlayerBulletLeft();
            removePlayerBulletDown();
            removePlayerBulletUp();
            moveEnemy();
            moveEnemyBullet();
            removeEnemyBullet();
            detectCollisionRight();
            detectCollisionLeft();
            detectCollisionUp();
            detectCollisionDown();
            enemyFireCurrentGeneration++;
            if (enemyFireCurrentGeneration == enemyFireGenerationTime)
            {
                creatEnemyBullet();
                enemyFireCurrentGeneration = 0;
            }
            playerHealth.Left = playerMovement.Left-10;
            playerHealth.Top = playerMovement.Top+72;
            //if (enemyIdel.Visible == false)
            //{
            //    gameLoop.Enabled = false;
            //    Image img = Vidyakali.Properties.Resources.game_background_4;
            //    frmEnd end = new frmEnd(img);
            //    DialogResult result = end.ShowDialog();
            //    if (result == DialogResult.Yes)
            //    {
            //        StartLevel2();
            //    }
            //    else
            //    {
            //        Close();
            //    }
            //}
            //else if (playerHealth.Value == 0)
            //{
            //    gameLoop.Enabled = false;
            //    Image img = Vidyakali.Properties.Resources.game_background_4;
            //    frmEnd end = new frmEnd(img);
            //    DialogResult result = end.ShowDialog();
            //    if (result == DialogResult.Yes)
            //    {
            //        StartLevel2();
            //    }
            //    else
            //    {
            //        Close();
            //    }
            //}
            #endregion
        }
        #region Level 2
        private void StartLevel2()
        {
            gameLoop.Enabled = true;
            random = new Random();
            playerSpeed = 5;
            playerBulletSpeed = 10;
            enemySpeed = 5;
            enemyFireGenerationTime = 20;
            enemyFireCurrentGeneration = 0;
            createPlayer();
            createEnemy();
        }
        private void detectCollisionRight()
        {
            foreach (PictureBox bullet in playerFireRight)
            {
                if (bullet.Bounds.IntersectsWith(enemyIdel.Bounds))
                {
                   
                    enemyIdel.Visible = false;
                    enemyFireGenerationTime = -1;
                }
            }
        }
        private void detectCollisionLeft()
        {
            foreach (PictureBox bullet in playerFireLeft)
            {
                if (bullet.Bounds.IntersectsWith(enemyIdel.Bounds))
                {
                    enemyIdel.Visible = false;
                    enemyFireGenerationTime = -1;
                }
            }
        }
        private void detectCollisionUp()
        {
            foreach (PictureBox bullet in playerFireUp)
            {
                if (bullet.Bounds.IntersectsWith(enemyIdel.Bounds))
                {
                    enemyIdel.Visible = false;
                    enemyFireGenerationTime = -1;
                }
            }
        }
        private void detectCollisionDown()
        {
            foreach (PictureBox bullet in playerFireDown)
            {
                if (bullet.Bounds.IntersectsWith(enemyIdel.Bounds))
                {
                    enemyIdel.Visible = false;
                    enemyFireGenerationTime = -1;
                }
            }
        }
        private void removePlayerBulletRight()
        {
            //Player Fire
            for (int i = 0; i < playerFireRight.Count; i++)
            {
                if (playerFireRight[i].Right <= 0)
                {
                    playerFireRight.RemoveAt(i);
                }
            }
        }
        private void removePlayerBulletLeft()
        {
            //Player Fire
            for (int i = 0; i < playerFireLeft.Count; i++)
            {
                if (playerFireLeft[i].Left <-50)
                {
                    playerFireLeft.RemoveAt(i);
                }
            }
        }
        private void removePlayerBulletDown()
        {
            //Player Fire
            for (int i = 0; i < playerFireDown.Count; i++)
            {
                if (playerFireDown[i].Bottom <0)
                {
                    playerFireDown.RemoveAt(i);
                }
            }
        }
        private void removePlayerBulletUp()
        {
            //Player Fire
            for (int i = 0; i < playerFireUp.Count; i++)
            {
                if (playerFireUp[i].Top < 0)
                {
                    playerFireUp.RemoveAt(i);
                }
            }
        }
        private void removeEnemyBullet()
        {
            //Enemy Fire 
            for (int i = 0; i < enemyFire.Count; i++)
            {
                if (enemyFire[i].Top >= this.Height || enemyFire[i].Visible == false)
                {
                    enemyFire.Remove(enemyFire[i]);
                }
            }
        }

        private void moveEnemyBullet()
        {
            
            //enemy bullet
            foreach (PictureBox bullet in enemyFire)
            {
                bullet.Left += 10;
            }
        }
        private void movePlayerBulletRight()
        {
            foreach (PictureBox bullet in playerFireRight)
            {
                bullet.Left += playerBulletSpeed;
            }
        }
        private void movePlayerBulletLeft()
        {
            foreach (PictureBox bullet in playerFireLeft)
            {
                bullet.Left -= playerBulletSpeed;
            }
        }
        private void movePlayerBulletUp()
        {
            foreach (PictureBox bullet in playerFireUp)
            {
                bullet.Top -= playerBulletSpeed;
            }
        }
        private void movePlayerBulletDown()
        {
            foreach (PictureBox bullet in playerFireDown)
            {
                bullet.Top += playerBulletSpeed;
            }
        }
        private void firePlayerBullet()
        {
            if (Keyboard.IsKeyPressed(Key.D))
            {
                createPlayerBulletRight();
            }
            if (Keyboard.IsKeyPressed(Key.A))
            {
                createPlayerBulletLeft();
            }
            if (Keyboard.IsKeyPressed(Key.W))
            {
                createPlayerBulletUp();
            }
            if (Keyboard.IsKeyPressed(Key.S))
            {
                createPlayerBulletDown();
            }
        }
        private void createPlayerBulletDown()
        {
            PictureBox bullet = new PictureBox();
            Image img = Vidyakali.Properties.Resources.laserPlayerDown;
            bullet.BackColor = Color.Transparent;
            bullet.Image = img;
            bullet.SizeMode = PictureBoxSizeMode.AutoSize;
            bullet.Left = playerMovement.Left+10;
            bullet.Top = playerMovement.Top + 40;
            playerFireDown.Add(bullet);
            this.Controls.Add(bullet);
        }
        private void createPlayerBulletUp()
        {
            PictureBox bullet = new PictureBox();
            Image img = Vidyakali.Properties.Resources.laserPlayerUp;
            bullet.BackColor = Color.Transparent;
            bullet.Image = img;
            bullet.SizeMode = PictureBoxSizeMode.AutoSize;
            bullet.Left = playerMovement.Left+10;
            bullet.Top = playerMovement.Top - 40;
            playerFireUp.Add(bullet);
            this.Controls.Add(bullet);
        }
        private void createPlayerBulletLeft()
        {
            PictureBox bullet = new PictureBox();
            Image img = Vidyakali.Properties.Resources.laserPlayerLeft;
            bullet.BackColor = Color.Transparent;
            bullet.Image = img;
            bullet.SizeMode = PictureBoxSizeMode.AutoSize;
            bullet.Left = playerMovement.Left-20;
            bullet.Top = playerMovement.Top + 40;
            playerFireLeft.Add(bullet);
            this.Controls.Add(bullet);
        }
        private void createPlayerBulletRight()
        {
            PictureBox bullet = new PictureBox();
            Image img = Vidyakali.Properties.Resources.laserPlayerLeft;
            bullet.BackColor = Color.Transparent;
            bullet.Image = img;
            bullet.SizeMode = PictureBoxSizeMode.AutoSize;
            bullet.Left = playerMovement.Left;
            bullet.Top = playerMovement.Top+40;
            playerFireRight.Add(bullet);
            this.Controls.Add(bullet);
        }
        private void creatEnemyBullet()
        {
            PictureBox bullet = new PictureBox();
            Image img = Vidyakali.Properties.Resources.laserEnemy;
            bullet.BackColor = Color.Transparent;
            bullet.Image = img;
            bullet.SizeMode = PictureBoxSizeMode.AutoSize;
            bullet.Left = enemyIdel.Left;
            bullet.Top = enemyIdel.Top+40;
            enemyFire.Add(bullet);
            this.Controls.Add(bullet);
        }
        private void movePlayer()
        {
            //PLayer Movement 
            playerMovement.Image = Vidyakali.Properties.Resources.idle;
            if (Keyboard.IsKeyPressed(Key.LeftArrow))
            {
                playerMovement.Image = Vidyakali.Properties.Resources.runLeft;
                playerMovement.Left -= playerSpeed;
            }
            if (Keyboard.IsKeyPressed(Key.RightArrow))
            {
                playerMovement.Image = Vidyakali.Properties.Resources.runRight;
                playerMovement.Left += playerSpeed;
            }
            if (Keyboard.IsKeyPressed(Key.UpArrow))
            {
                playerMovement.Top -= playerSpeed;
            }
            if (Keyboard.IsKeyPressed(Key.DownArrow))
            {
                playerMovement.Top += playerSpeed;
            }
            //Firing 
        }

        private void moveEnemy()
        {
            //Enemy Movement
            if (enemyIdel.Left <= 0)
            {
                enemyDirection = "right";
            }
            if (enemyIdel.Left + enemyIdel.Width >= this.Width)
            {
                enemyDirection = "left";
            }
            if (enemyDirection == "left")
            {
                enemyIdel.Left -= random.Next(0, enemySpeed);
            }
            if (enemyDirection == "right")
            {
                enemyIdel.Left += random.Next(0, enemySpeed);
            }
        }
        private void createEnemy()
        {
            enemyIdel = new PictureBox();
            Image img = Vidyakali.Properties.Resources.enemyIdel;
            enemyIdel.Image = img;
            enemyIdel.SizeMode = PictureBoxSizeMode.AutoSize;
            enemyIdel.Top = random.Next(100, 200);
            enemyIdel.Left = random.Next(100, this.Width - img.Width);
            this.Controls.Add(enemyIdel);
            enemyDirection = "left";
        }
        private void createPlayer()
        {
            //Create player Movement idle,left and right
            playerMovement = new PictureBox();
            playerMovement.Image = Vidyakali.Properties.Resources.idle;
            playerMovement.SizeMode = PictureBoxSizeMode.AutoSize;
            playerMovement.Left = this.Left;
            playerMovement.Top = this.Top;
            this.Controls.Add(playerMovement);
            //progress bar
            playerHealth = new ProgressBar();
            playerHealth.Value = 100;
            playerHealth.Top = playerMovement.Top + 72;
            playerHealth.Left = playerMovement.Left;
            playerHealth.Size=new Size(60,15);
            this.Controls.Add(playerHealth);
        }
        #endregion
    }
}

