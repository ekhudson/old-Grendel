using UnityEngine;
using System.Collections;
using System.Reflection;

	/// <summary>
	/// Title: Grendel Framework
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
	public bool DoNotDestroyOnLoad = false;
	public bool DestroyNewDuplicate = true;
	public bool DestroyGameObject = false;
	
	private static T _instance;
	
	static public T Instance
	{
		get 
		{
//			if (_instance == null)
//			{
//				Debug.LogWarning(string.Format("Component of type <{0}> does not exist in the scene", typeof(T).ToString() ));				
//			}
			
			return _instance;
		}
	}
		
	virtual protected void Awake()
	{
		//checks if there is already an instance in the game
		//and destroys this object if it is a duplicate
		if (_instance == null)
		{
			_instance = this as T;
			if (DoNotDestroyOnLoad){ DontDestroyOnLoad(gameObject); }
		}
		else
		{
			Console.Instance.OutputToConsole(string.Format("Destroying: {0}", typeof(T).ToString()), Console.Instance.Style_Admin);
			if (DestroyNewDuplicate)
			{
				if (DestroyGameObject)
				{
					Destroy( this.gameObject );
				}
				else
				{
					Destroy ( this );
				}
			}
			else
			{
				if (DestroyGameObject)
				{
					Destroy( (_instance as MonoBehaviour).gameObject );
				}
				else
				{
					Destroy( _instance as MonoBehaviour);
				}
				_instance = this as T;
			}
		}
	}
			
}
