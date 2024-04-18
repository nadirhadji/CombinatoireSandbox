namespace CombinatoireSandbox.ArbreBinaire
{
    public abstract class ElementArbre
    {
        public abstract string Afficher();
        public abstract override int GetHashCode();
        public override bool Equals(object obj)
        {
            return obj is ElementArbre other && GetHashCode() == other.GetHashCode();
        }
    }

    public class Noeud : ElementArbre
    {
        public ElementArbre Gauche { get; set; }
        public ElementArbre Droite { get; set; }

        public Noeud(ElementArbre gauche, ElementArbre droite)
        {
            Gauche = gauche;
            Droite = droite;
        }

        public override string Afficher()
        {
            return "(" + Gauche.Afficher() + Droite.Afficher() + ")";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;
                hash = hash * 31 + (Gauche != null ? Gauche.GetHashCode() : 1);
                hash = hash * 17 + (Droite != null ? Droite.GetHashCode() : 1);
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Noeud other)
            {
                return Gauche.Equals(other.Gauche) && Droite.Equals(other.Droite);
            }
            return false;
        }
    }

    public class Feuille : ElementArbre
    {
        public override string Afficher()
        {
            return ")";
        }

        public override int GetHashCode()
        {
            return 23;
        }

        public override bool Equals(object obj)
        {
            // Toutes les instances de Feuille sont considérées égales
            return obj is Feuille;
        }
    }
}
