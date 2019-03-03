namespace NendUnityPlugin
{
	using UnityEngine;
	using UnityEditor;
	using System.IO;
	using System.Xml;
	using System.Linq;

	public class NendAndroidSetup : EditorWindow
	{
		private static bool isOutputDebugLog = false;

		[MenuItem("NendSDK/Android Output Debug Log")]
		public static void CheckOutputDebugLog(){
			var path = "NendSDK/Android Output Debug Log";

			isOutputDebugLog = !Menu.GetChecked(path);
			Menu.SetChecked(path, isOutputDebugLog);

			Configure ();
		}

		private static void Configure ()
		{
			Debug.Log ("Processing...");

			AndroidSetupUtils.ConfigureAndroidManifest (isOutputDebugLog);
			AssetDatabase.Refresh ();
			Debug.Log ("Done!");
		}
	}
}
