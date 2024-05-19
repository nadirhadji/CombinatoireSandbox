using CombinatoireSandbox.Arbre.ArbreGeneraux;
using static CombinatoireSandbox.Arbre.ArbreUtils;

namespace CombinatoireSandbox.PrunningGrafting.PrunningGraftingK
{
    public class PrunningGraftingK
    {
        private readonly string repertoirePosets;
        private readonly string repertoireArbres;

        public PrunningGraftingK(string repertoirePosets, string repertoireArbres)
        {
            this.repertoirePosets = repertoirePosets;
            this.repertoireArbres = repertoireArbres;
        }

        public (string, bool) GenererPoset(int n, int k)
        {
            var graphvizService = new PrunningGraftingKGraphviz(repertoirePosets, repertoireArbres);
            var toutLesArbres = GenerateurArbreK.GenererTousLesArbres(n, k);
            var mapDesSucesseurs = ObtenirMapDesSucceseurs(toutLesArbres, k);
            var nomFichier = graphvizService.GenererVisualisationPruningGraftingK(mapDesSucesseurs, toutLesArbres, n, k);
            var isLattice = IsLattice(mapDesSucesseurs, k);
            return (nomFichier, isLattice);
        }

        public Dictionary<ElementArbreK, List<ElementArbreK>> ObtenirMapDesSucceseurs(List<ElementArbreK> toutLesArbres, int k)
        {
            var mapDesSucceseursVide = Initialiser(toutLesArbres);
            var mapDesSucceseurs = ConstruireDictionnaireDesSucceseurs(toutLesArbres, mapDesSucceseursVide, k);
            return mapDesSucceseurs;
        }

        public Dictionary<ElementArbreK, List<ElementArbreK>> Initialiser(List<ElementArbreK> toutLesArbresDeTailleN)
        {
            var mapDesSucceseurs = new Dictionary<ElementArbreK, List<ElementArbreK>>();

            foreach (var arbre in toutLesArbresDeTailleN)
            {
                mapDesSucceseurs.TryAdd(arbre, new List<ElementArbreK>());
            }

            return mapDesSucceseurs;
        }

        public Dictionary<ElementArbreK, List<ElementArbreK>> ConstruireDictionnaireDesSucceseurs(List<ElementArbreK> toutLesArbresDeTailleN,
                                                                                                  Dictionary<ElementArbreK, List<ElementArbreK>> mapDesSucceseurs,
                                                                                                  int k)
        {
            // Definir tout les successeurs
            foreach (var arbre in toutLesArbresDeTailleN)
            {
                foreach (var successeur in Successors(arbre, k))
                {
                    mapDesSucceseurs[arbre].Add(successeur);
                }
            }

            return mapDesSucceseurs;
        }

