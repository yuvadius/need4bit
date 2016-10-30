using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GlobeSize))]
public class GlobeSizeEditor : Editor {

	public override void OnInspectorGUI() {

		GlobeSize globe = target as GlobeSize;

		base.OnInspectorGUI();

		EditorGUILayout.LabelField("Surface: " + globe.surface);

		float surfaceVal = EditorGUILayout.Slider(globe.destinationSurface, globe.minSurface, globe.maxSurface);

		EditorGUILayout.LabelField("Radius: " + globe.radius);		

		EditorGUILayout.Space();

		if(surfaceVal != globe.destinationSurface) {
			globe.destinationSurface = surfaceVal;
		}

	}


}
