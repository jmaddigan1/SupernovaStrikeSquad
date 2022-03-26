using Supernova.Nodes.ViewGraph;
using Supernova.Utilities.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Supernova.UI.Views {
	public class SettingsView : ViewBase {
		
		#region Serialized Fields

		[SerializeField]
		private Button returnButton;
		
		[SerializeField]
		private Button quitGameButton;
		
		[SerializeField]
		private Button localJoinGameButton;

		[SerializeField]
		private Button videoSettingsButton;
		
		[SerializeField]
		private Button soundSettingsButton;
		
		[SerializeField]
		private Button keybindingSettingsButton;

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
		
		[ViewOutput]
		public EmptyNode VideoSettingsNode {
			get;
			set;
		}
		
		[ViewOutput]
		public EmptyNode SoundSettingsNode {
			get;
			set;
		}
		
		[ViewOutput]
		public EmptyNode KeybindingSettingsNode {
			get;
			set;
		}

		#endregion


		#region Properties

		[SerializedNodeProperty]
		public bool IsInGame {
			get;
			set;
		}

		#endregion
		
		public override void CleanupButtonListeners() {
			returnButton.onClick.RemoveAllListeners();
			quitGameButton.onClick.RemoveAllListeners();
			localJoinGameButton.onClick.RemoveAllListeners();
			videoSettingsButton.onClick.RemoveAllListeners();
			soundSettingsButton.onClick.RemoveAllListeners();
			keybindingSettingsButton.onClick.RemoveAllListeners();
		}

		public override void SetupButtonListeners() {
			returnButton.onClick.AddListener(() => OnProcessNode("ReturnNode"));
			quitGameButton.onClick.AddListener(() => OnProcessNode("QuitGameNode"));
			localJoinGameButton.onClick.AddListener(() => OnProcessNode("LocalGameJoinNode"));
			videoSettingsButton.onClick.AddListener(() => OnProcessNode("VideoSettingsNode"));
			soundSettingsButton.onClick.AddListener(() => OnProcessNode("SoundSettingsNode"));
			keybindingSettingsButton.onClick.AddListener(() => OnProcessNode("KeybindingSettingsNode"));
		}

		public void Update() {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				OnProcessNode("ReturnNode");
			}
		}
	}
}