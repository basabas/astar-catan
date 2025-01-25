using Pathing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bas.Catan.Collections
{
	public class NeighbourNodeCollection : IEnumerable<IAStarNode>
	{
		private readonly IAStarNode[,] _nodes;
		private readonly int _x;
        private readonly int _y;

        public NeighbourNodeCollection(IAStarNode[,] nodes, Vector2Int arrayIndex)
		{
			_nodes = nodes;
			_x = arrayIndex.x;
            _y = arrayIndex.y;
        }

        public IEnumerator<IAStarNode> GetEnumerator()
		{
			IAStarNode node = _nodes[_x, _y];
			if(_x > 0)
			{
				if(node.CostTo(_nodes[_x - 1, _y]) > 0)
				{
					yield return _nodes[_x - 1, _y];
				}
				if(_y % 2 == 0)
				{
					if(_y > 0 && node.CostTo(_nodes[_x - 1, _y - 1]) > 0)
					{
						yield return _nodes[_x - 1, _y - 1];
					}
					if(_y < _nodes.GetLength(1) - 1 && node.CostTo(_nodes[_x - 1, _y + 1]) > 0)
					{
						yield return _nodes[_x - 1, _y + 1];
					}
				}
			}

			if(_x < _nodes.GetLength(0) - 1)
			{
				if(node.CostTo(_nodes[_x + 1, _y]) > 0)
				{
					yield return _nodes[_x + 1, _y];
				}
				if(_y % 2 == 1)
				{
					if(_y > 0 && node.CostTo(_nodes[_x + 1, _y - 1]) > 0)
					{
						yield return _nodes[_x + 1, _y - 1];
					}
					if(_y < _nodes.GetLength(1) - 1 && node.CostTo(_nodes[_x + 1, _y + 1]) > 0)
					{
						yield return _nodes[_x + 1, _y + 1];
					}
				}
			}

			if(_y > 0 && node.CostTo(_nodes[_x, _y - 1]) > 0)
			{
				yield return _nodes[_x, _y - 1];
			}
			if(_y < _nodes.GetLength(1) - 1 && node.CostTo(_nodes[_x, _y + 1]) > 0)
			{
				yield return _nodes[_x, _y + 1];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
