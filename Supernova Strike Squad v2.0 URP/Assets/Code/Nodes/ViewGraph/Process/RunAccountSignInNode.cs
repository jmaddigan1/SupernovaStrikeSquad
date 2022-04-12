using System.Collections;
using Supernova.Managers;
using UnityEngine;

namespace Supernova.Nodes.ViewGraph.Process {
	public class RunAccountSignInNode : LogicNode {
		
		[Output]
		public EmptyNode onComplete;
		
		public override IEnumerator ProcessNode() {
			AccountManager.Instance.Initialize();

			while (!AccountManager.Instance.IsSignedIn) {
				yield return null;
			}
			
			Debug.Log("[View Graph] Signed in now loading avatar image");
			
			while (AccountManager.Instance.AvatarImage == null) {
				yield return null;
			}
			
			Debug.Log("[View Graph] Avatar image loaded and ready");
			
			yield return RunPort("onComplete");
		}
	}
}