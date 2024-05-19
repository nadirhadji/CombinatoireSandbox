using System.Text;

namespace CombinatoireSandbox.Arbre.ArbreGeneraux
{
    public abstract class ElementArbreK
    {
        public abstract string ObtenirParenthesage();
        public abstract override int GetHashCode();
        public override bool Equals(object obj)
        {
            return obj is ElementArbreK other && GetHashCode() == other.GetHashCode();
        }
    }

    public class Feuille : ElementArbreK
    {
        public override string ObtenirParenthesage()
        {
            return ")";
        }

        public override int GetHashCode()
        {
            return ObtenirParenthesage().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            // Toutes les instances de Feuille sont considérées égales
            return obj is Feuille;
        }
    }

    public class Noeud : ElementArbreK
    {
        public List<ElementArbreK> Enfants { get; set; }

        public Noeud(int nbEnfants)
        {
            Enfants = new List<ElementArbreK>(nbEnfants);
        }

        public Noeud(List<ElementArbreK> enfants)
        {
            Enfants = new List<ElementArbreK>(enfants);
        }

        public override string ObtenirParenthesage()
        {
            var builder = new StringBuilder();
            builder.Append('(');
            foreach (var enfant in Enfants)
            {
                builder.Append(enfant.ObtenirParenthesage());
            }
            builder.Append(')');
            return builder.ToString();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ObtenirParenthesage().GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Noeud other)
            {
                var hash1 = ObtenirParenthesage();
                var hash2 = other.ObtenirParenthesage();
                return hash1 == hash2;
            }
            return false;
        }
    }

    public class GenerateurArbreK
    {
        public static List<ElementArbreK> GenererTousLesArbres(int n, int k)
        {
            var generator = new GenerateurArbresK(k, n);
            generator.Execute();
            return generator.ObtenirToutLesArbres();
        }
    }
}
