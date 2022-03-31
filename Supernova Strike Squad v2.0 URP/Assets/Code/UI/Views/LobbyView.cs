using System.Collections.Generic;
using Mirror.Examples.Chat;
using Supernova.Managers;
using Supernova.Networking;
using Supernova.Nodes.ViewGraph;
using Supernova.UI.Views.Elements;
using Supernova.Utilities.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Supernova.UI.Views {
	public class LobbyView : ViewBase {

		#region Serialized Fields

		[SerializeField]
		private Button returnButton;

		[Header("Players")]
		[SerializeField]
		private List<LobbyViewPlayerElement> players;

		#endregion
		
		
		#region XNode

		[ViewOutput]
		public EmptyNode ReturnToGameNode {
			get;
			set;
		}

		#endregion
		
		
		#region Public Overrides

		public override void CleanupButtonListeners() {
			returnButton.onClick.RemoveAllListeners();
		}

		public override void SetupButtonListeners() {
			returnButton.onClick.AddListener(() => OnProcessNode("ReturnToGameNode"));
		}

		public override void SetupUserInterface() {
			int numberOfPlayersInGame = CustomNetworkManager.singleton.NumberOfPlayerInGame;
			for (int i = 0; i < players.Count; i++) {
				bool isShowing = i < numberOfPlayersInGame;
				players[i].gameObject.SetActive(isShowing);
				if (isShowing) {
					players[i].KickAction = OnKickAction;

					//TODO: Correct this for all players in the game and correct host user
					if (i == 0) {
						players[i].Initialize(AccountManager.Instance.AvatarImage, AccountManager.Instance.Username, true, true);
					} else {
						players[i].Initialize(null, "Connected User", false);
					}
				}
			}
		}

		#endregion


		#region MonoBehaviour

		public void Update() {
			if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F2)) {
				OnProcessNode("ReturnToGameNode");
			}
		}

		#endregion


		#region Private Functions

		private void OnKickAction() {
			//TODO: Implement kick functionality
		}

		#endregion
		
		
	}
}