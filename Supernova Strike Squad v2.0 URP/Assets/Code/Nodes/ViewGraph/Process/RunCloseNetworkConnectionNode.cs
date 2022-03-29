using System.Collections;
using Mirror;
using Supernova.Managers;

namespace Supernova.Nodes.ViewGraph.Process {
	public class RunCloseNetworkConnectionNode : LogicNode {
		
		[OutputAttribute]
		public EmptyNode exitNode;
		
		public override IEnumerator ProcessNode() {
			this.State = NodeState.Running;
			
			ApplicationManager.Instance.ShowMouseCursor();
			
			//TODO: Find out if we may just be a client to shut that down instead
			NetworkClient.Disconnect();
			NetworkManager.singleton.StopHost();

			yield return RunPort("exitNode");
		}
	}
}