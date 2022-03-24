using Supernova.Nodes.ViewGraph;
using Supernova.Utilities.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Supernova.UI.Views {
	public class InGameSettingsView : ViewBase {
		
		#region Serialized Fields

		[SerializeField]
		private Button startGameButton;
		
		[SerializeField]
		private Button quitGameButton;
		
		[SerializeField]
		private Button localJoinGameButton;

		#endregion
		
		
		#region XNode

		[ViewOutput]
		public EmptyNode ReturnNode {
			get;
			set;
		}
		
		[ViewOutput]
		public EmptyNode QuitGameNode {
			get;
			set;
		}
		
		[ViewOutput]
		public EmptyNode LocalGameJoinNode {
			get;
			set;
		}

		#endregion
		
		public override void CleanupButtonListeners() {
			startGameButton.onClick.RemoveAllListeners();
			quitGameButton.onClick.RemoveAllListeners();
			localJoinGameButton.onClick.RemoveAllListeners();
		}

		public override void SetupButtonListeners() {
			startGameButton.onClick.AddListener(() => OnProcessNode("ReturnNode"));
			quitGameButton.onClick.AddListener(() => OnProcessNode("QuitGameNode"));
			localJoinGameButton.onClick.AddListener(() => OnProcessNode("LocalGameJoinNode"));
		}

		public void Update() {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				OnProcessNode("ReturnNode");
			}
		}
	}
}