using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(ConnectionRegistry))]
public class ConnectionRegistryEditor : Editor 
{	
	private bool _drawAllConnections = false;
	private List<EditorObjectConnection> _scrubList = new List<EditorObjectConnection>();
	
	public ConnectionRegistry Target
    {
        get 
		{ 
			return (ConnectionRegistry)target; 
		}
    }
	
	public override void OnInspectorGUI()
	{
		EditorGUIUtility.LookLikeControls();	
		EditorGUILayout.LabelField("Connection Count: " + Target.Registry.Count);
		
		EditorGUILayout.Space();
		
		if (Target.Registry.Count <= 0) { return; }
		
		_drawAllConnections = GUILayout.Toggle(_drawAllConnections, "DRAW ALL CONNECTIONS", GUI.skin.button);
		
		if(GUILayout.Button("CLEAR CONNECTIONS"))
		{			
			ClearConnectionMenu();
		}		
		
		EditorGUILayout.Separator();
		
		EditorObject caller	= null;
		
		foreach(EditorObjectConnection connection in Target.Registry)
		{			
			//check for nulls
			if (connection.Caller == null || connection.Subject == null)
			{
				_scrubList.Add(connection);
				continue;
			}			
			
			if (caller != connection.Caller)
			{
				EditorGUI.indentLevel = 0;
				EditorGUILayout.LabelField(connection.Caller.name, EditorStyles.boldLabel);
				caller = connection.Caller;
			}
			
			EditorGUI.indentLevel = 2;
			connection.SetColor();
			GUI.color = connection.MessageColor;			
			EditorGUILayout.LabelField(string.Format("{0} {1} {2}",connection.Caller, connection.Message, connection.Subject), GUI.skin.box);			
			GUI.color = Color.white;						
		}
		
		EditorUtility.SetDirty(Target);	
		
		if(_scrubList.Count > 0)
		{
			foreach(EditorObjectConnection connection in _scrubList)
			{
				Target.Registry.Remove(connection);
			}
		}
	}	
	
	private void OnSceneGUI()
	{
		if (!_drawAllConnections)
		{
			return;
		}
		
		foreach(EditorObjectConnection connection in Target.Registry)
		{
			//Handles.color = connection.MessageColor;
			//Handles.DrawLine(connection.Caller.transform.position, connection.Subject.transform.position);
			DrawConnectionLine.DrawLine(connection.Caller, connection.Subject, connection.MessageColor);
		}
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
        GenericMenu menu = new GenericMenu ();
        
        menu.AddSeparator ("ARE YOU SURE YOU WANT TO CLEAR?");
		menu.AddSeparator ("");
		menu.AddItem(new GUIContent ("Clear"), false, ClearConnections, "");
        menu.AddItem(new GUIContent ("Don't Clear"), false, DontClearConnections, "");      
        
        menu.ShowAsContext ();
	}
}
