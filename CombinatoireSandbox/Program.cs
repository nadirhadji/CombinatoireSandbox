using CombinatoireSandbox.ArbreBinaire;
using CombinatoireSandbox.PrunningGraftingOrder;

int taille = 5;

var arbres = RecursivePrunningGraftingOrder.GenererToutLesArbres(taille);
var premierNoeud = (Noeud)arbres.First();

var succeseursPGOpremierNoeud = RecursivePrunningGraftingOrder.Successors(premierNoeud);

foreach (var t in succeseursPGOpremierNoeud)
{
    Console.WriteLine(t.Afficher());
}