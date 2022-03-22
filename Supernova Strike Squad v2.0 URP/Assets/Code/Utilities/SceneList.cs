namespace Supernova.Utilities {

	public static class SceneList {
		//Only give access to these scenes in Editor mode
		#if UNITY_EDITOR
			public const string BOOTSTRAP = "Assets/Scenes/Bootstrap.unity";
		#endif
		
		public const string MAIN_MENU = "MainMenu";
		public const string HANGAR = "Hangar";
		public const string GAMEPLAY = "Gameplay";
	}
}