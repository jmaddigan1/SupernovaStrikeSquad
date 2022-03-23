using Supernova.Nodes.ViewGraph;
using Supernova.Utilities.Attributes;

namespace Supernova.UI.Views {
	public class MainMenuView : ViewBase {

		#region XNode

		[ViewOutput]
		public EmptyNode StartGameNode {
			get;
			set;
		}

		#endregion
		
		
		public override void CleanupButtonListeners() {
			
		}

		public override void SetupButtonListeners() {
			
		}
	}
}