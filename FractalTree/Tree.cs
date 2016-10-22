using System;
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
    public class Tree
    {
        public float x, y, length, alpha; // first main branch's coordinates, length and angle
        public float k1, k2, k3; // branch lengths' coeficients
        public float m2, m3; // branches' position factors
        public float a, b, c; // branches' position factors
        public float minlength; // min length of a branch
        public Pen leafPen; // pen for tree's leaf

        public Tree(float x, float y, float length, float k1, float k2, float k3, float m2, float m3, float a, float b, float c, float alpha, Pen leafPen)
        {
            this.x = x; this.y = y; this.length = length;
            this.k1 = k1; this.k2 = k2; this.k3 = k3;
            this.m2 = m2; this.m3 = m3;
            this.a = a; this.b = b; this.c = c;
            this.minlength = 3F; this.alpha = alpha;
            this.leafPen = leafPen;
        } // constructor

        public void DrawTree(Graphics graph, int depth, float x, float y, float length, float alpha)
        {
            Pen brownPen = new Pen(Color.FromArgb(255, 191 + depth * 3, 82, 60));
            brownPen.Width = depth / 1.8F;
            // color and thickness of the branch is changing in accordance with recursion's level

            float x1 = (float)(x - length * Math.Cos(alpha)); // end-point, x
            float y1 = (float)(y - length * Math.Sin(alpha)); // end-point, y

            float branch_length = (float)(Math.Sqrt((x1 - x) * (x1 - x) + (y1 - y) * (y1 - y))); // abs length of branch

            if (branch_length < minlength)
            {
                brownPen.Width = 1F;
            } // if branch's length is less than given minimal length its minimal width is 1F

            if (branch_length >= minlength)
            {
                graph.DrawLine(brownPen, x, y, x1, y1);
            } // draws a line if branch's length is higher than given minimal length or equals it

            if (depth == 1 || branch_length < minlength)
            {
                graph.DrawLine(brownPen, x, y, x1, y1);
                Brush leafBrush = leafPen.Brush;
                graph.FillEllipse(leafBrush, x1, y1, 4.5F, 6.0F);
            } // draws a leaf if recursion's limit has been reached or branch has the min length

            float angleA = alpha + a; // angle for the main branch
            float angleB = alpha + b; // angle for the first sub-branch
            float angleC = alpha + c; // angle for the second sub-branch

            if (depth > 1 && branch_length >= minlength)
            {
                float x2 = x1 + m2 * k1 * length * (float)(Math.Cos(alpha));
                float y2 = y1 + m2 * k1 * length * (float)(Math.Sin(alpha));
                // coordinates of point that divides main branch, start-point for the first sub-branch

                float x3 = x1 + m3 * k1 * length * (float)(Math.Cos(alpha));
                float y3 = y1 + m3 * k1 * length * (float)(Math.Sin(alpha));
                // coordinates of point that divides main branch, start-point for the second sub-branch

                // stackoverflow.com/questions/22190193/finding-coordinates-of-a-point-on-a-line

                DrawTree(graph, depth - 1, x1, y1, length * k1, angleA);
                DrawTree(graph, depth - 1, x2, y2, length * k2, angleB);
                DrawTree(graph, depth - 1, x3, y3, length * k3, angleC);
            } // callback of the recursive function
        } // recursive function that draws a tree
    }
}
