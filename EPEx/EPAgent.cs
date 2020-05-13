using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using ProjetoGrafos.DataStructure;
using System.Linq;
using EP.DataStructure;

namespace EP
{
    /// <summary>
    /// EPAgent - searchs solution for the eight puzzle problem
    /// </summary>
    public class EightPuzzle : Graph
    {
        private int[] initState;
        private int[] target;

        /// <summary>
        /// Creating the agent and setting the initialstate plus target
        /// </summary>
        /// <param name="InitialState"></param>
        public EightPuzzle(int[] InitialState, int[] Target)
        {
            initState = InitialState;
            target = Target;
        }

        /// <summary>
        /// Accessor for the solution
        /// </summary>
        public int[] GetSolution()
        {
            return FindSolution();
        }

        /// <summary>
        /// Função principal de busca
        /// </summary>
        /// <returns></returns>
        private int[] FindSolution()
        {
            List<Node> path = new List<Node>();
            PriorityQueue fila = new PriorityQueue();

            Node n = new Node(null, initState, 0);
            HashSet<String> visitedStates = new HashSet<String>();

            path.Add(n);
            visitedStates.Add(n.ToString());
            fila.Enqueue(Heuristic(n) ,n);

            while (fila.Count > 0)
            {
                n = fila.Dequeue();
                if (TargetFound(n))
                {
                    return BuildAnswer(n);
                }
                List<Node> sucessors = GetSucessors(n);
                foreach (Node sucessor in sucessors)
                {
                    String stateStr = sucessor.ToString();
                    if (!visitedStates.Contains(stateStr))
                    {
                        if (TargetFound(sucessor))
                        {
                            return BuildAnswer(sucessor);
                        }
                        fila.Enqueue(Heuristic(sucessor), sucessor);
                        visitedStates.Add(stateStr);
                    }
                }
            }

            return null;
        }

        private int Heuristic(Node v)
        {
            int result = v.Nivel;
            int[] state = (int[])v.Info;
            int side = (int)Math.Sqrt(state.Length);
            for (int i = 0; i < state.Length; i++)
            {
                if (state[i] != 0)
                {
                    int lineDiff = Math.Abs(i / side - state[i] / side);
                    int colDiff = Math.Abs(i % side - state[i] % side);
                    result += lineDiff + colDiff;
                }
            }
            return result;
        }

        private List<Node> GetSucessors(Node n)
        {
            List<Node> sucessors = new List<Node>();
            int largura = (int) Math.Sqrt(initState.Length);
            int[] state = (int[])n.Info;

            // procurar pelo 0 no array
            int initPosition = 0;
            for(int i = 0; i < initState.Length; i++)
            {
                if (state[i] == 0)
                {
                    initPosition = i;
                    break;
                }
            }
            // troca 0 com o numero de cima
            if (initPosition >= largura)
            {
                int[] newState = (int[])state.Clone();
                newState[initPosition] = state[initPosition - largura];
                newState[initPosition - largura] = 0;
                Node sucessor = new Node(state[initPosition - largura].ToString(), newState, n.Nivel + 1);
                sucessor.Parent = n;
                sucessors.Add(sucessor);
            }
            // troca 0 com o numero da direita
            if ((initPosition + 1) % largura != 0)
            {
                int[] newState = (int[])state.Clone();
                newState[initPosition] = state[initPosition + 1];
                newState[initPosition + 1] = 0;
                Node sucessor = new Node(state[initPosition + 1].ToString(), newState, n.Nivel + 1);
                sucessor.Parent = n;
                sucessors.Add(sucessor);
            }
            // troca 0 com o numero de baixo
            if (initPosition + largura < initState.Length)
            {
                int[] newState = (int[])state.Clone();
                newState[initPosition] = state[initPosition + largura];
                newState[initPosition + largura] = 0;
                Node sucessor = new Node(state[initPosition + largura].ToString(), newState, n.Nivel + 1);
                sucessor.Parent = n;
                sucessors.Add(sucessor);
            }
            // troca 0 com o numero da esquerda
            if (initPosition % largura != 0)
            {
                int[] newState = (int[])state.Clone();
                newState[initPosition] = state[initPosition - 1];
                newState[initPosition - 1] = 0;
                Node sucessor = new Node(state[initPosition - 1].ToString(), newState, n.Nivel + 1);
                sucessor.Parent = n;
                sucessors.Add(sucessor);
            }

            return sucessors;
        }
        
        private int[] BuildAnswer(Node n)
        {
            Stack<int> answerStack = new Stack<int>();
            while(n.Parent != null)
            {
                answerStack.Push(Convert.ToInt32(n.Name));
                n = n.Parent;
            }
            return answerStack.ToArray();
        }

        private bool TargetFound(Node n)
        {
            int[] state = (int[])n.Info;
            int correctValue = 0;

            foreach(int num in state)
            {
                if (num != correctValue++)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

