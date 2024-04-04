using CombinatoireSandbox.ArbreBinaire;

namespace CombinatoireSandbox.PrunningGraftingOrder
{
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

    public class RecursivePrunningGraftingOrder
    {
        public static ElementArbre DeleteFirstNode(ElementArbre t)
        {
            switch (t)
            {
                case Feuille:
                    throw new Exception("Error.");

                case Noeud n when n.Gauche is Feuille && n.Droite is Feuille:
                    return new Feuille();

                case Noeud n when n.Gauche is ElementArbre && n.Droite is ElementArbre:
                    return new Noeud(DeleteFirstNode(n.Gauche), n.Droite);

                default:
                    return null; // Should not occur
            }
        }

        public static ElementArbre FillLastLeaf(ElementArbre t)
        {
            switch (t)
            {
                case Feuille:
                    return new Noeud(new Feuille(), new Feuille());

                case Noeud n when n.Gauche is ElementArbre && n.Droite is ElementArbre:
                    return new Noeud(n.Gauche, FillLastLeaf(n.Droite));

                default:
                    return null; // Should not occur
            }
        }

        public static List<ElementArbre> Successors(ElementArbre t)
        {
            switch (t)
            {
                case Feuille:
                    return new List<ElementArbre>();

                case Noeud n when n.Gauche is ElementArbre && n.Droite is Feuille:
                    return Successors(n.Gauche).Select(t1 => new Noeud(t1, new Feuille()) as ElementArbre).ToList();

                case Noeud n when n.Gauche is ElementArbre && n.Droite is ElementArbre:
                    var leftSuccessors = Successors(n.Gauche).Select(t1 => new Noeud(t1, n.Droite) as ElementArbre).ToList();
                    var rightSuccessors = Successors(n.Droite).Select(t2 => new Noeud(n.Gauche, t2) as ElementArbre).ToList();
                    var combined = leftSuccessors.Concat(rightSuccessors).ToList();
                    
                    try 
                    {
                        var centerSuccesor = new List<Noeud>();
                        centerSuccesor = centerSuccesor.Add(new Noeud(FillLastLeaf(n.Gauche), DeleteFirstNode(n.Droite)));
                        combined.Concat(centerSuccesor);
                    } catch (Exception e) {}
                                        
                    return combined;

                default:
                    return null;
            }
        }

        public static bool IsCoveringRelation(Noeud t, Noeud t1)
        {
            return Successors(t).Contains(t1); // This presumes Noeud implements an appropriate equality comparison
        }

        public static bool IsLeq(Noeud t, Noeud t1)
        {
            if (Equals(t, t1)) return true;
            return Successors(t).Any(t2 => IsLeq(t2 as Noeud, t1 as Noeud));
        }

        public static IEnumerable<ElementArbre> GenererToutLesArbres(int n)
        {
            if (n == 0)
            {
                return new List<ElementArbre>() { new Feuille() };
            }

            var resultat = new List<ElementArbre>();

            for (var n1 = 0; n1 <= (n - 1); n1++)
            {
                foreach (var t1 in GenererToutLesArbres(n1))
                {
                    foreach (var t2 in GenererToutLesArbres((n - 1) - n1))
                    {
                        var t = new Noeud(t1, t2);
                        resultat.Insert(0, t);
                    }
                }
            }

            return resultat;
        }
    }
}
