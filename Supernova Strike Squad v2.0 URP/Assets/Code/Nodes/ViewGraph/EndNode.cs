using System.Collections;

namespace Supernova.Nodes.ViewGraph {
	public class EndNode : LogicNode {
		public override IEnumerator ProcessNode() {
			this.State = NodeState.Ran;
			yield return null;
		}
	}
}