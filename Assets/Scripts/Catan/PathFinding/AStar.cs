using Bas.Catan.Collections;
using Pathing;
using System.Collections.Generic;

namespace Bas.Catan.PathFinding
{
	public class AStar
	{
		private readonly HashSet<IAStarNode> _closed;
		private readonly Dictionary<IAStarNode, IAStarNode> _cameFrom;
		private readonly Dictionary<IAStarNode, float> _gScore;
		private readonly Dictionary<IAStarNode, float> _hScore;
		private readonly Dictionary<IAStarNode, float> _fScore;
		private readonly AStarComparer _sorter;
		private readonly Heap<IAStarNode> _open;

		public AStar()
		{
			_closed = new HashSet<IAStarNode>();
			_cameFrom = new Dictionary<IAStarNode, IAStarNode>();
			_gScore = new Dictionary<IAStarNode, float>();
			_hScore = new Dictionary<IAStarNode, float>();
			_fScore = new Dictionary<IAStarNode, float>();
			_sorter = new AStarComparer(_fScore);
			_open = new Heap<IAStarNode>(1024, _sorter);
		}

		public bool TryGetPath(IAStarNode start, IAStarNode goal, out IList<IAStarNode> path)
		{
			path = new List<IAStarNode>();
			if(start != null && goal != null)
			{
				_closed.Clear();
				_open.Clear();

				_cameFrom.Clear();
				_gScore.Clear();
				_hScore.Clear();
				_fScore.Clear();

				_gScore.Add(start, 0f);
				_hScore.Add(start, start.EstimatedCostTo(goal));
				_fScore.Add(start, _hScore[start]);
				_open.Push(start);

				IAStarNode current;
				float tentativeGScore;
				float currentGScore;
				float hScore;

                while (_open.Count > 0)
				{
					current = _open.Pop();
					if(current == goal)
					{
						ReconstructPath(path, goal);
						return true;
					}
					_closed.Add(current);

					currentGScore = _gScore[current];

                    foreach (IAStarNode neighbour in current.Neighbours)
					{
						if (_closed.Contains(neighbour))
						{
							continue;
						}
                        tentativeGScore = currentGScore + current.CostTo(neighbour);
                        if (!_gScore.TryGetValue(neighbour, out float neighborGScore) || tentativeGScore < neighborGScore)
                        {
                            _cameFrom[neighbour] = current;
							hScore = neighbour.EstimatedCostTo(goal);
							_hScore[neighbour] = hScore;
                            _gScore[neighbour] = tentativeGScore;
                            _fScore[neighbour] = tentativeGScore + hScore;
                            _open.Push(neighbour);
                        }
                    }
				}
			}
			return false;
		}

		private void ReconstructPath(IList<IAStarNode> path, IAStarNode currentNode)
		{
			if(_cameFrom.TryGetValue(currentNode, out IAStarNode next))
			{
				ReconstructPath(path, next);
			}
			path.Add(currentNode);
		}
	}
}
