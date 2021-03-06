﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.IO;

namespace CatchMeIfYouCan
{

    public partial class CatchMeIfYouCan : Form
    {
        public const int picSize = 80;
        MainAction mainAction = null;
        private Graphics BitmapGraph;
        public PictureBox MainFrame;
        Bitmap Frame;
        Graphics PictureBoxGraph;

        public CatchMeIfYouCan()
        {
            InitializeComponent();
            MainFrame = pictureBox1;
            Frame = new Bitmap(MainFrame.Width, MainFrame.Height);
            PictureBoxGraph = MainFrame.CreateGraphics();
        }
      
        private void StartGameButton_Click_EventHandler(object sender, EventArgs e)
        {
            if (mainAction != null)
            {
                mainAction.EventEnemyCountChange -= new DelegateEnemyEventHandler(EnemyCountChange_EventHandler);
                mainAction.EventPlantCountChange -= new DelegatePlantEventHandler(PlantCountChange_EventHandler);
            }
            this.mainAction = new MainAction(this.start, this.Frame, this.PictureBoxGraph, this.BitmapGraph, (int)numericUpDown1.Value, (int)numericUpDown2.Value);

            mainAction.EventEnemyCountChange += new DelegateEnemyEventHandler(EnemyCountChange_EventHandler);
            mainAction.EventPlantCountChange += new DelegatePlantEventHandler(PlantCountChange_EventHandler);
            mainAction.EventEndGame += new DelegateEndEventHandler(EndGame_EventHandler);
            mainAction.StartGame();

            pictureBox1.Focus();
        }

        private void EnemyCountChange_EventHandler(int sender)
        {
            textBoxEnemyCount.Invoke(new Action(() => textBoxEnemyCount.Text = sender.ToString()));
        }
       
        private void PlantCountChange_EventHandler(int sender)
        {
            textBoxPlantCount.Invoke(new Action(() => textBoxPlantCount.Text = sender.ToString()));
        }

        private void StartGameButton_Enter_EventHandler(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }

        private void textBoxPlantCount_Enter_EventHandler(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }

        private void textBoxEnemyCount_Enter_EventHandler(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }

        private void MainFrame_KeyDown_EventHandler(object sender, PreviewKeyDownEventArgs e)
        {
            int currentKey = e.KeyValue;
            switch (currentKey)
            {
                case 37:    //левая
                    {
                        mainAction.Move('L');
                        break;
                    }
                case 39:    //правая
                    {
                        mainAction.Move('R');
                        break;
                    }
                case 38:    //вперед
                    {
                        mainAction.Move('W');
                        break;
                    }
                case 40:    //вниз
                    {
                        mainAction.Move('S');
                        break;
                    }
                case 32:    //space
                    {
                        mainAction.Move('K');
                        break;
                    }
            }


        }
        private void numericUpDown1_Enter_EventHandler(object sender, EventArgs e) //N
        {
            pictureBox1.Focus();
        }
        private void numericUpDown2_Enter_EventHandler(object sender, EventArgs e) //N
        {
            pictureBox1.Focus();
        }

        public void EndGame_EventHandler(object sender, MyEventArg Arg)
        {
            string str="";
            if (Arg.mainAction.EnemyCount == 0) str = "Congratulate! You win!";
            if (Arg.mainAction.PlantCount == 0) str = "Sorry! You lost!";
            if (MessageBox.Show(str + "\nPlay again??", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Arg.mainAction.Clearing();
                Arg.mainAction.StartGame();
            }
        }

        private void textBoxEnemyCount_TextChanged(object sender, EventArgs e)
        {

        }

    }

}
