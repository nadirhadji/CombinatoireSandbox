namespace CombinatoireSandbox.ArbreBinaire
{
    public abstract class ElementArbre
    {
        public abstract string Afficher();
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
            return "(" + Gauche.Afficher() + Droite.Afficher();
        }
    }

    public class Feuille : ElementArbre
    {
        public override string Afficher()
        {
            return ")";
        }
    }
}
