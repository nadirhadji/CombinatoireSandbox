using CombinatoireSandbox.PrunningGrafting.PrunningGraftingK;

const string repertoireArbresBinaires = "C:\\Users\\nadir\\source\\repos\\CombinatoireSandbox\\CombinatoireSandbox\\Images\\Arbres\\ArbresBinaires";
const string repertoirePosetsBinaire = "C:\\Users\\nadir\\source\\repos\\CombinatoireSandbox\\CombinatoireSandbox\\Images\\Posets\\PrunningGraftingBinaire";

const string repertoireArbresK = "C:\\Users\\nadir\\source\\repos\\CombinatoireSandbox\\CombinatoireSandbox\\Images\\Arbres";
const string repertoirePosetsK = "C:\\Users\\nadir\\source\\repos\\CombinatoireSandbox\\CombinatoireSandbox\\Images\\Posets";

//var prunningGraftingBinaire = new PrunningGraftingBinaire(repertoirePosetsBinaire, repertoireArbresBinaires);
//prunningGraftingBinaire.GenererPoset(4);

var prunningGraftingK = new PrunningGraftingK(repertoirePosetsK, repertoireArbresK);

prunningGraftingK.GenererPoset(5, 2);

//Console.WriteLine(nomFichier);
//Console.WriteLine($"Is Lattice -> {isLattice}");