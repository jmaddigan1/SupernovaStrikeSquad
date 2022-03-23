using System.Collections;
using Supernova.Managers;
using Supernova.Utilities;

namespace Supernova.Nodes.ViewGraph.Process {
	public class RunInGameNode : LogicNode {
		
		[Output]
		public EmptyNode exit;
		
		//TODO: Fill out with possible UI routes and graphs we can take in game
		
		public override IEnumerator ProcessNode() {
			this.State = NodeState.Running;
			bool isLoading = true;
			
			ViewGraphManager.Instance.PopTopView(); //Remove views off stack since we will be seeing preloader
			SceneManager.Instance.LoadScene(SceneList.HANGAR, sceneName => {
				SceneManager.Instance.UnloadScene(SceneList.MAIN_MENU);
				isLoading = false;
			}, true);

			while (isLoading) {
				yield return null;
			}

			yield return RunPort("exit");
		}
	}
}