using System.Collections;
using Supernova.Managers;
using Supernova.Utilities;
using UnityEngine;

namespace Supernova.Nodes.ViewGraph.Process {
	public class RunInGameNode : LogicNode {
		
		[Output]
		public EmptyNode settingsNode;


		//TODO: Fill out with possible UI routes and graphs we can take in game
		
		public override IEnumerator ProcessNode() {
			this.State = NodeState.Running;
			
			Debug.Log($"[Node] Run in Game Node");
			ViewGraphManager.Instance.PopWholeViewStack();

			//Keep node running while in game to navigate to different menus
			while (ApplicationManager.Instance.State == ApplicationManager.StateType.InGame) {

				if (Input.GetKeyDown(KeyCode.Escape)) {
					ApplicationManager.Instance.ShowMouseCursor();
					yield return RunPort("settingsNode");
					ApplicationManager.Instance.HideMouseCursor();
				}
				
				yield return null;
			}
		}
	}
}