using Supernova.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Supernova.Managers {

	public class ApplicationManager : Singleton<ApplicationManager> {

		#region Properties

		public StateType State {
			get;
			private set;
		}

		#endregion


		#region MonoBehaviour

		public void Start() {
			this.State = StateType.None;
			NavigateToStartScene();
		}

		public void NavigateToStartScene() {
			SceneManager.Instance.LoadScene(SceneList.MAIN_MENU, sceneName => {
				this.State = StateType.MainMenu;
			}, false);
		}

		public void NavigateToMainMenuScene() {
			SceneManager.Instance.LoadScene(SceneList.MAIN_MENU, sceneName => {
				this.State = StateType.MainMenu;
			}, true);
		}
		
		public void NavigateFromMainMenuToHangarScene() {
			SceneManager.Instance.LoadScene(SceneList.HANGAR, sceneName => {
				this.State = StateType.InGame;
				SceneManager.Instance.UnloadScene(SceneList.MAIN_MENU);
				SceneManager.Instance.HidePreloader();
			}, true);
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