using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class EditorObjectUpdater : MonoBehaviour 
{	
	void OnGUI()
	{
		if(Application.isPlaying)
		{
			if(SceneView.onSceneGUIDelegate == this.OnSceneGUI)
			{
				SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
			}
		}
		else if(SceneView.onSceneGUIDelegate != this.OnSceneGUI)
   		{
       		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    	}
	}
	
	void OnSceneGUI(SceneView scnView)
	{
		UpdateAllEditorObjects();
	}
	
	protected void UpdateAllEditorObjects()
	{		
		Handles.BeginGUI();		
		EditorObject[] objects = FindObjectsOfType(typeof(EditorObject)) as EditorObject[];
		
		foreach(EditorObject eo in objects)
		{
			if (eo.gameObject != Selection.activeGameObject)
			{				
				
				eo.DrawSimpleLabel(SceneView.currentDrawingSceneView.camera);
				
			}
		}	
		
		Handles.EndGUI();
	}	
}
