using UnityEngine;
using System.Collections;

[System.Serializable]
public class TriggerEventEnter : TriggerEventBase
{
   	
	public TriggerEventEnter(Collider collider, Object sender) : base(collider, sender)
	{		
		
	}
	
	public TriggerEventEnter() : base(null, null)
	{		
		
	}
}
