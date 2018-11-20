using System;
using System.Collections.Generic;
using System.Drawing;

namespace SystAnalys_lr1
{
    public class Vertex
    {
        public int x, y;

        public Vertex(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Weight
    {
        public int v1, v2;
        public String value;

        public Weight(int v1, int v2, String value)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.value = value;
        }
    }

    public class Edge
    {
        public int v1, v2;

        public Edge(int v1, int v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }
    }

    public class DrawGraph
    {
        Bitmap bitmap;
        Pen blackPen;
        Pen redPen;
        Pen darkGoldPen;
        Graphics gr;
        Font fo;
        Brush br;
        PointF point;
        public int R = 20; //радиус окружности вершины

        public DrawGraph(int width, int height)
        {
            bitmap = new Bitmap(width, height);
            gr = Graphics.FromImage(bitmap);
            clearSheet();
            blackPen = new Pen(Color.Black);
            blackPen.Width = 2;
            redPen = new Pen(Color.Red);
            redPen.Width = 2;
            darkGoldPen = new Pen(Color.DarkGoldenrod);
            darkGoldPen.Width = 2;
            fo = new Font("Arial", 15);
            br = Brushes.Black;
        }

        public struct Subset
        {
            public int Parent;
            public int Rank;
        }

        public Bitmap GetBitmap()
        {
            return bitmap;
        }

        public void clearSheet()
        {
            gr.Clear(Color.White);
        }

        public void drawVertex(int x, int y, string number)
        {
            gr.FillEllipse(Brushes.White, (x - R), (y - R), 2 * R, 2 * R);
            gr.DrawEllipse(blackPen, (x - R), (y - R), 2 * R, 2 * R);
            point = new PointF(x - 9, y - 9);
            gr.DrawString(number, fo, br, point);
        }

        public void drawSelectedVertex(int x, int y)
        {
            gr.DrawEllipse(redPen, (x - R), (y - R), 2 * R, 2 * R);
        }

        public void drawEdge(Vertex V1, Vertex V2, Edge E, int numberE, Weight weight)
        {
            if (E.v1 == E.v2)
            {
                gr.DrawArc(darkGoldPen, (V1.x - 2 * R), (V1.y - 2 * R), 2 * R, 2 * R, 90, 270);
                point = new PointF(V1.x - (int)(2.75 * R), V1.y - (int)(2.75 * R));
                gr.DrawString((weight.value).ToString(), fo, br, point);
                drawVertex(V1.x, V1.y, (E.v1 + 1).ToString());
            }
            else
            {
                gr.DrawLine(darkGoldPen, V1.x, V1.y, V2.x, V2.y);
                point = new PointF((V1.x + V2.x) / 2, (V1.y + V2.y) / 2);
                gr.DrawString((weight.value).ToString(), fo, br, point);
                drawVertex(V1.x, V1.y, (E.v1 + 1).ToString());
                drawVertex(V2.x, V2.y, (E.v2 + 1).ToString());
            }
        }

        public void drawALLGraph(List<Vertex> v, List<Edge> e, List<Weight> w)
        {
            //рисуем ребра
            for (int i = 0; i < e.Count; i++)
            {
                if (e[i].v1 == e[i].v2)
                {
                    gr.DrawArc(darkGoldPen, (v[e[i].v1].x - 2 * R), (v[e[i].v1].y - 2 * R), 2 * R, 2 * R, 90, 270);
                    point = new PointF(v[e[i].v1].x - (int)(2.75 * R), v[e[i].v1].y - (int)(2.75 * R));
                }
                else
                {
                    gr.DrawLine(darkGoldPen, v[e[i].v1].x, v[e[i].v1].y, v[e[i].v2].x, v[e[i].v2].y);
                    point = new PointF(((float)(v[e[i].v1].x + v[e[i].v2].x)) / 2, (float)(v[e[i].v1].y + v[e[i].v2].y) / 2);
                    for (int j = 0; j < w.Count; j++)
                    {
                        if (w[j] != null)
                        {
                            if (w[j].v1 == e[i].v1 && w[j].v2 == e[i].v2)
                            {
                                gr.DrawString((w[i].value).ToString(), fo, br, point);
                            }
                        }
                    }
                }
            }
            //рисуем вершины
            for (int i = 0; i < v.Count; i++)
            {
                drawVertex(v[i].x, v[i].y, (i + 1).ToString());
            }
        }
    }
}