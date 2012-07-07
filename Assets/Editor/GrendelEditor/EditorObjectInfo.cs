#define DEBUG
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(EditorObject))]
[CanEditMultipleObjects] 
public class EditorObjectInfo<T> : Editor where T : class
{	
	protected EditorObject _target;
	protected int _editorWindowWidth = 320;
	protected int _editorWindowHeight = 192;
	
	//protected List<EditorObjectConnection> _subjectActivateList = new List<EditorObjectConnection>();
	//protected List<EditorObjectConnection> _deactivateList = new List<EditorObjectConnection>();
	//protected List<EditorObjectConnection> _toggleList = new List<EditorObjectConnection>();
	//protected List<EditorObjectConnection> _ActivateList = new List<EditorObjectConnection>();
	//protected List<EditorObjectConnection> _deactivateList = new List<EditorObjectConnection>();
	//protected List<EditorObjectConnection> _toggleList = new List<EditorObjectConnection>();
	
	protected List<EditorObjectConnection> _activateList = new List<EditorObjectConnection>();
	protected List<EditorObjectConnection> _deactivateList = new List<EditorObjectConnection>();
	protected List<EditorObjectConnection> _toggleList = new List<EditorObjectConnection>();
	protected List<EditorObjectConnection> _enableList = new List<EditorObjectConnection>();
	protected List<EditorObjectConnection> _disableList = new List<EditorObjectConnection>();

	protected bool _quickMenu = false;
	protected static EditorObject _currentHoveredObject;
	protected Camera _currentCamera;
	protected int _toolbarInt;
	protected Dictionary<string, int> _nameConflictCounts = new Dictionary<string, int>();
	
	virtual public EditorObject Target
	{		
		get {return target as EditorObject;}		
	}
	
	virtual protected void OnEnable()
	{		
		if (!_target) { _target = target as EditorObject;}
		EditorMessenger.AddListener<EditorObject>("EditorObject Hovered", OnEditorObjectHover);
		CheckNameConflicts();
		//VerifyConnections();
		//EditorApplication.ExecuteMenuItem("Grendel/Show Toolbar");
	}
	
	void OnDisable()
	{
		CheckNameConflicts();
		EditorMessenger.Cleanup();
	}	
	
	protected void OnEditorObjectHover(EditorObject eo)
    {        
		EditorObject.CurrentHoveredEditorObject = eo;
    }
	
	void CheckNameConflicts()
	{
		_nameConflictCounts.Clear();
		
		EditorObject[] editorObjects = GameObject.FindObjectsOfType(typeof(EditorObject)) as EditorObject[];
		
		List<EditorObject> objectList = new List<EditorObject>(editorObjects);
		
		ObjectComparer comparer = new ObjectComparer();
		
		objectList.Sort(comparer);
		
		for (int i = 0; i < (objectList.Count); i++)
		{			
		
			if (objectList[i] == null) { continue; }
			
			objectList[i].NameConflict = false;	
			
			try
			{
				if (objectList[i].name == objectList[i + 1].name)
				{
					objectList[i].NameConflict = true;
					
					if (_nameConflictCounts.ContainsKey(objectList[i].name)){ _nameConflictCounts[objectList[i].name]++; }
					else{ _nameConflictCounts.Add(objectList[i].name, 1); }					
				
					continue;
				}
			}
			catch
			{
				
			}
			
			try
			{
				if (objectList[i].name == objectList[i - 1].name)
				{
					objectList[i].NameConflict = true;
						
					if (_nameConflictCounts.ContainsKey(objectList[i].name)){ _nameConflictCounts[objectList[i].name]++; }
					else{ _nameConflictCounts.Add(objectList[i].name, 1); }
							
					continue;
				}
			}
			catch
			{
				//we expect errors with this
			}
		}		
	}
	
	//clean up any bogus connections
	void VerifyConnections()
	{
		if (_target.Connections.Count <= 0)
		{
			return;
		}
		
		EditorObjectConnection connection = new EditorObjectConnection(EditorObject.EditorObjectMessage.Activate);
		
		for (int i = (_target.Connections.Count - 1); i >= 0; i--)
		{			
			connection = _target.Connections[i];			
			if (connection.Subject == null || connection == null)
			{
				_target.Connections.Remove(connection);
			}
		}
	}
	
