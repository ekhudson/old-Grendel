using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class Autorun
{
    static Autorun()
    {       
		if (EditorApplication.isCompiling)
		{
			return; //return if we are just initializing due to code compile
		}
		
		Debug.Log("Initializing Grendel Editor Extensions");
		EditorApplication.update += RunOnce;
    }	 
   
    static void RunOnce()
    {
        EditorApplication.update -= RunOnce;
		Debug.Log("GRENDEL: Checking the Scene for objects required by Grendel.");
		EditorApplication.ExecuteMenuItem("Grendel/Setup Default Objects");
		
		Debug.Log("GRENDEL: Starting up Editor Input.");
		if (SceneView.onSceneGUIDelegate != EditorInput.Update)
		{
			SceneView.onSceneGUIDelegate += EditorInput.Update;
		}
    }	
}