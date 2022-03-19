using Supernova.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Supernova.Managers {

	public class ApplicationManager : Singleton<ApplicationManager> {

		#region Serialized Fields

		[SerializeField]
		private string startingScene;

		#endregion


		#region MonoBehaviour

		public void Start() {
			SceneManager.Instance.LoadScene(startingScene, sceneName => {}, false);
		}

		#endregion
		

		#region State

		public enum State {
			None,
			MainMenu
		}

		#endregion
		
	}
}