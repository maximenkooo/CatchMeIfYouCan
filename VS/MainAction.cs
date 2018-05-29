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
    public delegate void DelegatePlantEventHandler(int sender);

    public class MainAction
    {
        public event DelegatePlantEventHandler EventPlantCountChange;

        private System.Threading.Timer Tms;
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
           
        }

        private void EnemyDisappear(Object enemy)
        {
           
        }

        private void Relocation(Enemy enemy)
        {
            
        }

        private void AllEnemiesPaint()
        {
            
        }

        public void Clearing()
        {
            DeadLockComingSoon.Clear();
            foreach (System.Threading.Timer Dead in DeadLockComingSoon) Dead.Dispose();
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
