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

public class GameManager : MonoBehaviour {	
	
	//PUBLIC	
	public static UserInput UserInputRef;
	public static EntityManager EntityManagerRef;
	public static Player PlayerRef;
	//public static MapCamera MapCameraRef;
		
	//PRIVATE
	private static GameManager instance;
	
	public static GameManager Instance
	{
		get{ return instance; }
	}
	
	void Awake()
	{
		if (instance != null) { Destroy(gameObject); } //if a GameManager already exists, destroy this instance 
		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () 
	{		
		//Grab our references
		UserInputRef = GetComponent<UserInput>();
		EntityManagerRef = GetComponent<EntityManager>();		
	
	}
}
