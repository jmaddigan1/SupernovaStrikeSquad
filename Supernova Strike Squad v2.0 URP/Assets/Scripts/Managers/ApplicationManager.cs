using Supernova.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Supernova.Managers {

	public class ApplicationManager : Singleton<ApplicationManager> {

		#region Serialized Fields

		[SerializeField]
		private string startingScene;

		#endregion


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
			SceneManager.Instance.LoadScene(startingScene, sceneName => {
				this.State = StateType.MainMenu;
			}, false);
		}

		public void NavigateToMainMenuScene() {
			SceneManager.Instance.LoadScene("Main Menu", sceneName => {
				this.State = StateType.MainMenu;
			}, true);
		}
		
		public void NavigateToHangarScene() {
			SceneManager.Instance.LoadScene("Hangar", sceneName => {
				this.State = StateType.InGame;
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