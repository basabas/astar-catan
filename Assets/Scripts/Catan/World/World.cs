using Bas.Catan.PathFinding;

namespace Bas.Catan.World
{
	public class World
	{
		public readonly WorldInformation WorldInformation;
		public readonly AStarNode[,] Nodes;

		public bool HasChanged { get; set; } = true;
		public int Count => Nodes.GetLength(0)* Nodes.GetLength(1);

        public World(AStarNode[,] nodes, WorldInformation worldInformation)
		{
			Nodes = nodes;
			WorldInformation = worldInformation;
		}
	}
}