	virtual protected void OnSceneGUI()
	{		
		//EditorObject.CurrentHoveredEditorObject = null;	
		_currentCamera = SceneView.currentDrawingSceneView.camera;
		//SortConnections();
		if(_target.LookingForSubject){ GetInput(); }
		DrawToolbar();
		DrawInfo();
		//if(_quickMenu){ DrawQuickMenu(); }		
	}
	
	private void DrawToolbar()
	{
		_toolbarInt = GUI.Toolbar(new Rect(_editorWindowWidth + 16, (Screen.height - _editorWindowHeight) - 80, 256, 32), _toolbarInt, new string[]{ "Subjects", "Masters"}, GUI.skin.button); 
	}
	
	virtual protected void DrawInfo()
	{				
		Vector3 screenPos = new Vector3(16, (Screen.height - _editorWindowHeight) - 32 ,0);	
//		
		Handles.BeginGUI();			
//
		GUILayout.Window(0, new Rect(screenPos.x, screenPos.y, Screen.width, _editorWindowHeight), InfoWindow, "", GUI.skin.box);		
//
		Handles.EndGUI();		
//						
		DrawConnectionLines();	
//		
		DrawSearchLine();		
//		
//		EditorUtility.SetDirty(_target);
	}
	
	virtual public void InfoWindow(int windowID)
	{		
		SerializedObject sObject = new SerializedObject (_target);
		SerializedProperty comment = sObject.FindProperty("Comment");
		string nameForTest = _target.name;
		
		int nameConflicts = _nameConflictCounts.ContainsKey(_target.name) ? _nameConflictCounts[_target.name] : 0;
		Color nameColor = _target.NameConflict == true ? Color.red : Color.white;
				
				
		GUILayout.BeginVertical();
		
		GUILayout.BeginArea(new Rect(0,0,_editorWindowWidth, _editorWindowHeight), _target.GetType().ToString(),GrendelCustomStyles.CustomElement(GUI.skin.window, Color.white, Color.yellow,TextAnchor.MiddleCenter,FontStyle.Bold) );
						
		GUILayout.BeginHorizontal();
		
			GUILayout.Label(new GUIContent(string.Format("Name: {0}", nameConflicts <= 0 ? "" : nameConflicts.ToString()),
							nameConflicts <= 0 ? "" : string.Format("There are {0} name conflicts.", nameConflicts.ToString())),
							GrendelCustomStyles.CustomElement(GUI.skin.label, Color.white, nameColor), GUILayout.Width(64) );
		
			GUI.skin.textField.focused.textColor = nameColor;
			_target.name = GUILayout.TextField(_target.name, GrendelCustomStyles.CustomElement(GUI.skin.label, Color.white, nameColor));	
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();			
			GUILayout.Label("Comment: ", GUILayout.Width(64));			
			GUI.skin.textField.wordWrap = true;
			comment.stringValue = GUILayout.TextField(comment.stringValue, 512, GUI.skin.textField, GUILayout.Height(64));		
		GUILayout.EndHorizontal();		
		
		GUILayout.EndArea();
	
		GUILayout.EndVertical();
		
		sObject.ApplyModifiedProperties();
		
		if (_target.name != nameForTest || _nameConflictCounts == null)
		{
			CheckNameConflicts();
		}
		
    	DrawConnectionBoxes();			
	}
	
