using System;
using System.Collections;
using System.Linq;
using Supernova.Managers;
using Supernova.UI.Views;
using UnityEngine;

namespace Supernova.Nodes.ViewGraph {
	public class ViewNode : BaseNode {
		
		[Input]
		public EmptyNode enter;

		[SerializeField]
		public ViewBase view;
		
		[SerializeField]
		private ViewType viewType;
		
		private string buttonID = String.Empty;
		
		public ViewType ViewType {
			get {
				return viewType;
			}
		}
		
		public override IEnumerator ProcessNode() {
			buttonID = String.Empty;
			this.State = NodeState.Running;
			ViewGraphManager.Instance.CurrentView = this;
			ViewBase viewBase = ViewGraphManager.Instance.Create<ViewBase>(view.gameObject, viewType);
			viewBase.Repaint();
			
			//Wait until a button is pressed
			while (String.IsNullOrEmpty(buttonID)) {
				yield return null;
			}
			
			yield return RunPort(buttonID);
		}

		public void ProcessSelection(string id) {
			buttonID = id;
		}
	}
}