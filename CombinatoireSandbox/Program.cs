using CombinatoireSandbox.PrunningGrafting.PrunningGraftingK;

const string repertoireArbresBinaires = "C:\\Users\\nadir\\source\\repos\\CombinatoireSandbox\\CombinatoireSandbox\\Images\\Arbres\\ArbresBinaires";
const string repertoirePosetsBinaire = "C:\\Users\\nadir\\source\\repos\\CombinatoireSandbox\\CombinatoireSandbox\\Images\\Posets\\PrunningGraftingBinaire";

const string repertoireArbresK = "C:\\Users\\nadir\\source\\repos\\CombinatoireSandbox\\CombinatoireSandbox\\Images\\Arbres\\ArbresK";
const string repertoirePosetsK = "C:\\Users\\nadir\\source\\repos\\CombinatoireSandbox\\CombinatoireSandbox\\Images\\Posets\\PrunningGraftingK";

//var prunningGraftingBinaire = new PrunningGraftingBinaire(repertoirePosetsBinaire, repertoireArbresBinaires);
//prunningGraftingBinaire.ObtenirPoset(4);

var prunningGraftingK = new PrunningGraftingK(repertoirePosetsK, repertoireArbresK);
prunningGraftingK.ObtenirPoset(3, 3);
