using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EditorObject : MonoBehaviour, IEditorObject
{
	public bool DebugMode = true;
	public string Comment = "";
	public int NumberOfUses = -1; //-1 == infinite
	//public List<EditorObjectConnection> Connections = new List<EditorObjectConnection>();
	//public Dictionary<Delegate, EditorObjectConnection> Connections = new Dictionary<Delegate, EditorObjectConnection>();
	
	protected Transform _transform;
	protected GameObject _gameObject;
	protected GUIStyle SelectedTextStyle = new GUIStyle();
	protected int _labelWidth = 128;
	protected int _labelHeight = 32;
	protected GameObject _currentActiveObject;	
	
//	[HideInInspector]
//	public bool SubjectActivateOpen = false;
//	[HideInInspector]
//	public bool SubjectDeactivateOpen = false;
//	[HideInInspector]
//	public bool SubjectToggleOpen = false;
//	
//	[HideInInspector]
//	public bool MasterActivateOpen = false;
//	[HideInInspector]
//	public bool MasterDeactivateOpen = false;
//	[HideInInspector]
//	public bool MasterToggleOpen = false;
	
	[HideInInspector]
	public bool LookingForSubject = false;
	
	public static EditorObject CurrentHoveredEditorObject;
	
	[HideInInspector]
	public bool ActivateHighlight = false; //the highlight to show when in editor when this object is going to be activated by another	
	[HideInInspector]
	public bool DeactivateHighlight = false; //the highlight to show when in editor when this object is going to be deactivated by another
	[HideInInspector]
	public bool ToggleHighlight = false; //the highlight to show when in editor when this object is going to be toggled by another
	[HideInInspector]
	public bool HighlightHighlight = false; //the highlight to show when in editor when this object highlight via the info panel
		
	protected Camera _cameraToUse;
	protected string _gizmoName = "";
	protected bool _nameConflict = false; //used by the editor to flag this object if it has a name conflict with another object
	
	public event EditorObjectEventHandler ActivateEvent;
	protected event EditorObjectEventHandler DeactivateEvent;
	protected event EditorObjectEventHandler ToggleEvent;
	protected event EditorObjectEventHandler EnableEvent;
	protected event EditorObjectEventHandler DisableEvent;
	public delegate void EditorObjectEventHandler(EditorObject caller);
	
	private bool _clearConnections = false;
	
	[System.Serializable]
	public enum EditorObjectMessage
	{
		None,
		Activate, //turn on the editor object
		Deactivate, //turn off the editor object
		Toggle, //toggle the state of the editor object
		Enable, //enable a disabled editor object
		Disable //disable an enabled editor object		
	}
	
	public enum EDITOROBJECTSTATES 
	{
		ACTIVE, //has been activated and is running
		INACTIVE, //not running and awaiting activation
		DISABLED //will not receive any messages from other editor objects
	};
	
	protected EDITOROBJECTSTATES _state = EDITOROBJECTSTATES.INACTIVE;
	
	public EDITOROBJECTSTATES State
	{
		get {return _state;}
		set {_state = value;}
	}
	
	public bool NameConflict
	{
		get {return _nameConflict;}
		set {_nameConflict = value;}
	}
	
	public string GizmoName
	{
		get {return _gizmoName;}		
	}
	
	public EditorObject() : base()
	{
		_clearConnections = true;				
	}
	
	virtual protected void Awake ()
	{
		
	}
	
	// Use this for initialization
	virtual protected void Start () 
	{
		ActivateEvent += new EditorObjectEventHandler(OnActivate);
		DeactivateEvent += new EditorObjectEventHandler(OnDeactivate);
		ToggleEvent += new EditorObjectEventHandler(OnToggle);
		EnableEvent += new EditorObjectEventHandler(OnEnabled);
		DisableEvent += new EditorObjectEventHandler(OnDisabled);
	}
	
	// Update is called once per frame
	virtual protected void Update () 
	{
	
	}	
	
	virtual public void OnSceneGUI()
	{		
		
	}
	
	public void DrawSimpleLabel(Camera cameraToUse)
	{		
		Vector3 distance = cameraToUse.transform.position - transform.position;	
		_cameraToUse = cameraToUse;
		
		SelectedTextStyle = GUI.skin.button;
		SelectedTextStyle.normal.textColor = Color.yellow;
		Vector2 screenCoords = cameraToUse.WorldToScreenPoint(transform.position);
		screenCoords.y = cameraToUse.pixelHeight - screenCoords.y;			
		
		Rect labelRect = new Rect(screenCoords.x - (_labelWidth * 0.5f), screenCoords.y - (_labelHeight * 0.5f), _labelWidth, _labelHeight);
	}
		
	public void LabelWindow(int windowID)
	{		
		//maybe put some kind of pop-up info here when the object is hovered over
	}
	
	virtual protected void OnDrawGizmos()
	{		
		if (!DebugMode) { return; }
		
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
				Gizmos.DrawIcon(transform.position, "Gizmo_Active");
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
				Gizmos.DrawIcon(transform.position, "Gizmo_Inactive");
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
				Gizmos.DrawIcon(transform.position, "Gizmo_Disabled");
			
			break;
		}
		
		if (_clearConnections)
		{
			//Connections.Clear();
			//_clearConnections = false;
		}
		
		//OnEditGizmos();
	}
	
	virtual protected void OnPlayGizmos()
	{
		
	}
	
	virtual protected void OnEditGizmos()
	{
		
	}
	
	public void Activate(EditorObject caller)
	{
		OnActivate(caller);
	}
	
	//This EditorObject has been Activated
	virtual public void OnActivate(EditorObject caller)
	{
		Debug.Log("Heard from: " + caller.name);
		
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
			break;
		}
	}
	
	//This EditorObject has been Deactivated
	virtual public void OnDeactivate(EditorObject caller)
	{		
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
			break;
		}		
	}
	
	//This EditorObject has been Toggled
	virtual public void OnToggle(EditorObject caller)
	{
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
			break;
		}		
	}
	
	virtual public void OnEnabled(EditorObject caller)
	{
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
			break;
		}		
	}
	
	virtual public void OnDisabled(EditorObject caller)
	{
		switch(_state)
		{
			case EDITOROBJECTSTATES.ACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.INACTIVE:
			
			break;
			
			case EDITOROBJECTSTATES.DISABLED:
			
			break;
		}		
	}
	
	//Run through connections
