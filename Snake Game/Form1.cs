using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging; // add this for the JPG compressor used to compress a bitmap screenshot that we take off the icbox into a jpg file

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();//when u make a new inst ,all of {}'s class will be refrences inside z memory
        int maxWidth;//allowed to travel
        int maxHeight;
        int score;
        int highScore;
        Random rand = new Random();//to create random numbers
        bool goLeft, goRight, goDown, goUp;//they're initially instantiated with false value
       
        public Form1()
        {
            InitializeComponent();
            //we can call it directly as it has static vars
            new Settings();
            //as soon as the form initializes i need to create an instance of the settings class
            //so we have z access to all z static vars & all z vals that,ve been set inside it.
            
        }
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && Settings.directions != "right")//as i cant go left when im right
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Settings.directions != "left")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && Settings.directions != "down")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down && Settings.directions != "up")
            {
                goDown = true;
            }
        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }
        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }
        private void TakeSnapShot(object sender, EventArgs e)
        {
            Label caption = new Label();
            caption.Text = "I scored: " + score + " and my Highscore is " + highScore + " on the Snake Game";
            caption.Font = new Font("Ariel", 12, FontStyle.Bold);
            caption.ForeColor = Color.Purple;
            caption.AutoSize = false;
            caption.Width = picCanvas.Width;
            caption.Height = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);

            //create a new save dialog box
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Snake Game";
            dialog.DefaultExt = "jpg";//the default file type i wanna save it as
            dialog.Filter = "JPG Image File | *.jpg";//what gives us the option to choose a file type & we don't wanna choose any other type(keep it as a jpg file img)
            dialog.ValidateNames = true;//u can't accidentally put a comma in z filename. that might cause issues with windows & so on


            //what to do when u click ok on the dialog box
            if (dialog.ShowDialog() == DialogResult.OK)//if i clicked save
            {
                //size of z pic
                int width = Convert.ToInt32(picCanvas.Width);
                int height = Convert.ToInt32(picCanvas.Height);
                Bitmap bmp = new Bitmap(width, height);
                picCanvas.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));//draw everything from canvas to bitmap
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                picCanvas.Controls.Remove(caption);
            }
        }
        private void GameTimerEvent(object sender, EventArgs e)
        {
            // setting the directions
            if (goLeft)
            {
                Settings.directions = "left";
            }
            if (goRight)
            {
                Settings.directions = "right";
            }
            if (goDown)
            {
                Settings.directions = "down";
            }
            if (goUp)
            {
                Settings.directions = "up";
            }
            // end of directions
            for (int i = Snake.Count - 1; i >= 0; i--)//to make sure that all body parts r moving
            {
                //by having that we're ensuring that ther's always a child present inside of the program
                //without  -1 it's calculating an empty one

                //the head is gonna set the direction for rest of the body
                if (i == 0)//head
                {
                    switch (Settings.directions)
                    {
                        case "left":
                            Snake[i].X--;
                            break;
                        case "right":
                            Snake[i].X++; //adds 1 to z pos
                            break;
                        case "down":
                            Snake[i].Y++;
                            break;
                        case "up":
                            Snake[i].Y--;
                            break;
                    }
                    //set z boundries
                    //tell the snake to appear on the other side when it reaches the edge of screen
                    if (Snake[i].X < 0)
                    {
                        Snake[i].X = maxWidth;
                    }
                    if (Snake[i].X > maxWidth)
                    {
                        Snake[i].X = 0;
                    }
                    if (Snake[i].Y < 0)
                    {
                        Snake[i].Y = maxHeight;
                    }
                    if (Snake[i].Y > maxHeight)
                    {
                        Snake[i].Y = 0;
                    }

                    if (Snake[i].X == food.X && Snake[i].Y == food.Y)
                    {
                        EatFood();
                    }
                    for (int j = 1; j < Snake.Count; j++)//if it hits itself
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            //i --> snake head
                            //j --> snake body
                            GameOver();
                        }
                    }
                }
                else
                {
                    //as it's incrementing , the last body part..one body part to follow z other parts in 
                    //the list so it looks like it's scrolling up the screen or down z screen
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
            picCanvas.Invalidate();
            //so with each tick it's gonna clear everything off from the
            //canvas and it will be redrawn
        }
        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            //this is the paint event that's linked to the picbox
            Graphics canvas = e.Graphics;//that's linking that paint event to that canvas
            Brush snakeColour;//to color snake's head & body
            for (int i = 0; i < Snake.Count; i++)//main loop for the snake's head & body
            {
                if (i == 0)//head 
                {
                    snakeColour = Brushes.Black;
                }
                else
                {
                    snakeColour = Brushes.DarkGreen;
                }
                canvas.FillEllipse(snakeColour, new Rectangle
                    (
                    Snake[i].X * Settings.Width,
                    Snake[i].Y * Settings.Height,
                    Settings.Width,
                    Settings.Height
                    )
                    );
            }
            canvas.FillEllipse(Brushes.DarkRed, new Rectangle
            (
            food.X * Settings.Width,
            food.Y * Settings.Height,
            Settings.Width, Settings.Height
            ));
        }

     

        private void RestartGame()
        {
            //default values before the game starts
            maxWidth = picCanvas.Width / Settings.Width - 1;//a little bit of padding for the snake so doesn't go too close to z edge so  i can control z snake and see when it's by edge & then for it to appear on z other side 
            maxHeight = picCanvas.Height / Settings.Height - 1;
            Snake.Clear();
            startButton.Enabled = false;
            snapButton.Enabled = false;
            score = 0;
            txtScore.Text = "Score: " + score;
            Circle head = new Circle { X = 10, Y = 5 };//{}to access z properities
            Snake.Add(head); // adding the head part of the snake to the list
            for (int i = 0; i < 3; i++)
            {
                Circle body = new Circle();
                Snake.Add(body);
                //it will follow the head where moving
                //every new object will follow the list's last object
            }
            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
            gameTimer.Start();//so it starts ticking
        }
        private void EatFood()
        {
            score += 1;
            txtScore.Text = "Score: " + score;
            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,//so we'll get to z last index of it
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(body);
            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
        }
        private void GameOver()
        {
            gameTimer.Stop();
            startButton.Enabled = true;
            snapButton.Enabled = true;

            if (score > highScore)
            {
                highScore = score;
                txtHighScore.Text = "High Score: " + Environment.NewLine + highScore;
                txtHighScore.ForeColor = Color.Maroon;
                txtHighScore.TextAlign = ContentAlignment.MiddleCenter;
            }
        }
    }
}