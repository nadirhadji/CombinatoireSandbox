using QuikGraph;
using System.Text;

namespace CombinatoireSandbox.ArbreBinaire
{
    // Classe pour encapsuler un graphe binaire
    public class GraphNode
    {
        public AdjacencyGraph<string, Edge<string>> InnerGraph { get; private set; }

        public GraphNode(AdjacencyGraph<string, Edge<string>> innerGraph)
        {
            InnerGraph = innerGraph;
        }

        //private static void ConstruireGraphe(ElementArbre element, AdjacencyGraph<string, Edge<string>> graph, string parent = null)
        //{
        //    if (element == null) return; // Condition d'arrêt pour les branches vides

        //    var currentId = element is Feuille ? $"F{++id}" : $"N{++id}";

        //    if (parent != null) // S'il y a un parent, ajoutez une arête entre le parent et l'élément actuel
        //    {
        //        graph.AddVerticesAndEdge(new Edge<string>(parent, currentId));
        //    }

        //    if (element is Noeud noeud)
        //    {
        //        // Traitement pour les nœuds
        //        // Ajouter le nœud actuel au graphe (si pas déjà fait par AddVerticesAndEdge)
        //        graph.AddVertex(currentId);

        //        // Appels récursifs pour les enfants gauche et droit
        //        ConstruireGraphe(noeud.Gauche, graph, currentId);
        //        ConstruireGraphe(noeud.Droite, graph, currentId);
        //    }
        //    else if (element is Feuille)
        //    {
        //        // Traitement pour les feuilles
        //        graph.AddVertex(currentId);
        //    }
        //}

        //public string GenererGraphviz(ElementArbre arbre)
        //{
        //    // Convertir l'arbre binaire en graphe QuikGraph
        //    var graph = new AdjacencyGraph<string, Edge<string>>();
        //    ConstruireGraphe(arbre, graph);

        //    // Générer la représentation Graphviz de l'arbre
        //    var graphviz = new GraphvizAlgorithm<string, Edge<string>>(graph);

        //    // Configurer le formatage des sommets
        //    graphviz.FormatVertex += (sender, args) =>
        //    {
        //        if (args.Vertex.StartsWith("F")) // Identifier les feuilles par leur ID
        //        {
        //            // Attribuer la forme de boîte aux feuilles
        //            args.VertexFormat.Shape = GraphvizVertexShape.Box;
        //            args.VertexFormat.Style = GraphvizVertexStyle.Filled;
        //            args.VertexFormat.FillColor = GraphvizColor.PowderBlue;
        //        }
        //        else
        //        {
        //            // Attribuer une autre forme ou des attributs par défaut aux nœuds
        //            args.VertexFormat.Shape = GraphvizVertexShape.Ellipse;
        //            args.VertexFormat.Style = GraphvizVertexStyle.Filled;
        //            args.VertexFormat.FillColor = GraphvizColor.PaleGreen;
        //        }
        //    };

        //    // Générer le text graphviz
        //    var graphvizText = graphviz.Generate();

        //    return graphvizText;
        //}
    }

    // Classe pour le poset, qui est un graphe de GraphNodes
    public class MetaGraph
    {
        public AdjacencyGraph<GraphNode, Edge<GraphNode>> Graph { get; private set; }

        public MetaGraph()
        {
            Graph = new AdjacencyGraph<GraphNode, Edge<GraphNode>>();
        }

        public void AddBinaryTree(GraphNode node)
        {
            Graph.AddVertex(node);
        }

        public void ConnectBinaryTrees(GraphNode fromNode, GraphNode toNode)
        {
            Graph.AddEdge(new Edge<GraphNode>(fromNode, toNode));
            Graph.AddEdge(new Edge<GraphNode>(toNode, fromNode));
        }

        private static int nodeIdCounter = 0; // Global counter for node IDs

        public static string GenerateDotForMetaGraph(MetaGraph metaGraph)
        {
            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph MetaGraph {");
            dot.AppendLine("  rankdir=TB;"); // Set direction from top to bottom
            dot.AppendLine("  node [shape=box];"); // Set the node shape

            var nodeIds = new Dictionary<GraphNode, string>();
            var representativeNodes = new Dictionary<string, string>(); // Stores representative node for each cluster
            var lastNodes = new Dictionary<string, string>(); // Stores representative node for each cluster end

            // Generate the DOT for the nodes (GraphNodes)
            foreach (var node in metaGraph.Graph.Vertices)
            {
                string nodeId = $"node_{nodeIdCounter++}"; // Create a unique ID for the node
                nodeIds[node] = nodeId;
                string repNode = $"rep_{nodeId}"; // Representative node for connecting clusters
                representativeNodes[nodeId] = repNode;
                var (subgraphDot, lastEdge) = GenerateDotForSingleNode(node, nodeId, repNode);
                lastNodes[nodeId] = lastEdge;
                dot.AppendLine(subgraphDot);
            }

            // Generate the DOT for the edges connecting GraphNodes using representative nodes
            foreach (var edge in metaGraph.Graph.Edges)
            {
                string fromRepNode = representativeNodes[nodeIds[edge.Source]];
                string toRepNode = representativeNodes[nodeIds[edge.Target]];
                dot.AppendLine($"  \"{fromRepNode}\" -> \"{toRepNode}\";");
            }

            dot.AppendLine("}");
            return dot.ToString();
        }

        private static (string, string) GenerateDotForSingleNode(GraphNode node, string nodeId, string repNode)
        {
            StringBuilder subgraph = new StringBuilder();
            subgraph.AppendLine($"  subgraph cluster_{nodeId} {{"); // Ensure the subgraph name is unique
            subgraph.AppendLine("    color=lightgrey;");
            subgraph.AppendLine("    node [style=filled,color=white];");
            subgraph.AppendLine($"    label=\"{nodeId}\";"); // Label the subgraph with the unique ID

            // Create an invisible representative node for connecting clusters
            subgraph.AppendLine($"    \"{repNode}\" [style=invis];");

            Edge<string>? dernierEdge = null;
            foreach (var edge in node.InnerGraph.Edges)
            {
                dernierEdge = edge;
                subgraph.AppendLine($"    \"{edge.Source}\" -> \"{edge.Target}\";");
            }

            subgraph.AppendLine("  }");
            return (subgraph.ToString(), dernierEdge.ToString());
        }
    }
}
