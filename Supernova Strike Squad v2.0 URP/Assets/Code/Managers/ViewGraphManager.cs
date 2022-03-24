using System;
using System.Collections.Generic;
using Supernova.Graphs;
using Supernova.Nodes.ViewGraph;
using Supernova.UI.Views;
using Supernova.Utilities;
using UnityEngine;

namespace Supernova.Managers {
	public class ViewGraphManager : Singleton<ViewGraphManager> {

		#region Serialized Fields

		[SerializeField]
		private ViewGraph startingGraph;

		[SerializeField]
		private RectTransform viewContainer;

		#endregion


		#region NonSerialized Fields

		[NonSerialized]
		private Stack<ViewBase> views;

		#endregion


		#region Properties

		public BaseNode CurrentBaseNode {
			get;
			set;
		}
		
		public ViewNode CurrentView {
			get;
			set;
		} = null;

		public ViewBase TopView {
			get {
				return this.Views.Peek();
			}
		}
		
		private Stack<ViewBase> Views {
			get {
				return views = views ?? new Stack<ViewBase>();
			}
		}

		private Coroutine GraphCoroutine {
			get;
			set;
		}

		#endregion


		#region Public Functions

		public void Start() {
			this.GraphCoroutine = StartCoroutine(((BaseNode) startingGraph.nodes[0]).ProcessNode());
		}

		public T Create<T>(GameObject gameObject, ViewType viewType) where T : ViewBase {
			var s = gameObject.GetComponent<T>().GetType();
			
			//TODO: Check if our incoming view already works
			if (viewType == ViewType.View) {
				while (this.Views.Count > 0 && this.Views.Peek().GetType() != s) {
					Debug.Log($"{this.Views.Peek().GetType()} | {s}");
					DestroyImmediate(this.Views.Pop().gameObject);
				}
			}

			if (this.Views.Count == 0 || this.Views.Peek().GetType() != s) {
				GameObject go = Instantiate(gameObject, viewContainer);
				var t = go.GetComponent<T>();
				this.Views.Push(t);
				return t;
			}

			return (T)this.Views.Peek();
		}

		public void ProcessViewNodeSelection(string id) {
			Debug.Log($"[View Graph] Attempting to Process ({id})");
			if (this.CurrentView != null) {
				this.CurrentView.ProcessSelection(id);
				this.CurrentView = null;
			} else {
				Debug.Log($"Current View is NULL");
			}
		}
		
		public void PopTopView() {
			if (this.Views.Count > 0) {
				DestroyImmediate(this.Views.Pop().gameObject);
			}
		}
		
		public void PopWholeViewStack() {
			while (this.Views.Count > 0) {
				DestroyImmediate(this.Views.Pop().gameObject);
			}
		}

		#endregion
		

	}
}