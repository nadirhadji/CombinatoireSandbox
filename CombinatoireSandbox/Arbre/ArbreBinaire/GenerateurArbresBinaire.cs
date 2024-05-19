namespace CombinatoireSandbox.Arbre.ArbreBinaire
{
    public class GenerateurArbreBinaire
    {
        public static List<ElementArbreBinaire> GenererToutLesArbres(int n)
        {
            if (n == 0)
            {
                return new List<ElementArbreBinaire>() { new Feuille() };
            }

            var resultat = new List<ElementArbreBinaire>();

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
