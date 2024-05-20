using CombinatoireSandbox.Arbre.ArbreBinaire;
using CombinatoireSandbox.Arbre.ArbreGeneraux;
using CombinatoireSandbox.PrunningGrafting.PrunningGraftingBinaire;
using CombinatoireSandbox.PrunningGrafting.PrunningGraftingK;

namespace Combinatoire
{
    class Program
    {
        static string RepertoireResultatArbres = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Arbres");
        static string RepertoireResultatPosets = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Posets");

        static void Main(string[] args)
        {
            while (true)
            {
                AfficherMenu();
                string choice = Console.ReadLine();
                GererChoix(choice);
            }
        }

        static void AfficherMenu()
        {
            Console.Clear();
            Console.WriteLine(" ======================================================================= ");
            Console.WriteLine("                    Combinatoire sur les arbres binaires                 ");
            Console.WriteLine(" ======================================================================= ");
            Console.WriteLine();
            Console.WriteLine("  Université du Québec a Montréal                               ");
            Console.WriteLine("  Initiation a la recherche                                     ");
            Console.WriteLine("  Étudiant  : Hadji Nadir                                       ");
            Console.WriteLine("  Encadrant : Samuel Giraudo                                    ");
            Console.WriteLine("  Mai 2024                                                      ");

            Console.WriteLine();
            Console.WriteLine(" +---------------------------------------------------------------------+ ");
            Console.WriteLine(" |  Génerateurs d'arbres                                               | ");
            Console.WriteLine(" +---------------------------------------------------------------------+ ");
            Console.WriteLine(" |      [1] : Générateur d'arbres binaires                             | ");
            Console.WriteLine(" |      [2] : Générateur d'arbres d'arité K                            | ");
            Console.WriteLine(" +---------------------------------------------------------------------+ ");
            Console.WriteLine(" |  Ordre Coupe-Greffe                                                 | ");
            Console.WriteLine(" +---------------------------------------------------------------------+ ");
            Console.WriteLine(" |      [3] : Coupe-Greffe sur les arbres binaires                     | ");
            Console.WriteLine(" |      [4] : Coupe-Greffe sur les arbres d'arité k                    | ");
            Console.WriteLine(" +---------------------------------------------------------------------+ ");
            Console.WriteLine(" |  Configuration                                                      | ");
            Console.WriteLine(" +---------------------------------------------------------------------+ ");
            Console.WriteLine(" |      [5] : Afficher nom répertoire des résultats                    | ");
            Console.WriteLine(" +---------------------------------------------------------------------+ ");
            Console.WriteLine(" |  Autres                                                             | ");
            Console.WriteLine(" +---------------------------------------------------------------------+ ");
            Console.WriteLine(" |      [0] : Quitter                                                  | ");
            Console.WriteLine(" +---------------------------------------------------------------------+ ");
            Console.WriteLine();
            Console.Write("Veuillez saisir une option :");
        }

        static void GererChoix(string choice)
        {
            Console.Clear();
            switch (choice)
            {
                case "1":
                    GenererArbresBinaires();
                    break;
                case "2":
                    GenererArbresK();
                    break;
                case "3":
                    PrunningGraftingSurArbresBinaires();
                    break;
                case "4":
                    PrunningGraftingSurArbresK();
                    break;
                case "5":
                    AfficherRepertoireResultats();
                    break;
                case "0":
                    AfficherLigneAvecTemps("Merci d'avoir utilisé l'application. Au revoir !");
                    Environment.Exit(0);
                    break;
                default:
                    AfficherLigneAvecTemps("Choix invalide. Appuyez sur une touche pour réessayer.");
                    Console.ReadKey();
                    break;
            }
        }

        static void GenererArbresBinaires()
        {
            Console.WriteLine("===========================================================");
            Console.WriteLine("            Générer tous les arbres binaires ");
            Console.WriteLine("===========================================================");
            Console.WriteLine();
            if (TryGetValidInput("Nombre de noeuds = ", out int n))
            {
                Console.WriteLine();
                AfficherLigneAvecTemps("En cours ...");
                Console.WriteLine();

                var generateur = new ArbreBinaireGraphviz();
                var repertoire = generateur.GenererImagesToutLesArbreBinaire(n, RepertoireResultatArbres);

                AfficherLigneAvecTemps($"Succès - Les arbres binaires avec {n} nœuds ont été générés.");
                Console.WriteLine();
                AfficherLigneAvecTemps($"Répertoire des résultats: {repertoire}");
                WaitForKeyPress();
            }
        }

