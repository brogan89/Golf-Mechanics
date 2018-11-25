using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SignController))]
public class SignCtrlEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Reset"))
		{
			Debug.Log("Resetting sign controller");
			SignController controller = target as SignController;
			controller.count = 1;
			controller.pool.Clear();
		}
	}
}
