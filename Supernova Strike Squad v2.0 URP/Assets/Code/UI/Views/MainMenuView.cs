using Supernova.Managers;
using Supernova.Nodes.ViewGraph;
using Supernova.Utilities.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Supernova.UI.Views {
	public class MainMenuView : ViewBase {

		#region Serialized Fields

		[SerializeField]
		private Button startGameButton;

		[SerializeField]
		private Button settingsButton;

		[SerializeField]
		private Image userAvatar;

		#endregion
		
		
		#region XNode

		[ViewOutput]
		public EmptyNode StartGameNode {
			get;
			set;
		}
		
		[ViewOutput]
		public EmptyNode SettingsNode {
			get;
			set;
		}

		#endregion
		
		
		public override void CleanupButtonListeners() {
			startGameButton.onClick.RemoveAllListeners();
			settingsButton.onClick.RemoveAllListeners();
		}

		public override void SetupButtonListeners() {
			startGameButton.onClick.AddListener(() => OnProcessNode("StartGameNode"));
			settingsButton.onClick.AddListener(() => OnProcessNode("SettingsNode"));
		}

		public override void SetupUserInterface() {
			userAvatar.sprite = AccountManager.Instance.AvatarImage;

			ApplicationManager.Instance.JoinState = ApplicationManager.JoinStateType.Host;
		}

		public void Update() {
			if (Input.GetKeyDown(KeyCode.P)) {
				ApplicationManager.Instance.JoinState = ApplicationManager.JoinStateType.LocalClient;
			}
		}
	}
}