        public List<ElementArbreK> Successors(ElementArbreK elementArbre, int k)
        {
            if (elementArbre is Feuille)
            {
                return new List<ElementArbreK>();
            }
            else if (elementArbre is Noeud)
            {
                var noeud = elementArbre as Noeud;
                var nombre_enfants = noeud.Enfants.Count;

                // Etape 1 - Plongée recursive ************************
                var combinee = new List<ElementArbreK>();
                for (int i = 0; i < nombre_enfants; i++)
                {
                    var enfant = noeud.Enfants[i];

                    var freresGauche = noeud.Enfants.Take(i).ToList();
                    var freresDroite = noeud.Enfants.Skip(i + 1).ToList();

                    var successeurs = Successors(enfant, k);

                    var nouveauxNoeuds = new List<ElementArbreK>();

                    for (int j = 0; j < successeurs.Count; j++)
                    {
                        var nouveauEnfants = new List<ElementArbreK>();
                        var sucesseur = successeurs[j];
                        nouveauEnfants.AddRange(freresGauche);
                        nouveauEnfants.Add(sucesseur);
                        nouveauEnfants.AddRange(freresDroite);
                        var nouveauNoeud = new Noeud(nouveauEnfants);
                        nouveauxNoeuds.Add(nouveauNoeud);
                    }

                    combinee.AddRange(nouveauxNoeuds);
                }

                // Etape 2 - Generation des noeud sont le premier noeud enfant est supprimee 
                var arbresAvecPremierNoeudSuprimme = new List<Option<ElementArbreK>>();

                for (int ii = 1; ii < nombre_enfants; ii++)
                {
                    var nouveauNoeudApresSupression = DeleteFirstNode(noeud.Enfants[ii], k);
                    arbresAvecPremierNoeudSuprimme.Add(nouveauNoeudApresSupression);
                }

                // Etape 3 - Transfert des noeud 
                for (int jj = 0; jj < arbresAvecPremierNoeudSuprimme.Count; jj++)
                {
                    var nouveauJ = arbresAvecPremierNoeudSuprimme[jj];

                    if (nouveauJ.HasValue == true)
                    {
                        var ancienGauche = noeud.Enfants.Take(jj).ToList();
                        var ancienDroite = noeud.Enfants.Skip(jj + 2).ToList();

                        var filledLastLeaf = FillLastLeaf(noeud.Enfants[jj], k);

                        // Construction du nouveau noeud apres coupe et greffe
                        var newEnfants = new List<ElementArbreK>();
                        newEnfants.AddRange(ancienGauche);
                        newEnfants.Add(filledLastLeaf);
                        newEnfants.Add(nouveauJ.Value);
                        newEnfants.AddRange(ancienDroite);
                        var noeudCoupeGreffe = new Noeud(newEnfants);

                        combinee.Insert(0, noeudCoupeGreffe);
                    }
                }

                return combinee;
            }
            else
            {
                throw new InvalidOperationException("Oulala, c'est le temps de debugger!");
            }

            //var leftSuccessors = Successors(n.Gauche).Select(t1 => new Noeud(t1, n.Milieu1, n.Milieu2, n.Droite) as ElementArbreBinaire).ToList();
            //var milleu1Successors = Successors(n.Milieu1).Select(t2 => new Noeud(n.Gauche, t2, n.Milieu2, n.Droite) as ElementArbreBinaire).ToList();
            //var milleu2Successors = Successors(n.Milieu2).Select(t3 => new Noeud(n.Gauche, n.Milieu1, t3, n.Droite) as ElementArbreBinaire).ToList();
            //var rightSuccessors = Successors(n.Droite).Select(t4 => new Noeud(n.Gauche, n.Milieu1, n.Milieu2, t4) as ElementArbreBinaire).ToList();

            //var combined = leftSuccessors.Concat(milleu1Successors).Concat(milleu2Successors).Concat(rightSuccessors).ToList();

            //Option<ElementArbreBinaire> nouveauMilieu1 = DeleteFirstNode(n.Milieu1);
            //Option<ElementArbreBinaire> nouveauMilieu2 = DeleteFirstNode(n.Milieu2);
            //Option<ElementArbreBinaire> nouveauDroite = DeleteFirstNode(n.Droite);

            //if (nouveauMilieu1.HasValue == true)
            //{
            //    ElementArbreBinaire filledLastLeafGauche = FillLastLeaf(n.Gauche);
            //    combined.Insert(0, new Noeud(filledLastLeafGauche, nouveauMilieu1.Value, n.Milieu, n.Droite));
            //}

            //if (nouveauMilieu2.HasValue == true)
            //{
            //    ElementArbreBinaire filledLastLeafMilieu1 = FillLastLeaf(n.Milieu1);
            //    combined.Insert(0, new Noeud(n.Gauche, filledLastLeafMilieu1, nouveauMilieu2.Value, n.Droite));
            //}

            //if (nouveauDroite.HasValue == true)
            //{
            //    ElementArbreBinaire filledLastLeafMilieu2 = FillLastLeaf(n.Milieu2);
            //    combined.Insert(0, new Noeud(n.Gauche, n.Milieu1, filledLastLeafMilieu2, nouveauDroite.Value));
            //}

            // Autrement , 
            ////if (nouveauDroite.HasValue == false)
            ////{
            ////    return combined;
            ////}
            ////else
            ////{
            ////    ElementArbreBinaire filledLastLeafGauche = FillLastLeaf(n.Gauche);
            ////    ElementArbreBinaire filledLastLeafMilieu1 = FillLastLeaf(n.Milieu1);
            ////    ElementArbreBinaire filledLastLeafMilieu2 = FillLastLeaf(n.Milieu2);


            ////    combined.Insert(0, new Noeud(filledLastLeafGauche, nouveauMilieu1.Value, ancienMilieu2, ancienDroit));
            ////    combined.Insert(0, new Noeud(ancienGauche, filledLastLeafMilieu1, nouveauMilieu2.Value, ancienDroit));
            ////    combined.Insert(0, new Noeud(ancienGauche, ancienMilieu1, filledLastLeafMilieu2, nouveauDroite.Value));
            ////}

            //return combined;
        }


