using System.Collections;

namespace Supernova.Nodes.ViewGraph {
	public abstract class LogicNode : BaseNode {
		[Input]
		public EmptyNode entry;

		public abstract override IEnumerator ProcessNode();
	}
}