	void SortConnections()
	{		
		_activateList.Clear();
		_deactivateList.Clear();
		_toggleList.Clear();
		_enableList.Clear();
		_disableList.Clear();
		
		foreach(EditorObjectConnection connection in _target.Connections)
		{			
			if (connection.Subject == null)
			{
				continue;
			}		
			
			connection.SetColor();					
			
			switch(connection.Message)
			{
				case EditorObject.EditorObjectMessage.Activate:
				
					_activateList.Add(connection);				
				
				break;
				
				case EditorObject.EditorObjectMessage.Deactivate:
				
					_deactivateList.Add(connection);
				
				break;
				
				case EditorObject.EditorObjectMessage.Toggle:
				
					_toggleList.Add(connection);
								
				break;
				
				case EditorObject.EditorObjectMessage.Enable:	
				
					_enableList.Add(connection);
							
				break;
				
				case EditorObject.EditorObjectMessage.Disable:
				
					_disableList.Add(connection);
								
				break;
				
				default:				
								
				break;				
			}			
		}
	}
	
	virtual protected void DrawConnectionLines()
	{	
		foreach(EditorObjectConnection connection in _target.Connections)
		{
			
			if (connection.Subject == null)
			{
				continue;
			}
			else if (connection.Subject.HighlightHighlight)
			{
				Handles.color = Color.cyan;				
				DrawConnectionHelper(Target.transform.position, connection.Subject.transform.position);
				continue;
			}		
			
			connection.SetColor();
			
			Handles.color = connection.Caller != _target ? connection.MessageColorDark : connection.MessageColor;			
			
			switch(connection.Message)
			{
				case EditorObject.EditorObjectMessage.None:				
				
				DrawConnectionHelper(connection.Caller.transform.position, connection.Subject.transform.position);
				
				break;	
			
				case EditorObject.EditorObjectMessage.Activate:				
				
				connection.Subject.ActivateHighlight = true;
				DrawConnectionHelper(connection.Caller.transform.position, connection.Subject.transform.position);
				
				break;
				
				case EditorObject.EditorObjectMessage.Deactivate:				
				
				connection.Subject.DeactivateHighlight = true;
				DrawConnectionHelper(connection.Caller.transform.position, connection.Subject.transform.position);
				
				break;
				
				case EditorObject.EditorObjectMessage.Toggle:				
				
				connection.Subject.ToggleHighlight = true;
				DrawConnectionHelper(connection.Caller.transform.position, connection.Subject.transform.position);
				
				break;
				
				case EditorObject.EditorObjectMessage.Enable:				
				
				connection.Subject.ActivateHighlight = true;
				DrawConnectionHelper(connection.Caller.transform.position, connection.Subject.transform.position);	
				
				break;
				
				case EditorObject.EditorObjectMessage.Disable:				
				
				connection.Subject.DeactivateHighlight = true;
				DrawConnectionHelper(connection.Caller.transform.position, connection.Subject.transform.position);
				
				break;
				
				default:				
								
				break;				
			}
		}		
	}

