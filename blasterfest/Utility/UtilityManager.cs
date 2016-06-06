using UnityEngine;

public static class UtilityManager {

	public struct SceneNames {
		public static readonly string menuScene = "Scene_Menu";
		public static readonly string gameScene = "Scene_Game";
		public static readonly string mainScene = "Scene_Main";
	}

	public struct Tags {
		public static readonly string player = "Player";
		public static readonly string bullet = "Bullet";
	}

	public struct Layers {
		public static readonly string player = "Player";
		public static readonly string oneWayPlatform = "OneWayPlatform";
		public static readonly string triggers = "Triggers";
	}

	public struct AnimationHash {
		public static readonly int run = Animator.StringToHash("run");
		public static readonly int die = Animator.StringToHash("die");
	}

}
