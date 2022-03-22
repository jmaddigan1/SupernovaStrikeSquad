using Supernova.Assets.Code.ProjectSettings;
using UnityEditor;
using UnityEngine;

namespace Supernova.Editor {
	public static class EditorMethodExtensions {
		public static BuildTargetGroup GetBuildTargetGroup(this PlatformType platform) {
			switch (platform) {
				case PlatformType.Mac:
				case PlatformType.Windows:
					return BuildTargetGroup.Standalone;
				case PlatformType.Android:
					return BuildTargetGroup.Android;
				case PlatformType.iOS:
					return BuildTargetGroup.iOS;
				case PlatformType.No_Target:
				default:
					return BuildTargetGroup.Unknown;
			}
		}
		
		public static string ToAppExe(this PlatformType platform) {
			switch (platform) {
				case PlatformType.Mac:
					return $"{Application.version}.app";
				case PlatformType.Windows:
					return "Crashers of Aetheria.exe";
				case PlatformType.Android:
					return "CrashersOfAetheria.apk";
				case PlatformType.iOS:
					return "CrashersOfAetheria.ipa";
				case PlatformType.No_Target:
				default:
					return string.Empty;
			}
		}
	}
}