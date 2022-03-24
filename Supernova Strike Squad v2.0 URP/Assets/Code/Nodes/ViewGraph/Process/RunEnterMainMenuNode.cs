using System.Collections;
using Supernova.Managers;

namespace Supernova.Nodes.ViewGraph.Process {
	public class RunEnterMainMenuNode : LogicNode {
		
		[Output]
		public EmptyNode onComplete;
		
		public override IEnumerator ProcessNode() {
			this.State = NodeState.Running;
			ApplicationManager.Instance.State = ApplicationManager.StateType.MainMenu;
			yield return RunPort("onComplete");
		}
	}
}