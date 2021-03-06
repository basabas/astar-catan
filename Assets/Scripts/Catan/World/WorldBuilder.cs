using Bas.Catan.Collections;
using Bas.Catan.PathFinding;
using UnityEngine;

namespace Bas.Catan.World
{
	public class WorldBuilder
	{
		public World BuildWorld(WorldInformation worldInformation)
		{
			AStarNode[,] nodes = new AStarNode[(int)worldInformation.WorldSize.x, (int)worldInformation.WorldSize.y];

			for(int x = 0; x < nodes.GetLength(0); x++)
			{
				for(int z = 0; z < nodes.GetLength(1); z++)
				{
					NodeInfo nodeInfo = worldInformation.Nodes[Random.Range(0, worldInformation.Nodes.Count)];
					Vector2Int nodeIndex = new Vector2Int(x, z);
					nodes[x, z] = new AStarNode(nodeInfo, nodeIndex, new NeighbourNodeCollection(nodes, nodeIndex));
				}
			}

			return new World(nodes, worldInformation);
		}
	}
}
