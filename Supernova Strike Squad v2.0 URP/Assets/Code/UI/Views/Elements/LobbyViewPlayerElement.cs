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
		private Image hostImage;

		[SerializeField]
		private Button kickButton;

		[SerializeField]
		private GameObject kickSpacerGameObject;

		public Action KickAction {
			get;
			set;
		}

		public void Initialize(Sprite avatar, string username, bool isSelf, bool isHost = false, bool hasKickingAbility = false) {
			avatarImage.sprite = avatar;
			usernameText.text = username;
			hostImage.enabled = isHost;
			kickButton.onClick.RemoveAllListeners();
			kickButton.gameObject.SetActive(!isSelf && hasKickingAbility);
			kickSpacerGameObject.SetActive(isSelf || !hasKickingAbility);

			if (hasKickingAbility) {
				kickButton.onClick.AddListener(DispatchKickAction);	
			}
		}

		private void DispatchKickAction() {
			this.KickAction?.Invoke();
		}

	}
}