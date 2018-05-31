using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CatchMeIfYouCan
{
    public delegate void DelegateEnemyEventHandler(int sender); 
    public delegate void DelegatePlantEventHandler(int sender);

    public class MainAction
    {
        public event DelegateEnemyEventHandler EventEnemyCountChange; 
        public event DelegatePlantEventHandler EventPlantCountChange;

        private System.Threading.Timer Tms;
        private System.Threading.Timer Eatting;
        static List<System.Threading.Timer> DeadLockComingSoon = new List<System.Threading.Timer>();

        private Graphics PictureBoxGraph;
        private Graphics BitmapGraph;
        private Bitmap Frame;

        private Man Man = new Man();
        private GameField GameField;

        public int EnemyCount = 0;
        public int EnemyCreated = 0;
        public int PlantCount = 0;

        private const int picSize = 80;

        List<Enemy> EnemyList = new List<Enemy>();

        public MainAction(Button startGameButton, Bitmap Frame, Graphics PictureBoxGraph, Graphics BitmapGraph, int gameFieldI, int gamefieldJ) //Конструктор
        {
            this.GameField = new GameField(gameFieldI, gamefieldJ);
            this.PlantCount = this.GameField.plantCount;
            this.PictureBoxGraph = PictureBoxGraph;
            this.BitmapGraph = BitmapGraph;
            this.Frame = Frame;
        }

        private void InitializeMyComponent()
        {
            GameField.Game_Field_Init();
            BitmapGraph = Graphics.FromImage(Frame);
            BitmapGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        }

        private void Killing(Man man)
        {

        }

        private void EnemyAppear(object jbj)
        {
            if (EnemyCreated == 49)
            {
                Tms.Dispose();
            }
            else
            {
                Enemy enemy = new Enemy(GameField.field_sizeI, GameField.field_sizeJ);

                EnemyCreated++;
                EnemyCount++;
                EventEnemyCountChange(EnemyCount);

                EnemyList.Add(enemy);
                Relocation(enemy);

                enemy.Enemy_Paint(PictureBoxGraph, BitmapGraph, Frame, enemy.location.X, enemy.location.Y);

                GameField.game_field[(int)(enemy.location.X / picSize), (int)(enemy.location.Y / picSize)].IsOccupied = true;
                Object obj = enemy;

                System.Threading.Timer Eatting = new System.Threading.Timer(EnemyDisappear, obj, 5000, 3000); // Вызываем таймер съедения растений

                DeadLockComingSoon.Add(Eatting);
            }
        }

        private void EnemyDisappear(Object enemy)
        {
            Enemy tempEnemy = (enemy as Enemy);

            if (tempEnemy.IsAlive)
            {
                GameField.game_field[(int)(tempEnemy.location.X / picSize), (int)(tempEnemy.location.Y / picSize)].image = Image.FromFile(@"cell_digged.png");

                if (GameField.game_field[(int)(tempEnemy.location.X / picSize), (int)(tempEnemy.location.Y / picSize)].IsPlant)
                    PlantCount--;
                EventPlantCountChange(PlantCount);
                GameField.game_field[(int)(tempEnemy.location.X / picSize), (int)(tempEnemy.location.Y / picSize)].IsEatten = true;

                if ((PlantCount == 0))

                {
                }

                if (PlantCount != 0)
                {
                    Point oldXY = tempEnemy.location;
                    Relocation(tempEnemy);/// рандомить вредителя на новую клетку                    

                    GameField.game_field[oldXY.X / picSize, oldXY.Y / picSize].IsOccupied = false;
                    GameField.Game_Field_Paint(PictureBoxGraph, BitmapGraph, Frame);

                    AllEnemiesPaint();

                    Man.Man_Paint(PictureBoxGraph, BitmapGraph, Frame, Man.location.X, Man.location.Y);
                }
            }
            else DeadLockComingSoon.Remove(Eatting);
        }

        private void Relocation(Enemy enemy)
        {
            enemy.location = Enemy.RandomLocation(GameField.field_sizeI, GameField.field_sizeJ);

            while ((GameField.game_field[(int)(enemy.location.X / picSize), (int)(enemy.location.Y / picSize)].IsOccupied)
                    || (GameField.game_field[(int)(enemy.location.X / picSize), (int)(enemy.location.Y / picSize)].IsEatten))
            {
                enemy.location = Enemy.RandomLocation(GameField.field_sizeI, GameField.field_sizeJ);
            }
            GameField.game_field[(int)(enemy.location.X / picSize), (int)(enemy.location.Y / picSize)].IsOccupied = true;
        }

        private void AllEnemiesPaint()
        {
            foreach (Enemy el in EnemyList)
            {
                bool flag = false;

                do
                {
                    try
                    {
                        if (el.IsAlive)
                        {
                            BitmapGraph.DrawImage(el.image, el.location.X, el.location.Y, picSize, picSize);
                        }
                        flag = true;
                    }
                    catch (InvalidOperationException)
                    {
                        System.Threading.Thread.Sleep(5);
                    }
                }
                while (flag == false);
            }
            try
            {
                PictureBoxGraph.DrawImage(Frame, 0, 0);
            }
            catch (Exception e)
            {

            }
        }

        public void Clearing()
        {
            EnemyList.Clear();
            DeadLockComingSoon.Clear();
            foreach (System.Threading.Timer Dead in DeadLockComingSoon) Dead.Dispose();
            EnemyCreated = 0;
            EnemyCount = 0;
            PlantCount = (int)(GameField.field_sizeI * GameField.field_sizeJ / 3);

        }

        public void StartGame()
        {
            InitializeMyComponent();

            GameField.Game_Field_Paint(PictureBoxGraph, BitmapGraph, Frame);
            Man.Man_Paint(PictureBoxGraph, BitmapGraph, Frame, 0, 0);
            EventPlantCountChange(PlantCount);
            Tms = new System.Threading.Timer(EnemyAppear, null, 1000, 1000);
        }

        public void Move()
        {
            
        }
    }
}
