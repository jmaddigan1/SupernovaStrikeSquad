using System;
using System.Collections;
using Steamworks;
using Supernova.Account;
using Supernova.Utilities;
using UnityEngine;

namespace Supernova.Managers {
	public class AccountManager : Singleton<AccountManager> {

		#region Serialized Fields

		[SerializeField]
		private Sprite defaultMockAvatar;

		[SerializeField]
		private Texture2D defaultMockAvatarTexture;

		#endregion
		
		
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

		public Sprite AvatarImage {
			get;
			set;
		}

		public Texture2D AvatarTexture2DImage {
			get;
			set;
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

			this.AvatarImage = defaultMockAvatar;
			this.AvatarTexture2DImage = defaultMockAvatarTexture;
			this.Account.LoadAvatarAction = LoadAvatar;
			this.Account.Initialize();
			this.Account.SignIn();
		}

		#endregion


		#region Private Functions

		private void LoadAvatar() {
			StartCoroutine(RunLoadAvatarImage(SteamUser.GetSteamID()));
		}

		#endregion
		
		
		#region Coroutines

		//TODO: Move this to a better general area for any loading of a player avatar with a dictionary<CSteamID, Texture2D> map
		//This is assuming the only way to load an image is via the SteamAPI
		private IEnumerator RunLoadAvatarImage(CSteamID user) {
			int avatar = SteamFriends.GetMediumFriendAvatar(user);
			uint width;
			uint height;
			bool isSuccessful = SteamUtils.GetImageSize(avatar, out width, out height);
			
			if (isSuccessful && width > 0 && height > 0) {
				byte[] Image = new byte[width * height * 4];
				Texture2D returnTexture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
				isSuccessful = SteamUtils.GetImageRGBA(avatar, Image, (int)(width * height * 4));
				if (isSuccessful) {
					returnTexture.LoadRawTextureData(Image);
					returnTexture.Apply();
					
					Texture2D originalTexture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
					originalTexture.LoadRawTextureData(Image);
					originalTexture.Apply();
					int yInvert = returnTexture.height - 1;
					for (int x = 0; x < returnTexture.width; x++) {
						for (int y = 0; y < returnTexture.height; y++) {
							Color c = originalTexture.GetPixel(x, yInvert);
							returnTexture.SetPixel(x, y, c);
							yInvert--;
						}
					}
					
					returnTexture.Apply();
				}
				this.AvatarTexture2DImage =  returnTexture;
			} else {
				Debug.LogError("Couldn't get avatar.");
				this.AvatarTexture2DImage = new Texture2D(0, 0);
			}
			
			
			yield return null;
			this.AvatarImage = Sprite.Create(this.AvatarTexture2DImage, new Rect(0, 0, width, height), Vector2.zero);
		}

		#endregion
		
	}
}