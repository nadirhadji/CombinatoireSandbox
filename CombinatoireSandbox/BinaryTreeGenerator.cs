using CombinatoireSandbox.Catalan;

namespace CombinatoireSandbox.BinaryTrees
{
    public enum TreeElement
    {
        Node = 0,
        Lead = 1
    }

    public struct TreeAsLinearNotation
    {
        public int N;
        public IEnumerable<TreeElement> LinearTree;
    }

    public class BinaryTreeGenerator
    {
        //The n represent le number of internal nodes inside the binary tree.
        public static IEnumerable<TreeAsLinearNotation> GenerateAllBinaryTrees(int n)
        {
            if (n < 1)
            {
                throw new InvalidOperationException("A binary tree must contain at least one internal node");
            }

            var result = new List<TreeAsLinearNotation>();
            var numberOfTrees = CatalanNumber.NthCatalanNumber(n);


            // BIG TODO

            return result;
        }

        // n is the number of internal nodes
        public static void Generate(int n)
        {
            foreach (string tree in GenerateTrees(n))
            {
                Console.WriteLine(tree);
            }
        }

        public static List<string> GenerateTrees(int n)
        {
            List<string> trees = new List<string>();
            if (n == 0)
            {
                trees.Add("");
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    foreach (string left in GenerateTrees(i))
                    {
                        foreach (string right in GenerateTrees(n - i - 1))
                        {
                            trees.Add("(" + left + ")" + "(" + right + ")");
                        }
                    }
                }
            }
            return trees;
        }
    }
}
