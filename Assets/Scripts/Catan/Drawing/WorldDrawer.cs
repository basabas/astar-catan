using Bas.Catan.PathFinding;
using System.Collections.Generic;
using UnityEngine;

namespace Bas.Catan.Drawing
{
	[System.Serializable]
	public class WorldDrawer
	{
		private static readonly Bounds _drawBounds = new Bounds(Vector3.zero, Vector3.one * 1000);
		private static readonly int _positionBufferID = Shader.PropertyToID("positionBuffer");

		[SerializeField] private Material _nodeMaterial;
		[SerializeField] private Mesh _nodeMesh;

		private readonly List<NodeDrawInfo> _nodeDrawInfos = new List<NodeDrawInfo>();

		private ComputeBuffer _positionBuffer;
		private ComputeBuffer _argsBuffer;
		private uint[] _args;

		private void RebuildBuffers(World.World world)
		{
			_nodeDrawInfos.Clear();
			foreach(AStarNode node in world.Nodes)
			{
				_nodeDrawInfos.Add(NodeDrawInfo.Create(node, world));
			}

			_positionBuffer?.Dispose();
			_argsBuffer?.Dispose();

			uint numIndices = _nodeMesh ? _nodeMesh.GetIndexCount(0) : 0;
			_args = new uint[] { numIndices, (uint)_nodeDrawInfos.Count, 0, 0, 0 };
			_argsBuffer = new ComputeBuffer(1, _args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
			_argsBuffer.SetData(_args);
			_positionBuffer = new ComputeBuffer(_nodeDrawInfos.Count, NodeDrawInfo.Size);
			_nodeMaterial.SetBuffer(_positionBufferID, _positionBuffer);
			_positionBuffer.SetData(_nodeDrawInfos, 0, 0, _nodeDrawInfos.Count);
		}

		public void DrawWorld(World.World world)
		{
			if(world.HasChanged)
			{
				RebuildBuffers(world);
				world.HasChanged = false;
			}
			Graphics.DrawMeshInstancedIndirect(_nodeMesh, 0, _nodeMaterial, _drawBounds, _argsBuffer);
		}
	}
}
