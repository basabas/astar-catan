using System;
using System.Collections.Generic;
using System.Linq;
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
		[SerializeField] private Button debugButton;

        private WorldBuilder _builder;

		private World.World _world;

		private void Start()
		{
			_builder = new WorldBuilder();
			_nodeSelector.Init();
			_worldInformationDropdown.OnDropdownChanged += RebuildWorld;
			if(Application.isEditor)
			{
				debugButton.onClick.AddListener(Test);
			}
			else
			{
				Destroy(debugButton.gameObject);
			}
			RebuildWorld();
		}

		private void RebuildWorld() => RebuildWorld(_worldInformationDropdown.Current);

        private void RebuildWorld(WorldInformation information)
		{
			_world = _builder.BuildWorld(information);
			_nodeSelector.SetWorld(_world);
			_pathFinder.Clear();
		}

		public void FindPath()
        {
			if(_nodeSelector.TryGetStartAndEnd(out AStarNode start, out AStarNode end))
			{
				if(CalculateShortestPath(start, end, out var time))
				{
					resultText.text = $"{time}";
                    Debug.Log($"Getting path using {(_pathFinder.ImprovedAStart ? "Improved AStar" : "Base AStar")}, took {time}");
                }
				else
				{
					Debug.LogError("Could not find a result");
                    _nodeSelector.Reset();
                }
            }
            else
            {
                Debug.LogError("No start or end found");
            }
		}

		private bool CalculateShortestPath(AStarNode start, AStarNode end, out TimeSpan calculationTime)
		{
			_world.HasChanged = true;
            Stopwatch stopwatch = Stopwatch.StartNew();

			List<AStarNode> result = _pathFinder.FindPath(start, end);

            if (result.Count > 2)
			{
				stopwatch.Stop();
				result.ForEach(node => node.HighLight());
                calculationTime = stopwatch.Elapsed;
                return true;
				
			}
			calculationTime = stopwatch.Elapsed;
            return false;
		}


        private void Test()
        {
	        const int testAmount = 10;

			List<TimeSpan> times = new List<TimeSpan>();
			while (times.Count < testAmount)
			{
				if(DoDebugTest(out var time))
				{
                    times.Add(time);
				}
			}

            double doubleAverageTicks = times.Average(timeSpan => timeSpan.Ticks);
            long longAverageTicks = Convert.ToInt64(doubleAverageTicks);

            TimeSpan average = new TimeSpan(longAverageTicks);

            resultText.text = $"{average}";
        }

        private bool DoDebugTest(out TimeSpan time)
        {
			RebuildWorld();
			AStarNode start = null;
			AStarNode end = null;

			int x = 0, y = 0;
			for (int i = 0; i < _world.Count; i++)
			{
				start = _world.Nodes[x, y];

                if (start.NodeInfo.TravelCost > 0)
                {
	                break;
                }

                if(x == y)
                {
	                x++;
	                y = 0;
                }
                else
                {
                    y++;
                }
            }

            x = _world.Nodes.GetLength(0) - 1;
            y = _world.Nodes.GetLength(1) - 1;
            for (int i = 0; i < _world.Count; i++)
            {
                end = _world.Nodes[x, y];
                if (end.NodeInfo.TravelCost > 0)
                {
                    break;
                }
                if (x == y)
                {
                    x--;
                    y = _world.Nodes.GetLength(1) - 1;
                }
                else
                {
                    y--;
                }
            }

            if(start == null || end == null)
            {
				time = TimeSpan.MaxValue;
	            return false;
            }
			
            return CalculateShortestPath(start, end, out time);
        }

		private void Update() => _nodeDrawer.DrawWorld(_world);

        private void OnDestroy() => _nodeSelector.Dispose();
    }
}
