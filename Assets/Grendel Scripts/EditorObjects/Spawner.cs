using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Spawner : EditorObject, IEditorObject 
{
	
	public List<GameObject> EntitiesToSpawn = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{
		base.Start();
	}
	
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		
		if (Application.isPlaying) { OnPlayGizmos(); } else { OnEditGizmos(); }	
		_gizmoName = "Gizmo_Spawner";
			
	}
	
	public override void OnActivate(EditorObject caller) //called when the editor object is activated
	{
		Debug.Log("ACTIVATE!");
		base.OnActivate(caller);
		
		foreach(GameObject entityToSpawn in EntitiesToSpawn)
		{
			GameObject.Instantiate(entityToSpawn, transform.position, Quaternion.identity); 
		}
	}
	
	public override void OnDeactivate(EditorObject caller) //called when the editor object is deactivated
	{
		base.OnDeactivate(caller);
	}
	
	public override void OnToggle(EditorObject caller) //called when the editor object is toggled
	{
		base.OnToggle(caller);
	}
	
	public override void OnEnabled(EditorObject caller) //called when the editor object is deactivated
	{
		base.OnEnabled(caller);
	}
	
	public override void OnDisabled(EditorObject caller) //called when the editor object is toggled
	{
		base.OnDisabled(caller);
	}
	
	
	
}
