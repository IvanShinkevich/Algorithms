using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SystAnalys_lr1.Services
{
    public class ReqAlgosService
    {
        internal int time = 0;
        internal const int StartupValueForBiconnectionCheck = -1;
        private List<Weight> Weights;

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

        private int MinimumDistance(int[] distance, bool[] shortestPathTreeSet, int verticesCount)
        {
            int min = int.MaxValue;
            int minIndex = 0;

            for (int v = 0; v < verticesCount; ++v)
            {
                if (shortestPathTreeSet[v] == false && distance[v] <= min)
                {
                    min = distance[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }

        private void PrintDijkstraResult(int[] distances, ListBox listBoxMatrix)
        {
            listBoxMatrix.Items.Add("Vertex    Distance from source");

            for (int i = 0; i < distances.Length; ++i)
                listBoxMatrix.Items.Add($"{i+1}\t  {distances[i]}");
        }

        public int[] DijkstraAlgo(int[,] graph, int source, int verticesCount)
        {
            int[] distance = new int[verticesCount];
            bool[] shortestPathTreeSet = new bool[verticesCount];

            for (int i = 0; i < verticesCount; ++i)
            {
                distance[i] = int.MaxValue;
                shortestPathTreeSet[i] = false;
            }

            distance[source] = 0;

            for (int count = 0; count < verticesCount - 1; ++count)
            {
                int u = MinimumDistance(distance, shortestPathTreeSet, verticesCount);
                shortestPathTreeSet[u] = true;

                for (int v = 0; v < verticesCount; ++v)
                    if (!shortestPathTreeSet[v] && Convert.ToBoolean(graph[u, v]) && distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v])
                        distance[v] = distance[u] + graph[u, v];
            }

            return distance;
        }

        public void UseAndPrintDijkstraAlgo(int[,] graph, int source, int verticesCount, ListBox listBoxMatrix)
        {
            var distances = DijkstraAlgo(graph, source, verticesCount);
            PrintDijkstraResult(distances, listBoxMatrix);
        }

        public void SearchCenterByFloudUorshell(int[,] adjacencyMatrix, int verticesAmount, ListBox listBoxMatrix)
        {
            for (int i = 0; i < verticesAmount; i++) adjacencyMatrix[i, i] = 0;

            for (int k = 0; k < verticesAmount; k++)
                for (int i = 0; i < verticesAmount; i++)
                    for (int j = 0; j < verticesAmount; j++)
                        if (adjacencyMatrix[i, k] != 0 && adjacencyMatrix[k, j] != 0 && i != j)
                            if (adjacencyMatrix[i, k] + adjacencyMatrix[k, j] < adjacencyMatrix[i, j] || adjacencyMatrix[i, j] == 0)
                                adjacencyMatrix[i, j] = adjacencyMatrix[i, k] + adjacencyMatrix[k, j];

            int[] max = new int[verticesAmount];
            for (int i = 0; i < verticesAmount; i++)
            {
                max[i] = adjacencyMatrix[0, i];

                for (int j = 0; j < verticesAmount; j++)
                {
                    if (adjacencyMatrix[j, i] > max[i])
                    {
                        max[i] = adjacencyMatrix[j, i];
                    }
                }
                listBoxMatrix.Items.Add(max[i]);
            }
            int center = verticesAmount;
            int centerPos = 0;
            for (int i = 0; i < verticesAmount; i++)
            {
                if (max[i] < center)
                {
                    center = max[i];
                    centerPos = i;
                }
            }
            listBoxMatrix.Items.Add($"Center vertice: {centerPos + 1}");
        }

        // A recursive function that returns true if there is an articulation 
        // point in given graph, otherwise returns false.
        // u --> The vertex to be visited next 
        // visited[] --> keeps tract of visited vertices 
        // disc[] --> Stores discovery times of visited vertices 
        // parent[] --> Stores parent vertices in DFS tree 
        internal virtual bool IsBCUtil(int u, bool[] visited,
            int[] disc, int[] low, int[] parent, int[,] adjMatrix, int verticesAmo)
        {

            // Count of children in DFS Tree 
            int children = 0;

            // Mark the current node as visited 
            visited[u] = true;

            // Initialize discovery time and low value 
            disc[u] = low[u] = ++time;

            // Go through all vertices aadjacent to this 
            for(int i=0;i< verticesAmo; i++)
            {
                if (adjMatrix[u, i] == 0)
                {
                    continue;
                }

                int v = i; // v is current adjacent of u

                // If v is not visited yet, then make it a child of u 
                // in DFS tree and recur for it 
                if (!visited[v])
                {
                    children++;
                    parent[v] = u;

                    // check if subgraph rooted with v has an articulation point 
                    if (IsBCUtil(v, visited, disc, low, parent, adjMatrix, verticesAmo))
                    {
                        return true;
                    }

                    // Check if the subtree rooted with v has a connection to 
                    // one of the ancestors of u 
                    low[u] = Math.Min(low[u], low[v]);

                    // u is an articulation point in following cases 

                    // (1) u is root of DFS tree and has two or more chilren. 
                    if (parent[u] == StartupValueForBiconnectionCheck && children > 1)
                    {
                        return true;
                    }

                    // (2) If u is not root and low value of one of its 
                    // child is more than discovery value of u. 
                    if (parent[u] != StartupValueForBiconnectionCheck && low[v] >= disc[u])
                    {
                        return true;
                    }
                }

                // Update low value of u for parent function calls. 
                else if (v != parent[u])
                {
                    low[u] = Math.Min(low[u], disc[v]);
                }
            }
            return false;
        }

        // The main function that returns true if graph is Biconnected, 
        // otherwise false. It uses recursive function isBCUtil() 
        public bool BC(int verticesAmount, int[,] adjMatrix)
        {
                // Mark all the vertices as not visited 
                bool[] visited = new bool[verticesAmount];
                int[] disc = new int[verticesAmount];
                int[] low = new int[verticesAmount];
                int[] parent = new int[verticesAmount];

                // Initialize parent and visited, and ap(articulation point) 
                // arrays 
                for (int i = 0; i < verticesAmount; i++)
                {
                    parent[i] = StartupValueForBiconnectionCheck;
                    visited[i] = false;
                }

                // Call the recursive helper function to find if there is an 
                // articulation/ point in given graph. We do DFS traversal 
                // starring from vertex 0 
                if (IsBCUtil(0, visited, disc, low, parent, adjMatrix, verticesAmount) == true)
                {
                    return false;
                }

                // Now check whether the given graph is connected or not. 
                // An undirected graph is connected if all vertices are 
                // reachable from any starting point (we have taken 0 as 
                // starting point) 
                for (int i = 0; i < verticesAmount; i++)
                {
                    if (visited[i] == false)
                    {
                        return false;
                    }
                }

                return true;
        }

        //For making graph biconnected
        private bool MakeGraphBiconnected(int u, bool[] visited, int[] disc, int[] low, int[] parent, int[,] adjMatrix, int verticesAmo, List<Vertex> vertices)
        {

            // Count of children in DFS Tree 
            int children = 0;

            // Mark the current node as visited 
            visited[u] = true;

            // Initialize discovery time and low value 
            disc[u] = low[u] = ++time;

            // Go through all vertices aadjacent to this 
            for (int i = 0; i < verticesAmo; i++)
            {
                if (adjMatrix[u, i] == 0)
                {
                    continue;
                }

                int v = i; // v is current adjacent of u 

                // If v is not visited yet, then make it a child of u 
                // in DFS tree and recur for it 
                if (!visited[v])
                {
                    children++;
                    parent[v] = u;

                    // check if subgraph rooted with v has an articulation point 
                    if (MakeGraphBiconnected(v, visited, disc, low, parent, adjMatrix, verticesAmo, vertices))
                    {
                        return true;
                    }

                    // Check if the subtree rooted with v has a connection to 
                    // one of the ancestors of u 
                    low[u] = Math.Min(low[u], low[v]);

                    // u is an articulation point in following cases 

                    // (1) u is root of DFS tree and has two or more chilren. 
                    if (parent[u] == StartupValueForBiconnectionCheck && children > 1)
                    {
                        List<int> vert = new List<int>();
                        for (int j = 0; j < vertices.Count; j++)
                        {
                            if (adjMatrix[u, j] != 0)
                            {
                                vert.Add(j);
                                adjMatrix[u, j] = 0;
                                adjMatrix[j, u] = 0;
                            }
                        }
                        
                        string[] used = new string[vertices.Count];
                        string symbol = "symbol";
                        int m = 0;

                        for (int j = 0; j < vertices.Count; j++)
                        {

                            if (String.IsNullOrEmpty(used[j]))
                            {
                                bfs(j, adjMatrix, used, symbol, vertices.Count);
                                m++;
                                symbol = (string)("symbol" + m);
                            }
                        }

                        bool flag = false;
                        for (int j = 0; j < vertices.Count; j++)
                        {
                            for (int k = j + 1; k < vertices.Count; k++)
                            {
                                if (k == u || u == j) continue;
                                if (!used[j].Equals(used[k]) && adjMatrix[j, k] == 0 && adjMatrix[k, j] == 0 && !(vert.Contains(k) || vert.Contains(j)))
                                {
                                    Weight w = new Weight(j, k, "1");
                                    Weights.Add(w);
                                    adjMatrix[j, k] = 1;
                                    adjMatrix[k, j] = 1;
                                    flag = true;
                                    break;
                                }
                            }

                            if (flag) break;
                        }

                        for (int j = 0; j < vert.Count; j++)
                        {
                            if (adjMatrix[u, vert[j]] == 0)
                            {
                                adjMatrix[u, vert[j]] = 1;
                                adjMatrix[vert[j], u] = 1;
                            }
                        }
                    }

                    // (2) If u is not root and low value of one of its 
                    // child is more than discovery value of u. 
                    if (parent[u] != StartupValueForBiconnectionCheck && low[v] >= disc[u])
                    {
                        // listBoxMatrix.Items.Add(v);
                        //adj. vertices
                        List<int> vert = new List<int>();
                        //add to adj, remove from matrix
                        for (int j = 0; j < vertices.Count; j++)
                        {
                            if (adjMatrix[u, j] != 0)
                            {
                                vert.Add(j);
                                adjMatrix[u, j] = 0;
                                adjMatrix[j, u] = 0;
                            }
                        }
                        string[] used = new string[vertices.Count];
                        string symbol = "symbol";
                        int m = 0;

                        for (int j = 0; j < vertices.Count; j++)
                        {

                            if (String.IsNullOrEmpty(used[j]))
                            {
                                bfs(j, adjMatrix, used, symbol, vertices.Count);
                                m++;
                                symbol = (string)("symbol" + m);
                            }
                        }

                        bool flag = false;
                        for (int j = 0; j < vertices.Count; j++)
                        {
                            for (int k = j + 1; k < vertices.Count; k++)
                            {
                                if (k == u || u == j) continue; 
                                if (!used[j].Equals(used[k]) && adjMatrix[j, k] == 0 && adjMatrix[k, j] == 0 && !(vert.Contains(k) || vert.Contains(j)))
                                {
                                    Weight w = new Weight(j, k, "1");
                                    Weights.Add(w);
                                    adjMatrix[j, k] = 1;
                                    adjMatrix[k, j] = 1;
                                    flag = true;
                                    break;
                                }


                            }

                            if (flag) break;
                        }
                        for (int j = 0; j < vert.Count; j++)
                        {
                            if (adjMatrix[u, vert[j]] == 0)
                            {
                                adjMatrix[u, vert[j]] = 1;
                                adjMatrix[vert[j], u] = 1;
                            }
                        }
                    }
                }

                // Update low value of u for parent function calls. 
                else if (v != parent[u])
                {
                    low[u] = Math.Min(low[u], disc[v]);
                }
            }
            return false;
        }

        public List<Weight> GetBiconnectedGraph(int verticesAmount, int[,] adjMatrix, List<Vertex> vertices, List<Weight> weights)
        {
            // Mark all the vertices as not visited 
            bool[] visited = new bool[verticesAmount];
            int[] disc = new int[verticesAmount];
            int[] low = new int[verticesAmount];
            int[] parent = new int[verticesAmount];
            Weights = weights;
            // Initialize parent and visited, and ap(articulation point) 
            // arrays 
            for (int i = 0; i < verticesAmount; i++)
            {
                parent[i] = StartupValueForBiconnectionCheck;
                visited[i] = false;
            }

            // Call the recursive helper function to find if there is an 
            // articulation/ point in given graph. We do DFS traversal 
            // starring from vertex 0 
            if (MakeGraphBiconnected(0, visited, disc, low, parent, adjMatrix, verticesAmount, vertices) == true)
            {
                return Weights;
            }

            // Now check whether the given graph is connected or not. 
            // An undirected graph is connected if all vertices are 
            // reachable from any starting point (we have taken 0 as 
            // starting point) 
            for (int i = 0; i < verticesAmount; i++)
            {
                if (visited[i] == false)
                {
                    return Weights;
                }
            }

            return Weights;
        }

        private void bfs(int v, int[,] adjMatrix, string[] used, string symbol, int verticesAmount)
        {
            used[v] = symbol;
            Queue<int> q = new Queue<int>();

            q.Enqueue(v);
            while (q.Count != 0)
            {

                v = q.Dequeue();
                for (int i = 0; i < verticesAmount; i++)
                {
                    if (adjMatrix[i, v] != 0 && String.IsNullOrEmpty(used[i]))
                    {
                        used[i] = symbol;
                        q.Enqueue(i);
                    }
                }
            }
        }
    }
}