	void DrawConnectionBoxes()
	{
				
		GUILayoutOption[] buttonSize = new GUILayoutOption[] { GUILayout.Width(64), GUILayout.Height(64) };
		
		GUILayout.BeginVertical();
		
			GUILayout.BeginHorizontal();
			
			Vector3 origin = Vector3.zero;
			origin.x += _editorWindowWidth;						
				
			
			for (int i = (_target.Connections.Count - 1); i >= 0; i--)
			{							
				if (_target.Connections[i] == null)
				{
					continue;
				}
			
				DrawConnectionBox(_target.Connections[i].Caller != _target ? _target.Connections[i].MessageColorDark : _target.Connections[i].MessageColor, 
								  _target.Connections[i], origin);
				origin.x += 192;		
			}
		
			GUI.color = Color.green;
			
			if (_toolbarInt == 0)
			{			
				_target.LookingForSubject = GUI.Toggle(new Rect(origin.x, origin.y, 64, 64), _target.LookingForSubject, "Add", GrendelCustomStyles.CustomElement(GUI.skin.button, Color.green, Color.white, TextAnchor.MiddleCenter, FontStyle.Bold));			
			}		
		
			GUILayout.EndHorizontal();		
//				
//			origin.y += 64;
//			origin.x = _editorWindowWidth;
//		
//			GUILayout.BeginHorizontal();			
//			
//			foreach(EditorObjectConnection eoc in _toolbarInt == 0 ? _activateList : _activateList)
//			{
//				DrawConnectionBox(Color.red, eoc, origin);
//				origin.x += 192;
//			}
//		
//			GUI.color = Color.red;
		
//			if (_toolbarInt == 0)
//			{
//				_target.LookingForSubject = GUI.Toggle(new Rect(origin.x, origin.y, 64, 64), _target.LookingForSubject, "Add", GrendelCustomStyles.CustomElement(GUI.skin.button, Color.red, Color.white, TextAnchor.MiddleCenter, FontStyle.Bold));
//			}
//		
//			if (_target.LookingForSubject)
//			{
//				_target.LookingForSubject = false;
//				_target.LookingForSubject = false;
//			}	
		
//			GUILayout.EndHorizontal();
		
//			origin.y += 64;
//			origin.x = _editorWindowWidth;
//		
//			GUILayout.BeginHorizontal();		
//			
//			foreach(EditorObjectConnection eoc in _toolbarInt == 0 ? _toggleList : _toggleList)
//			{
//				DrawConnectionBox(Color.yellow, eoc, origin);
//				origin.x += 192;
//			}
//		
//			GUI.color = Color.yellow;
				
//			if (_toolbarInt == 0)
//			{
//				_target.LookingForSubject = GUI.Toggle(new Rect(origin.x, origin.y, 64, 64), _target.LookingForSubject, "Add", GrendelCustomStyles.CustomElement(GUI.skin.button, Color.yellow, Color.white, TextAnchor.MiddleCenter, FontStyle.Bold));
//			}
//			
//			if (_target.LookingForSubject)
//			{
//				_target.LookingForSubject = false;
//				_target.LookingForSubject = false;
//			}			
		
//			GUILayout.EndHorizontal();		
		
		GUILayout.EndVertical();		

	}	
	
	void DrawConnectionBox(Color color, EditorObjectConnection editorObjectConnection, Vector3 origin)
	{	
		int connectionWidth = 192;
		GUI.color = color;		
		
		Rect boxRect = new Rect(origin.x, origin.y, 192, 64);
		
		Event e = Event.current;
		
		if (boxRect.Contains(e.mousePosition))
		{			
			editorObjectConnection.Subject.HighlightHighlight = true;			
		}
		
		if (editorObjectConnection.Subject.HighlightHighlight)
		{
			GUI.color = Color.cyan;			
		}
		
		GUILayout.BeginArea(boxRect, GrendelCustomStyles.CustomElement(GUI.skin.textArea, color, Color.white,TextAnchor.UpperCenter));
		
		GUILayout.BeginVertical();
		
			editorObjectConnection.Message = (EditorObject.EditorObjectMessage)EditorGUILayout.EnumPopup(editorObjectConnection.Message);
			
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
				GUILayout.Label(editorObjectConnection.Subject.GetType().ToString(), GrendelCustomStyles.CustomElement(GUI.skin.label, color, Color.white,TextAnchor.MiddleLeft));
				GUILayout.Label(editorObjectConnection.Subject.name, GrendelCustomStyles.CustomElement(GUI.skin.label, color, Color.white,TextAnchor.MiddleLeft));
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();		
				
				if( GUILayout.Button("Del", GrendelCustomStyles.CustomElement(GUI.skin.button, color, Color.white, TextAnchor.MiddleCenter), GUILayout.Width(32)) )
				{					
					_target.RemoveConnection(editorObjectConnection);
				}
		
				if( GUILayout.Button("Sel", GrendelCustomStyles.CustomElement(GUI.skin.button, Color.grey, Color.white, TextAnchor.MiddleCenter), GUILayout.Width(32)) )
				{
					Selection.activeGameObject = editorObjectConnection.Subject.gameObject;	
				}
		
			GUILayout.EndVertical();
		
			GUILayout.EndHorizontal();
		
		GUILayout.EndVertical();
		GUILayout.EndArea();		
		
		GUI.color = Color.white;
	}
	
//	void RemoveConnection(EditorObjectConnection myConnection)
//	{
//		EditorObjectConnection subjectConnection = new EditorObjectConnection(EditorObject.EditorObjectMessage.Activate);
//		
//		foreach(EditorObjectConnection connection in myConnection.Subject.Connections)
//		{
//			if (connection.Subject == _target)
//			{
//				subjectConnection = connection;
//				break;
//			}
//		}
//		
//		Debug.Log("removing connection from " + myConnection.Subject.name);
//		myConnection.Subject.Connections.Remove( subjectConnection );
//		_target.Connections.Remove(	myConnection );	
//				
//	}
	
