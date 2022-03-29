using UnityEngine;

namespace Supernova.Controllers {
	
	public class PreloaderController : MonoBehaviour {

		[SerializeField]
		private GameObject preloaderContainer;

		public void Awake() {
			HidePreloader();
		}

		public void ShowPreloader() {
			preloaderContainer.SetActive(true);
		}

		public void HidePreloader() {
			preloaderContainer.SetActive(false);
		}


	}
}