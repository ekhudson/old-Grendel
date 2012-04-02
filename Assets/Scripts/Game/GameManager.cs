using UnityEngine;
using System.Collections;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Feb 15, 2012
	/// 
	/// Filename: GameManager.cs
	/// 
	/// Summary: Essentially holds information that might need to be
	/// accessed by a variety of objects in the scene, and contains
	/// global references to useful scripts (ie. UserInput, EntityManager)
	///  
	/// </summary>

public class GameManager : Singleton<GameManager>
{
	#region PUBLIC VARIABLES
	public static Player PlayerRef;
	#endregion		
	
	void Awake()
	{		
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () 
	{				
	
	}
}
