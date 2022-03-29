#if UNITY_EDITOR
using System;
using System.Reflection;
using Supernova.Nodes.ViewGraph;
using Supernova.UI.Views;
using Supernova.Utilities.Attributes;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;



namespace Supernova.Assets.Code.Nodes.Editor {
	[CustomNodeEditor(typeof(ViewNode))]
	public class ViewNodeEditor : BaseNodeEditor {

		private ViewBase vb;

		public override void OnBodyGUI() {
			serializedObject.Update();

			NodeEditorGUILayout.PortField(target.GetPort("enter"));

			var prop = serializedObject.FindProperty("view");
			EditorGUILayout.PropertyField(prop);
			
			var viewTypeProp = serializedObject.FindProperty("viewType");
			EditorGUILayout.PropertyField(viewTypeProp);
			
			EditorGUILayout.Space();

			ViewBase viewBase = ((ViewNode) target).view;
			if (prop.objectReferenceValue && viewBase != null && (vb == null || (vb != null && vb != viewBase))) {
				vb = viewBase;
				PropertyInfo[] properties = viewBase.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

				foreach (var propertyInfo in properties) {
					if (!propertyInfo.CanWrite || !propertyInfo.CanRead) {
						continue;
					}

					MethodInfo mget = propertyInfo.GetGetMethod(false);
					MethodInfo mset = propertyInfo.GetSetMethod(false);

					// Get and set methods have to be public
					if (mget == null) {
						continue;
					}

					if (mset == null) {
						continue;
					}

					if (!Attribute.IsDefined(propertyInfo, typeof(ViewOutputAttribute))) {
						continue;
					}
					
					target.AddDynamicOutput(typeof(EmptyNode), XNode.Node.ConnectionType.Override, XNode.Node.TypeConstraint.None, fieldName: propertyInfo.Name);
				}
			} else if (!prop.objectReferenceValue && vb != null) {
				vb = null;
			}
			
			foreach (NodePort dynamicPort in target.DynamicPorts) {
				if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
				NodeEditorGUILayout.PortField(dynamicPort);
			}
			
			if (GUILayout.Button("Clear Output")) {
				target.ClearDynamicPorts();
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
#endif