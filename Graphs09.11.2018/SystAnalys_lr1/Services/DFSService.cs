using System.Collections.Generic;
using System.Windows.Forms;

namespace SystAnalys_lr1.Services
{
    public class DFSService
    {
        public void GetAndPrintCycles(List<Vertex> vertices, List<Edge> edges, ListBox listBoxMatrix)
        {
            listBoxMatrix.Items.Clear();
            //1-white 2-black
            int[] color = new int[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                for (int k = 0; k < vertices.Count; k++)
                    color[k] = 1;
                List<int> cycle = new List<int>();
                cycle.Add(i + 1);
                DFScycle(i, i, edges, color, -1, cycle, listBoxMatrix);
            }
        }

        //обход в глубину. поиск элементарных циклов. (1-white 2-black)
        //Вершину, для которой ищем цикл, перекрашивать в черный не будем. Поэтому, для избежания неправильной
        //работы программы, введем переменную unavailableEdge, в которой будет хранится номер ребра, исключаемый
        //из рассмотрения при обходе графа. В действительности это необходимо только на первом уровне рекурсии,
        //чтобы избежать вывода некорректных циклов вида: 1-2-1, при наличии, например, всего двух вершин.

        private void DFScycle(int u, int endV, List<Edge> edges, int[] color, int unavailableEdge, List<int> cycle, ListBox listBoxMatrix)
        {
            //если u == endV, то эту вершину перекрашивать не нужно, иначе мы в нее не вернемся, а вернуться необходимо
            if (u != endV)
                color[u] = 2;
            else
            {
                if (cycle.Count >= 2)
                {
                    cycle.Reverse();
                    string s = cycle[0].ToString();
                    for (int i = 1; i < cycle.Count; i++)
                        s += "-" + cycle[i].ToString();
                    bool flag = false; //есть ли палиндром для этого цикла графа в листбоксе?
                    for (int i = 0; i < listBoxMatrix.Items.Count; i++)
                        if (listBoxMatrix.Items[i].ToString() == s)
                        {
                            flag = true;
                            break;
                        }
                    if (!flag)
                    {
                        cycle.Reverse();
                        s = cycle[0].ToString();
                        for (int i = 1; i < cycle.Count; i++)
                            s += "-" + cycle[i].ToString();
                        listBoxMatrix.Items.Add(s);
                    }
                    return;
                }
            }
            for (int w = 0; w < edges.Count; w++)
            {
                if (w == unavailableEdge)
                    continue;
                if (color[edges[w].v2] == 1 && edges[w].v1 == u)
                {
                    List<int> cycleNEW = new List<int>(cycle);
                    cycleNEW.Add(edges[w].v2 + 1);
                    DFScycle(edges[w].v2, endV, edges, color, w, cycleNEW, listBoxMatrix);
                    color[edges[w].v2] = 1;
                }
                else if (color[edges[w].v1] == 1 && edges[w].v2 == u)
                {
                    List<int> cycleNEW = new List<int>(cycle);
                    cycleNEW.Add(edges[w].v1 + 1);
                    DFScycle(edges[w].v1, endV, edges, color, w, cycleNEW, listBoxMatrix);
                    color[edges[w].v1] = 1;
                }
            }
        }
    }
}