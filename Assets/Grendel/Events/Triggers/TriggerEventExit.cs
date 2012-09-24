using UnityEngine;
using System.Collections;

[System.Serializable]
public class TriggerEventExit : TriggerEventBase
{
	public TriggerEventExit(Collider collider, object sender) : base(collider, sender)
	{	
		
	}	
	
	public TriggerEventExit() : base(null, null)
	{	
		
	}
}
