using System.Collections;

namespace Supernova.Nodes.ViewGraph {
	public class StartNode : BaseNode {
		[Output]
		public EmptyNode exit;

		public override IEnumerator ProcessNode() {
			yield return RunPort("exit");
		}
	}
}