using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System;
using Steamworks;

public class PlayerInfoDisplay : NetworkBehaviour
{
	[SerializeField] private RawImage profileImage = null;
	[SerializeField] private Text displayNameText = null;

	[SyncVar(hook = nameof(HandleSteamIDUpdated))]
	private ulong steamID;

	protected Callback<AvatarImageLoaded_t> avatarImageLoaded;

	#region Server

	[Server]
	public void SetSteamID(ulong steamID)
	{
		this.steamID = steamID;
	}

	#endregion

	#region Client

	public override void OnStartClient()
	{
		avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
	}

	private void HandleSteamIDUpdated(ulong oldSteamID, ulong newSteamID)
	{
		CSteamID cSteamID = new CSteamID(newSteamID);

		// Get the players username from steam and set our display name to the given name
		displayNameText.text = SteamFriends.GetFriendPersonaName(cSteamID);

		int imageID = SteamFriends.GetLargeFriendAvatar(cSteamID);

		// The image has not been previously loaded
		// It is not cashed on this computer yet
		if (imageID == -1)
		{
			return;
		}

		profileImage.texture = GetSteamIcon(imageID);
	}

	private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
	{
		if (callback.m_steamID.m_SteamID != steamID) {
			return;
		}
		else
		{
			profileImage.texture = GetSteamIcon(callback.m_iImage);
		}
	}

	private Texture2D GetSteamIcon(int iImage)
	{
		Texture2D texture = null;

		bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);

		if (isValid)
		{
			byte[] image = new byte[width * height * 4];

			isValid = SteamUtils.GetImageRGBA(iImage, image, (int)width * (int)height * 4);

			if (isValid)
			{
				texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
				texture.LoadRawTextureData(image);
				texture.Apply();
			}
		}

		return texture;
	}

	#endregion
}
