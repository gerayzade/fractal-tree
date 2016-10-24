using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FractalTree
{
    public partial class FormMain : Form
    {
        public Tree Tree1, Tree2;

        public FormMain()
        {
            InitializeComponent();
            DoubleBuffered = true;

            Pen greenPen = new Pen(Color.Green);
            Tree1 = new Tree(pictureBox1.Width / 2.0F, pictureBox1.Height, pictureBox1.Height / 3.5F,
                0.6F, 0.7F, 0.7F, 0.6F, 0.3F, -0.3F, 0.6F, -0.7F, 1.77F, greenPen);
            // instance of a tree object (x, y, length, k1, k2, k3, m2, m3, a, b, c, alpha, leafBrush)
        }

        public Boolean treeWasDrawn = false; // boolean - if the tree was drawn
        public Boolean forestWasDrawn = false; // boolean - if the forest was drawn

        public static int depth_num = 8; // default value of recursion depth

        private void drawGraphics(Tree objTree)
        {
            pictureBox1.SetBounds(0, 0, Width, Height); // sets parent size for pictureBox1
            Graphics graph = pictureBox1.CreateGraphics(); // creating...
            if (checkBox1.Checked == false)
            {
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics gdraw = Graphics.FromImage(bmp);
                gdraw.SetClip(new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
                objTree.DrawTree(gdraw, depth_num, objTree.x, objTree.y, objTree.length, objTree.alpha);
                graph.DrawImage(bmp, 0, 0);
                gdraw.Dispose();
                // this will create graphics first in memory
            }
            else
            {
                objTree.DrawTree(graph, depth_num, objTree.x, objTree.y, objTree.length, objTree.alpha);
            } // and here comes the tree
            graph.Dispose();
        } // sets picturebox1 size to window's size and draws a tree

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            pictureBox1.Refresh();
            drawGraphics(Tree1);
            treeWasDrawn = true;
            forestWasDrawn = false;
        } // mouse click listener on button1, draws (redraws) a tree

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = "Lvl: " + trackBar1.Value;
            depth_num = trackBar1.Value;
        } // listener on trackbar1

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox1.Text = "Drawing Process: ON";
            }
            else
            {
                checkBox1.Text = "Drawing Process: OFF";
            }
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
            pictureBox1.SetBounds(0, 0, Width, Height);
            pictureBox2.SetBounds(Width - 70, 12, 40, 40);
            button1.SetBounds(12, Height - 74, 75, 23);
        } // sets picturebox1 size to window's size while window is resizing

        private void FormMain_ResizeEnd(object sender, EventArgs e)
        {
            if (treeWasDrawn && !forestWasDrawn)
            {
                Tree1.x = pictureBox1.Width / 2.0F;
                Tree1.y = pictureBox1.Height;
                Tree1.length = pictureBox1.Height / 3.5F;
                drawGraphics(Tree1);
            }
            else if (!treeWasDrawn && forestWasDrawn)
            {
                runForestrun();
            }
        }  // redraws a tree when the window is resized

        private void runForestrun()
        {
            checkBox1.CheckState = CheckState.Unchecked; // for higher perfomance
            forestWasDrawn = true;
            treeWasDrawn = false;

            Pen[] pens = new Pen[11]; // array of pens with different colors
            pens[0] = new Pen(Color.Green);
            pens[1] = new Pen(Color.DarkOliveGreen);
            pens[2] = new Pen(Color.OliveDrab);
            pens[3] = new Pen(Color.MediumSeaGreen);
            pens[4] = new Pen(Color.ForestGreen);
            pens[5] = new Pen(Color.DarkGreen);
            pens[6] = new Pen(Color.LawnGreen);
            pens[7] = new Pen(Color.SpringGreen);
            pens[8] = new Pen(Color.LightGreen);
            pens[9] = new Pen(Color.Olive);
            pens[10] = new Pen(Color.MediumSpringGreen);

            Random random = new Random();

            for (int i = 0; i < random.Next(8, Width / 18); i++)
            {
                drawGraphics(new Tree(random.Next(50, Width - 50), random.Next(150, Height), random.Next(50, 90),
                    (float)random.Next(5, 7) / 10, (float)random.Next(4, 9) / 10, (float)random.Next(4, 9) / 10,
                    (float)random.Next(5, 8) / 10, (float)random.Next(3, 5) / 10,
                    (float)random.Next(-4, -2) / 10, (float)random.Next(5, 7) / 10, (float)random.Next(-8, -5) / 10, 1.57F,
                    pens[random.Next(0, 10)]));
            } // drawing random amount of trees with random parameteres

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            runForestrun();
        } // listener on pictureBox2, run Forest run

        private void FormMain_HelpButtonClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.gerayzade.me/apps/fractal-tree");
        } // listener on helpbutton, runs official web-page
    }
}
