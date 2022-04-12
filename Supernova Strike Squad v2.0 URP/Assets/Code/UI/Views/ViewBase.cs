using System.Collections;
using Supernova.Managers;
using Supernova.UI.Views.Animation;
using UnityEngine;

namespace Supernova.UI.Views {
	public abstract class ViewBase : MonoBehaviour {

		public abstract void CleanupButtonListeners();

		public abstract void SetupButtonListeners();

		public abstract void SetupUserInterface();

		public virtual void Repaint() {
			CleanupButtonListeners();
			SetupButtonListeners();
			SetupUserInterface();
			IntroAnimation();
		}

		protected virtual void RepaintComplete() { }

		protected void IntroAnimation() {
			StartCoroutine(RunIntroAnimation());
		}

		protected void OnProcessNode(string id) {
			StartCoroutine(RunOutroAnimation(id));
		}

		private IEnumerator RunIntroAnimation() {
			var animation = GetComponent<IViewAnimation>();
			if (animation != null) {
				yield return animation.IntroAnimation();
			}
			RepaintComplete();
		}
		
		private IEnumerator RunOutroAnimation(string id) {
			var animation = GetComponent<IViewAnimation>();
			if (animation != null) {
				yield return animation.OutroAnimation();
			}
			ViewGraphManager.Instance.ProcessViewNodeSelection(id);
		}

	}
}