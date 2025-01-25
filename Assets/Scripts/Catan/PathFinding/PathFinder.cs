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
		public bool ImprovedAStart => _improvedAStarToggle.isOn;

        [SerializeField] private Toggle _improvedAStarToggle;

		private readonly AStar _aStar = new AStar();
		private readonly List<AStarNode> _result = new List<AStarNode>();

		public List<AStarNode> FindPath(AStarNode start, AStarNode end)
		{
			Clear();

			if(TryGetPath(start, end, out IList<IAStarNode> result))
			{
				_result.AddRange(result.Select(node => node as AStarNode));
				return _result;
			}

            return Enumerable.Empty<AStarNode>().ToList();
        }

		private bool TryGetPath(AStarNode start, AStarNode end, out IList<IAStarNode> aStarResult)
		{
			if(_improvedAStarToggle.isOn)
			{
				return _aStar.TryGetPath(start, end, out aStarResult);
			}
			aStarResult = Pathing.AStar.GetPath(start, end);
			return aStarResult != null;
		}

		public void Clear()
		{
			_result.ForEach(node => node.HighLight(false));
			_result.Clear();
		}
	}
}
