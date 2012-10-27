using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class StateBase : MonoBehaviour, IState 
{
//	public class PermittedState
//	{
//		public EventBase TriggerEvent;
//		public StateBase State;
//	}
	
//	public StateBase SubstateOf;
//	public List<PermittedState> PermittedStates;
	
	public delegate void OnEntryDelegate();
	public delegate void OnExitDelegate();
		
	public void OnEntry()
	{
		
	}
	
	public void OnExit()
	{
		
	}
}
