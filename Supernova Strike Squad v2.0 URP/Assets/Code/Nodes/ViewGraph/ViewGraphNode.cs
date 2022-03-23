using System.Collections;
using UnityEngine;

namespace Supernova.Nodes.ViewGraph {
	public class ViewGraphNode : BaseNode {
		[Input]
		public EmptyNode enter;
		
		[Output]
		public EmptyNode exit;
		
		[SerializeField]
		private Graphs.ViewGraph viewGraph;

		public override IEnumerator ProcessNode() {
			this.State = viewGraph != null ? NodeState.Running : NodeState.Ran;
			if (viewGraph != null) {
				yield return ((BaseNode) viewGraph.nodes[0]).ProcessNode();
				yield return RunPort("exit");
			} else {
				yield return null;
			}
		}
	}
}