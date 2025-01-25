using Bas.Catan.Input;
using Bas.Catan.PathFinding;
using Bas.Catan.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Bas.Catan.NodeSelecting
{
	[System.Serializable]
	public class NodeSelector
	{
		[SerializeField] private Camera _camera;
		[SerializeField] private Button _startSelectButton;
		[SerializeField] private Button _endSelectButton;

		private InputHandler _inputHandler;

		private bool _findStart;
		private AStarNode _start;
		private AStarNode _end;

		private World.World _world;

		public void Init()
		{
			_inputHandler = new InputHandler(OnClickEvent);
			_startSelectButton?.onClick.AddListener(() => ChangeTarget(true));
			_endSelectButton?.onClick.AddListener(() => ChangeTarget(false));
		}

		public void SetWorld(World.World world)
		{
			_world = world;
			Reset();
			_camera.transform.position = world.WorldInformation.CameraPosition;
			_camera.transform.eulerAngles = world.WorldInformation.CameraRotation;
		}

		private void OnClickEvent(Vector2 screenPosition)
		{
			if(TryGetAstarNode(_camera.ScreenPointToRay(screenPosition), out AStarNode node))
			{
				if(_findStart)
				{
					_start?.HighLight(false);
					_start = node;
					_start.HighLight();
				}
				else
				{
					_end?.HighLight(false);
					_end = node;
					_end.HighLight();
				}
				ChangeTarget(!_findStart);
				_world.HasChanged = true;
			}
		}

		private void ChangeTarget(bool findStart)
		{
			_findStart = findStart;
			_startSelectButton.interactable = !findStart;
			_endSelectButton.interactable = findStart;
		}

		private bool TryGetAstarNode(Ray ray, out AStarNode node)
		{
			node = null;
			if(VectorUtilities.TryGetPlaneIntersection(ray, new Vector3(0, 0.2f, 0), Vector3.up, out Vector3 point))
			{
				Vector2Int nodeIndex = point.ToNodeIndex(_world.WorldInformation);
				if(nodeIndex.x >= 0 && nodeIndex.x < _world.Nodes.GetLength(0) && nodeIndex.y >= 0 && nodeIndex.y < _world.Nodes.GetLength(1))
				{
					node = _world.Nodes[nodeIndex.x, nodeIndex.y];
				}
			}
			return node != null && node.NodeInfo.TravelCost > 0;
		}

        public void ResetHighLight()
        {
            _start?.HighLight(false);
            _end?.HighLight(false);
        }

		public void Reset()
		{
			_start = _end = null;
            ResetHighLight();
            ChangeTarget(true);
		}

		public bool TryGetStartAndEnd(out AStarNode start, out AStarNode end)
		{
			start = _start;
			end = _end;
			return _start != null && _end != null;
		}

		public void Dispose() => _inputHandler.Dispose();
    }
}
