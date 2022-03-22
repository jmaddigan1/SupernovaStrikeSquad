using Supernova.Managers;
using UnityEngine;

namespace Supernova.Camera {

	[RequireComponent(typeof(UnityEngine.Camera))]
	public class CameraTakeOverComponent : MonoBehaviour {

		private UnityEngine.Camera internalCamera;
		
		private UnityEngine.Camera Camera {
			get {
				return internalCamera = internalCamera ?? GetComponent<UnityEngine.Camera>();
			}
		}

		public void OnEnable() {
			CameraManager.Instance.TakeOverCamera(this.Camera);
		}
	}
}