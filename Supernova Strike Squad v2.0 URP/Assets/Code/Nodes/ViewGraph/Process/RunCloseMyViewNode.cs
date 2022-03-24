using System.Collections;
using Supernova.Managers;

namespace Supernova.Nodes.ViewGraph.Process {
	public class RunCloseMyViewNode : LogicNode {
		
		[Output]
        public EmptyNode onComplete;
        
        public override IEnumerator ProcessNode() {
            this.State = NodeState.Running;
            
            ViewGraphManager.Instance.PopTopView();
            
            yield return RunPort("onComplete");
        }
		
	}
}