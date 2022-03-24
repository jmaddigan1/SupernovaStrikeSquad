using Supernova.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Supernova.Managers {

	public class ApplicationManager : Singleton<ApplicationManager> {

		#region Properties

		public StateType State {
			get;
			set;
		}

		#endregion


		#region MonoBehaviour

		public void Start() {
			this.State = StateType.None;
			
			SceneManager.Instance.LoadScene(SceneList.MAIN_MENU, sceneName => {
				this.State = StateType.MainMenu;
			}, false);
		}

		#endregion
		

		#region State

		public enum StateType {
			None,
			MainMenu,
			InGame
		}

		#endregion
		
	}
}