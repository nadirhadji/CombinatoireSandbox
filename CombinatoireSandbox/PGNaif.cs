public class PrunningGraftingOrder
{
    //N is the number of internal nodes
    public static char[] FirstElement(int n)
    {
        var initializedTree = new char[(2 * n) + 1];

        for (int k = 0; k < n; k++)
        {
            initializedTree[k] = '(';
            initializedTree[(n + k)] = ')';
        }

        initializedTree[2 * n] = ')';

        return initializedTree;
    }

    //The input tree must be a valid binary tree
    public static char[] PruneAndGraft(char[] tree, out bool isPossible)
    {
        isPossible = false;
        int length = tree.Length;

        for (int i = 1; i < length; i++)
        {
            if (IsPrunableAtIndex(tree, i) && IsGraftableAtIndex(tree, i, out var graftIndex))
            {
                isPossible = true;

                var treeAsList = tree.ToList();

                //Prunning
                var pruneSubtree = treeAsList.GetRange(i, 3);
                treeAsList.RemoveRange(i, 3);
                treeAsList.Insert(i, ')');

                //Graffting
                treeAsList.RemoveAt(graftIndex);
                treeAsList.InsertRange(graftIndex, pruneSubtree);

                tree = treeAsList.ToArray();
                break;
            }
        }

        return tree;
    }

    public static bool IsPrunableAtIndex(char[] tree, int index)
    {
        var isPrunable = tree[index] == '(' && tree[index + 1] == ')' && tree[index + 2] == ')';
        return isPrunable;
    }

    public static bool IsGraftableAtIndex(char[] tree, int index, out int graftIndex)
    {
        index--;
        bool isGraftIndexFound = false;

        while (!isGraftIndexFound && index > 0)
        {
            if (tree[index] == '(')
            {
                isGraftIndexFound = true;
            }
            else
            {
                index--;
            }
        }

        graftIndex = index;
        return isGraftIndexFound;
    }
}