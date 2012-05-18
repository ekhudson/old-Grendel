using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trigger : EditorObject 
{
	public delegate void OnTriggerEnterHandler(Trigger trigger, Collider intruder);
	public delegate void OnTriggerExitHandler(Trigger trigger, Collider intruder);
	public event OnTriggerEnterHandler TriggerEntered;
	public event OnTriggerExitHandler TriggerExited;
	
	public List<Collider> ObjectList = new List<Collider>();
	private List<Collider> _removeList = new List<Collider>();
	private float _scrubTimeInterval = 0.5f; //how often the list is scrubbed for nulls
	
	// Use this for initialization
	protected override void Start () {

		base.Start();
		StartCoroutine( ScrubList() );
	}	
	
	/// <summary>
	/// Scrubs the list for nulls
	/// </summary>	
	IEnumerator ScrubList()
	{
		while(true)
		{
			foreach(Collider other in ObjectList)
			{
				if (other != null)
				{
					continue;
				}
				else
				{
					_removeList.Add(other);
				}
			}
			
			foreach(Collider other in _removeList)
			{				
				ObjectList.Remove(other);				
			}
			
			_removeList.Clear();
			
			yield return new WaitForSeconds(_scrubTimeInterval);
		}		
		
	}
	
	virtual protected void OnTriggerEnter(Collider collider)
	{		
		if (TriggerEntered != null){ TriggerEntered(this, collider); }
		ObjectList.Add(collider);
	}
	
	virtual protected void OnTriggerExit(Collider collider)
	{		
		if (TriggerExited != null){ TriggerExited(this, collider); }
		ObjectList.Remove(collider);
	}
	
	void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		
		if (Application.isPlaying) { OnPlayGizmos(); } else { OnEditGizmos(); }	
			
	}
	
	protected override void OnPlayGizmos()
	{
		base.OnPlayGizmos();
		
		if (ObjectList.Count > 0){ Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
			Gizmos.DrawWireSphere(transform.position, collider.bounds.extents.x);
		
		foreach(Collider other in ObjectList)
		{
			if (other == null){ continue; }
			Gizmos.DrawLine(transform.position, other.transform.position);
		}
	}
	
	protected override void OnEditGizmos()
	{
		base.OnEditGizmos();
		
		if (_currentActiveObject == gameObject){ return; }
		
		Gizmos.color = Color.gray;
		Gizmos.DrawWireSphere(transform.position, collider.bounds.extents.x);
	}
	
	
	void OnGUI()
	{
		OnSceneGUI();
	}
	
}
