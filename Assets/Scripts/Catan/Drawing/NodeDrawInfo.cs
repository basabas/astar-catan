using Bas.Catan.PathFinding;
using Bas.Catan.Utilities;
using UnityEngine;

namespace Bas.Catan.Drawing
{
	public struct NodeDrawInfo
	{
		/// <summary>Size that a <see cref="NodeDrawInfo"/> object is in memory</summary>
		public static int Size => 16;

		public float X;
		public float Y;
		public float HighlightValue;
		public float NodeType;

		public static NodeDrawInfo Create(AStarNode node, World.World world)
		{
			const float highLightValue = 3f;//Has effect on the color(red part) and scale of a node

			Vector3 position = node.ArrayIndex.ToNodePosition(world.WorldInformation);
			return new NodeDrawInfo
			{
				X = position.x,
				Y = position.z,
				HighlightValue = node.HighLighted ? highLightValue : 1,
				NodeType = (int)node.NodeInfo.NodeType
			};
		}
	}
}
