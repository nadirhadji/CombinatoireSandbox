using CombinatoireSandbox.PrunningGrafting.PrunningGraftingK;

const string repertoireArbresBinaires = "C:\\Users\\nadir\\source\\repos\\CombinatoireSandbox\\CombinatoireSandbox\\Images\\Arbres\\ArbresBinaires";
const string repertoirePosetsBinaire = "C:\\Users\\nadir\\source\\repos\\CombinatoireSandbox\\CombinatoireSandbox\\Images\\Posets\\PrunningGraftingBinaire";

const string repertoireArbresK = "C:\\Users\\nadir\\source\\repos\\CombinatoireSandbox\\CombinatoireSandbox\\Images\\Arbres\\ArbresK";
const string repertoirePosetsK = "C:\\Users\\nadir\\source\\repos\\CombinatoireSandbox\\CombinatoireSandbox\\Images\\Posets\\PrunningGraftingK";

//var prunningGraftingBinaire = new PrunningGraftingBinaire(repertoirePosetsBinaire, repertoireArbresBinaires);
//prunningGraftingBinaire.GenererPoset(4);

var prunningGraftingK = new PrunningGraftingK(repertoirePosetsK, repertoireArbresK);
var (nomFichier, isLattice) = prunningGraftingK.GenererPoset(3, 4);

Console.WriteLine(nomFichier);
Console.WriteLine($"Is Lattice -> {isLattice}");