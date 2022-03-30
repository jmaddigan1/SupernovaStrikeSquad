using System;

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
		
		string AvatarLink {
			get;
		}
		
		
		void Initialize();
		
		void SignIn(Action callback);
	}
}