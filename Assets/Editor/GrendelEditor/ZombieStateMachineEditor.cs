using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ZombieStateMachine))]
public class ZombieStateMachineEditor : StateMachineEditor
{
	public override void OnInspectorGUI()
	{
		GUILayout.Label("ZOMBIE");
		base.OnInspectorGUI();
	}	
}
