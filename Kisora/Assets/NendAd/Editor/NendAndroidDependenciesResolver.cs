using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Google.JarResolver;

[InitializeOnLoad]
public static class NendAndroidDependenciesResolver {

	private const string GmsBasementArtifactName = "play-services-basement";
	private const string GmsLocationArtifactName = "play-services-location";
	private const string RecyclerviewArtifactName = "recyclerview-v7";
	private const string CardviewArtifactName = "cardview-v7";
	private const string ConstraintLayoutArtifactName = "constraint-layout";

	private const string AndroidLibraryTypeGoogle = "com.google.android.gms";
	private const string AndroidLibraryTypeSupport = "com.android.support";
	private const string AndroidLibraryTypeConstraint = "com.android.support.constraint";

	static NendAndroidDependenciesResolver () {
		#if UNITY_ANDROID
		PlayServicesSupport svcSupport = PlayServicesSupport.CreateInstance(
			"NendUnityPlugin", 
			EditorPrefs.GetString("AndroidSdkRoot"), 
			"ProjectSettings");

		//Require
		svcSupport.DependOn(AndroidLibraryTypeGoogle, GmsBasementArtifactName, "11.8.0");
		svcSupport.DependOn(AndroidLibraryTypeSupport, CardviewArtifactName, "27.1.0");
		svcSupport.DependOn(AndroidLibraryTypeSupport, RecyclerviewArtifactName, "27.1.0");
		svcSupport.DependOn(AndroidLibraryTypeConstraint, ConstraintLayoutArtifactName, "1.0.2");

		//Optional
		svcSupport.DependOn(AndroidLibraryTypeGoogle, GmsLocationArtifactName, "11.8.0");
		#endif
	}
}
