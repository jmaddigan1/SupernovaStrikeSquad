using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Supernova.Assets.Code.ProjectSettings;
using UnityEditor;
using UnityEngine;

namespace Supernova.Editor {
	public class ProjectBuilderWindow : OdinEditorWindow {

		#region Private Static Properties
		
		private static PlatformType PlatformType {
			get;
			set;
		}

		private static bool SUPERNOVA_DEBUG {
			get;
			set;
		}
		
		private static bool SUPERNOVA_FORCE_MOCK {
			get;
			set;
		}

		#endregion
		

		#region Private Static Functions

		[MenuItem("Supernova/Project Builder")]
		private static void OpenWindow() {
			SyncEditor();
			GetWindow<ProjectBuilderWindow>().Show();
		}
		
		[UnityEditor.Callbacks.DidReloadScripts]
		private static void SyncEditor() {
			PlatformType = (PlatformType)EditorUserBuildSettings.activeBuildTarget;
			SUPERNOVA_DEBUG = EntryIsInDefineSymbols("SUPERNOVA_DEBUG");
			SUPERNOVA_FORCE_MOCK = EntryIsInDefineSymbols("SUPERNOVA_FORCE_MOCK");
		}

		private static List<string> GetDefineSymbols() {
			string defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
			return defineSymbols.Split(new char[] {';'}, System.StringSplitOptions.RemoveEmptyEntries).ToList();
		}

		private static bool EntryIsInDefineSymbols(string symbol) {
			var defineSymbols = GetDefineSymbols();
			return defineSymbols.Contains(symbol);
		}

		[OnInspectorGUI]
		private void DrawWindow() {
			EditorGUILayout.Space();
			
			//Select Platform
			var platformType = EnumSelector<PlatformType>.DrawEnumField(new GUIContent("Platform Type"), PlatformType);
			if (platformType != PlatformType) {
				EditorUserBuildSettings.SwitchActiveBuildTarget(platformType.GetBuildTargetGroup(), (BuildTarget) platformType);
				PlatformType = platformType;
			}
			
			//Added Define Symbols
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Optional Defines");
			var supernovaDebug = EditorGUILayout.Toggle("SUPERNOVA_DEBUG", SUPERNOVA_DEBUG);
			if (supernovaDebug != SUPERNOVA_DEBUG) {
				var symbol = "SUPERNOVA_DEBUG";
				var defineSymbols = RemoveFromDefines(symbol);
				if (supernovaDebug) { defineSymbols.Add(symbol); }
				PlayerSettings.SetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", defineSymbols.ToArray()));
				SUPERNOVA_DEBUG = supernovaDebug;
			}
			
			var supernovaForceMock = EditorGUILayout.Toggle("SUPERNOVA_FORCE_MOCK", SUPERNOVA_FORCE_MOCK);
			if (supernovaForceMock != SUPERNOVA_FORCE_MOCK) {
				var symbol = "SUPERNOVA_FORCE_MOCK";
				var defineSymbols = RemoveFromDefines(symbol);
				if (supernovaForceMock) { defineSymbols.Add(symbol); }
				PlayerSettings.SetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", defineSymbols.ToArray()));
				SUPERNOVA_FORCE_MOCK = supernovaForceMock;
			}

			//Define Symbols
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Current Define Symbols");
			foreach (var symbol in GetDefineSymbols()) {
				EditorGUILayout.LabelField($"{symbol}");
			}
			
			//End Buttons
			EditorGUILayout.Space();
			EditorGUILayout.LabelField($"Version {Application.version}");
			if (PlatformType == PlatformType.Android) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField($"Android Bundle Version {PlayerSettings.Android.bundleVersionCode}");
				if (GUILayout.Button("+")) {
					PlayerSettings.Android.bundleVersionCode++;
				}
				EditorGUILayout.EndHorizontal();
			}

			if (GUILayout.Button("Start Build")) {
				var scenes = EditorBuildSettings.scenes;
				var path = $"{Application.dataPath}/../../Builds/{PlatformType}/{Application.version}/";
				Directory.CreateDirectory(path);

				BuildPipeline.BuildPlayer(scenes, $"{path}{PlatformType.ToAppExe()}", (BuildTarget) PlatformType, BuildOptions.None);
				System.Diagnostics.Process.Start("explorer.exe", path);
			}
		}

		private List<string> RemoveFromDefines(string defineSymbol) {
			var defineSymbols = GetDefineSymbols();
			for (int i = defineSymbols.Count - 1; i >= 0; i--) {
				if (defineSymbols[i] == defineSymbol) {
					defineSymbols.RemoveAt(i);
				}
			}

			return defineSymbols;
		}

		#endregion
	}
}