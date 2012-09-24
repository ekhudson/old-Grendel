using UnityEngine;
using System.Collections;

[System.Serializable]
public class EventTransceiver 
{
	public enum Events
	{
		TriggerEventEnter,
		TriggerEventExit,
		TriggerEventStay,
		GameStateEvent,
	}
	
	public static EventBase LookupEvent(Events evt)
	{
		
		//EventBase returnEvent;
		
		switch(evt)
		{
			case Events.TriggerEventEnter:
			
				return new TriggerEventEnter();
			
			//break;
			
			case Events.TriggerEventExit:
			
				return new TriggerEventExit();
			
			//break;
			
			case Events.TriggerEventStay:
			
				return new TriggerEventStay();
			
			//break;
			
			case Events.GameStateEvent:
			
				return new GameStateEvent();
			//break;
			
			default:
			
				return null;
			
			//break;			
		}
		
		//return returnEvent;
	}
	
	
}
