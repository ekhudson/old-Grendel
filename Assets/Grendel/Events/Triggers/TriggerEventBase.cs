using UnityEngine;
using System.Collections;

[System.Serializable]
public class TriggerEventBase : EventBase
{
	//[SerializeField]protected Collider _collider;
	[SerializeField]public readonly Collider mCollider;
	
	public Collider GetCollider
	{
		get { return mCollider; }
	}  

    public TriggerEventBase(Collider collider, object sender) : base(collider != null ? collider.transform.position : Vector3.zero, sender)
    {    
		mCollider = collider;
    }	
	
	public TriggerEventBase()
	{
		mCollider = null;
	}
}
