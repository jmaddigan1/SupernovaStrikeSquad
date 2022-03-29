using Supernova.Utilities;
using UnityEngine;

namespace Supernova.Managers {

	public class CameraManager : Singleton<CameraManager> {

		[SerializeField]
		private UnityEngine.Camera defaultCamera;

		public UnityEngine.Camera Current {
			get;
			private set;
		}

		public override void Awake() {
			base.Awake();

			FreeCamera();
		}

		public void TakeOverCamera(UnityEngine.Camera camera) {
			if (this.Current) {
				this.Current.gameObject.SetActive(false);
			}
			
			this.Current = camera;
		}

		public void FreeCamera() {
			TakeOverCamera(defaultCamera);
		}

	}
}