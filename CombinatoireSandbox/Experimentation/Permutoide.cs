using System;
using System.Collections.Generic;
using System.Linq;

namespace CombinatoireSandbox.Experimentation
{
    public class Permutoide
    {

        public Permutoide() { }

        public void ObtenirInverseDePermutation(string permutation)
        {
            // Convertir la chaîne de permutation en tableau d'entiers
            int[] permutationArray = Array.ConvertAll(permutation.Split(' '), int.Parse);

            // Créer un tableau pour stocker la permutation inverse
            int[] inverse = new int[permutationArray.Length];

            // Calculer la permutation inverse
            for (int i = 0; i < permutationArray.Length; i++)
            {
                // La position de l'élément actuel devient la valeur de l'élément à sa place dans la permutation inverse
                inverse[permutationArray[i] - 1] = i + 1;
            }

            // Convertir le tableau inverse en chaîne pour l'affichage
            string inversePermutation = string.Join(" ", inverse);
            Console.WriteLine($"La permutation inverse de {permutation} est {inversePermutation}");
        }

        // Cette méthode calcule l'inverse d'une permutation.
        private int[] InversePermutation(int[] permutation)
        {
            int[] inverse = new int[permutation.Length];
            for (int i = 0; i < permutation.Length; i++)
            {
                inverse[permutation[i] - 1] = i + 1;
            }
            return inverse;
        }

        public IEnumerable<Tuple<int, int>> ObtenirCoinversions(string permutation)
        {
            int[] permArray = Array.ConvertAll(permutation.Split(' '), int.Parse);
            int[] inverse = InversePermutation(permArray);
            var coinversions = new List<Tuple<int, int>>();

            // Parcourir chaque paire possible de la permutation
            for (int a = 0; a < inverse.Length - 1; a++)
            {
                for (int b = a + 1; b < inverse.Length; b++)
                {
                    // Vérifier si la paire (a+1, b+1) est une coinversion
                    if (inverse[a] > inverse[b])
                    {
                        coinversions.Add(new Tuple<int, int>(a + 1, b + 1));
                    }
                }
            }

            return coinversions;
        }

        public IEnumerable<Tuple<int, int>> ObtenirInversions(string permutation)
        {
            int[] permArray = Array.ConvertAll(permutation.Split(' '), int.Parse);
            var inversions = new List<Tuple<int, int>>();

            // Parcourir chaque paire possible de la permutation
            for (int i = 0; i < permArray.Length - 1; i++)
            {
                for (int j = i + 1; j < permArray.Length; j++)
                {
                    // Vérifier si la paire (i+1, j+1) est une inversion
                    if (permArray[i] > permArray[j])
                    {
                        inversions.Add(new Tuple<int, int>(i + 1, j + 1));
                    }
                }
            }

            return inversions;
        }

        // Cette méthode détermine si σ est liée à µ par la relation ≤R.
        public bool EstRelieeParOrdreDroite(string sigmaStr, string muStr)
        {
            var coinversionsSigma = ObtenirCoinversions(sigmaStr);
            var coinversionsMu = ObtenirCoinversions(muStr);

            return coinversionsSigma.All(x => coinversionsMu.Contains(x));
        }

        // Cette méthode détermine si σ est liée à µ par la relation ≤L.
        public bool EstRelieeParOrdreGauche(string sigmaStr, string muStr)
        {
            var inversionsSigma = ObtenirInversions(sigmaStr);
            var inversionsMu = ObtenirInversions(muStr);

            return inversionsSigma.All(x => inversionsMu.Contains(x));
        }
    }
}
