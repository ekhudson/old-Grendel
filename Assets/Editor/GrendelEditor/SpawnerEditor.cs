using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Spawner))]
[CanEditMultipleObjects] 
public class SpawnerEditor : EditorObjectInfo<Spawner> 
{	
	protected override void OnEnable()
	{		
		base.OnEnable();	
	}
	
	protected override void OnSceneGUI()
	{
		base.OnSceneGUI();		
	}
	
}
