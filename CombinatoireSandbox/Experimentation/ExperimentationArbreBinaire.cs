using CombinatoireSandbox.Arbre.ArbreBinaire;

namespace CombinatoireSandbox.Experimentation
{
    public class ExperimentationArbreBinaire
    {
        public void TesterRobustesseHashcode()
        {
            // Comparer les hashcode genere par differentes tailles d'arbres
            for (int i = 0; i < 15; i++)
            {
                var allTrees = GenerateurArbreBinaire.GenererToutLesArbres(i);

                var treesCount = allTrees.Count();

                var hashcodes = new HashSet<string>();

                foreach (var tree in allTrees)
                {
                    var hashcode = tree.ObtenirParenthesage();
                    hashcodes.Add(hashcode);
                    //Console.WriteLine($"hashcode : {hashcode} | tree : {tree.ObtenirParenthesage()}");
                }

                Console.WriteLine($" Trees count = {treesCount} | number of unique parenthese generated {hashcodes.Count}");
            }
        }
    }
}
