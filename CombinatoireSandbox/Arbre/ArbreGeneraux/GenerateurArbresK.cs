namespace CombinatoireSandbox.Arbre.ArbreGeneraux
{
    // THE COMPUTER JOURNAL, VOL. 35, NO. 3, PAGE. 308, 1992

    public class GenerateurArbresK
    {
        private int compteur = 1;
        private List<int> lowerBound;
        private List<int> upperBound;
        private List<int> sequence;
        private int k;
        private int n;
        private int kln;

        private List<ElementArbreK> arbres;

        public GenerateurArbresK(int k, int n)
        {
            this.n = n;
            this.k = k;
            kln = ((k - 1) * n) - 1;
            var size = kln + 1;
            lowerBound = Init(size);
            upperBound = Init(size);
            sequence = Init(size);
            lowerBound[0] = 0;
            upperBound[0] = kln;
            arbres = new List<ElementArbreK>();
        }

        private List<int> Init(int size)
        {
            return Enumerable.Repeat(-1, size).ToList();
        }

        private void GenSeq(int i)
        {
            int j;
            var x = lowerBound[i];

            while (x <= upperBound[i])
            {
                sequence[i] = x;

                if (i == kln)
                {
                    Console.Write(compteur + " - ");
                    PrintSeq();
                    var tree = Convert(0, 0, kln);
                    arbres.Add(tree);
                    compteur++;
                }

                else
                {
                    if (lowerBound[i] < x)
                    {
                        lowerBound[(i + 1)] = lowerBound[i];
                        upperBound[(i + 1)] = (x - 1);
                    }

                    if (x < upperBound[i])
                    {
                        j = i + x - lowerBound[i] + 1;
                        lowerBound[j] = (x + 1);
                        upperBound[j] = upperBound[i];
                    }

                    GenSeq(i + 1);
                }

                x = (x + k - 1);
            }
        }

        private void PrintSeq()
        {
            foreach (var elem in sequence)
            {
                Console.Write((elem + 1) + " ");
            }
            Console.WriteLine();
        }

        public ElementArbreK Convert(int i, int lowerBound, int upperBound)
        {
            int j, temp;
            var node = new Noeud(k);

            if (lowerBound > upperBound)
            {
                return new Feuille();
            }
            else
            {
                for (j = 0; j < (k - 1); j++)
                {
                    node.Enfants.Add(Convert((i + 1), lowerBound, (sequence[i]) - 1));
                    temp = lowerBound;
                    lowerBound = sequence[i] + 1;
                    i += sequence[i] - temp + 1;
                }

                node.Enfants.Add(Convert(i, lowerBound, upperBound));
                return node;
            }
        }

        // k = Length of each sequence
        // n = Range of values in each sequence
        public void Execute()
        {
            GenSeq(0);
        }

        public List<ElementArbreK> ObtenirToutLesArbres()
        {
            return arbres;
        }
    }
}
