using SystAnalys_lr1.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SystAnalys_lr1
{
    public partial class Form1 : Form
    {
        DrawGraph G;
        List<Vertex> V;
        List<Edge> E;
        List<String> Value;
        List<Weight> W;
        
        int[,] AMatrix; //матрица смежности
        int[,] IMatrix; //матрица инцидентности
        List<int> Euler;
        int[,] e;
        int selected1; //выбранные вершины, для соединения линиями
        int selected2;

        public Form1()
        {
            InitializeComponent();
            V = new List<Vertex>();
            G = new DrawGraph(sheet.Width, sheet.Height);
            E = new List<Edge>();
            W = new List<Weight>();
            Value = new List<String>();
            Euler = new List<int>();
            
            sheet.Image = G.GetBitmap();
        }

        private static int Find(DrawGraph.Subset[] subsets, int i)
        {
            if (subsets[i].Parent != i)
                subsets[i].Parent = Find(subsets, subsets[i].Parent);

            return subsets[i].Parent;
        }

        private static void Union(DrawGraph.Subset[] subsets, int x, int y)
        {
            int xroot = Find(subsets, x);
            int yroot = Find(subsets, y);

            if (subsets[xroot].Rank < subsets[yroot].Rank)
                subsets[xroot].Parent = yroot;
            else if (subsets[xroot].Rank > subsets[yroot].Rank)
                subsets[yroot].Parent = xroot;
            else
            {
                subsets[yroot].Parent = xroot;
                ++subsets[xroot].Rank;
            }
        }

        private List<Weight> Kruskal()
        {
            int verticesCount = V.Count;
            Weight[] result = new Weight[verticesCount];
            int i = 0;
            int e = 0;
            
            W = W.OrderBy(edge => Convert.ToInt32(edge.value)).ToList();
           
            DrawGraph.Subset[] subsets = new DrawGraph.Subset[verticesCount];

            for (int v = 0; v < verticesCount; ++v)
            {
                subsets[v].Parent = v;
                subsets[v].Rank = 0;
            }

            while (e < verticesCount - 1)
            {
                var nextEdge = W[i++];
                int x = Find(subsets, nextEdge.v1);
                int y = Find(subsets, nextEdge.v2);

                if (x != y)
                {
                    result[e++] = nextEdge;
                    Union(subsets, x, y);
                }
            }

            return result.ToList();
        }

        //кнопка - выбрать вершину
        private void selectButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled = false;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            DrawGraph(V, E, W);
            selected1 = -1;
        }

        //кнопка - рисовать вершину
        private void drawVertexButton_Click(object sender, EventArgs e)
        {
            drawVertexButton.Enabled = false;
            selectButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            DrawGraph(V, E, W);
        }

        //кнопка - рисовать ребро
        private void drawEdgeButton_Click(object sender, EventArgs e)
        {
            drawEdgeButton.Enabled = false;
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            deleteButton.Enabled = true;
            DrawGraph(V, E, W);
            selected1 = -1;
            selected2 = -1;
        }

        //кнопка - удалить элемент
        private void deleteButton_Click(object sender, EventArgs e)
        {
            deleteButton.Enabled = false;
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            DrawGraph(V, E, W);
        }

        //кнопка - удалить граф
        private void deleteALLButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            const string message = "Вы действительно хотите полностью удалить граф?";
            const string caption = "Удаление";
            var MBSave = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (MBSave == DialogResult.Yes)
            {
                V.Clear();
                E.Clear();
                G.clearSheet();
                sheet.Image = G.GetBitmap();
            }
        }

        //кнопка - матрица смежности
        private void buttonAdj_Click(object sender, EventArgs e)
        {
            createAdjAndOut();
        }

        //кнопка - матрица инцидентности 
        private void buttonInc_Click(object sender, EventArgs e)
        {
            createIncAndOut();
        }

        private void sheet_MouseClick(object sender, MouseEventArgs e)
        {
            //нажата кнопка "выбрать вершину", ищем степень вершины
            if (selectButton.Enabled == false)
            {
                for (int i = 0; i < V.Count; i++)
                {
                    if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                    {
                        if (selected1 != -1)
                        {
                            selected1 = -1;
                            G.clearSheet();
                            G.drawALLGraph(V, E, W);
                            sheet.Image = G.GetBitmap();
                        }
                        if (selected1 == -1)
                        {
                            G.drawSelectedVertex(V[i].x, V[i].y);
                            selected1 = i;
                            sheet.Image = G.GetBitmap();
                            createAdjAndOut();
                            listBoxMatrix.Items.Clear();
                            int degree = 0;
                            for (int j = 0; j < V.Count; j++)
                                degree += AMatrix[selected1, j];
                            listBoxMatrix.Items.Add("Степень вершины №" + (selected1 + 1) + " равна " + degree);
                            break;
                        }
                    }
                }
            }
            //нажата кнопка "рисовать вершину"
            if (drawVertexButton.Enabled == false)
            {
                V.Add(new Vertex(e.X, e.Y));
                G.drawVertex(e.X, e.Y, V.Count.ToString());
                sheet.Image = G.GetBitmap();
            }
            //нажата кнопка "рисовать ребро"
            if (drawEdgeButton.Enabled == false)
            {
                if (e.Button == MouseButtons.Left)
                {
                    for (int i = 0; i < V.Count; i++)
                    {
                        if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                        {
                            if (selected1 == -1)
                            {
                                G.drawSelectedVertex(V[i].x, V[i].y);
                                selected1 = i;
                                sheet.Image = G.GetBitmap();
                                break;
                            }
                            if (selected2 == -1)
                            {
                                G.drawSelectedVertex(V[i].x, V[i].y);
                                selected2 = i;
                                E.Add(new Edge(selected1, selected2));
                                Value.Add(textBox1.Text);
                                W.Add(new Weight(selected1, selected2, Value[Value.Count-1]));
                                G.drawEdge(V[selected1], V[selected2], E[E.Count - 1], E.Count - 1, W[W.Count - 1] );
                                selected1 = -1;
                                selected2 = -1;
                                sheet.Image = G.GetBitmap();
                                break;
                            }
                        }
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    if ((selected1 != -1) &&
                        (Math.Pow((V[selected1].x - e.X), 2) + Math.Pow((V[selected1].y - e.Y), 2) <= G.R * G.R))
                    {
                        G.drawVertex(V[selected1].x, V[selected1].y, (selected1 + 1).ToString());
                        selected1 = -1;
                        sheet.Image = G.GetBitmap();
                    }
                }
            }
            //нажата кнопка "удалить элемент"
            if (deleteButton.Enabled == false)
            {
                bool flag = false; //удалили ли что-нибудь по ЭТОМУ клику
                //ищем, возможно была нажата вершина
                for (int i = 0; i < V.Count; i++)
                {
                    if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                    {
                        for (int j = 0; j < E.Count; j++)
                        {
                            if ((E[j].v1 == i) || (E[j].v2 == i))
                            {
                                E.RemoveAt(j);
                                j--;
                            }
                            else
                            {
                                if (E[j].v1 > i) E[j].v1--;
                                if (E[j].v2 > i) E[j].v2--;
                            }
                        }
                        V.RemoveAt(i);
                        flag = true;
                        break;
                    }
                }
                //ищем, возможно было нажато ребро
                if (!flag)
                {
                    for (int i = 0; i < E.Count; i++)
                    {
                        if (E[i].v1 == E[i].v2) //если это петля
                        {
                            if ((Math.Pow((V[E[i].v1].x - G.R - e.X), 2) + Math.Pow((V[E[i].v1].y - G.R - e.Y), 2) <= ((G.R + 2) * (G.R + 2))) &&
                                (Math.Pow((V[E[i].v1].x - G.R - e.X), 2) + Math.Pow((V[E[i].v1].y - G.R - e.Y), 2) >= ((G.R - 2) * (G.R - 2))))
                            {
                                E.RemoveAt(i);
                                flag = true;
                                break;
                            }
                        }
                        else //не петля
                        {
                            if (((e.X - V[E[i].v1].x) * (V[E[i].v2].y - V[E[i].v1].y) / (V[E[i].v2].x - V[E[i].v1].x) + V[E[i].v1].y) <= (e.Y + 4) &&
                                ((e.X - V[E[i].v1].x) * (V[E[i].v2].y - V[E[i].v1].y) / (V[E[i].v2].x - V[E[i].v1].x) + V[E[i].v1].y) >= (e.Y - 4))
                            {
                                if ((V[E[i].v1].x <= V[E[i].v2].x && V[E[i].v1].x <= e.X && e.X <= V[E[i].v2].x) ||
                                    (V[E[i].v1].x >= V[E[i].v2].x && V[E[i].v1].x >= e.X && e.X >= V[E[i].v2].x))
                                {
                                    E.RemoveAt(i);
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                //если что-то было удалено, то обновляем граф на экране
                if (flag)
                {
                    DrawGraph(V, E, W);
                }
            }
        }

        //создание матрицы смежности и вывод в листбокс
        private void createAdjAndOut()
        {
            MatrixService mService = new MatrixService();
            AMatrix = mService.CreateAdjAndOut(V, E, W, listBoxMatrix);
        }

        //создание матрицы инцидентности и вывод в листбокс
        private void createIncAndOut()
        {
            MatrixService mService = new MatrixService();
            IMatrix = mService.CreateIncidenceMatrixAndOut(V, E, IMatrix, listBoxMatrix);
        }

        //поиск элементарных цепей
        private void chainButton_Click(object sender, EventArgs e)
        {
            listBoxMatrix.Items.Clear();
            //1-white 2-black
            int[] color = new int[V.Count];
            for (int i = 0; i < V.Count - 1; i++)
                for (int j = i + 1; j < V.Count; j++)
                {
                    for (int k = 0; k < V.Count; k++)
                        color[k] = 1;
                    DFSchain(i, j, E, color, (i + 1).ToString());
                }
        }

        //обход в глубину. поиск элементарных цепей. (1-white 2-black)
        private void DFSchain(int u, int endV, List<Edge> E, int[] color, string s)
        {
            //вершину не следует перекрашивать, если u == endV (возможно в нее есть несколько путей)
            if (u != endV)  
                color[u] = 2;
            else
            {
                listBoxMatrix.Items.Add(s);
                return;
            }
            for (int w = 0; w < E.Count; w++)
            {
                if (color[E[w].v2] == 1 && E[w].v1 == u)
                {
                    DFSchain(E[w].v2, endV, E, color, s + "-" + (E[w].v2 + 1).ToString());
                    color[E[w].v2] = 1;
                }
                else if (color[E[w].v1] == 1 && E[w].v2 == u)
                {
                    DFSchain(E[w].v1, endV, E, color, s + "-" + (E[w].v1 + 1).ToString());
                    color[E[w].v1] = 1;
                }
            }
        }

        //поиск элементарных циклов
        private void cycleButton_Click(object sender, EventArgs e)
        {
            DFSService dfsService = new DFSService();
            dfsService.GetAndPrintCycles(V, E, listBoxMatrix);
        }
        
        private void saveButton_Click(object sender, EventArgs e)
        {
            if (sheet.Image != null)
            {
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Сохранить картинку как...";
                savedialog.OverwritePrompt = true;
                savedialog.CheckPathExists = true;
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                savedialog.ShowHelp = true;
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        sheet.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.Value.Add(textBox1.Text);
            }
        }

        private List<Weight> algorithmByPrim(List<Weight> E, List<Weight> MST)
        {
            //неиспользованные ребра
            List<Weight> notUsedE = new List<Weight>(E);
            //использованные вершины
            List<int> usedV = new List<int>();
            //неиспользованные вершины
            List<int> notUsedV = new List<int>();
            for (int i = 0; i < V.Count; i++)
                notUsedV.Add(i);
            //выбираем случайную начальную вершину
            Random rand = new Random();
            usedV.Add(rand.Next(0, V.Count));
            notUsedV.RemoveAt(usedV[0]);
            while (notUsedV.Count > 0)
            {
                int minE = -1; //номер наименьшего ребра
                               //поиск наименьшего ребра
                for (int i = 0; i < notUsedE.Count; i++)
                {
                    if ((usedV.IndexOf(notUsedE[i].v1) != -1) && (notUsedV.IndexOf(notUsedE[i].v2) != -1) ||
                        (usedV.IndexOf(notUsedE[i].v2) != -1) && (notUsedV.IndexOf(notUsedE[i].v1) != -1))
                    {
                        if (minE != -1)
                        {
                            if (Convert.ToInt32(notUsedE[i].value) < Convert.ToInt32(notUsedE[minE].value))
                                minE = i;
                        }
                        else
                            minE = i;
                    }
                }
                //заносим новую вершину в список использованных и удаляем ее из списка неиспользованных
                if (usedV.IndexOf(notUsedE[minE].v1) != -1)
                {
                    usedV.Add(notUsedE[minE].v2);
                    notUsedV.Remove(notUsedE[minE].v2);
                }
                else
                {
                    usedV.Add(notUsedE[minE].v1);
                    notUsedV.Remove(notUsedE[minE].v1);
                }
                //заносим новое ребро в дерево и удаляем его из списка неиспользованных
                MST.Add(notUsedE[minE]);
                notUsedE.RemoveAt(minE);
            }
            return MST;
        }

        private List<Edge> SynchronizeEdgesAndWeights(List<Weight> weights)
        {
            var edges = new List<Edge>();
            foreach (var weight in weights)
            {
                if (weight != null)
                { 
                    edges.Add(new Edge(weight.v1, weight.v2));
                }
            }

            return edges;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var weights = algorithmByPrim(W, new List<Weight>());
            var edges = SynchronizeEdgesAndWeights(weights);
            DrawGraph(V, edges, weights);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var weights = Kruskal();
            var edges = SynchronizeEdgesAndWeights(weights);
            DrawGraph(V, edges, weights);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DrawGraph(V,E,W);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            var weightsByPrime = algorithmByPrim(W, new List<Weight>());
            var weightsByKruskal = Kruskal();

            var resultWeights = weightsByPrime.Union(weightsByKruskal).ToList();
            var edges = SynchronizeEdgesAndWeights(resultWeights);

            var dfsService = new DFSService();

            dfsService.GetAndPrintCycles(V, edges, listBoxMatrix);
            DrawGraph(V, edges, resultWeights);            
        }

        private void DrawGraph(List<Vertex> vertices, List<Edge> edges, List<Weight> weights)
        {
            G.clearSheet();
            G.drawALLGraph(vertices, edges, weights);
            sheet.Image = G.GetBitmap();
        }
    }    
}