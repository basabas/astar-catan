using System.Collections.Generic;
using Bas.Catan.PathFinding;
using Bas.Catan.Utilities;
using UnityEngine;

namespace Bas.Catan.Drawing
{
	[System.Serializable]
	public class WorldDrawer
	{
		private const float highLightValue = 3f;//Has effect on the color(red part) and scale of a node

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
			if(_nodeDrawInfos.Count > world.Count)
			{
				_nodeDrawInfos.RemoveRange(world.Count, _nodeDrawInfos.Count -world.Count);	
			}
            int i = 0;
			for (int x = 0; x < world.Nodes.GetLength(0); x++)
			{
				for (int y = 0; y < world.Nodes.GetLength(1); y++)
				{
                    AStarNode node = world.Nodes[x, y];
                    if (i == _nodeDrawInfos.Count)
					{
						_nodeDrawInfos.Add(NodeDrawInfo.Create(node, world));
					}
					else
					{
						NodeDrawInfo info = _nodeDrawInfos[i];

						Vector2 position = node.ArrayIndex.ToNodePosition(world.WorldInformation);
						info.X = position.x;
						info.Y = position.y;
						info.HighlightValue = node.HighLighted ? highLightValue : 1;
						info.NodeType = (int) node.NodeInfo.NodeType;
						_nodeDrawInfos[i] = info;
                    }

					i++;
				}
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
