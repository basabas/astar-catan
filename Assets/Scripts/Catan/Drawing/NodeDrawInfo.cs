using Bas.Catan.PathFinding;
using Bas.Catan.Utilities;
using UnityEngine;

namespace Bas.Catan.Drawing
{
	public struct NodeDrawInfo
	{
		private const float HighLightValue = 3f;//Has effect on the color(red part) and scale of a node

        /// <summary>Size that a <see cref="NodeDrawInfo"/> object is in memory</summary>
        public const int Size = 16;

		public float X;
		public float Y;
		public float HighlightValue;
		public float NodeType;

		public static NodeDrawInfo Create(AStarNode node, World.World world)
		{
			Vector2 position = node.ArrayIndex.ToNodePosition(world.WorldInformation);
			return new NodeDrawInfo
			{
				X = position.x,
				Y = position.y,
				HighlightValue = node.HighLighted ? HighLightValue : 1,
				NodeType = (int)node.NodeInfo.NodeType
			};
		}
	}
}
