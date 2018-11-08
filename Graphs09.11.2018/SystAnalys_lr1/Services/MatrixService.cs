using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SystAnalys_lr1.Services
{
    public class MatrixService
    {
        List<int> Euler;
        int[,] e;

        private void FillIncidenceMatrix(int numberV, List<Edge> edges, int[,] matrix)
        {
            for (int i = 0; i < numberV; i++)
                for (int j = 0; j < edges.Count; j++)
                    matrix[i, j] = 0;
            for (int i = 0; i < edges.Count; i++)
            {
                matrix[edges[i].v1, i] = 1;
                matrix[edges[i].v2, i] = 1;
            }
        }

        public int[,] CreateIncidenceMatrixAndOut(List<Vertex> vertices, List<Edge> edges, int[,] IMatrix, ListBox listBoxMatrix)
        {
            if (edges.Count > 0)
            {
                IMatrix = new int[vertices.Count, edges.Count];
                FillIncidenceMatrix(vertices.Count, edges, IMatrix);
                listBoxMatrix.Items.Clear();
                string sOut = "    ";
                for (int i = 0; i < edges.Count; i++)
                    sOut += (char)('a' + i) + " ";
                listBoxMatrix.Items.Add(sOut);
                for (int i = 0; i < vertices.Count; i++)
                {
                    sOut = (i + 1) + " | ";
                    for (int j = 0; j < edges.Count; j++)
                        sOut += IMatrix[i, j] + " ";
                    listBoxMatrix.Items.Add(sOut);
                }
            }
            else
                listBoxMatrix.Items.Clear();

            return IMatrix;
        }

        //создание матрицы смежности и вывод в листбокс
        public int[,] CreateAdjAndOut(List<Vertex> vertices, List<Edge> edges, List<Weight> weights, ListBox listBoxMatrix)
        {
            Euler = new List<int>();
            var AMatrix = new int[vertices.Count, vertices.Count];
            FillAdjacencyMatrix(vertices.Count, edges, AMatrix, weights);
            listBoxMatrix.Items.Clear();
            string sOut = "    ";
            for (int i = 0; i < vertices.Count; i++)
                sOut += (i + 1) + " ";
            listBoxMatrix.Items.Add(sOut);
            for (int i = 0; i < vertices.Count; i++)
            {
                sOut = (i + 1) + " | ";
                for (int j = 0; j < vertices.Count; j++)
                    sOut += AMatrix[i, j] + " ";
                listBoxMatrix.Items.Add(sOut);
            }
            int k = 0;
            bool isEuler = true;
            for (int i = 0; i < vertices.Count; i++)
            {
                for (int j = 0; j < vertices.Count; j++)
                {
                    if (AMatrix[i, j] != 0)
                    {
                        k++;
                    }
                }
                if (k % 2 != 0 || k == 0)
                {
                    isEuler = false;
                    break;
                }
                k = 0;
            }
            listBoxMatrix.Items.Add("Эйлеров цикл существует? - " + isEuler.ToString());

            if (isEuler)
            {
                e = new int[vertices.Count, vertices.Count];
                FindEulerCycle(0, AMatrix, vertices,listBoxMatrix);
            }

            BFS(vertices, listBoxMatrix, AMatrix);
            return AMatrix;
        }

        private void FindEulerCycle(int i, int[,] AMatrix, List<Vertex> vertices, ListBox listBoxMatrix)
        {
            if (Euler.Count == 0)
            {
                Euler.Add(0); //начинаем с 1ой вершины
            }
            for (int j = 0; j < vertices.Count; j++) //для каждого i идем по строке в мце см-ти
            {
                if (AMatrix[i, j] != 0 && e[i, j] == 0) //если ещё не занесли
                {
                    Euler.Add(j);
                    e[i, j] = 1;
                    e[j, i] = 1;
                    if ((j == 0) && (Euler.Count == vertices.Count + 1))
                    {
                        var str = string.Empty;
                        Euler.ForEach(el => { str += (el + 1) + " "; });
                        Euler = new List<int>();
                        listBoxMatrix.Items.Add("Эйлеров цикл найден:\n" + str);
                    }
                    else
                    {
                        FindEulerCycle(j, AMatrix, vertices, listBoxMatrix);
                    }
                }
            }
        }

        private List<int> GetAdjacencyOfVertex(int vertex, List<Vertex> vertices, int[,] AMatrix)
        {
            List<int> adjOfVertex = new List<int>();
            for (int i = 0; i < vertices.Count; i++)
            {
                if (AMatrix[vertex, i] == 1)
                {
                    adjOfVertex.Add(i);
                }
            }
            return adjOfVertex;
        }

        private List<List<int>> linkedComponentsList;
        public void BFS(List<Vertex> vertices, ListBox listBoxMatrix, int[,] AMatrix)
        {
            List<int> firstShare = new List<int>();//firstComponent
            List<int> secondShare = new List<int>();//firstComponent
            linkedComponentsList = new List<List<int>>();
            List<int> listOfNotVisitedVertices = new List<int>();
            bool isBichromatic = true;
            for (int i = 0; i < vertices.Count; i++)
            {
                listOfNotVisitedVertices.Add(i);
            }
            for (int i = 0; i < vertices.Count; i++)
            {
                if (!(listOfNotVisitedVertices.Count == 0))
                {
                    linkedComponentsList.Add(new List<int>());
                    int first = listOfNotVisitedVertices[0];
                    linkedComponentsList[linkedComponentsList.Count - 1].Add(first);
                    listOfNotVisitedVertices.Remove(0);
                    Queue<int> queue = new Queue<int>();
                    queue.Enqueue(first);
                    firstShare.Add(first);
                    while (!(queue.Count == 0))
                    {
                        int temp = queue.Dequeue();
                        var adjacencyList = GetAdjacencyOfVertex(temp,vertices, AMatrix);
                        if (adjacencyList.Count == 0)
                        {
                            continue;
                        }
                        foreach (var el in adjacencyList)
                        {
                            if (listOfNotVisitedVertices.IndexOf(el) != -1)
                            {
                                if (secondShare.IndexOf(temp) != -1)
                                {
                                    firstShare.Add(el);
                                }
                                else
                                {
                                    secondShare.Add(el);
                                }
                                linkedComponentsList[linkedComponentsList.Count - 1].Add(el);
                                listOfNotVisitedVertices.Remove(el);
                                queue.Enqueue(el);
                            }
                            else
                            {
                                if (secondShare.IndexOf(temp) != -1 && secondShare.IndexOf(el) != -1)
                                {
                                    isBichromatic = false;
                                }
                                else if (firstShare.IndexOf(temp) != -1 && firstShare.IndexOf(el) != -1)
                                {
                                    isBichromatic = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            if (linkedComponentsList.Count != 1)
            {
                isBichromatic = false;
            }
            listBoxMatrix.Items.Add("Linked components\n");
            for (int i = 0; i < linkedComponentsList.Count; i++)
            {
                listBoxMatrix.Items.Add(i + ":\n");
                for (int j = 0; j < linkedComponentsList[i].Count; j++)
                {
                    listBoxMatrix.Items.Add(linkedComponentsList[i][j] + " ");
                }
                listBoxMatrix.Items.Add("\n");
            }
            if (isBichromatic)
            {
                listBoxMatrix.Items.Add("Двудольный граф\n" + "Первая доля:\n");
                foreach (var el in firstShare)
                {
                    listBoxMatrix.Items.Add(el + " ");
                }
                listBoxMatrix.Items.Add("\nВторая доля:\n");
                foreach (var el in secondShare)
                {
                    listBoxMatrix.Items.Add(el + " ");
                }
                listBoxMatrix.Items.Add("\n");
            }
            else
            {
                listBoxMatrix.Items.Add("Не двудольный граф");
            }
        }

        private void FillAdjacencyMatrix(int numberV, List<Edge> edges, int[,] matrix, List<Weight> weights)
        {
            for (int i = 0; i < numberV; i++)
                for (int j = 0; j < numberV; j++)
                    matrix[i, j] = 0;
            for (int i = 0; i < edges.Count; i++)
            {
                for (int j = 0; j < weights.Count; j++)
                {
                    if (weights[j].v1 == edges[i].v1 && weights[j].v2 == edges[i].v2)
                    {
                        matrix[edges[i].v1, edges[i].v2] = Convert.ToInt32(weights[j].value);
                        matrix[edges[i].v2, edges[i].v1] = Convert.ToInt32(weights[j].value);
                    }
                }

            }
        }
    }
}