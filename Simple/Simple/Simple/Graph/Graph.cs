using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple
{
    public class NonWeightVertex
    {
        public NonWeightVertex(int index)
        {
            m_index = index;
        }

        private int m_index = -1;
        public float m_min_dist = 0f;// minimum distance form origin to me
        public NonWeightVertex m_path_vertex = null;
        public List<NonWeightVertex> m_neighbours = new List<NonWeightVertex>();
    }
    public class NonWeightGraph
    {
        private List<NonWeightVertex> m_vertices = new List<NonWeightVertex>();
        public void Test()
        {
            NonWeightVertex v1 = new NonWeightVertex(1);
            NonWeightVertex v2 = new NonWeightVertex(2);
            NonWeightVertex v3 = new NonWeightVertex(3);
            NonWeightVertex v4 = new NonWeightVertex(4);
            NonWeightVertex v5 = new NonWeightVertex(5);
            NonWeightVertex v6 = new NonWeightVertex(6);
            NonWeightVertex v7 = new NonWeightVertex(7);

            v1.m_neighbours.Add(v4);
            v1.m_neighbours.Add(v2);

            v2.m_neighbours.Add(v4);
            v2.m_neighbours.Add(v5);

            v3.m_neighbours.Add(v1);
            v3.m_neighbours.Add(v6);

            v4.m_neighbours.Add(v5);
            v4.m_neighbours.Add(v6);
            v4.m_neighbours.Add(v7);

            v5.m_neighbours.Add(v7);

            v7.m_neighbours.Add(v6);

            m_vertices.Add(v1);
            m_vertices.Add(v2);
            m_vertices.Add(v3);
            m_vertices.Add(v4);
            m_vertices.Add(v5);
            m_vertices.Add(v6);
            m_vertices.Add(v7);
            CaculateNonWeightMinDistanceToEachVertex(v3);
        }

        public void CaculateNonWeightMinDistanceToEachVertex(NonWeightVertex origin)
        {
            float INFINITY = float.MaxValue;
            for (int i = 0; i < m_vertices.Count; ++i)
            {
                m_vertices[i].m_min_dist = INFINITY;
            }
            origin.m_min_dist = 0;

            Queue<NonWeightVertex> queue = new Queue<NonWeightVertex>();
            queue.Enqueue(origin);

            NonWeightVertex vert = null;
            while (queue.Count > 0)
            {
                vert = queue.Dequeue();
                for (int i = 0; i < vert.m_neighbours.Count; ++i)
                {
                    if (vert.m_neighbours[i].m_min_dist == INFINITY)
                    {
                        vert.m_neighbours[i].m_min_dist = vert.m_min_dist + 1;
                        queue.Enqueue(vert.m_neighbours[i]);
                        vert.m_neighbours[i].m_path_vertex = vert;
                    }
                }
            }
        }
    }

    public class WeightedVertex
    {
        public WeightedVertex(int index)
        {
            m_index = index;
        }

        public int m_index = -1;
        public Dictionary<WeightedVertex, int> m_neighbours = new Dictionary<WeightedVertex, int>();
        public WeightedVertex m_path = null;
        public float m_dist = -1;
        public bool m_known = false;
    }

    public class WeightedGraph
    {
        private List<WeightedVertex> m_vertices = new List<WeightedVertex>();
        public void test()
        {
            WeightedVertex v1 = new WeightedVertex(1);
            WeightedVertex v2 = new WeightedVertex(2);
            WeightedVertex v3 = new WeightedVertex(3);
            WeightedVertex v4 = new WeightedVertex(4);
            WeightedVertex v5 = new WeightedVertex(5);
            WeightedVertex v6 = new WeightedVertex(6);
            WeightedVertex v7 = new WeightedVertex(7);

            v1.m_neighbours.Add(v4, 1);
            v1.m_neighbours.Add(v2, 2);

            v2.m_neighbours.Add(v4, 3);
            v2.m_neighbours.Add(v5, 10);

            v3.m_neighbours.Add(v1, 4);
            v3.m_neighbours.Add(v6, 5);

            v4.m_neighbours.Add(v5, 2);
            v4.m_neighbours.Add(v6, 8);
            v4.m_neighbours.Add(v7, 4);
            v4.m_neighbours.Add(v3, 2);

            v5.m_neighbours.Add(v7, 6);

            v7.m_neighbours.Add(v6, 1);

            m_vertices.Add(v1);
            m_vertices.Add(v2);
            m_vertices.Add(v3);
            m_vertices.Add(v4);
            m_vertices.Add(v5);
            m_vertices.Add(v6);
            m_vertices.Add(v7);
            m_vertices.Add(v1);

            Caculate(v1);
        }

        public void Caculate(WeightedVertex origin)
        {
            float INFINITY = float.MaxValue;
            for (int i = 0; i < m_vertices.Count; ++i)
            {
                m_vertices[i].m_dist = INFINITY;
                m_vertices[i].m_known = false;
            }
            origin.m_dist = 0;
            WeightedVertex vert = null;
            for (; ; )
            {
                vert = GetMinDistVert();
                if (vert == null)
                    break;
                vert.m_known = true;
                foreach(var pair in vert.m_neighbours)
                {
                    if(!pair.Key.m_known && pair.Value + vert.m_dist < pair.Key.m_dist)
                    {
                        pair.Key.m_dist = pair.Value + vert.m_dist;
                        pair.Key.m_path = vert;
                    }
                }
            }
        }

        private WeightedVertex GetMinDistVert()
        {
            WeightedVertex vert = null;
            float min_dist = float.MaxValue;
            for (int i = 0; i < m_vertices.Count; ++i)
            {
               if(!m_vertices[i].m_known && min_dist > m_vertices[i].m_dist)
               {
                   vert = m_vertices[i];
                   min_dist = vert.m_dist;
               }
            }
            return vert;
        }
    }
}
