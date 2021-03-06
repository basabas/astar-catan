using Bas.Catan.Drawing;
using Bas.Catan.NodeSelecting;
using Bas.Catan.PathFinding;
using Bas.Catan.UI;
using Bas.Catan.World;
using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace Bas.Catan
{
	public class GameController : MonoBehaviour
	{
		[SerializeField] private WorldInformationDropdown _worldInformationDropdown;
		[SerializeField] private PathFinder _pathFinder;
		[SerializeField] private NodeSelector _nodeSelector;
		[SerializeField] private WorldDrawer _nodeDrawer;

		private WorldBuilder _builder;

		private World.World _world;

		private void Start()
		{
			_builder = new WorldBuilder();
			_nodeSelector.Init();
			_worldInformationDropdown.OnDropdownChanged += RebuildWorld;
			RebuildWorld();
		}

		public void RebuildWorld()
		{
			RebuildWorld(_worldInformationDropdown.Current);
		}

		private void RebuildWorld(WorldInformation information)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			_world = _builder.BuildWorld(information);
			Debug.Log($"Building world took {stopwatch.ElapsedMilliseconds} ms.");
			_nodeSelector.SetWorld(_world);
			_pathFinder.Clear();
		}

		public void FindPath()
		{
			if(_nodeSelector.TryGetStartAndEnd(out AStarNode start, out AStarNode end))
			{
				Stopwatch stopwatch = Stopwatch.StartNew();

				if(_pathFinder.FindPath(start, end))
				{
					Debug.Log($"Getting path took {stopwatch.ElapsedMilliseconds} ms.");
					_nodeSelector.Reset(unHighlight: false);
				}
				else
				{
					Debug.LogError("Could not find a result");
					_nodeSelector.Reset(unHighlight: true);
				}
				_world.HasChanged = true;
			}
		}

		private void Update()
		{
			_nodeDrawer.DrawWorld(_world);
		}

		private void OnDestroy()
		{
			_nodeSelector.Dispose();
		}
	}
}
