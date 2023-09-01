using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman_Coding
{
    public class HuffmanNode
    {
        public char Char { get; set; }
        public int Frequency { get; set; }
        public HuffmanNode Left { get; set; }
        public HuffmanNode Right { get; set; }
    }

    public class HuffmanTree
    {
        public HuffmanNode Root { get; set; }

        public HuffmanTree(List<HuffmanNode> nodes)
        {
            while (nodes.Count > 1)
            {
                var leftNode = nodes[0];
                var rightNode = nodes[1];

                HuffmanNode newNode = new HuffmanNode()
                {
                    Frequency = leftNode.Frequency + rightNode.Frequency,
                    Left = leftNode,
                    Right = rightNode
                };

                nodes.RemoveRange(0, 2);

                nodes.Add(newNode);

                nodes.Sort((a, b) => a.Frequency.CompareTo(b.Frequency));
            }

            Root = nodes[0];
        }
    }
}
