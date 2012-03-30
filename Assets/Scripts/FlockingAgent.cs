using UnityEngine;
using System.Collections;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Jan 10, 2012
	/// 
	/// Filename: FlockingAgent.cs
	/// 
	/// Summary: Attach this to entities and assign their Species to
	/// have them flock together, avoid enemies, etc.
	/// 
	/// </summary>

public class FlockingAgent : MonoBehaviour 
{
	public float AwarenessRadius = 10f;
	public float SeparationWeighting = 1f;
	public float CohesionWeighting = 1f;
	public float AlignmentWeighting = 1f;
	
	private SearchRadius _agentSearchRadius;
	private Entity _agent;
	private Entity[] _flock;
		
	// Use this for initialization
	void Start () 
	{
		_agent = gameObject.GetComponent<Entity>();		
		AwarenessRadius *= AwarenessRadius; //square for performance
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void CalculateSeparation()
	{		
		int count = 0;
		Vector3 flockingAmount = Vector3.zero;
		
		foreach(Entity agent in _flock)
		{			
			if (agent == this) { continue; }
			
			Vector3 difference = (agent.BaseTransform.position - _agent.BaseTransform.position);
			float distance = difference.sqrMagnitude;
			if (distance <= AwarenessRadius)
			{
				count++;
				difference.Normalize();
				flockingAmount += (difference / distance); //weight by distance				
			}
		}
		
		if (count > 1)
		{
			flockingAmount /= count;
		}
		
		if (flockingAmount.sqrMagnitude > 0)
		{
			_agent.FlockingAmount = flockingAmount;
		}		
	}//end CalculateSeparation	
	
	
}//end class
	

