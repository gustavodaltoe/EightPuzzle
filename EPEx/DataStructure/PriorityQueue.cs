using ProjetoGrafos.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EP.DataStructure
{
    class PriorityQueue
    {
        private SortedDictionary<int, Queue<Node>> dictionary;
        int count;

        public int Count { get; set; }

        public PriorityQueue()
        {
            dictionary = new SortedDictionary<int, Queue<Node>>();
            count = 0;
        }

        public void Enqueue(int key, Node node)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, new Queue<Node>());

            dictionary[key].Enqueue(node);
            Count++;
        }

        public Node Dequeue()
        {
            int minKey = dictionary.Keys.Min();
            Node n = dictionary[minKey].Dequeue();
            if (dictionary[minKey].Count == 0)
                dictionary.Remove(minKey);

            Count--;
            return n;
        }
    }
}
