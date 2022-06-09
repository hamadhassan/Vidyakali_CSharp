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
        PictureBox playerMovement;
        private int playerSpeed;
        PictureBox enemyIdel;
        Random random;
        private string enemyDirection="left";
        private int enemySpeed=0;
        List<PictureBox> playerFire = new List<PictureBox>();
        List<PictureBox> enemyFire = new List<PictureBox>();
        private int enemyFireGenerationTime;
        private int enemyFireCurrentGeneration;
        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {

            createPlayer();
            playerSpeed = 5;
            random = new Random();
            enemySpeed = 5;
            createEnemy();
            enemyFireGenerationTime = 20;
            enemyFireCurrentGeneration = 0;
        }
        private void gameLoop_Tick(object sender, EventArgs e)
        {
            movePlayer();
            moveEnemy();
            //Bullets Fire 
            if (Keyboard.IsKeyPressed(Key.Space))
            {
                createBullet();
            }
            moveBullet();
            //detectCollision();
            removeBullet();
            enemyFireCurrentGeneration++;
            if (enemyFireCurrentGeneration == enemyFireGenerationTime)
            {
                creatEnemyBullet();
                enemyFireCurrentGeneration=0;
            }
            
        }

        private void detectCollision()
        {
            foreach(PictureBox bullet in playerFire)
            {
                if (bullet.Bounds.IntersectsWith(enemyIdel.Bounds))
                {
                    enemyIdel.Visible = false ;
                    bullet.Visible = false; 
                    enemyFire.Clear();
                }
            }
        }

        private void creatEnemyBullet()
        {
            PictureBox bullet = new PictureBox();
            Image img = Vidyakali.Properties.Resources.enemyBullet;
            bullet.BackColor = Color.Transparent;
            bullet.Image = img;
            bullet.SizeMode = PictureBoxSizeMode.Zoom;
            bullet.Left = enemyIdel.Left + 40;
            bullet.Top = enemyIdel.Top;
            playerFire.Add(bullet);
            this.Controls.Add(bullet);
        }

        private void removeBullet()
        {
            //Player Fire
            for(int i = 0; i < playerFire.Count; i++)
            {
                if (playerFire[i].Right <= 0)
                {
                    playerFire.RemoveAt(i);
                }
            }
            //Enemy Fire 
            for (int i = 0; i < enemyFire.Count; i++)
            {
                if (enemyFire[i].Right <= 0)
                {
                    enemyFire.RemoveAt(i);
                }
            }
        }

        private void moveBullet()
        {
            //player bullet
            foreach(PictureBox bullet in playerFire)
            {
                bullet.Left+=10;
            }
            //enemy bullet
            foreach(PictureBox bullet in enemyFire)
            {
                bullet.Left += 10;
            }
        }

        private void movePlayer()
        {
            //PLayer Movement 
            playerMovement.Image = Vidyakali.Properties.Resources.PlayerIdle;
            if (Keyboard.IsKeyPressed(Key.LeftArrow))
            {
                playerMovement.Image = Vidyakali.Properties.Resources.PlayerRunLeft;
                playerMovement.Left -= playerSpeed;
            }
            if (Keyboard.IsKeyPressed(Key.RightArrow))
            {
                playerMovement.Image = Vidyakali.Properties.Resources.PlayerRunRight;
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
        private void createBullet()
        {

            PictureBox bullet = new PictureBox();
            Image img = Vidyakali.Properties.Resources.bullet;
            bullet.BackColor = Color.Transparent;
            bullet.Image = img;
            bullet.SizeMode = PictureBoxSizeMode.Zoom;
            bullet.Left = playerMovement.Left+40;
            bullet.Top =playerMovement.Top;
            playerFire.Add(bullet);
            this.Controls.Add(bullet);
        }
        private void createEnemy()
        {
            enemyIdel = new PictureBox();
            Image img = Vidyakali.Properties.Resources.EnemyIdel;
            enemyIdel.Image =img;
            enemyIdel.SizeMode = PictureBoxSizeMode.Zoom;
            enemyIdel.Top = random.Next(100,200);
            enemyIdel.Left = random.Next(100,this.Width-img.Width);
            this.Controls.Add(enemyIdel);
            enemyDirection = "left";
        }
        private void createPlayer()
        {
            //Create player Movement idle,left and right
            playerMovement = new PictureBox();
            playerMovement.Image = Vidyakali.Properties.Resources.PlayerIdle;
            playerMovement.SizeMode = PictureBoxSizeMode.Zoom;
            playerMovement.Left = this.Left;
            playerMovement.Top = this.Top;
            this.Controls.Add(playerMovement);
        }
    }
}

