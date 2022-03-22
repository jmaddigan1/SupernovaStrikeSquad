using Supernova.Utilities;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Supernova.Editor {
	public class BootstrapSceneLoader {

		[MenuItem("Supernova/Bootstrap")]
		public static void OpenBootstrapScene() {
			EditorSceneManager.OpenScene(SceneList.BOOTSTRAP);
		}

	}
}