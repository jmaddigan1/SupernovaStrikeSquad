using Supernova.Nodes.ViewGraph;
using UnityEngine;
using XNodeEditor;

#if UNITY_EDITOR
namespace Supernova.Assets.Code.Nodes.Editor {
	[CustomNodeEditor(typeof(BaseNode))]
	public class BaseNodeEditor : NodeEditor {
		public override Color GetTint() {
			//Unknown: base.GetTint();
			//Used: new Color(79f/255f,132f/255f,156f/255f);
			//Running: new Color(175f/255f,79f/255f,83f/255f);

			if (!Application.isPlaying) {
				((BaseNode) target).State = NodeState.Unknown;
			}

			switch(((BaseNode)target).State) {
				case NodeState.Ran:
					return new Color(79f/255f,132f/255f,156f/255f);
				case NodeState.Running:
					return new Color(175f/255f,79f/255f,83f/255f);
				default:
					return base.GetTint();
			}
		}
	}
}
#endif