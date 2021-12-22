using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake2._0
{
    public partial class Form1 : Form
    {
        private int rX, rY;//Координаты фрукта
        private PictureBox fruit;//Фрукты
        private PictureBox[] snake = new PictureBox[400];//Змейка
        private Label labelScore;//Label счета
        private int dirX, dirY;//Движение по X Y
        private int _widht = 900;//Ширина окна
        private int _height= 800;//Высота окна
        private int _sizeOfSides = 40;//Разделение на клетки
        private int score = 0;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Snake";
            this.Width = _widht;
            this.Height = _height;

            dirX = 1;//Начальное направление змейки по X(1- вправо, -1- влево)
            dirY = 0;//Начальное направление замейки по Y(1-вниз, -1-вверх)

            labelScore = new Label();
            labelScore.Text = "Score: 0";
            labelScore.Location = new Point(810, 10);
            this.Controls.Add(labelScore);

            snake[0] = new PictureBox();
            snake[0].Location = new Point(201, 201);
            snake[0].Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
            snake[0].BackColor = Color.Red;
            this.Controls.Add(snake[0]);

            fruit = new PictureBox();
            fruit.BackColor = Color.Yellow;
            fruit.Size = new Size(_sizeOfSides, _sizeOfSides);

            GenerateMap();
            GenerateFruit();

            timer.Tick += new EventHandler(Update);
            timer.Interval = 200;
            timer.Start();
            this.KeyDown += new KeyEventHandler(OKP);        
        }

        /// <summary>
        /// Создание карты
        /// </summary>
        private void GenerateMap()
        {
            for(int i = 0; i < _widht / _sizeOfSides; i++)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.Black;
                pic.Location = new Point(0, _sizeOfSides * i);
                pic.Size = new Size(_widht - 100, 1);
                this.Controls.Add(pic);
            }
            for (int i = 0; i < _height / _sizeOfSides; i++)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.Black;
                pic.Location = new Point(_sizeOfSides * i,0);
                pic.Size = new Size(1,_widht);
                this.Controls.Add(pic);
            }
        }

        /// <summary>
        /// Создание фруктов
        /// </summary>
        private void GenerateFruit()
        {
            Random r = new Random();
            rX = r.Next(0, _height - _sizeOfSides);
            int tempI = rX % _sizeOfSides;
            rX -= tempI;
            rY = r.Next(0, _height - _sizeOfSides);
            int tempJ = rY % _sizeOfSides;
            rY -= tempJ;
            rX++;
            rY++;
            fruit.Location = new Point(rX, rY);
            this.Controls.Add(fruit);
        }

        /// <summary>
        /// Обновление
        /// </summary>
        /// <param name="myObject"></param>
        /// <param name="eventArgs"></param>
        private void Update(Object myObject, EventArgs eventArgs)
        {
            CheckBorder();
            EatFruit();
            MoveSnake();
        }

        /// <summary>
        /// Проверка на стены
        /// </summary>
        private void CheckBorder()
        {
            if(snake[0].Location.X < 0)
            {
                for(int i = 1; i <=score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                labelScore.Text = "Score: " + score;
                dirX = 1;
            }
            if (snake[0].Location.X > _height)
            {
                for (int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                labelScore.Text = "Score: " + score;
                dirX = -1;
            }
            if (snake[0].Location.Y < 0)
            {
                for (int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                labelScore.Text = "Score: " + score;
                dirY = 1;
            }
            if (snake[0].Location.Y > _height )
            {
                for (int i = 1; i <= score; i++)
                {
                    this.Controls.Remove(snake[i]);
                }
                score = 0;
                labelScore.Text = "Score: " + score;
                dirY = -1;
            }
        }


        /// <summary>
        /// Поедание Фруков
        /// </summary>
        private void EatFruit()
        {
            if(snake[0].Location.X == rX && snake[0].Location.Y == rY)
            {
                labelScore.Text = "Score: " + ++score;
                snake[score] = new PictureBox();
                snake[score].Location = new Point(snake[score - 1].Location.X + 40 * dirX, snake[score - 1].Location.Y - 40 * dirY);
                snake[score].Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
                snake[score].BackColor = Color.Red;
                this.Controls.Add(snake[score]);
                GenerateFruit();
            }
        }

        /// <summary>
        /// Поедание себя
        /// </summary>
        private void EarItSelf()
        {
            for(int i = 1; i < score; i ++)
            {
                if(snake[0].Location == snake[i].Location)
                {
                    for(int j = i; j <=score; j++)
                    {
                        this.Controls.Remove(snake[j]);
                    }
                    score = score - (score - i + 1);
                    labelScore.Text = "Score: " + score;
                }
            }
        }
        
        /// <summary>
        /// Движения змейки
        /// </summary>
        private void MoveSnake()
        {
            for(int i = score; i>=1; i--)
            {
                snake[i].Location = snake[i - 1].Location;
            }
            snake[0].Location = new Point(snake[0].Location.X + dirX * (_sizeOfSides), snake[0].Location.Y + dirY * (_sizeOfSides));
            EarItSelf();
        }

        /// <summary>
        /// Клавиши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKP(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "Right":
                    dirX = 1;
                    dirY = 0;
                    break;
                case "Left":
                    dirX = -1;
                    dirY = 0;
                    break;
                case "Up":
                    dirY = -1;
                    dirX = 0;
                    break;
                case "Down":
                    dirY = 1;
                    dirX = 0;
                    break;
            }
        }
    }
}
