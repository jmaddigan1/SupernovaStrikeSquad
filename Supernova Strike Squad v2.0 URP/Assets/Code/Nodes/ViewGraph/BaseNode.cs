using System.Collections;
using Supernova.Managers;
using UnityEngine;

namespace Supernova.Nodes.ViewGraph {
	public abstract class BaseNode : XNode.Node {

		public NodeState State {
			get;
			set;
		} = NodeState.Unknown;

		public abstract IEnumerator ProcessNode();

		protected IEnumerator RunPort(string name) {
			this.State = NodeState.Ran;
			var port = GetPort(name);
			if (port.IsConnected) {
				var n = port.Connection.node;
				ViewGraphManager.Instance.CurrentBaseNode = (BaseNode) n;
				return ((BaseNode)n).ProcessNode();
			}
			
			Debug.LogError($"Port ({name}) is not connected in the view graph");
			return null;
		}

	}
}