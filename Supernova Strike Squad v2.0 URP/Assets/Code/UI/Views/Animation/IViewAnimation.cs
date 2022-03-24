using System.Collections;

namespace Supernova.UI.Views.Animation {
	public interface IViewAnimation {
		IEnumerator IntroAnimation();
		IEnumerator OutroAnimation();
	}
}