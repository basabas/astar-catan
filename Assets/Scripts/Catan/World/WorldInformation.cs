using System.Collections.Generic;
using UnityEngine;

namespace Bas.Catan.World
{
	[CreateAssetMenu(fileName = "New World", menuName = "Bas/New WorldSettings", order = 1)]
	public class WorldInformation : ScriptableObject
	{
		[SerializeField] private Vector2 _worldSize;
		[SerializeField] private Vector2 _distanceBetweenNodes;
		[SerializeField] private Vector3 _cameraPosition;
		[SerializeField] private Vector3 _cameraRotation;
		[SerializeField] private List<NodeInfo> _nodes = new List<NodeInfo>();

		public Vector2 WorldSize => _worldSize;
		public Vector2 DistanceBetweenNodes => _distanceBetweenNodes;
		public Vector3 CameraPosition => _cameraPosition;
		public Vector3 CameraRotation => _cameraRotation;
		public List<NodeInfo> Nodes => _nodes;
	}
}