//	virtual public void CallSubjects()
//	{
//		if (Connections == null || Connections.Count <= 0)
//		{
//			return;
//		}		
//		
//		foreach(EditorObjectConnection connection in Connections)
//		{			
//			connection.Subject.GetComponent<EditorObject>().Call(connection.Message, this); //call the subject with the message
//		}
//	}
	virtual public void CallSubjects()
	{
	}
	
	virtual public void Call(EditorObject.EditorObjectMessage message, EditorObject caller)
	{
		switch(message)
		{
			case EditorObjectMessage.Activate:
			
			break;
			
			case EditorObjectMessage.Deactivate:
			
			break;
			
			case EditorObjectMessage.Toggle:
			
			break;
			
			case EditorObjectMessage.Disable:
			
			break;
			
			case EditorObjectMessage.Enable:
			
			break;
			
			default:
			
				Debug.LogWarning(string.Format("Message: {0} from Editor Object {1} is unrecognized", message, caller), this);
			
			break;
		}		
	}
	
//	public void AddConnection(EditorObject subject)		
//	{
//		if (subject == null)
//		{
//			return;
//		}
//		
//		EditorObjectConnection newConnection = new EditorObjectConnection(EditorObject.EditorObjectMessage.None);		
//					
//		newConnection.Subject = subject;
//		newConnection.Caller = this;		
//		
//		EditorObjectConnection testConnection;
//		
//		for(int i = (Connections.Count - 1); i >= 0; i--)
//		{			
//			testConnection = Connections[i];
//			
//			if (testConnection.Subject != newConnection.Subject) //test for matching subjects
//			{
//				continue;
//			}
//			else if (testConnection.Message == newConnection.Message) //now test for matching message
//			{
//				LookingForSubject = false; //this connection already exists, let's stop			
//				return;
//			}
//			else
//			{
//				Connections.Remove(testConnection); //message changed, remove old connection
//			}
//		}	
//		
//		Connections.Add(newConnection);
//		subject.Connections.Add(newConnection);		
//		
//		LookingForSubject = false;			
//	}
	
//	public void RemoveConnection(EditorObjectConnection connection)
//	{
//		EditorObjectConnection testConnection;		
//		
//		if (connection == null || !Connections.Contains(connection)) 
//		{
//			Debug.LogWarning(string.Format("EditorObject {0} tried to remove a connection that didn't exist.", this.name), this);
//			return;
//		}
//		else
//		{			
//			
//			EditorObject editorObject = connection.Caller == this ? connection.Subject.GetComponent<EditorObject>() : connection.Caller.GetComponent<EditorObject>();
//			
//			for(int i = (editorObject.Connections.Count - 1); i >= 0; i--)
//			{				
//				testConnection = editorObject.Connections[i];
//			
//				if (testConnection.GUID != connection.GUID) //test for matching subjects
//				{
//					continue;
//				}
//				else
//				{					
//					editorObject.Connections.Remove(testConnection); //message matched, remove old connection
//					break;
//				}
//			}
//			
//			Connections.Remove(connection);
//		}		
//	}	
	
	//returns false if there are no more uses left
	private bool DecrementUses()
	{
		if (NumberOfUses > 0)
		{
			if (NumberOfUses == -1)
			{
				//do not decrement
				return true;
			}
			else
			{
				NumberOfUses--; //do decrement
				return true;
			}
		}
		else
		{
			return false; //no uses left
		}
	}
}//end class
