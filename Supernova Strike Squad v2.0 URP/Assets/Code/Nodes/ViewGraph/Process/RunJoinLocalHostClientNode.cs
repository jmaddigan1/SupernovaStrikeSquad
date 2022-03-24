using System.Collections;
using Mirror;

namespace Supernova.Nodes.ViewGraph.Process {
	public class RunJoinLocalHostClientNode : LogicNode {
		
		[Output]
		public EmptyNode onComplete;
		
		public override IEnumerator ProcessNode() {
			this.State = NodeState.Running;
            
			//TODO: Join local host
			NetworkClient.Disconnect();
			NetworkManager.singleton.StopHost();
			NetworkManager.singleton.StartClient();
            
			yield return RunPort("onComplete");
		}
	}
}