namespace NendUnityPlugin
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using System.IO;
	using System.Xml;
	using System.Linq;

	public class AndroidSetupUtils : EditorWindow
	{
		public const string AndroidLibraryDirectoryPath = "Plugins/Android";

		internal static void ConfigureAndroidManifest (bool isOutputDebugLog)
		{
			string libraryDirectoryPath = Path.Combine (Application.dataPath, ToPlatformDirectorySeparator (AndroidLibraryDirectoryPath));
			if (!File.Exists (libraryDirectoryPath)) {
				Directory.CreateDirectory (libraryDirectoryPath);
			}

			string manifestPathDest = Path.Combine (Application.dataPath, ToPlatformDirectorySeparator (AndroidLibraryDirectoryPath + "/AndroidManifest.xml"));
			if (!File.Exists (manifestPathDest)) {
				string[] UnityAndroidManifestPathList = {
					Path.Combine (EditorApplication.applicationPath, ToPlatformDirectorySeparator ("../PlaybackEngines/AndroidPlayer/Apk/AndroidManifest.xml")),
					Path.Combine (EditorApplication.applicationContentsPath, ToPlatformDirectorySeparator ("PlaybackEngines/AndroidPlayer/Apk/AndroidManifest.xml")),
					Path.Combine (EditorApplication.applicationContentsPath, ToPlatformDirectorySeparator ("PlaybackEngines/AndroidPlayer/AndroidManifest.xml"))
				};

				string defaultManifestPath = null;
				foreach (string path in UnityAndroidManifestPathList) {
					if (File.Exists (path)) {
						defaultManifestPath = path;
						Debug.Log ("Found AndroidManifest at " + path);
						break;
					}
				}
				if (null == defaultManifestPath) {
					Debug.LogWarning ("Couldn't find default AndroidManifest.");
					return;
				}
				FileUtil.CopyFileOrDirectory (defaultManifestPath, manifestPathDest);
			} else {
				Debug.Log ("The AndroidManifest is already exist.");
			}

			var doc = new XmlDocument ();
			doc.Load (manifestPathDest);

			XmlNode applicationNode = doc.SelectSingleNode ("manifest/application");
			if (null == applicationNode) {
				Debug.LogWarning ("The application tag is not found.");
				return;
			}

			string ns = applicationNode.GetNamespaceOfPrefix ("android");
			var nsManager = new XmlNamespaceManager (doc.NameTable);
			nsManager.AddNamespace ("android", ns);

			XmlElement rootElement = doc.DocumentElement;

			XmlNode nendDebuggableNode = applicationNode.SelectSingleNode (@"//meta-data[@android:name='NendDebuggable']", nsManager);
			if (null != nendDebuggableNode) {
				XmlElement element = (XmlElement)nendDebuggableNode;
				element.SetAttribute ("value", ns, isOutputDebugLog.ToString ().ToLower ());
				Debug.Log ("Modified: " + element.OuterXml);
			} else if (isOutputDebugLog) {
				XmlElement element = CreateNendDebuggableElement (doc, ns);
				applicationNode.AppendChild (element);
				Debug.Log ("Added: " + element.OuterXml);
			} else {
				Debug.Log ("There is no need to create a NendDebuggable element.");
			}

			doc.Save (manifestPathDest);
		}

		private static XmlElement CreateNendDebuggableElement (XmlDocument doc, string ns)
		{
			XmlElement element = doc.CreateElement ("meta-data");
			element.SetAttribute ("name", ns, "NendDebuggable");
			element.SetAttribute ("value", ns, "true");
			return element;
		}

		private static string ToPlatformDirectorySeparator (string path)
		{
			return path.Replace ("/", Path.DirectorySeparatorChar.ToString ());
		}
	}
}
