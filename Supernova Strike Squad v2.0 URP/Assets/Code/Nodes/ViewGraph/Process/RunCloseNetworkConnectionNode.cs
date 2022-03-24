using System.Collections;
using Mirror;

namespace Supernova.Nodes.ViewGraph.Process {
	public class RunCloseNetworkConnectionNode : LogicNode {
		
		[OutputAttribute]
		public EmptyNode exitNode;
		
		public override IEnumerator ProcessNode() {
			this.State = NodeState.Running;
			
			//TODO: Find out if we may just be a client to shut that down instead
			NetworkManager.singleton.StopHost();
			
			yield return RunPort("exitNode");
		}
	}
}