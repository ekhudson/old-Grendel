using UnityEngine;
using System.Collections;

[System.Serializable]
public class TriggerEventStay : TriggerEventBase 
{	
	public TriggerEventStay(Collider collider, object sender) : base(collider, sender)
	{		
		
	}
	
	public TriggerEventStay() : base(null, null)
	{	
		
	}
}