	public void DrawConnectionHelper(Vector3 sourcePosition, Vector3 targetPosition)
	{		
		//reverse if we're drawing the backwards connection
		if (targetPosition == Target.transform.position)
		{			
			targetPosition = sourcePosition;
			sourcePosition = Target.transform.position;
		}
		
		float arrowSize = 0.5f;
		Vector3 direction = (sourcePosition - targetPosition).normalized;		
		Quaternion rotation = Quaternion.LookRotation(direction);
		rotation *= Quaternion.Euler(0, 180, 0);		
		Handles.ArrowCap(0, targetPosition + (( direction * arrowSize) / 0.5f), rotation, arrowSize);	
		Handles.DrawLine( sourcePosition,  targetPosition + (( direction * arrowSize) / 0.5f));
	}
	
	void DrawSearchLine()
	{			
		Event e = Event.current;
		Vector3 mousePos = e.mousePosition;
		mousePos.y = _currentCamera.pixelHeight - mousePos.y;	
		mousePos.z = 10f;
		mousePos = _currentCamera.camera.ScreenToWorldPoint(mousePos);
		
		if (_target.LookingForSubject) 
		{ 
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
			Handles.color = Color.green;
			DrawConnectionHelper(_target.transform.position, mousePos);
			
		}		
	}
	
	void GetInput()
	{		
		Event e = Event.current;
		
		if (e.button == 0 && e.type == EventType.MouseDown)			    
		{			
			ChooseObject();
		}
		else if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Escape))
		{
			CloseOpenConnections();
		}
