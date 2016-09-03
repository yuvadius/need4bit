﻿using UnityEngine;
using System.Collections;

public class GizmoDrawer : MonoBehaviour {

	public string gizmoName;
	public float dieTime;

	IEnumerator Start() {
		yield return new WaitForSeconds(dieTime);
		Destroy(gameObject);
	}

	public void OnDrawGizmos() {
		Gizmos.DrawIcon(transform.position, gizmoName, true);
	}



}
