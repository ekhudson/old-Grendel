using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Stateless;

/// <summary>
/// The base State Machine Component. Inherit from this when
/// creating unique state machines for objects
/// </summary>
public class StateMachineComponent<TStateBase, TTrigger> : MonoBehaviour where TStateBase : class where TTrigger : class
{		
	[System.Serializable]
	public class StateMachineComponentEntry
	{		
		public TStateBase State;		
		public StateBase SubstateOf;
		public List<PermittedState> PermittedStates;		
	}	
	
	[System.Serializable]
	public class PermittedState
	{
		public EventBase TriggerEvent;
		public StateBase State;
	}	
	
	public List<StateMachineComponentEntry> States = new List<StateMachineComponentEntry>();
	public StateBase InitialState;
	
	public Stateless.StateMachine<TStateBase, TTrigger> Machine;
		
	protected void Start () 
	{
		Machine = new StateMachine<TStateBase, TTrigger>(InitialState as TStateBase);
		
		foreach(StateMachineComponentEntry state in States)
		{
			if (state.SubstateOf != null)
			{
				Machine.Configure(state.State).SubstateOf(state.SubstateOf as TStateBase);
			}
			
			foreach(PermittedState permittedState in state.PermittedStates)
			{
				Machine.Configure(state.State).Permit(permittedState.TriggerEvent as TTrigger, permittedState.State as TStateBase);
			}
		}	
		
	}	
}