//		else if (e.button == 1 && e.type == EventType.MouseDown && _currentHoveredObject != null)
//		{			
//			_quickMenu = true;
//		}
//		else if(e.button == 1 && e.type == EventType.MouseUp)
//		{
//			_quickMenu = false;
//		}
//		else if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.A))
//		{
//			_target.LookingForSubject = false;
//			_target.LookingForSubject = false;
//			_target.LookingForSubject = _target.LookingForSubject ? false : true;
//		}
//		else if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.D))
//		{
//			_target.LookingForSubject = false;
//			_target.LookingForSubject = false;
//			_target.LookingForSubject = _target.LookingForSubject ? false : true;
//		}
//		else if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.S))
//		{
//			_target.LookingForSubject = false;
//			_target.LookingForSubject = false;
//			_target.LookingForSubject = _target.LookingForSubject ? false : true;
//		}
	}
	
	void DrawQuickMenu()
	{
//		Event e = Event.current;
//		
//		Vector3 pos = _currentCamera.WorldToScreenPoint(_currentHoveredObject.transform.position);
//		pos.y = _currentCamera.pixelHeight - pos.y;
//				
//		Handles.BeginGUI();
//		
//		GUI.Box(new Rect(pos.x, pos.y, 100, 100), "QUICK!");
//		
//		Handles.EndGUI();
	}
	
	
	void CloseOpenConnections()
	{		
		_target.LookingForSubject = false;		
	}
	
	void ChooseObject()
	{		
		if (EditorObject.CurrentHoveredEditorObject == null)
		{
			return;
		}
		_target.AddConnection(EditorObject.CurrentHoveredEditorObject);
		EditorObject.CurrentHoveredEditorObject = null;
		
//		EditorObject eo = _currentHoveredObject;
//		
//		if (eo == null)
//		{
//			return;
//		}
//		
//		EditorObjectConnection newConnection = new EditorObjectConnection(EditorObject.EditorObjectMessage.Activate);
//		EditorObjectConnection masterConnection = new EditorObjectConnection(EditorObject.EditorObjectMessage.Activate);
//		
//		if (_target.LookingForSubject)
//		{
//			newConnection.Message = EditorObject.EditorObjectMessage.Activate;
//			masterConnection.Message = EditorObject.EditorObjectMessage.Activate;
//		}
//		else if (_target.LookingForSubject)
//		{
//			newConnection = new EditorObjectConnection(EditorObject.EditorObjectMessage.Deactivate);
//			masterConnection.Message = EditorObject.EditorObjectMessage.Deactivate;
//		}
//		else if (_target.LookingForSubject)
//		{
//			newConnection = new EditorObjectConnection(EditorObject.EditorObjectMessage.Toggle);
//			masterConnection.Message = EditorObject.EditorObjectMessage.Toggle;
//		}
//		else
//		{
//			return;
//		}
//		
//		newConnection.Subject = eo;
//		newConnection.SetColor();
//		masterConnection.Subject = _target;
//		masterConnection.SetColor();
//		
//		EditorObjectConnection testConnection;
//		
//		for(int i = (_activateList.Count - 1); i >= 0; i--)
//		{			
//			testConnection = _activateList[i];
//			
//			if (newConnection.Message == EditorObject.EditorObjectMessage.Activate)
//			{
//				if(testConnection.Subject.GetInstanceID() == eo.GetInstanceID())
//				{
//					return;
//				}
//				else
//				{
//					//nada
//				}
//			}
//			else
//			{
//				if(testConnection.Subject.GetInstanceID() == eo.GetInstanceID())
//				{	
//					RemoveConnection(testConnection);
//				}
//			}
//		}
//		
//		for(int i = (_deactivateList.Count - 1); i >= 0; i--)
//		{			
//			
//			testConnection = _deactivateList[i];
//			
//			if (newConnection.Message == EditorObject.EditorObjectMessage.Deactivate)
//			{
//				if(testConnection.Subject.GetInstanceID() == eo.GetInstanceID())
//				{
//					return;
//				}
//				else
//				{
//					//nada
//				}
//			}
//			else
//			{
//				if(testConnection.Subject.GetInstanceID() == eo.GetInstanceID())
//				{					
//					RemoveConnection(testConnection);
//				}
//			}
//		}
//		
//		for(int i = (_toggleList.Count - 1); i >= 0; i--)
//		{			
//			testConnection = _toggleList[i];
//			
//			if (newConnection.Message == EditorObject.EditorObjectMessage.Toggle)
//			{
//				if(testConnection.Subject.GetInstanceID() == eo.GetInstanceID())
//				{
//					return;
//				}
//				else
//				{
//					//nada
//				}
//			}
//			else
//			{
//				if(testConnection.Subject.GetInstanceID() == eo.GetInstanceID())
//				{					
//					RemoveConnection(testConnection);
//				}
//			}
//		}		
//		
//		_target.Connections.Add(newConnection);
//		eo.Connections.Add(masterConnection);	
//		
//		_target.LookingForSubject = false;
//		_currentHoveredObject = null;
	}	

	[DrawGizmo (GizmoType.NotSelected | GizmoType.Selected | GizmoType.Pickable)]
	static void DrawGizmos(EditorObject eo, GizmoType gizmoType)
	{	
		if (!Application.isPlaying)
		{			
			if (SceneView.currentDrawingSceneView.camera == null)
			{
				return;
			}		
			 
			Camera currentCamera = SceneView.currentDrawingSceneView.camera;
			
			if (eo.ActivateHighlight) { Gizmos.DrawIcon(eo.transform.position, "Gizmo_Activate_Ring"); }
			if (eo.DeactivateHighlight) { Gizmos.DrawIcon(eo.transform.position, "Gizmo_Deactivate_Ring"); }
			if (eo.ToggleHighlight) { Gizmos.DrawIcon(eo.transform.position, "Gizmo_Toggle_Ring"); }
			//if (eo.HighlightHighlight) { Gizmos.DrawIcon(eo.transform.position, "Gizmo_Cyan_Ring"); Gizmos.DrawIcon(eo.transform.position, "Gizmo_Fill"); }
							
			eo.ActivateHighlight = false;
			eo.DeactivateHighlight = false;
			eo.ToggleHighlight = false;					
			
			if (Selection.activeGameObject != eo.gameObject)
			{				
				Vector3 centre = eo.transform.position;
				Vector3 right = (centre + (currentCamera.transform.right * 0.4f));
				Vector3 centreConverted = currentCamera.WorldToScreenPoint(centre);
				Vector3 rightConverted = currentCamera.WorldToScreenPoint(right);
				float dist = Mathf.Abs(centreConverted.x - rightConverted.x);
				
				centreConverted.y = currentCamera.pixelHeight - centreConverted.y;			
				
				Rect labelRect = new Rect(centreConverted.x - dist, centreConverted.y - dist, dist * 2, dist * 2);				
				EditorGUIUtility.AddCursorRect(labelRect, MouseCursor.Link);
				
				#if DEBUG
				Handles.BeginGUI();
					GUI.Label(labelRect, "", GUI.skin.box);
				Handles.EndGUI();
				#endif
				Event e = Event.current;
				
				
				if (labelRect.Contains(e.mousePosition))
				{					
					eo.HighlightHighlight = true;
					
//					try
//					{
//						EditorMessenger.Broadcast< EditorObject > ( "EditorObject Hovered", eo );
//					}
//					catch
//					{
//						
//					}				
						foreach(EditorObjectConnection connection in eo.Connections)
						{
						
						if (connection.Subject == null)
						{
							continue;
						}
						else if (connection.Subject.HighlightHighlight)
						{
							Handles.color = Color.cyan;
							Handles.DrawLine(eo.transform.position, connection.Subject.transform.position);
							continue;
						}	
						
						Handles.color = GrendelColor.FlashingColor(connection.MessageColorDark, 4f);			
						
							switch(connection.Message)
							{
								case EditorObject.EditorObjectMessage.Activate:				
								
								Handles.DrawLine(eo.transform.position, connection.Subject.transform.position);	
								//DrawConnectionHelper(eo.transform.position, connection.Subject.transform.position);
								
								break;
								
								case EditorObject.EditorObjectMessage.Deactivate:				
								
								Handles.DrawLine(eo.transform.position, connection.Subject.transform.position);
								//DrawConnectionHelper(eo.transform.position, connection.Subject.transform.position);
								
								break;
								
								case EditorObject.EditorObjectMessage.Toggle:				
								
								Handles.DrawLine(eo.transform.position, connection.Subject.transform.position);
								//DrawConnectionHelper(eo.transform.position, connection.Subject.transform.position);
								
								break;
							
								default:
							
								break;
							}		
						}
					
					Gizmos.DrawIcon(eo.transform.position, "Gizmo_Cyan_Ring"); 
					Gizmos.DrawIcon(eo.transform.position, "Gizmo_Fill");
					EditorObject.CurrentHoveredEditorObject = eo;
				}
				else
				{
					eo.HighlightHighlight = false;						
				}
				
				Vector3 pos = eo.transform.position;
				pos += (currentCamera.transform.right) * 0.5f;
				pos += (currentCamera.transform.up) * 0.15f;							
				
				Handles.color = eo.NameConflict == true ? Color.red : Color.white;				
			
				Handles.Label(pos, eo.name, GrendelCustomStyles.CustomElement(GUI.skin.label, Color.white, Handles.color));		
			
			}
			else
			{
				//eo.LookingForSubject = false;
				eo.HighlightHighlight = false;
				Gizmos.DrawIcon(eo.transform.position, "Gizmo_Selected");
				Gizmos.DrawIcon(eo.transform.position, "Gizmo_White_Ring");
			}			
			
		}//end Application.isPlaying check		
		
		Gizmos.DrawIcon(eo.transform.position, eo.GizmoName);
		
//		try
//		{
//			if (SceneView.mouseOverWindow.GetInstanceID() == SceneView.currentDrawingSceneView.GetInstanceID()) 
//			{				
//				SceneView.RepaintAll(); 
//			}
//		}
//		catch
//		{
//			//mouse is out of scene view, no biggie
//		}
	}
}//end class
