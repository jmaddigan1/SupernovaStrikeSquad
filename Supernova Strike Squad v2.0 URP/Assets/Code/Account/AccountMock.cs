using System;
using UnityEngine;
using Random = System.Random;

namespace Supernova.Account {
	public class AccountMock : IAccount {

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

		public string AvatarLink {
			get;
		}

		#endregion


		#region IAccount

		public void Initialize() {
			Debug.Log($"[Account] Mock Account");
		}

		public void SignIn(Action callback) {
			this.IsSignedIn = true;
			this.Username = $"Supernova User";
			this.ID = "Supernova User";
			callback();
		}

		#endregion
		

		
	}
}