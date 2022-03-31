using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Supernova.UI.Views.Elements {
	public class LobbyViewPlayerElement : MonoBehaviour {

		[SerializeField]
		private Image avatarImage;

		[SerializeField]
		private TextMeshProUGUI usernameText;

		[SerializeField]
		private GameObject hostImageGameObject;

		[SerializeField]
		private Button kickButton;

		public Action KickAction {
			get;
			set;
		}

		public void Initialize(Sprite avatar, string username, bool isSelf, bool isHost = false) {
			avatarImage.sprite = avatar;
			usernameText.text = username;
			hostImageGameObject.SetActive(isHost);
			kickButton.onClick.RemoveAllListeners();
			kickButton.gameObject.SetActive(!isSelf && isHost);

			if (isHost) {
				kickButton.onClick.AddListener(DispatchKickAction);	
			}
		}

		private void DispatchKickAction() {
			this.KickAction?.Invoke();
		}

	}
}