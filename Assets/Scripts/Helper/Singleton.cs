using UnityEngine;
using System.Collections;
using System.Reflection;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Mar 28, 2012
	/// 
	/// Filename: Singleton.cs
	/// 
	/// Summary: Subclass in order to create a singleton.
	/// Ensures that only one instance exists in the scene
	/// 
	/// </summary>

public class Singleton<T> : MonoBehaviour where T : class
{ 
	private static T _instance;
	
	static public T Instance
	{
		get {return _instance;}
	}
		
	void Awake()
	{
		//checks if there is already an instance in the game
		//and destroys this object if it is a duplicate
		if (_instance == null)
		{
			_instance = this as T;
		}
		else
		{
			Destroy( gameObject );
		}
	}
			
}
