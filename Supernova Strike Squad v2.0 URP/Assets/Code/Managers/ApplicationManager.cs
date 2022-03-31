using Supernova.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Supernova.Managers {

	public class ApplicationManager : Singleton<ApplicationManager> {

		#region State

		public enum StateType {
			None,
			MainMenu,
			InGame
		}

		public enum JoinStateType {
			Host,
			LocalClient
		}

		#endregion
		
		
		#region Properties

		public StateType State {
			get;
			set;
		}

		public JoinStateType JoinState {
			get;
			set;
		}

		#endregion


		#region MonoBehaviour

		public void Start() {
			this.State = StateType.None;
		}

		#endregion
		

		#region Public Functions

		public void ShowMouseCursor() {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		public void HideMouseCursor() {
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		#endregion
		
	}
}