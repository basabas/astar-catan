using Pathing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Bas.Catan.PathFinding
{
	[System.Serializable]
	public class PathFinder
	{
		[SerializeField] private Toggle _improvedAStarToggle;

		private readonly AStar _astar = new AStar();
		private readonly List<AStarNode> _result = new List<AStarNode>();

		public bool FindPath(AStarNode start, AStarNode end)
		{
			Clear();

			if(TryGetPath(start, end, out IList<IAStarNode> result))
			{
				_result.AddRange(result.Select(node => node as AStarNode));
				_result.ToList().ForEach(node => node.HighLight());
				return true;
			}
			return false;
		}

		private bool TryGetPath(AStarNode start, AStarNode end, out IList<IAStarNode> astarResult)
		{
			if(_improvedAStarToggle.isOn)
			{
				return _astar.TryGetPath(start, end, out astarResult);
			}
			astarResult = Pathing.AStar.GetPath(start, end);
			return astarResult != null;
		}

		public void Clear()
		{
			_result.ForEach(node => node.HighLight(false));
			_result.Clear();
		}
	}
}
