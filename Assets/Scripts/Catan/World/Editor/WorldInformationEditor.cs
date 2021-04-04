using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Bas.Catan.World.Editor
{
	[CustomEditor(typeof(WorldInformation))]
	public class WorldInformationEditor : UnityEditor.Editor
	{
		private SerializedProperty _worldSize;
		private SerializedProperty _distanceBetweenNodes;

		private SerializedProperty _cameraPosition;
		private SerializedProperty _cameraRotation;

		private ReorderableList _nodesList;

		private void OnEnable()
		{
			_worldSize = serializedObject.FindProperty("_worldSize");
			_distanceBetweenNodes = serializedObject.FindProperty("_distanceBetweenNodes");

			_cameraPosition = serializedObject.FindProperty("_cameraPosition");
			_cameraRotation = serializedObject.FindProperty("_cameraRotation");

			_nodesList = new ReorderableList(serializedObject, serializedObject.FindProperty("_nodes"), true, true, true, true)
			{
				drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Nodes", EditorStyles.boldLabel),
				drawElementCallback = DrawNodesElement
			};
		}

		private void DrawNodesElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			const float typeWidth = 75;
			SerializedProperty element = _nodesList.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 1;
			EditorGUI.PropertyField(new Rect(rect.x, rect.y, typeWidth, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("NodeType"), GUIContent.none);
			EditorGUI.PropertyField(new Rect(rect.x + typeWidth, rect.y, rect.width - typeWidth, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("TravelCost"));
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			GUILayout.Label("World Scale Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(_worldSize);
			EditorGUILayout.PropertyField(_distanceBetweenNodes);

			EditorGUILayout.Space();
			GUILayout.Label("Camera Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(_cameraPosition);
			EditorGUILayout.PropertyField(_cameraRotation);

			EditorGUILayout.Space();
			_nodesList.DoLayoutList();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
