using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace Bas.Catan.Input
{
	public class InputHandler
	{
		private readonly Action<Vector2> _onScreenClickAction;

		public InputHandler(Action<Vector2> onClickAction)
		{
			_onScreenClickAction = onClickAction;
			Object.FindFirstObjectByType<PlayerInput>().onActionTriggered += OnInputActionCallBack;
		}

		private void OnInputActionCallBack(InputAction.CallbackContext context)
		{
			if(context.phase == InputActionPhase.Started)
			{
				_onScreenClickAction?.Invoke(Pointer.current.position.ReadValue());
			}
		}

		public void Dispose()
		{
			PlayerInput input = Object.FindFirstObjectByType<PlayerInput>();
			if(input != null)
			{
				input.onActionTriggered -= OnInputActionCallBack;
			}
		}
	}
}
