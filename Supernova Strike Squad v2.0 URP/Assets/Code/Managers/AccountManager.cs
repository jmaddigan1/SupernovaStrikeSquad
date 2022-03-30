using System;
using Supernova.Account;
using Supernova.Utilities;
using UnityEngine;

namespace Supernova.Managers {
	public class AccountManager : Singleton<AccountManager> {

		#region Properties

		private IAccount Account {
			get;
			set;
		}


		public bool IsSignedIn {
			get {
				return this.Account?.IsSignedIn ?? false;
			}
		}
		
		public string Username {
			get {
				return this.Account?.Username ?? "Supernova User";
			}
		}
		
		public string ID {
			get {
				return this.Account?.ID ?? string.Empty;
			}
		}
		
		public string AvatarLink {
			get {
				return this.Account?.AvatarLink ?? String.Empty;
			}
		}

		#endregion
		
		
		#region Public Functions

		public void Initialize() {
			if (SteamManager.Initialized) {
				this.Account = new AccountSteam();
			} else {
				#if UNITY_EDITOR
					this.Account = new AccountMock();
				#else
					Debug.LogError($"[Account Manager] Failed to initialize account.");
					Application.Quit();
				#endif
			}
			
			this.Account.Initialize();
			SignIn();
		}

		#endregion


		#region Private Functions

		private void SignIn() {
			this.Account?.SignIn(() => {
				if (this.IsSignedIn) {
					Debug.Log($"[Account] User Information ({this.Username}:{this.ID})");
				}
			});
		}

		#endregion
		
	}
}