        static void GenererArbresK()
        {
            Console.WriteLine("===========================================================");
            Console.WriteLine("            Générer tous les arbres d'arité K ");
            Console.WriteLine("===========================================================");
            Console.WriteLine();
            if (TryGetValidInput("Nombre de noeuds = ", out int n) &&
                TryGetValidInput("Nombre d'arètes  =  ", out int k))
            {
                Console.WriteLine();
                AfficherLigneAvecTemps("En cours ...");
                Console.WriteLine();

                var generateur = new ArbreKGraphviz();
                var repertoire = generateur.GenererImagesToutLesArbreK(n, k, RepertoireResultatArbres);
                // Placeholder for actual tree generation logic

                AfficherLigneAvecTemps($"Succès - Les arbres d'arité K avec {n} nœuds et {k} arêtes par nœud ont été générés.");
                Console.WriteLine();
                AfficherLigneAvecTemps($"Répertoire des résultats: {repertoire}");
                WaitForKeyPress();
            }
        }

        static void PrunningGraftingSurArbresBinaires()
        {
            Console.WriteLine("===========================================================");
            Console.WriteLine("       Ordre coupe-greffe sur les arbres binaires ");
            Console.WriteLine("===========================================================");
            Console.WriteLine();
            if (TryGetValidInput("Nombre de noeuds =  ", out int nbNoeud))
            {
                Console.WriteLine();
                AfficherLigneAvecTemps("En cours ...");
                Console.WriteLine();

                // Execution
                var prunningGraftingBinaire = new PrunningGraftingBinaire(RepertoireResultatPosets, RepertoireResultatArbres);
                var cheminFichier = prunningGraftingBinaire.GenererPoset(nbNoeud);

                AfficherLigneAvecTemps($"Succès - Le poset coupe-greffe sur les arbres binaires avec {nbNoeud} nœuds a été généré.");
                Console.WriteLine();
                AfficherLigneAvecTemps($"Répertoire des résultats :  {cheminFichier}");
                WaitForKeyPress();
            }
        }

        static void PrunningGraftingSurArbresK()
        {
            Console.WriteLine("===========================================================");
            Console.WriteLine("         Ordre coupe-greffe sur les arbres généraux ");
            Console.WriteLine("===========================================================");
            Console.WriteLine();
            if (TryGetValidInput("Nombre de noeuds = ", out int n) && TryGetValidInput("Nombre d'arètes  =  ", out int k))
            {
                Console.WriteLine();
                AfficherLigneAvecTemps("En cours ...");
                Console.WriteLine();

                // Execution
                var prunningGraftingK = new PrunningGraftingK(RepertoireResultatPosets, RepertoireResultatArbres);
                var cheminFichier = prunningGraftingK.GenererPoset(n, k);

                AfficherLigneAvecTemps($"Succès - Le poset coupe-greffe sur les arbres généraux avec {n} nœuds et {k} arêtes par nœud a été généré.");
                Console.WriteLine();
                AfficherLigneAvecTemps($"Répertoire des résultats :  {cheminFichier}");
                WaitForKeyPress();
            }
        }

        static void AfficherRepertoireResultats()
        {
            Console.WriteLine("===========================================================");
            Console.WriteLine("         Afficher nom des répertoires de résultats         ");
            Console.WriteLine("===========================================================");
            Console.WriteLine();

            Console.WriteLine($"- Répertoire de destination pour images d'arbres: \n\n\t {RepertoireResultatArbres}");
            Console.WriteLine();

            Console.WriteLine($"- Répertoire de destination pour images de posets: \n\n\t {RepertoireResultatPosets}");
            Console.WriteLine();

            WaitForKeyPress();
        }

        static bool TryGetValidInput(string message, out int result)
        {
            result = 0;
            while (true)
            {
                AfficherLigneAvecTemps(message, true);
                string input = Console.ReadLine();
                if (int.TryParse(input, out result) && result > 0)
                {
                    return true;
                }
                else
                {
                    AfficherLigneAvecTemps("Entrée invalide. Veuillez entrer un nombre entier positif.");
                    Console.WriteLine();
                    AfficherLigneAvecTemps("Voulez-vous réessayer (o/n)? ");
                    Console.WriteLine();
                    string retry = Console.ReadLine().ToLower();
                    if (retry != "o")
                    {
                        return false; // Retour au menu
                    }
                }
            }
        }

        static void WaitForKeyPress()
        {
            Console.WriteLine();
            Console.WriteLine("Appuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        static void AfficherLigneAvecTemps(string ligne, bool pourSaisie = false)
        {
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("yyyy-MM-dd HH:mm:ss");
            var message = $"{formattedDate} | {ligne}";

            if (pourSaisie)
            {
                Console.Write(message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}

