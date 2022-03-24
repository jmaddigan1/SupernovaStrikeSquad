﻿using System.Collections;
using Supernova.Managers;
using Supernova.Utilities;

namespace Supernova.Nodes.ViewGraph.Process {
	public class RunLoadMainMenuNode : LogicNode {
		
		[OutputAttribute]
		public EmptyNode exitNode;
		
		public override IEnumerator ProcessNode() {
			this.State = NodeState.Running;
			
			//Clear top view main menu or in game
			ViewGraphManager.Instance.PopTopView();
			
			ApplicationManager.Instance.State = ApplicationManager.StateType.InGame;
			bool isLoading = true;
			
			//Remove views off stack since we will be seeing preloader
			SceneManager.Instance.LoadScene(SceneList.MAIN_MENU, sceneName => {
				SceneManager.Instance.UnloadScene(SceneList.HANGAR);
				//TODO: Might need to unload other scenes later on
				isLoading = false;
			}, true);

			while (isLoading) {
				yield return null;
			}
			
			yield return RunPort("exitNode");
		}
	}
}