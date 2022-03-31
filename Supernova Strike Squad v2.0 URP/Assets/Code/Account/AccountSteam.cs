using System;
using System.Collections;
using Steamworks;
using UnityEngine;

namespace Supernova.Account {
	public class AccountSteam : IAccount {

		#region Properties

		public bool IsSignedIn {
			get;
			set;
		}

		public string Username {
			get;
			set;
		}

		public string ID {
			get;
			set;
		}

		public Action LoadAvatarAction {
			get;
			set;
		}

		protected Callback<GameOverlayActivated_t> GameOverlayActivated {
			get;
			set;
		}

		#endregion


		#region IAccount

		public void Initialize() {
			Debug.Log($"[Account] Steam");
			if (SteamManager.Initialized) {
				Debug.Log($"[Account] Initialized SteamManager and SteamWorks");
				this.GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
			}
		}

		public void SignIn() {
			if (SteamManager.Initialized) {
				this.IsSignedIn = true;
				this.Username = SteamFriends.GetPersonaName();
				this.ID = SteamUser.GetSteamID().ToString();
				this.LoadAvatarAction();
			}
		}

		#endregion
		
		
		#region Private Functions

		private void OnGameOverlayActivated(GameOverlayActivated_t callback) {
			if (callback.m_bActive != 0) {
				Debug.Log($"[Steam] Steam overlay have been activated");
			} else {
				Debug.Log($"[Steam] Steam overlay have been closed");
			}
		}

		#endregion

	}
}