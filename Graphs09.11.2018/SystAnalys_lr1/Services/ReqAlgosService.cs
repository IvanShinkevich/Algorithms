using System;
using System.Collections.Generic;
using System.Linq;

namespace SystAnalys_lr1.Services
{
    public class ReqAlgosService
    {
        public List<Weight> UsePrimeAlgo(List<Vertex> vertices, List<Weight> edges, List<Weight> MST)
        {
            //неиспользованные ребра
            List<Weight> notUsedE = new List<Weight>(edges);
            //использованные вершины
            List<int> usedV = new List<int>();
            //неиспользованные вершины
            List<int> notUsedV = new List<int>();
            for (int i = 0; i < vertices.Count; i++)
                notUsedV.Add(i);
            //выбираем случайную начальную вершину
            Random rand = new Random();
            usedV.Add(rand.Next(0, vertices.Count));
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

        public List<Weight> UseKruskalAlgo(List<Vertex> vertices, List<Weight> weights)
        {
            int verticesCount = vertices.Count;
            Weight[] result = new Weight[verticesCount];
            int i = 0;
            int e = 0;

            weights = weights.OrderBy(edge => Convert.ToInt32(edge.value)).ToList();

            DrawGraph.Subset[] subsets = new DrawGraph.Subset[verticesCount];

            for (int v = 0; v < verticesCount; ++v)
            {
                subsets[v].Parent = v;
                subsets[v].Rank = 0;
            }

            while (e < verticesCount - 1)
            {
                var nextEdge = weights[i++];
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
    }
}