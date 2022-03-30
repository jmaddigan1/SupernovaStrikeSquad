using System.Collections;
using Supernova.Managers;

namespace Supernova.Nodes.ViewGraph.Process {
	public class RunAccountSignInNode : LogicNode {
		
		[Output]
		public EmptyNode onComplete;
		
		public override IEnumerator ProcessNode() {
			AccountManager.Instance.Initialize();

			while (!AccountManager.Instance.IsSignedIn) {
				yield return null;
			}
			
			yield return RunPort("onComplete");
		}
	}
}