        public ElementArbreK FillLastLeaf(ElementArbreK element, int k)
        {
            if (element is Feuille)
            {
                var feuilles = Enumerable.Repeat(new Feuille() as ElementArbreK, k).ToList();
                return new Noeud(feuilles);
            }
            else
            {
                var noeud = element as Noeud;
                var nouveauNoeud = new Noeud(k);

                var premiersNoeud = noeud.Enfants.GetRange(0, noeud.Enfants.Count - 1);
                var dernierNoeud = noeud.Enfants.Last();

                nouveauNoeud.Enfants.AddRange(premiersNoeud);
                var filled = FillLastLeaf(dernierNoeud, k);
                nouveauNoeud.Enfants.Add(filled);

                return nouveauNoeud;
            }
        }

        public Option<ElementArbreK> DeleteFirstNode(ElementArbreK tree, int k)
        {
            if (tree is Feuille)
            {
                return Option<ElementArbreK>.None;
            }
            else if (tree is Noeud node)
            {
                if (node.Enfants.All(n => n is Feuille))
                {
                    return Option<ElementArbreK>.Some(new Feuille());
                }
                else
                {
                    var premierNoeud = node.Enfants.First();
                    Option<ElementArbreK> nouveauPremier = DeleteFirstNode(premierNoeud, k);

                    if (nouveauPremier.HasValue)
                    {
                        var nouveauNoeud = new Noeud(k);
                        nouveauNoeud.Enfants.Add(nouveauPremier.Value);

                        var resteNoeud = node.Enfants.GetRange(1, node.Enfants.Count - 1);
                        nouveauNoeud.Enfants.AddRange(resteNoeud);

                        return Option<ElementArbreK>.Some(nouveauNoeud);
                    }
                    else
                    {
                        return Option<ElementArbreK>.None;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Erreur Innatendu : type de l'element ne correspond pas a ElementArbreK");
            }
        }

        public bool IsCoveringRelation(ElementArbreK t, ElementArbreK t1, int k)
        {
            return Successors(t, k).Contains(t1);
        }

        public bool IsLeq(ElementArbreK t, ElementArbreK t1, int k)
        {
            if (Equals(t, t1)) return true;
            return Successors(t, k).Any(t2 => IsLeq(t2 as Noeud, t1 as Noeud, k));
        }

        public bool IsLattice(Dictionary<ElementArbreK, List<ElementArbreK>> mapDesSucesseurs, int k)
        {
            var elements = mapDesSucesseurs.Keys.ToList();

            foreach (var a in elements)
            {
                foreach (var b in elements)
                {
                    if (!ExistsJoin(elements, a, b, k) || !ExistsMeet(elements, a, b, k))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ExistsJoin(List<ElementArbreK> elements, ElementArbreK a, ElementArbreK b, int k)
        {
            //var upperBounds = new List<ElementArbreK>();
            //foreach (var x in elements)
            //{
            //    if (IsLeq(a, x, k) && IsLeq(b, x, k))
            //    {
            //        upperBounds.Add(x);
            //    }
            //}
            //if (upperBounds.Count == 0) return false;

            //ElementArbreK leastUpperBound = upperBounds[0];
            //foreach (var x in upperBounds)
            //{
            //    if (IsLeq(x, leastUpperBound, k))
            //    {
            //        leastUpperBound = x;
            //    }
            //}
            //return true;

            var upperBounds = elements.Where(x => IsLeq(a, x, k) && IsLeq(b, x, k)).ToList();
            if (upperBounds.Count == 0) return false;

            var leastUpperBound = upperBounds.FirstOrDefault(ub => upperBounds.All(x => !IsLeq(x, ub, k)
                                                                || EqualityComparer<ElementArbreK>.Default.Equals(x, ub)));
            return leastUpperBound != null;
        }

        private bool ExistsMeet(List<ElementArbreK> elements, ElementArbreK a, ElementArbreK b, int k)
        {
            //var lowerBounds = new List<ElementArbreK>();
            //foreach (var x in elements)
            //{
            //    if (IsLeq(x, a, k) && IsLeq(x, b, k))
            //    {
            //        lowerBounds.Add(x);
            //    }
            //}
            //if (lowerBounds.Count == 0) return false;

            //ElementArbreK greatestLowerBound = lowerBounds[0];
            //foreach (var x in lowerBounds)
            //{
            //    if (IsLeq(greatestLowerBound, x, k))
            //    {
            //        greatestLowerBound = x;
            //    }
            //}
            //return true;

            var lowerBounds = elements.Where(x => IsLeq(x, a, k) && IsLeq(x, b, k)).ToList();
            if (lowerBounds.Count == 0) return false;

            var greatestLowerBound = lowerBounds.FirstOrDefault(lb => lowerBounds.All(x => !IsLeq(lb, x, k)
                                                                                        || EqualityComparer<ElementArbreK>.Default.Equals(x, lb)));
            return greatestLowerBound != null;
        }
    }
}
