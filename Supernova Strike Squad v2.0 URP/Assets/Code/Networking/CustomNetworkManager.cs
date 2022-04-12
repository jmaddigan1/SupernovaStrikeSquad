using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Supernova.Managers;
using Supernova.Utilities;


// The CustomNetworkManager extends the normal NetworkManager.
// 1) We manage if the lobby is Open or Closed.
// 2) We manage the visibility of the NetworkManagerHUD.
// 3) We manage player IDS.
namespace Supernova.Networking {

	public class CustomNetworkManager : NetworkManager {
		// A Stack of open ID we can hand out to connecting players.
		public static Stack<int> OpenIDs = new Stack<int>();


		// If the Lobby open? or do we need to refuse new connections.
		public static bool Open = true;

		#region Public Overrides

		public override void Start() {
			base.Start();

			switch (ApplicationManager.Instance.JoinState) {
				case ApplicationManager.JoinStateType.Host:
					//Default to a server/client when hitting start
					StartHost();
					break;
				case ApplicationManager.JoinStateType.LocalClient:
					StartClient();
					break;
			}
		}

		// When the server starts we fill out the IDStack with 
		// We turn off the NetworkManagerHUD
		public override void OnStartServer() {
			for (int i = NetworkServer.maxConnections; i >= 0; i--) OpenIDs.Push(i);

			if (TryGetComponent<NetworkManagerHUD>(out var HUD)) {
				HUD.enabled = false;
			}
		}

		// Refuse connections if the server is not open
		public override void OnServerConnect(NetworkConnection conn) {
			if (Open == false) {
				conn.Disconnect();
			}
		}

		//Server Change Scene but load via Bootstrap additive instead
		public override void ServerChangeScene(string newSceneName) {
			if (string.IsNullOrEmpty(newSceneName))
			{
				Debug.LogError("ServerChangeScene empty scene name");
				return;
			}

			// Debug.Log("ServerChangeScene " + newSceneName);
			NetworkServer.SetAllClientsNotReady();
			networkSceneName = newSceneName;

			// Let server prepare for scene change
			OnServerChangeScene(newSceneName);

			// Suspend the server's transport while changing scenes
			// It will be re-enabled in FinishLoadScene.
			Transport.activeTransport.enabled = false;

			//loadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName);
			//NOTE: Possibly need to movement logic under this to the action callback on scene loaded for the server
			SceneManager.Instance.LoadScene(newSceneName, sceneName => {
				SceneManager.Instance.UnloadScene(SceneList.HANGAR);
			});
			

			// ServerChangeScene can be called when stopping the server
			// when this happens the server is not active so does not need to tell clients about the change
			if (NetworkServer.active)
			{
				// notify all clients about the new scene
				NetworkServer.SendToAll(new SceneMessage { sceneName = newSceneName });
			}

			startPositionIndex = 0;
			startPositions.Clear();
			
		}

		#endregion


		#region Static Functions
		
		// When a client connects is Requests an ID
		public static void RequestID(Player player) {
			player.ID = OpenIDs.Pop();
		}

		// When a client disconnects it returns its ID
		public static void ReleaseID(int ID) {
			OpenIDs.Push(ID);
		}

		#endregion
		

		#region ApplicationIsQuitting

		public static bool ApplicationIsQuitting;

		[RuntimeInitializeOnLoadMethod]
		static void RunOnStart() {
			Application.quitting += () => ApplicationIsQuitting = true;
		}

		#endregion
	}
}
