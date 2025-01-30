using Pathing;
using System.Collections.Generic;
using Bas.Catan.Collections;

namespace Bas.Catan.PathFinding
{
	internal class AStarComparer : IComparer<IAStarNode>
	{
		private readonly Dictionary<IAStarNode, float> _fScore;

		public AStarComparer(Dictionary<IAStarNode, float> fScore)
		{
			_fScore = fScore;
		}

		public int Compare(IAStarNode x, IAStarNode y)
		{
			if(x != null && y != null)
			{
				return _fScore[x].CompareTo(_fScore[y]);
			}
			return 1;
		}
	}
}
