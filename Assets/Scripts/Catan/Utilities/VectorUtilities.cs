using Bas.Catan.World;
using UnityEngine;

namespace Bas.Catan.Utilities
{
	public static class VectorUtilities
	{
		public static bool TryGetPlaneIntersection(Ray ray, Vector3 planePosition, Vector3 planeNormal, out Vector3 intersectionPoint)
		{
			intersectionPoint = Vector3.zero;

			float dotDenominator = Vector3.Dot(ray.direction, planeNormal);

			if(!Mathf.Approximately(dotDenominator, Mathf.Epsilon))
			{
				float dotNumerator = Vector3.Dot(planePosition - ray.origin, planeNormal);
				float length = dotNumerator / dotDenominator;

				intersectionPoint = ray.origin + ray.direction.normalized * length;

				return true;
			}
			return false;
		}

		public static Vector2 ToNodePosition(this Vector2Int index, WorldInformation worldInfo)
		{
			Vector2 halfWorldSize = (worldInfo.WorldSize - Vector2.one) / 2f;
			float xPos = (index.x - halfWorldSize.x + index.y % 2 * 0.5f) * worldInfo.DistanceBetweenNodes.x;
			float yPos = (index.y - halfWorldSize.y) * worldInfo.DistanceBetweenNodes.y;
			return new Vector2(xPos, yPos);
		}

		public static Vector2Int ToNodeIndex(this Vector3 position, WorldInformation worldInfo)
		{
			Vector2 halfWorldSize = (worldInfo.WorldSize - Vector2.one) / 2f;
			int y = Mathf.RoundToInt(position.z / worldInfo.DistanceBetweenNodes.y + halfWorldSize.y);
			int x = Mathf.RoundToInt(position.x / worldInfo.DistanceBetweenNodes.x + halfWorldSize.x - y % 2 * 0.5f);
			return new Vector2Int(x, y);
		}
	}
}
