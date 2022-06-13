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
        #region All Level
        private int levelNumberStatus;
        private float score;
        private bool gameStatus=true;
        private bool isLevel1 = true;
        private bool isLevel2 = false;
        private bool isLevel3 = false;
        private int playerSpeed;
        PictureBox nextLevelBox;
        private int boxOpeningLevel;
        ProgressBar playerHealth;
        PictureBox playerMovement;
        Random random= new Random();
        #endregion
        #region Level1
        PictureBox enemyIdelMovement;
        ProgressBar enemyIdelHealth;
        private int enemyIdelSpeed;
        #endregion

        #region Level 2
        PictureBox enemyRunMovement;
        ProgressBar enemyRunlHealth;
        private int enemyRunSpeed;
        List<PictureBox> energyPointList = new List<PictureBox>();
        private int fixedEnegyTime;
        private int energyTime;
        #endregion

        #region Level3 
        private int playerBulletSpeed;
        PictureBox enemyFire;
        private string enemyDirection = "left";
        private int enemySpeed = 0;
        List<PictureBox> enemyIdelList = new List<PictureBox>();
        private int enemeyIdelListSpeed;
        List<PictureBox> playerFireRight = new List<PictureBox>();
        List<PictureBox> playerFireLeft = new List<PictureBox>();
        List<PictureBox> playerFireUp = new List<PictureBox>();
        List<PictureBox> playerFireDown = new List<PictureBox>();
        List<PictureBox> enemyFireList=new List<PictureBox>();
        private int enemyFireGenerationTime;
        private int enemyFireCurrentGeneration;
        private int reducePlayerHealth;
        private int countDiedEnemy;

        #endregion

        #region Form Setup 
        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            startLevel1();
            createnextLevel();
            nextLevelBox.Visible = false;
        }
        #endregion

        #region Game Loop
        private void gameLoop_Tick(object sender, EventArgs e)
        {
            if (gameStatus == true)
            {
                lblLevel.Text = levelNumberStatus.ToString();
                lblScore.Text = score.ToString();
                movePlayer();
                if (isLevel1 == true)
                {
                    moveEnemyIdel();
                    playerAttackByHand();
                    detectCollisionwithEnemyIdel();
                    detectCollisionOfEnemyIdelWithPlayerL1();
                    detectBoxCollisionL1();
                    gameOver();
                    //energyPoint
                    showEnergyPoint();
                    detectCollisionOfEnergyPoint();
                    removeEnergyPoint();
                }
                else if (isLevel2 == true)
                {
                    playerAttackByHand();
                    moveEnemyIdel();
                    detectCollisionwithEnemyIdel();
                    detectCollisionOfEnemyIdelWithPlayerL2();
                    moveRunEnemy();
                    detectCollisionwithEnemyRun();
                    detectCollisionOfEnemyRunWithPlayer();
                    nexLevel();
                    detectBoxCollisionL2();
                    gameOver();
                    //energyPoint
                    showEnergyPoint();
                    detectCollisionOfEnergyPoint();
                    removeEnergyPoint();

                }
                else if (isLevel3 == true)
                {
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
                    detectEnemyFireCollision();
                    detectEnemyCollision();
                    detectPlayerCollisionRight();
                    detectPlayerCollisionLeft();
                    detectPlayerCollisionUp();
                    detectPlayerCollisionDown();
                    enemyFireCurrentGeneration++;
                    if (enemyFireCurrentGeneration == enemyFireGenerationTime)
                    {
                        creatEnemyBullet();
                        enemyFireCurrentGeneration = 0;
                    }
                    //energyPoint
                    showEnergyPoint();
                    detectCollisionOfEnergyPoint();
                    removeEnergyPoint();
                    //Randon Enemy
                    genertaeEneyIdel();
                    moveEnemyIdelList();
                    removeEnemyFromList();
                    detectCollisionofEnemyIdelList();
                    detectEnemyCollisionwithPlayer();
                    gameOver();
                    nexLevel();
                    detectBoxCollisionL3();
                }
            }
            else
            {
                gameLoop.Enabled = false;
                string message = "Game Over";
                frmEnd end = new frmEnd(message);
                DialogResult result = end.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    restartLevel1();
                }
                else
                {
                    Close();
                }
            }
        }
        #endregion

        #region Level1
        private void startLevel1()
        {
            score = 0;
            levelNumberStatus = 1;
            playerSpeed = 10;
            enemyIdelSpeed = 1;
            pgbarPlayerLife.Value = 100;
            createEnemyIdel();
            createPlayer();
            boxOpeningLevel = 1;
        }
        private void restartLevel1()
        {
            //gameLoop.Enabled = true;
            score = 0;
            levelNumberStatus = 1;
            playerSpeed = 10;
            enemyIdelSpeed = 1;
            pgbarPlayerLife.Value = 100;
            createEnemyIdel();
            createPlayer();
            boxOpeningLevel = 1;
        }
        private void playerAttackByHand()
        {
            if (Keyboard.IsKeyPressed(Key.A))
            {
                playerMovement.Image = Vidyakali.Properties.Resources.attackLeft;
            }
            if (Keyboard.IsKeyPressed(Key.D))
            {
                playerMovement.Image = Vidyakali.Properties.Resources.attackRight;
            }
        }
        private void detectCollisionOfEnemyIdelWithPlayerL1()
        {
            if (enemyIdelMovement.Bounds.IntersectsWith(playerMovement.Bounds) && Keyboard.IsKeyPressed(Key.D) || Keyboard.IsKeyPressed(Key.A))
            {
                score += 0.5F;
                if (enemyIdelHealth.Value > 0)
                {
                    enemyIdelHealth.Value -= 1;
                }
                if (enemyIdelHealth.Value <= 0)
                {//level 1 won 
                    nextLevelBox.Visible = true;
                    isLevel1 = false;
                    this.Controls.Remove(enemyIdelHealth);
                    this.Controls.Remove(enemyIdelMovement);
                    if(boxOpeningLevel == 1)
                    {
                        boxOpeningLevel = 2;
                    }
                }
            }
        }
        private void detectCollisionwithEnemyIdel()
        {
            if (playerMovement.Bounds.IntersectsWith(enemyIdelMovement.Bounds))
            {
                if (playerHealth.Value > 0)
                {
                    playerHealth.Value -= 2;
                }
            }
        }
        private void moveEnemyIdel()
        {
            enemyIdelHealth.Left = enemyIdelMovement.Left - 10;
            enemyIdelHealth.Top = enemyIdelMovement.Top + 60;
            if (enemyIdelMovement.Left > playerMovement.Left)
            {
                enemyIdelMovement.Left -= enemyIdelSpeed;
            }
            if (enemyIdelMovement.Left < playerMovement.Left)
            {
                enemyIdelMovement.Left += enemyIdelSpeed;
            }
            if (enemyIdelMovement.Top > playerMovement.Top)
            {
                enemyIdelMovement.Top -= enemyIdelSpeed;
            }
            if (enemyIdelMovement.Top < playerMovement.Top)
            {
                enemyIdelMovement.Top += enemyIdelSpeed;
            }
        }
        private void createEnemyIdel()
        {
            enemyIdelMovement = new PictureBox();
            Image img = Vidyakali.Properties.Resources.enemyIdel;
            enemyIdelMovement.Image = img;
            enemyIdelMovement.BackColor = Color.Transparent;
            enemyIdelMovement.SizeMode = PictureBoxSizeMode.AutoSize;
            enemyIdelMovement.Left = random.Next(0,700);
            enemyIdelMovement.Top = random.Next(100,400);
            this.Controls.Add(enemyIdelMovement);
            //progress bar
            enemyIdelHealth = new ProgressBar();
            enemyIdelHealth.Value = 100;
            enemyIdelHealth.Top = enemyIdelMovement.Top + 60;
            enemyIdelHealth.Left = enemyIdelMovement.Left-12;
            enemyIdelHealth.Size = new Size(40, 15);
            enemyIdelHealth.ForeColor = Color.Red;
            this.Controls.Add(enemyIdelHealth);
        }

        private void movePlayer()
        {
            playerHealth.Left = playerMovement.Left - 8;
            playerHealth.Top = playerMovement.Top + 80;
            //PLayer Movement 
            playerMovement.Image = Vidyakali.Properties.Resources.idle;
            if (Keyboard.IsKeyPressed(Key.LeftArrow))
            {
                if (playerMovement.Left + playerSpeed > 25)
                {
                    playerMovement.Image = Vidyakali.Properties.Resources.runLeft;
                    playerMovement.Left -= playerSpeed;
                }
            }
            if (Keyboard.IsKeyPressed(Key.RightArrow))
            {
                if (playerMovement.Left + playerMovement.Width+38 <this.Width)
                {
                    playerMovement.Image = Vidyakali.Properties.Resources.runRight;
                    playerMovement.Left += playerSpeed;
                }
            }
            if (Keyboard.IsKeyPressed(Key.UpArrow))
            {
                if (playerMovement.Top > 60)
                {
                    playerMovement.Top -= playerSpeed;
                }
            }
            if (Keyboard.IsKeyPressed(Key.DownArrow))
            {
                if (playerMovement.Top + playerMovement.Height+85 < this.Height)
                {
                    playerMovement.Top += playerSpeed;
                }
            }
        }
        private void createPlayer()
        {
            playerMovement = new PictureBox();
            playerMovement.Image = Vidyakali.Properties.Resources.idle;
            playerMovement.SizeMode = PictureBoxSizeMode.AutoSize;
            playerMovement.BackColor = Color.Transparent;
            playerMovement.Left = this.Left;
            playerMovement.Top = this.Top;
            this.Controls.Add(playerMovement);
            //progress bar
            playerHealth = new ProgressBar();
            playerHealth.Value = 100;
            playerHealth.Top = playerMovement.Top + 80;
            playerHealth.Left = playerMovement.Left;
            playerHealth.Size = new Size(40, 15);
            this.Controls.Add(playerHealth);
        }

        #endregion

        #region Level 2
        private void startLevel2()
        {
            createEnemyRun();
            createEnemyIdel();
            enemyRunlHealth.Value = 100;
            levelNumberStatus = 2;
            playerSpeed = 10;
            enemyIdelSpeed = 1;
            enemyRunSpeed = 3;
            pgbarPlayerLife.Value = 100;
            enemyIdelHealth.Value = 100;
            enemyIdelMovement.Visible = true;
            enemyIdelHealth.Visible = true;
            this.BackColor = Color.DarkSlateGray;
        }
        private void showEnergyPoint()
        {
            energyTime++;
            fixedEnegyTime = random.Next(0, 150);
            if (20 == energyTime)
            {
                if (pgbarPlayerLife.Value <= random.Next(0, 80))
                {
                    createEnergyPoint();
                }
                energyTime = 0;
            }
        }
        private void removeEnergyPoint()
        {
            for(int x=0;x<energyPointList.Count;x++)
            {
                if (energyPointList[x].Visible == false)
                {
                    energyPointList.RemoveAt(x);
                }
            }
        }
        private void detectCollisionOfEnergyPoint()
        {
            foreach (PictureBox energy in energyPointList)
            {
                if (energy.Bounds.IntersectsWith(playerMovement.Bounds))
                {
                    energy.Visible = false;
                    if (pgbarPlayerLife.Value < 80)
                    {
                        pgbarPlayerLife.Value +=17;
                    }
                }
            }
        }
        private void createEnergyPoint()
        {
            PictureBox enegy = new PictureBox();
            Image img = Vidyakali.Properties.Resources.energy;
            enegy.BackColor = Color.Transparent;
            enegy.Image = img;
            enegy.SizeMode = PictureBoxSizeMode.AutoSize;
            enegy.Left = random.Next(100,300);
            enegy.Top = random.Next(100,400);
            energyPointList.Add(enegy);
            this.Controls.Add(enegy);
        }
      
        private void detectCollisionOfEnemyIdelWithPlayerL2()
        {
            if (enemyIdelMovement.Bounds.IntersectsWith(playerMovement.Bounds) && Keyboard.IsKeyPressed(Key.D) || Keyboard.IsKeyPressed(Key.A))
            {
                score += 0.5F;
                if (enemyIdelHealth.Value > 0)
                {
                    enemyIdelHealth.Value -= 1;
                }
                if (enemyIdelHealth.Value <= 0)
                {//level 2 idel enemy remove  
                    this.Controls.Remove(enemyIdelHealth);
                    this.Controls.Remove(enemyIdelMovement);
                }
            }
        }
        private void detectCollisionOfEnemyRunWithPlayer()
        {
            if (enemyRunMovement.Bounds.IntersectsWith(playerMovement.Bounds))
            {
                score += 0.5F;
                if (enemyRunlHealth.Value > 0)
                {
                    enemyRunlHealth.Value -= 1;
                }
                if (enemyRunlHealth.Value <= 0)
                {//level 2 run enemy remove 
                    this.Controls.Remove(enemyRunMovement);
                    this.Controls.Remove(enemyRunlHealth);
                }
            }
        }
        private void detectCollisionwithEnemyRun()
        {
            if (playerMovement.Bounds.IntersectsWith(enemyRunMovement.Bounds))
            {
                if (playerHealth.Value > 0)
                {
                    playerHealth.Value -= 2;
                }
            }
        }
        private void createEnemyRun()
        {
            enemyRunMovement = new PictureBox();
            Image img = Vidyakali.Properties.Resources.enemyRunRight;
            enemyRunMovement.Image = img;
            enemyRunMovement.BackColor = Color.Transparent;
            enemyRunMovement.SizeMode = PictureBoxSizeMode.AutoSize;
            enemyRunMovement.Left = random.Next(0, 700);
            enemyRunMovement.Top = random.Next(100, 400);
            this.Controls.Add(enemyRunMovement);
            //progress bar
            enemyRunlHealth = new ProgressBar();
            enemyRunlHealth.Value = 100;
            enemyRunlHealth.Top = enemyRunMovement.Top + 60;
            enemyRunlHealth.Left = enemyRunMovement.Left - 12;
            enemyRunlHealth.Size = new Size(40, 15);
            enemyRunlHealth.ForeColor = Color.Red;
            this.Controls.Add(enemyRunlHealth);
        }
        private void moveRunEnemy()
        {
            enemyRunlHealth.Left = enemyRunMovement.Left - 10;
            enemyRunlHealth.Top = enemyRunMovement.Top + 60;
            if (enemyRunMovement.Left > playerMovement.Left)
            {
                enemyRunMovement.Left -= enemyRunSpeed;
            }
            if (enemyRunMovement.Left < playerMovement.Left)
            {
                enemyRunMovement.Left += enemyRunSpeed;
            }
            if (enemyRunMovement.Top > playerMovement.Top)
            {
                enemyRunMovement.Top -= enemyRunSpeed;
            }
            if (enemyRunMovement.Top < playerMovement.Top)
            {
                enemyRunMovement.Top += enemyRunSpeed;
            }
        }
        #endregion

        #region Level 3
        private void StartLevel3()
        {
            levelNumberStatus = 3;
            playerSpeed = 10;
            playerBulletSpeed = 10;
            enemySpeed = 5;
            enemyFireGenerationTime = 20;
            enemyFireCurrentGeneration = 0;
            playerHealth.Value = 100;
            reducePlayerHealth = 1;
            enemeyIdelListSpeed = 3;
            createEnemy();
            this.BackColor = Color.Silver;
        }
       
        private void detectPlayerCollisionRight()
        {
            foreach (PictureBox bullet in playerFireRight)
            {
                foreach(PictureBox enemy in enemyIdelList)
                {
                    if (bullet.Bounds.IntersectsWith(enemy.Bounds))
                    {
                        enemy.Visible = false;
                        score += 0.5F;
                    }
                }
                if (bullet.Bounds.IntersectsWith(enemyFire.Bounds))
                {
                    enemyFire.Visible = false;
                    enemyFireGenerationTime = -1;
                    score += 0.5F;
                }
            }
        }
        private void detectPlayerCollisionLeft()
        {
            foreach (PictureBox bullet in playerFireLeft)
            {
                foreach (PictureBox enemy in enemyIdelList)
                {
                    if (bullet.Bounds.IntersectsWith(enemy.Bounds))
                    {
                        enemy.Visible = false;
                        score += 0.5F;
                    }
                }
                if (bullet.Bounds.IntersectsWith(enemyFire.Bounds))
                {
                    enemyFire.Visible = false;
                    enemyFireGenerationTime = -1;
                    score += 0.5F;
                }
            }
        }
        private void detectPlayerCollisionUp()
        {
            foreach (PictureBox bullet in playerFireUp)
            {
                foreach (PictureBox enemy in enemyIdelList)
                {
                    if (bullet.Bounds.IntersectsWith(enemy.Bounds))
                    {
                        enemy.Visible = false;
                        score += 0.5F;
                    }
                }
                if (bullet.Bounds.IntersectsWith(enemyFire.Bounds))
                {
                    enemyFire.Visible = false;
                    enemyFireGenerationTime = -1;
                    score += 0.5F;
                }
            }
        }
        private void detectPlayerCollisionDown()
        {
            foreach (PictureBox bullet in playerFireDown)
            {
                foreach (PictureBox enemy in enemyIdelList)
                {
                    if (bullet.Bounds.IntersectsWith(enemy.Bounds))
                    {
                        enemy.Visible = false;
                        score += 0.5F;
                    }
                }
                if (bullet.Bounds.IntersectsWith(enemyFire.Bounds))
                {
                    enemyFire.Visible = false;
                    enemyFireGenerationTime = -1;
                    score += 0.5F;
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
                if (playerFireLeft[i].Left < -50)
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
                if (playerFireDown[i].Top < 0)
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
            bullet.Left = playerMovement.Left + 10;
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
            bullet.Left = playerMovement.Left + 10;
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
            bullet.Left = playerMovement.Left - 20;
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
            bullet.Top = playerMovement.Top + 40;
            playerFireRight.Add(bullet);
            this.Controls.Add(bullet);
        }
        private void detectEnemyFireCollision()
        {
            foreach (PictureBox bullet in enemyFireList)
            {
                if (bullet.Bounds.IntersectsWith(playerMovement.Bounds))
                {
                    if (playerHealth.Value > reducePlayerHealth)
                    {
                        playerHealth.Value -= reducePlayerHealth;
                    }
                }
            }
        }
        private void detectEnemyCollision()
        {
            if (enemyFire.Bounds.IntersectsWith(playerMovement.Bounds))
            {
                if (playerHealth.Value > reducePlayerHealth)
                {
                    playerHealth.Value -= reducePlayerHealth;
                }
               
            }
        }
        private void moveEnemyBullet()
        {
            //enemy bullet
            foreach (PictureBox bullet in enemyFireList)
            {
                bullet.Left += 10;
               
            }
        }
        private void removeEnemyBullet()
        {
            //Enemy Fire 
            for (int i = 0; i < enemyFireList.Count; i++)
            {
                if (enemyFireList[i].Left >= this.Width)
                {
                    enemyFireList.Remove(enemyFireList[i]);
                }
            }
        }
        private void creatEnemyBullet()
        {
            PictureBox bullet = new PictureBox();
            Image img = Vidyakali.Properties.Resources.laserEnemy;
            bullet.BackColor = Color.Transparent;
            bullet.Image = img;
            bullet.SizeMode = PictureBoxSizeMode.AutoSize;
            bullet.Left = enemyFire.Left;
            bullet.Top = enemyFire.Top + 40;
            enemyFireList.Add(bullet);
            this.Controls.Add(bullet);

        }
        private void moveEnemy()
        {
            //Enemy Movement
            if (enemyFire.Left <= 0)
            {
                enemyDirection = "right";
            }
            if (enemyFire.Left + enemyFire.Width >= this.Width)
            {
                enemyDirection = "left";
            }
            if (enemyDirection == "left")
            {
                enemyFire.Left -= random.Next(0, enemySpeed);
            }
            if (enemyDirection == "right")
            {
                enemyFire.Left += random.Next(0, enemySpeed);
            }
        }
        private void detectEnemyCollisionwithPlayer()
        {
            foreach(PictureBox enemy in enemyIdelList)
            {
                if (enemy.Bounds.IntersectsWith(playerMovement.Bounds))
                {
                    if (playerHealth.Value > reducePlayerHealth)
                    {
                        playerHealth.Value -= reducePlayerHealth;
                    }
                }
            }
           
        }
        private void detectCollisionofEnemyIdelList()
        {
            foreach (PictureBox enemy in enemyIdelList)
            { 
                foreach(PictureBox right in playerFireRight)
                {
                    if (enemy.Bounds.IntersectsWith(right.Bounds))
                    {
                        if (playerHealth.Value > reducePlayerHealth)
                        {
                            playerHealth.Value -= reducePlayerHealth;
                        }
                    }
                }
                foreach (PictureBox left in playerFireLeft)
                {
                    if (enemy.Bounds.IntersectsWith(left.Bounds))
                    {
                        if (playerHealth.Value > reducePlayerHealth)
                        {
                            playerHealth.Value -= reducePlayerHealth;
                        }
                    }
                }
                foreach (PictureBox up in playerFireUp)
                {
                    if (enemy.Bounds.IntersectsWith(up.Bounds))
                    {
                        if (playerHealth.Value > reducePlayerHealth)
                        {
                            playerHealth.Value -= reducePlayerHealth;
                        }
                    }
                }
                foreach (PictureBox down in playerFireDown)
                {
                    if (enemy.Bounds.IntersectsWith(down.Bounds))
                    {
                        if (playerHealth.Value > reducePlayerHealth)
                        {
                            playerHealth.Value -= reducePlayerHealth;
                        }
                    }
                }

            }
        }
        private void removeEnemyFromList()
        {
            for(int x = 0; x < enemyIdelList.Count; x++)
            {
                if (enemyIdelList[x].Visible == false)
                {
                    enemyIdelList.RemoveAt(x);
                    countDiedEnemy++;
                }
            }
        }
        private void moveEnemyIdelList()
        {
            foreach (PictureBox enemy in enemyIdelList )
            {
                if (enemy.Visible != false)
                {
                    if (enemy.Left > playerMovement.Left)
                    {
                        enemy.Left -= enemeyIdelListSpeed;
                    }
                    if (enemy.Left < playerMovement.Left)
                    {
                        enemy.Left += enemeyIdelListSpeed;
                    }
                    if (enemy.Top > playerMovement.Top)
                    {
                        enemy.Top -= enemeyIdelListSpeed;
                    }
                    if (enemy.Top < playerMovement.Top)
                    {
                        enemy.Top += enemeyIdelListSpeed;
                    }
                }
            }
        }
        private void genertaeEneyIdel()
        {
            if (enemyIdelList.Count < 2)
            {
                creatEnemyIdel();
            }
        }
        private void creatEnemyIdel()
        {
            PictureBox enemyidel = new PictureBox();
            Image img = Vidyakali.Properties.Resources.enemyIdel;
            enemyidel.Image = img;
            enemyidel.SizeMode = PictureBoxSizeMode.AutoSize;
            enemyidel.Top = random.Next(10, 300);
            enemyidel.Left = random.Next(10, 300);
            this.Controls.Add(enemyidel);
            enemyIdelList.Add(enemyidel);
            //Bullet
            PictureBox bullet = new PictureBox();
            Image img1 = Vidyakali.Properties.Resources.laserEnemy;
            bullet.BackColor = Color.Transparent;
            bullet.Image = img1;
            bullet.SizeMode = PictureBoxSizeMode.AutoSize;
            bullet.Left = enemyidel.Left;
            bullet.Top = enemyidel.Top + 40;
            enemyFireList.Add(bullet);
            this.Controls.Add(bullet);
        }

        private void createEnemy()
        {
            enemyFire = new PictureBox();
            Image img = Vidyakali.Properties.Resources.enemyIdel;
            enemyFire.Image = img;
            enemyFire.SizeMode = PictureBoxSizeMode.AutoSize;
            enemyFire.Top = random.Next(100, 200);
            enemyFire.Left = random.Next(100, this.Width - img.Width);
            this.Controls.Add(enemyFire);
            enemyDirection = "left";
        }
        #endregion

        #region Next Level 

        private void nexLevel()
        {
            if (enemyIdelHealth.Value <= 0 && enemyRunlHealth.Value <= 0)
            {//level 2 won
                nextLevelBox.Visible = true;
                boxOpeningLevel = 3;
                isLevel2 = false;
            }
            if (countDiedEnemy == 10)
            {
                nextLevelBox.Visible = true;
                boxOpeningLevel = 4;
                isLevel3 = false;
            }
        }
        private void gameOver()
        {
            if (playerHealth.Value <= 2)
            {
                if (pgbarPlayerLife.Value > 0)
                {
                    pgbarPlayerLife.Value -= 33;
                }
                playerHealth.Value = 100;
            }
            if (pgbarPlayerLife.Value <= 1)
            {
                this.Controls.Remove(playerMovement);
                this.Controls.Remove(playerHealth);
                gameStatus = false;
            }
        }

        private void detectBoxCollisionL1()
        {
            if (boxOpeningLevel == 2)
            {
                isLevel1 = true;
                if (playerMovement.Bounds.IntersectsWith(nextLevelBox.Bounds))
                {
                    nextLevelBox.Visible = false;
                    isLevel1 = false;
                    isLevel2 = true;
                    startLevel2();
                }
            }
        }
        private void detectBoxCollisionL2()
        {
            if (boxOpeningLevel == 3)
            {
                isLevel2 = true;
                if (playerMovement.Bounds.IntersectsWith(nextLevelBox.Bounds))
                {
                    nextLevelBox.Visible = false;
                    isLevel2 = false;
                    isLevel3 = true;
                    StartLevel3();
                }
            }
        }
        private void detectBoxCollisionL3()
        {
            if (boxOpeningLevel == 4) //4 mean won 
            {
                isLevel3 = true;
                if (playerMovement.Bounds.IntersectsWith(nextLevelBox.Bounds))
                {
                    nextLevelBox.Visible = false;
                    isLevel3 = false;
                    gameLoop.Enabled = false;
                    string message = "You Won";
                    frmEnd end = new frmEnd(message);
                    DialogResult result = end.ShowDialog();
                    if (result == DialogResult.Yes)
                    {
                        restartLevel1();
                    }
                    else
                    {
                        Close();
                    }
                }
            }
        }
        private void createnextLevel()
        {
            nextLevelBox = new PictureBox();
            nextLevelBox.Image = Vidyakali.Properties.Resources.boxOpen;
            nextLevelBox.SizeMode = PictureBoxSizeMode.AutoSize;
            nextLevelBox.Left = random.Next(25, 300);
            nextLevelBox.Top = random.Next(100, 300);
            this.Controls.Add(nextLevelBox);
        }
        #endregion
    }
}
