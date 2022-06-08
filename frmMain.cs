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
        //PictureBox playerIdle;
        //PictureBox playerRunLeft;
        //PictureBox playerRunRight;
        PictureBox playerMovement;
        private int PlayerMovementStatus=0;
        PictureBox enemyIdel;
        Random random;
        private string enemyDirection="left";
        private int enemySpeed=0;
        List<PictureBox> playerFire = new List<PictureBox>();
        public frmMain()
        {
            InitializeComponent();
        }
        
        private void gameLoop_Tick(object sender, EventArgs e)
        {

            PlayerMovementStatus = 1;
            if (Keyboard.IsKeyPressed(Key.LeftArrow))
            {
                PlayerMovementStatus = 1;
                playerMovement.Left -= 10;
            }
            if (Keyboard.IsKeyPressed(Key.RightArrow))
            {
                PlayerMovementStatus = 2;
                playerMovement.Left += 10;
            }
            if (Keyboard.IsKeyPressed(Key.UpArrow))
            {
                playerMovement.Top -= 10;
            }
            if (Keyboard.IsKeyPressed(Key.DownArrow))
            {
                playerMovement.Top += 10;
            }

            if (Keyboard.IsKeyPressed(Key.Space))
            {
                createBullet();
            }
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
                enemyIdel.Left -= random.Next(0,enemySpeed);
            }
            if (enemyDirection == "right")
            {
                enemyIdel.Left += random.Next(0, enemySpeed);
            }
           

        }

       

        private void frmMain_Load(object sender, EventArgs e)
        {
            createPlayer();
            random = new Random();
            enemySpeed = 5;
            createEnemy();
        }
        private void createBullet()
        {

            PictureBox bullet = new PictureBox();
            Image img = Vidyakali.Properties.Resources.bullet;
            bullet.BackColor = Color.Transparent;
            bullet.Image = img;
            bullet.SizeMode = PictureBoxSizeMode.Zoom;
            bullet.Left = playerMovement.Left+40;
            bullet.Top =playerMovement.Top-10;
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
            ////Create Idle player
            //playerIdle = new PictureBox();
            //Image img = Vidyakali.Properties.Resources.PlayerIdle;
            //playerIdle.Image = img;
            //playerIdle.SizeMode = PictureBoxSizeMode.Zoom;
            //playerIdle.Left = this.Left;
            //playerIdle.Top = this.Top;
            //this.Controls.Add(playerIdle);
            //Create run left player
            //playerRunLeft = new PictureBox();
            //playerRunLeft.Image = Vidyakali.Properties.Resources.PlayerRunLeft;
            //playerRunLeft.SizeMode = PictureBoxSizeMode.Zoom;
            //playerRunLeft.Left = this.Left;
            //playerRunLeft.Top = this.Top;
            //this.Controls.Add(playerRunLeft);
            ////Create run Right player
            //playerRunRight = new PictureBox();
            //playerRunRight.Image = Vidyakali.Properties.Resources.PlayerRunRight;
            //playerRunRight.SizeMode = PictureBoxSizeMode.Zoom;
            //playerRunRight.Left = this.Left;
            //playerRunRight.Top = this.Top;
            //this.Controls.Add(playerRunRight);
            //Create player Movement
            playerMovement = new PictureBox();
            if (PlayerMovementStatus == 0)
            {//player idle
                playerMovement.BackColor = Color.Transparent;
                playerMovement.Image = Vidyakali.Properties.Resources.PlayerAttackLeft;
            }
            else if (PlayerMovementStatus == 1)
            {//player  run left  
                playerMovement.Image = Vidyakali.Properties.Resources.PlayerRunLeft;
            }
            else if (PlayerMovementStatus == 2)
            {// player run right  
                playerMovement.Image = Vidyakali.Properties.Resources.PlayerRunRight;
            }
            playerMovement.SizeMode = PictureBoxSizeMode.Zoom;
            playerMovement.Left = this.Left;
            playerMovement.Top = this.Top;
            this.Controls.Add(playerMovement);
        }
    }
}

