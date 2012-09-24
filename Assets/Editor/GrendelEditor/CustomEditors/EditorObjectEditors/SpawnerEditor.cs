using System.Collections;

using UnityEditor;
using UnityEngine;

using GrendelEditor.UI;

[CustomEditor(typeof(Spawner))]
[CanEditMultipleObjects] 
public class SpawnerEditor : EditorObjectEditor<Spawner> 
{	
	
	protected override void OnEnable()
	{		
		base.OnEnable();	
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		GUILayout.BeginVertical();
		(Target as Spawner).MyColor = CustomEditorGUI.ColorGridLayout(4, 4, 32f, 2f, new Color[]{Color.red, Color.black, Color.blue, Color.cyan, 
												   Color.gray, Color.white, Color.yellow, GrendelColor.DarkBlue, 
			                                       GrendelColor.DarkCyan, GrendelColor.DarkGreen, GrendelColor.DarkOrange, GrendelColor.DarkPink,
												   GrendelColor.Orange, GrendelColor.Pink, GrendelColor.DarkYellow, GrendelColor.DarkMagenta}, (Target as Spawner).MyColor);		
		
	    GUILayout.EndVertical();						
	}
	
	protected override void OnSceneGUI()
	{
		base.OnSceneGUI();		
		
		Handles.color = Color.cyan;
		Handles.SelectionFrame(1, Target.transform.position, SceneView.currentDrawingSceneView.camera.transform.rotation, 1f);
	}
	
}

