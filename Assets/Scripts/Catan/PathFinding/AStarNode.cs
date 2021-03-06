using Bas.Catan.World;
using Pathing;
using System.Collections.Generic;
using UnityEngine;

namespace Bas.Catan.PathFinding
{
	public class AStarNode : IAStarNode
	{
		public IEnumerable<IAStarNode> Neighbours { get; private set; }

		public NodeInfo NodeInfo { get; private set; }
		public Vector2Int ArrayIndex { get; private set; }
		public bool HighLighted { get; private set; }

		public AStarNode(NodeInfo nodeInfo, Vector2Int arrayIndex, IEnumerable<IAStarNode> neighbours)
		{
			NodeInfo = nodeInfo;
			ArrayIndex = arrayIndex;
			Neighbours = neighbours;
		}

		public float CostTo(IAStarNode neighbour)
		{
			return neighbour is AStarNode node ? node.NodeInfo.TravelCost : int.MaxValue;
		}

		public float EstimatedCostTo(IAStarNode goal)
		{
			if(goal is AStarNode goalNode)
			{
				Vector2Int diff = goalNode.ArrayIndex - ArrayIndex;
				diff.y = Mathf.Abs(diff.y);
				float yHalfGoalSign = goalNode.ArrayIndex.y % 2 - 0.5f;
				float cost = Mathf.Abs(diff.x + yHalfGoalSign) + diff.y / 2f;

				if(diff.y % 2 == 0)
				{
					cost -= yHalfGoalSign * Mathf.Sign(diff.x);
				}

				return Mathf.Max(diff.y, cost);
			}

			return float.MaxValue;
		}

		public void HighLight(bool highLight = true)
		{
			HighLighted = highLight;
		}
	}
}
