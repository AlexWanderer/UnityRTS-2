using UnityEngine;
using System.Collections;

namespace RTS {
    public static class ResourceManager {
 		public static float ScrollSpeed { get { return 25; } }
 		public static int ScrollWidth { get { return 15; } }
 		public static float MinCameraHeight { get { return 10; } }
		public static float MaxCameraHeight { get { return 40; } }
		private static Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
		public static Vector3 InvalidPosition { get { return invalidPosition; } }

		private static GUISkin selectBoxSkin;
		public static GUISkin SelectBoxSkin { get { return selectBoxSkin; } }

		private static Texture2D healthyTexture, damagedTexture, criticalTexture;
		public static Texture2D HealthyTexture { get { return healthyTexture; } }
		public static Texture2D DamagedTexture { get { return damagedTexture; } }
		public static Texture2D CriticalTexture { get { return criticalTexture; } }
		 
		public static void StoreSelectBoxItems(GUISkin skin, Texture2D healthy, Texture2D damaged, Texture2D critical) {
		    selectBoxSkin = skin;
		    healthyTexture = healthy;
		    damagedTexture = damaged;
		    criticalTexture = critical;
		}

		private static Bounds invalidBounds = new Bounds(new Vector3(-99999, -99999, -99999), new Vector3(0, 0, 0));
		public static Bounds InvalidBounds { get { return invalidBounds; } }
		public static int BuildSpeed { get { return 2; } }

		public static void SetGameObjectList(GameObjectList objectList) { gameObjectList = objectList; }

		private static GameObjectList gameObjectList;
		public static GameObject GetBuilding(string name) {
			return gameObjectList.GetBuilding(name);
		}

		public static GameObject GetUnit(string name) {
			return gameObjectList.GetUnit(name);
		}

		public static GameObject GetWorldObject(string name) {
			return gameObjectList.GetWorldObject(name);
		}

		public static GameObject GetPlayerObject() {
			return gameObjectList.GetPlayerObject();
		}

		public static Texture2D GetBuildImage(string name) {
			return gameObjectList.GetBuildImage(name);
		}
	}
    public enum ResourceType { Money, Power }

    public enum CursorState { Select, Move, Attack, PanLeft, PanRight, PanUp, PanDown, Harvest, RallyPoint }

}