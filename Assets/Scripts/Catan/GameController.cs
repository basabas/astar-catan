using System.Collections.Generic;
using Bas.Catan.Drawing;
using Bas.Catan.NodeSelecting;
using Bas.Catan.PathFinding;
using Bas.Catan.UI;
using Bas.Catan.World;
using UnityEngine;
using UnityEngine.UI;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace Bas.Catan
{
	public class GameController : MonoBehaviour
	{
		[SerializeField] private WorldInformationDropdown _worldInformationDropdown;
		[SerializeField] private PathFinder _pathFinder;
		[SerializeField] private NodeSelector _nodeSelector;
		[SerializeField] private WorldDrawer _nodeDrawer;
		[SerializeField] private Text resultText;

		private WorldBuilder _builder;

		private World.World _world;

		private void Start()
		{
			_builder = new WorldBuilder();
			_nodeSelector.Init();
			_worldInformationDropdown.OnDropdownChanged += RebuildWorld;
			RebuildWorld();
		}

		private void RebuildWorld() => RebuildWorld(_worldInformationDropdown.Current);

        private void RebuildWorld(WorldInformation information)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			_world = _builder.BuildWorld(information);
			Debug.Log($"Building world took {stopwatch.Elapsed}");
			_nodeSelector.SetWorld(_world);
			_pathFinder.Clear();
		}

		public void FindPath()
        {
			if(_nodeSelector.TryGetStartAndEnd(out AStarNode start, out AStarNode end))
			{
				Stopwatch stopwatch = Stopwatch.StartNew();

				List<AStarNode> result = _pathFinder.FindPath(start, end);

                if (result.Count > 2)
                {
					stopwatch.Stop();
                    result.ForEach(node => node.HighLight());
                    resultText.text = $"{stopwatch.Elapsed}";
                    Debug.Log($"Getting path using {(_pathFinder.ImprovedAStart ? "Improved AStar" : "Base AStar")}, path cost is: {TotalCost(result)}, took {stopwatch.Elapsed}");
                }
                else
                {
                    _nodeSelector.Reset();
                    Debug.LogError("Could not find a result");
                }
				_world.HasChanged = true;
			}
            else
            {
                Debug.LogError("No start or end found");
            }
		}

        private float TotalCost(List<AStarNode> path)
        {
            float cost = 0;
            for (int i = 0; i < path.Count-1; i++)
            {
                cost  +=path[i].CostTo(path[i + 1]);
            }

            return cost;
        }

		private void Update() => _nodeDrawer.DrawWorld(_world);

        private void OnDestroy() => _nodeSelector.Dispose();
    }
}
