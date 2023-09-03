using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Huffman_Coding
{
    internal class Program
    {
        //Can be set to any wished value except 0 or 1
        const char seperator = ' ';

        static Dictionary<char, string> huffmanCodes = new Dictionary<char, string>();
        static Dictionary<char, int> characters = new Dictionary<char, int>();

        static void Main(string[] args)
        {
            while (true)
            {
                ProcessAndDisplayResult();
                Reset();
            }
        }

        private static void Reset()
        {
            huffmanCodes.Clear();
            characters.Clear();
        }

        private static void ProcessAndDisplayResult()
        {
            string filePath = GetFilePath();

            string input = File.ReadAllText(filePath);
            CountCharacters(input);

            HuffmanTree tree = BuildTree();
            BuildHuffmanCodes(tree.Root);
            string binary = ConvertToBinary(input);
            string text = ConvertToText(binary);
            double percentageDifference = CalculatePercentageDifference(binary, input);
            Display(binary, text, percentageDifference);
        }

        private static double CalculatePercentageDifference(string binary, string original)
        {
            double binaryBytes = binary.Length;
            double originalBytes = original.Length * 8;
            double percentageDifference = (1.0 - binaryBytes / originalBytes) * 100;
            return Math.Round(percentageDifference, 2);
        }

        private static void Display(string binary, string text, double percentageDifference)
        {
            Console.Write("Press any key to show compressed text (as binary).");
            Console.ReadKey(true);
            Console.Clear();
            Console.WriteLine(binary + Environment.NewLine + Environment.NewLine + "File size reduced by: " + percentageDifference + "%.");
            Console.WriteLine(Environment.NewLine + "Press any key to decompress to readable text.");
            Console.ReadKey(true);
            Console.Clear();
            Console.WriteLine(text);
            Console.ReadKey(true);
            Console.Clear();
        }

        private static string GetFilePath()
        {
            Console.WriteLine("Enter file path:" + Environment.NewLine);
            string filePath = string.Empty;
            while (true)
            {
                filePath = Console.ReadLine();
                if (File.Exists(filePath))
                {
                    Console.Clear();
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Not a valid file path, try again:" + Environment.NewLine);

                }
            }

            return filePath;
        }

        private static string ConvertToText(string binary)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in binary.Split(seperator))
            {
                char character = huffmanCodes.First(x => x.Value == c).Key;
                sb.Append(character);
            }
            return sb.ToString();
        }

        private static string ConvertToBinary(string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in text)
            {
                sb.Append(huffmanCodes[c] + seperator);
            }
            sb = sb.Remove(sb.Length - 1, 1); 
            return sb.ToString();
        }

        private static HuffmanTree BuildTree()
        {
            List<HuffmanNode> nodes = new List<HuffmanNode>();
            for (int i = 0; i < characters.Count; i++)
            {
                nodes.Add(new HuffmanNode()
                {
                    Char = characters.ElementAt(i).Key,
                    Frequency = characters.ElementAt(i).Value
                });
            }
            HuffmanTree tree = new HuffmanTree(nodes);
            return tree;
        }

        private static void CountCharacters(string text)
        {
            foreach (char c in text)
            {
                if (characters.ContainsKey(c))
                    characters[c]++;
                else
                    characters.Add(c, 1);
            }
            characters = characters.OrderBy(kv => kv.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        static void BuildHuffmanCodes(HuffmanNode node, string bits = "")
        {
            if (node != null)
            {
                if (node.Left == null && node.Right == null)
                {
                    huffmanCodes.Add(node.Char, bits);
                }
                BuildHuffmanCodes(node.Left, bits + "0");
                BuildHuffmanCodes(node.Right, bits + "1");
            }
        }
    }
}
