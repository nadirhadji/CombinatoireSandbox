using System.Text;

namespace CombinatoireSandbox.Arbre.ArbreBinaire
{
    public abstract class ElementArbreBinaire
    {
        public abstract string ObtenirParenthesage();
        public abstract override int GetHashCode();
        public override bool Equals(object obj)
        {
            return obj is ElementArbreBinaire other && GetHashCode() == other.GetHashCode();
        }
    }

    public class Feuille : ElementArbreBinaire
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

    public class Noeud : ElementArbreBinaire
    {
        public ElementArbreBinaire Gauche { get; set; }
        public ElementArbreBinaire Droite { get; set; }

        public Noeud(ElementArbreBinaire gauche, ElementArbreBinaire droite)
        {
            Gauche = gauche;
            Droite = droite;
        }

        public override string ObtenirParenthesage()
        {
            var builder = new StringBuilder();
            builder.Append('(');
            builder.Append(Gauche.ObtenirParenthesage());
            builder.Append(Droite.ObtenirParenthesage());
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
}
