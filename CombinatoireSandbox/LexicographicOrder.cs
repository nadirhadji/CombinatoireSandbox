using System.Text;

namespace CombinatoireSandbox.LexicographicOrder
{
    public class LexicographicOrder
    {
        public static void ShowTree(char[] tree)
        {
            var treeAsLinearNotationBuilder = new StringBuilder();
            foreach (var token in tree)
            {
                treeAsLinearNotationBuilder.AppendJoin(" ", token.ToString());
            }

            Console.WriteLine($"{treeAsLinearNotationBuilder} \n");
        }

        //public static void ShowKnuthKnuthLexicographicOrder(int n)
        //{
        //    var trees = KnuthLexicographicOrder(n);

        //    Console.WriteLine("\n*************************************************");
        //    Console.WriteLine("************** Lexicographic Order **************");
        //    Console.WriteLine("*************************************************\n");

        //    int counter = 1;
        //    foreach (var tree in trees)
        //    {
        //        var treeAsLinearNotationBuilder = new StringBuilder();
        //        foreach (var token in tree)
        //        {
        //            treeAsLinearNotationBuilder.AppendJoin(" ", token.ToString());
        //        }

        //        Console.WriteLine($"{counter} - {treeAsLinearNotationBuilder.ToString()} \n");
        //        counter++;
        //    }
        //}

        //public static List<char[]> KnuthLexicographicOrder(int n)
        //{
        //    var m = 2 * n - 1;
        //    var initialTree = InitializeTrees(n);


        //    foreach (var tree in trees)
        //    {
        //        tree[m] = ')';
        //        if (tree[m - 1] == ')')
        //        {
        //            tree[m - 1] = '(';
        //            m--;
        //            break;
        //        }

        //        int j = m - 1;
        //        int k = 2 * n - 1;

        //        while (tree[j] == '(')
        //        {
        //            tree[j] = ')';
        //            tree[k] = '(';
        //            j--;
        //            k -= 2;
        //        }

        //        if (j == 0)
        //            return trees;
        //        else
        //        {
        //            tree[j] = '(';
        //            m = 2 * n - 1;
        //        }
        //    }

        //    return trees;
        //}

        public static List<string> GenerateParentheses(int n)
        {
            var results = new List<string>();
            Generate(results, "", n, 0);
            return results;
        }

        private static void Generate(List<string> results, string s, int n, int openCount)
        {
            if (s.Length == 2 * n)
            {
                results.Add(s);
                return;
            }

            if (openCount > 0)
            {
                Generate(results, s + ")", n, openCount - 1);
            }

            Generate(results, s + "(", n, openCount + 1);
        }

        public static void ShowParentheses(int n)
        {
            List<string> results = GenerateParentheses(n);

            Console.WriteLine("\n*************************************************");
            Console.WriteLine("************** Lexicographic Order **************");
            Console.WriteLine("*************************************************\n");
            foreach (string result in results)
            {
                Console.WriteLine(result);
            }
        }

        //public static List<char[]> KnuthLexicographicOrder(int n)
        //{
        //    var list = new List<char[]>();

        //    var m = 2 * n - 1;
        //    var tree = InitializeTrees(n);
        //    list.Add(tree);
        //}

        public static char[] InitializeTrees(int n)
        {
            var initializedTree = new char[(2 * n) + 1];

            for (int k = 1; k <= n; k++)
            {
                initializedTree[(2 * k) - 1] = '(';
                initializedTree[(2 * k)] = ')';
                initializedTree[0] = ')';
            }

            return initializedTree;
        }
    }
}
