using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ConnectionRegistry : Singleton<ConnectionRegistry>
{	
	[SerializeField]private List<EditorObjectConnection> _registry;	
	private static ConnectionRegistry _instance = null;
	private EditorObjectConnectionComparer _comparer = new EditorObjectConnectionComparer();
	private static ConnectionRegistry _designInstance;

	protected override void Awake()
	{		
		base.Awake();		
	}
	
	public static ConnectionRegistry DesignInstance
	{
		get
		{
			if(_designInstance == null)
			{
				_designInstance = GameObject.Find("ConnectionRegistry").GetComponent<ConnectionRegistry>();
			}
			
			return _designInstance;
		}
	}
	
	public List<EditorObjectConnection> Registry
	{
		get
		{			
			if (_registry == null)
			{				
				_registry = new List<EditorObjectConnection>();
			}
			
			return _registry;			
		}
		set
		{
			_registry = value;
			
		}		
	}
	
	public EditorObjectConnection ContainsConnection(EditorObject subject, EditorObject caller)
	{
		EditorObjectConnection testConnection = null;		
		
		foreach(EditorObjectConnection connection in Registry)
		{
			if(connection.Subject == subject && connection.Caller == caller)
			{					
				testConnection = connection;			
			}			
		}
		
		return testConnection;			
	}
	
	public void AddConnection(EditorObject subject, EditorObject caller, EditorObject.EditorObjectMessage message)
	{					
		if (ContainsConnection(subject, caller) == null)
		{
		
			EditorObjectConnection newConnection = new EditorObjectConnection(message, caller as EditorObject, subject as EditorObject);						
			
			Registry.Add(newConnection);
			Registry.Sort(_comparer);
		}
		else
		{
			Debug.LogWarning(string.Format("Attempted to add a connection between {0} and {1}, but a connection already exists", subject.ToString(), caller.ToString()), this); 
		}
	}
	
	public void RemoveConnection(EditorObject subject, EditorObject caller)
	{
		EditorObjectConnection connectionToRemove = ContainsConnection(subject, caller);
		
		if (connectionToRemove != null)
		{			
			Registry.Remove(connectionToRemove);
		}
		else
		{
			Debug.LogWarning(string.Format("Attempted to remove a connection between {0} and {1}, but a connection does not exist", subject.ToString(), caller.ToString()), this); 
		}
	}
	
	class EditorObjectConnectionComparer : IComparer<EditorObjectConnection>
    {
        public int Compare(EditorObjectConnection x, EditorObjectConnection y)
        {
            return x.Caller.name.CompareTo(y.Caller.name);
        }
    }
}
