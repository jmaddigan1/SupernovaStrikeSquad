using Supernova.Managers;
using UnityEngine;

namespace Supernova.Camera {

	[RequireComponent(typeof(UnityEngine.Camera))]
	public class CameraTakeOverComponent : MonoBehaviour {

		private UnityEngine.Camera camera;
		
		private UnityEngine.Camera Camera {
			get {
				return camera = camera ?? GetComponent<UnityEngine.Camera>();
			}
		}

		public void OnEnable() {
			CameraManager.Instance.TakeOverCamera(this.Camera);
		}
	}
}