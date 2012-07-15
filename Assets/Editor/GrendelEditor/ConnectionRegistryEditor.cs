using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(ConnectionRegistry))]
public class ConnectionRegistryEditor : Editor 
{		
	public ConnectionRegistry Target
    {
        get 
		{ return (ConnectionRegistry)target; }
    }
	
	public override void OnInspectorGUI()
	{
		EditorGUIUtility.LookLikeControls();	
		EditorGUILayout.LabelField("Connection Count: " + Target.Registry.Count);
		
		EditorGUILayout.Space();
		
		if(GUILayout.Button("CLEAR CONNECTIONS"))
		{
			//Target.Registry.Clear();
			ClearConnectionMenu();
		}
		
		EditorGUILayout.Space();
				
		foreach(EditorObjectConnection connection in Target.Registry)
		{
			connection.SetColor();
			GUI.color = connection.MessageColor;
			EditorGUILayout.LabelField(string.Format("{0} {1} {2}",connection.Caller, connection.Message, connection.Subject), GUI.skin.box);
			EditorGUILayout.LabelField(connection.Caller.GetType().ToString());
			GUI.color = Color.white;				
		}
		
		EditorUtility.SetDirty(Target);
	}
	
	public void ClearConnections(object obj)
	{
		Target.Registry.Clear();
	}
	
	public void DontClearConnections(object obj)
	{
		
	}
	
	public void ClearConnectionMenu()
	{
		  // Now create the menu, add items and show it
        GenericMenu menu = new GenericMenu ();
        
        menu.AddSeparator ("ARE YOU SURE YOU WANT TO CLEAR?");
		menu.AddSeparator ("");
		menu.AddItem(new GUIContent ("Clear"), false, ClearConnections, "item 1");
        menu.AddItem(new GUIContent ("Don't Clear"), false, DontClearConnections, "item 2");      
        
        menu.ShowAsContext ();
	}
}
