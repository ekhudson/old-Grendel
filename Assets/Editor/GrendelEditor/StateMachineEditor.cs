using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StateMachineComponent<StateBase, EventBase>))]
public class StateMachineEditor : GrendelEditor<StateMachineComponent<StateBase, EventBase>>
{
	public override void OnInspectorGUI()
	{		
		GUILayout.Label("BASE");
		base.OnInspectorGUI();
		
		if (Target.States != null)
		{		
			foreach(StateMachineComponent<StateBase,EventBase>.StateMachineComponentEntry state in Target.States)
			{
				state.State = (StateBase)EditorGUILayout.ObjectField(state.State, typeof(StateBase), false);			
			}
		}
		
		if (GUILayout.Button("Add State"))
		{
			Target.States.Add(new StateMachineComponent<StateBase, EventBase>.StateMachineComponentEntry());
		}
	}	
}
