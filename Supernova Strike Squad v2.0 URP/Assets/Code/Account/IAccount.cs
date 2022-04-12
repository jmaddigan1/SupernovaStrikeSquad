using System;
using UnityEngine;

namespace Supernova.Account {
	public interface IAccount {
		
		bool IsSignedIn {
			get;
			set;
		} 
		
		string Username {
			get;
			set;
		}
		
		string ID {
			get;
			set;
		}

		Action LoadAvatarAction {
			get;
			set;
		}


		void Initialize();
		
		void SignIn();
	}
}