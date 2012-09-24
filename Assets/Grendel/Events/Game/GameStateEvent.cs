using UnityEngine;
using System.Collections;

public class GameStateEvent : EventBase 
{
	public readonly GameManager.GAMESTATE NewState;
	public readonly GameManager.GAMESTATE OldState;
	
	
	public GameStateEvent(GameManager.GAMESTATE newState, GameManager.GAMESTATE oldState, object sender) : base (Vector3.zero, sender)
	{
		NewState = newState;
		OldState = oldState;
	}
	
	public GameStateEvent() : base (Vector3.zero, null)
	{
		
	}
}
