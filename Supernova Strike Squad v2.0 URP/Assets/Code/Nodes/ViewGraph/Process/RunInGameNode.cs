using System.Collections;

namespace Supernova.Nodes.ViewGraph.Process {
	public class RunInGameNode : LogicNode {
		
		[Output]
		public EmptyNode exit;
		
		//TODO: Fill out with possible UI routes and graphs we can take in game
		
		public override IEnumerator ProcessNode() {
			this.State = NodeState.Running;
			
			yield return RunPort("exit");
		}
	}
}