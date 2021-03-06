using Pathing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bas.Catan.Collections
{
	public class NeighbourNodeCollection : IEnumerable<IAStarNode>
	{
		private readonly IAStarNode[,] _nodes;
		private readonly Vector2Int _arrayIndex;

		public NeighbourNodeCollection(IAStarNode[,] nodes, Vector2Int arrayIndex)
		{
			_nodes = nodes;
			_arrayIndex = arrayIndex;
		}

		public IEnumerator<IAStarNode> GetEnumerator()
		{
			IAStarNode node = _nodes[_arrayIndex.x, _arrayIndex.y];
			if(_arrayIndex.x > 0)
			{
				if(node.CostTo(_nodes[_arrayIndex.x - 1, _arrayIndex.y]) > 0)
				{
					yield return _nodes[_arrayIndex.x - 1, _arrayIndex.y];
				}
				if(_arrayIndex.y % 2 == 0)
				{
					if(_arrayIndex.y > 0 && node.CostTo(_nodes[_arrayIndex.x - 1, _arrayIndex.y - 1]) > 0)
					{
						yield return _nodes[_arrayIndex.x - 1, _arrayIndex.y - 1];
					}
					if(_arrayIndex.y < _nodes.GetLength(1) - 1 && node.CostTo(_nodes[_arrayIndex.x - 1, _arrayIndex.y + 1]) > 0)
					{
						yield return _nodes[_arrayIndex.x - 1, _arrayIndex.y + 1];
					}
				}
			}

			if(_arrayIndex.x < _nodes.GetLength(0) - 1)
			{
				if(node.CostTo(_nodes[_arrayIndex.x + 1, _arrayIndex.y]) > 0)
				{
					yield return _nodes[_arrayIndex.x + 1, _arrayIndex.y];
				}
				if(_arrayIndex.y % 2 == 1)
				{
					if(_arrayIndex.y > 0 && node.CostTo(_nodes[_arrayIndex.x + 1, _arrayIndex.y - 1]) > 0)
					{
						yield return _nodes[_arrayIndex.x + 1, _arrayIndex.y - 1];
					}
					if(_arrayIndex.y < _nodes.GetLength(1) - 1 && node.CostTo(_nodes[_arrayIndex.x + 1, _arrayIndex.y + 1]) > 0)
					{
						yield return _nodes[_arrayIndex.x + 1, _arrayIndex.y + 1];
					}
				}
			}

			if(_arrayIndex.y > 0 && node.CostTo(_nodes[_arrayIndex.x, _arrayIndex.y - 1]) > 0)
			{
				yield return _nodes[_arrayIndex.x, _arrayIndex.y - 1];
			}
			if(_arrayIndex.y < _nodes.GetLength(1) - 1 && node.CostTo(_nodes[_arrayIndex.x, _arrayIndex.y + 1]) > 0)
			{
				yield return _nodes[_arrayIndex.x, _arrayIndex.y + 1];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
