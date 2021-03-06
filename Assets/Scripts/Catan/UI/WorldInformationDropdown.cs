﻿using Bas.Catan.World;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Bas.Catan.UI
{
	public class WorldInformationDropdown : Dropdown
	{
		public delegate void DropdownChanged(WorldInformation worldInformation);
		public event DropdownChanged OnDropdownChanged;

		public WorldInformation Current => _worldInformationObjects[value];

		[SerializeField] private WorldInformation[] _worldInformationObjects;

		protected override void Awake()
		{
			base.Awake();
			onValueChanged.AddListener(newIndex => OnDropdownChanged(Current));
		}

#if UNITY_EDITOR
		protected override void OnValidate()
		{
			base.OnValidate();
			_worldInformationObjects = Resources.LoadAll<WorldInformation>("Worlds");
			options = _worldInformationObjects.Where(setting => setting != null).Select(setting => new OptionData(setting.name)).ToList();
		}
#endif
	}
}
