using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Bas.Catan.Editor
{
	public class TextureArrayWindow : EditorWindow
	{
		private static readonly Rect _windowRect = new Rect(50, 50, 300, 260);

		private List<Texture2D> _textures = new List<Texture2D>();
		private int? _width;
		private int? _height;
		private bool _applyMipmapping;
		private TextureFormat _textureFormat = TextureFormat.ARGB32;
		private string _arrayName = "TextureArray";

		[MenuItem("Tools/Create TextureArray")]
		public static void ShowWindow()
		{
			GetWindowWithRect<TextureArrayWindow>(_windowRect, true, "Texture Array Creator", true);
		}

		private void OnGUI()
		{
			for(int i = 0; i < _textures.Count; i++)
			{
				EditorGUI.BeginChangeCheck();
				_textures[i] = (Texture2D)EditorGUILayout.ObjectField(_textures[i], typeof(Texture2D), false);
				if(EditorGUI.EndChangeCheck() && !_width.HasValue && _textures[i] != null)
				{
					_width = _textures[i].width;
					_height = _textures[i].height;
				}
			}

			if(GUILayout.Button("Add"))
			{
				_textures.Add(null);
			}

			if(_textures.Any(texture => texture != null))
			{
				_width = EditorGUILayout.IntSlider("Width", _width.Value, 0, 4096);
				_height = EditorGUILayout.IntSlider("Heigth", _height.Value, 0, 4096);
				_applyMipmapping = EditorGUILayout.Toggle("Mipmapping", _applyMipmapping);
				_textureFormat = (TextureFormat)EditorGUILayout.EnumPopup("TextureFormat", _textureFormat);
				_arrayName = EditorGUILayout.TextField("Array Name", _arrayName);

				if(GUILayout.Button("Create TextureArray"))
				{
					List<Texture2D> textures = _textures.Where(texture => texture != null).ToList();

					Texture2DArray array = new Texture2DArray(_width.Value, _height.Value, textures.Count, _textureFormat, _applyMipmapping);
					for(int i = 0; i < textures.Count; i++)
					{
						Texture2D texture = textures[i];
						array.SetPixels32(texture.GetPixels32(), i);
					}
					array.Apply();

					AssetDatabase.CreateAsset(array, Path.Combine("Assets/TextureArrays", $"{_arrayName}.asset"));
					AssetDatabase.Refresh();
				}
			}
		}
	}
}
