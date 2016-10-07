using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GlobeSize))]
public class GlobeSizeEditor : Editor {

	public override void OnInspectorGUI() {

		GlobeSize globe = target as GlobeSize;

		base.OnInspectorGUI();

		EditorGUILayout.LabelField("Radius: " + globe.radius);		
		float radiusVal = EditorGUILayout.Slider(globe.radius, 0.5f, 10f);
		if(radiusVal != globe.radius)
			globe.SetRadius(radiusVal);

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Surface: " + globe.surface);
		float surfaceVal = EditorGUILayout.Slider(globe.surface, 1f, 1000f);
		if(surfaceVal != globe.surface)
			globe.surface = surfaceVal;
    }


}
