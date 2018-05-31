using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CatchMeIfYouCan
{
    class Counts
    {
        public const int picSize = 80;
        private static Image[] gallery;
        
        public static Image[] GetGallery() 
        {
            if (gallery == null)
            {
                gallery = new Image[3];
                gallery[0] = Image.FromFile(@"cell_food.png"); // 1 stage
                gallery[1] = Image.FromFile(@"cell_grace.png"); // 2 stage
                gallery[2] = Image.FromFile(@"cell_digged.png"); // 3 stage
            }
            return gallery;
        }
    }

    class GameField : Counts // Field - поле, Cell - клетка
    {
        public Cell[,] game_field; 
        public int field_sizeI;
        public int field_sizeJ;
        public int plantCount;

        public GameField(int field_sizeI, int field_sizeJ)
        { 
            this.field_sizeI = field_sizeI;
            this.field_sizeJ = field_sizeJ;
            this.game_field = new Cell[field_sizeI, field_sizeJ];
            this.plantCount = (int) (field_sizeI * field_sizeJ / 3);
        }

        private Point Random(int key)
        {
            Random rand1 = new Random((int)(key * DateTime.Now.Ticks));
            Random rand2 = new Random(key * rand1.Next(- field_sizeI, field_sizeI));

            int i = Math.Abs(rand1.Next(-field_sizeI+1, field_sizeI-1));
            int j = Math.Abs(rand2.Next(-field_sizeJ+1, field_sizeJ-1));

            Point point = new Point(i, j);

            return point;
        }

        private void RandomPlants()
        {
            Point point;

            for (int t = 0; t < plantCount; t++)
            {
                do
                    point = Random((int)DateTime.Now.Ticks);
                while (game_field[point.X, point.Y].IsPlant);

                game_field[point.X, point.Y] = new Cell(1, 0, 0, 0);
            }
        }

        public void Game_Field_Init()
        {
            for (int i = 0; i < field_sizeI; i++)
            {
                for (int j = 0; j < field_sizeJ; j++)
                {
                    game_field[i, j] = new Cell(0, 1, 0, 0);
                }
            }
            RandomPlants();
        }

        public void Game_Field_Paint(Graphics PictureBoxGraph, Graphics BitmapGraph, Bitmap Frame)
        {
            try
            {
                for (int i = 0; i < field_sizeI; i++)
                {
                    for (int j = 0; j < field_sizeJ; j++)
                    {
                        BitmapGraph.DrawImage(game_field[i, j].image, i * picSize, j * picSize, picSize, picSize);
                    }
                }
                PictureBoxGraph.DrawImage(Frame, 0, 0);
            }
            catch (Exception e)
            { 
            
            }
        }
    }

    class Main
    {
        public const int picSize = 80;
        public Image image;
        public Point location;
    }

    class Cell : Main
    {
        public bool IsPlant = false; // 1 stge 
        public bool IsGrace = false; // 2 stage
        public bool IsEatten = false; // 3 stage
        public bool IsOccupied = false;

        public Cell(int IsPlant, int IsGrace, int IsEatten, int IsOccurpied) 
        {
            if (IsPlant == 1) { this.IsPlant = true; this.image = Counts.GetGallery()[0]; } 
            if (IsGrace == 1) { this.IsGrace = true; this.image = Counts.GetGallery()[1]; }
            if (IsEatten == 1) { this.IsEatten = true; this.image = Counts.GetGallery()[2]; }
            if (IsOccurpied == 1) { this.IsOccupied = true; }
        }
    }

    class Enemy : Main
    {
        public bool IsAlive = true;

        public Enemy(int game_fieldI, int game_fieldJ)
        {
            this.image = Image.FromFile(@"enemy.png");
            this.location = RandomLocation(game_fieldI, game_fieldJ);
            this.IsAlive = true;
        }

        public static Point RandomLocation(int game_fieldI, int game_fieldJ)
        {

            Random rand1 = new Random((int)DateTime.Now.Ticks);
            Random rand2 = new Random((int)DateTime.Now.Millisecond * rand1.Next(- game_fieldI, game_fieldJ));

            int i = Math.Abs(rand1.Next(0, game_fieldI));
            int j = Math.Abs(rand2.Next(0, game_fieldJ));

            Point point = new Point(i * picSize, j * picSize);

            return point;
        }

        public void Enemy_Paint(Graphics PictureBoxGraph, Graphics BitmapGraph, Bitmap Frame, int X, int Y)
        {
            bool u = false;
            
            do
            {
                try
                {
                    BitmapGraph.DrawImage(image, X, Y, picSize, picSize);
                    PictureBoxGraph.DrawImage(Frame, 0, 0);
                    u = true;
                }
                catch (InvalidOperationException)//Экзепшн который вылетает при допросе
                {
                    System.Threading.Thread.Sleep(1);
                    u = false;
                }
            }
            while (!u);
        }
    }

    class Man : Main
    {
        public Man()
        {
            this.location = new Point(0, 0);
            this.image = Image.FromFile(@"man.PNG");
        }

        public void Man_Paint(Graphics PictureBoxGraph, Graphics BitmapGraph, Bitmap Frame, int X, int Y)
        {
            try
            {
                BitmapGraph.DrawImage(image, X, Y, picSize, picSize);
                try
                {
                    PictureBoxGraph.DrawImage(Frame, 0, 0);
                }
                catch (Exception e)
                { }
            }
            catch (InvalidOperationException)//Экзепшн который вылетает при допросе
            {
                System.Threading.Thread.Sleep(5);
            }
        }
    }
}
