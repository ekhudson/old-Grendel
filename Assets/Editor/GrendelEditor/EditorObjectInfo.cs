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
	
//	protected EditorConnectionPoint _inActivatePoint;
//	protected EditorConnectionPoint _inDeactivatePoint;
//	protected EditorConnectionPoint _inTogglePoint;
//	protected EditorConnectionPoint _outActivatePoint;
//	protected EditorConnectionPoint _outDeactivatePoint;
//	protected EditorConnectionPoint _outTogglePoint;
	
	protected List<EditorObjectConnection> _subjectActivateList = new List<EditorObjectConnection>();
	protected List<EditorObjectConnection> _subjectDeactivateList = new List<EditorObjectConnection>();
	protected List<EditorObjectConnection> _subjectToggleList = new List<EditorObjectConnection>();
	protected List<EditorObjectConnection> _masterActivateList = new List<EditorObjectConnection>();
	protected List<EditorObjectConnection> _masterDeactivateList = new List<EditorObjectConnection>();
	protected List<EditorObjectConnection> _masterToggleList = new List<EditorObjectConnection>();

	protected bool _quickMenu = false;
	protected static EditorObject _currentHoveredObject;
	protected Camera _currentCamera;
	
	virtual public EditorObject Target
	{		
		get {return target as EditorObject;}		
	}
	
	virtual protected void OnEnable()
	{		
		if (!_target) { _target = target as EditorObject;}
		EditorMessenger.AddListener<EditorObject>("EditorObject Hovered", OnEditorObjectHover);
		CheckNameConflicts();
	}
	
	void OnDisable()
	{
		CheckNameConflicts();
	}	
	
	protected void OnEditorObjectHover(EditorObject eo)
    {
        //Debug.Log(_target.name + " Heard Hover over: " + eo.name);
		_currentHoveredObject = eo;
    }
	
	void CheckNameConflicts()
	{
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
				}
				
				if (objectList[i].name == objectList[i - 1].name)
				{
					objectList[i].NameConflict = true;				
				}
			}
			catch
			{
				//we expect errors with this
			}
		}
	}
	
	virtual protected void OnSceneGUI()
	{		
		_currentCamera = SceneView.currentDrawingSceneView.camera;
		SortConnections();
		GetInput();
		DrawInfo();
		if(_quickMenu){ DrawQuickMenu(); }
	}
	
	virtual protected void DrawInfo()
	{	
		
				
		Vector3 screenPos = new Vector3(0 + (_editorWindowWidth * 0.5f) + 32, Screen.height - _editorWindowHeight ,0);	
		
		Handles.BeginGUI();			
		//GUI.Box(new Rect(screenPos.x - ( (_editorWindowWidth * 0.5f) - 20) , screenPos.y - (_editorWindowHeight * 0.5f), _editorWindowWidth - 36, _editorWindowHeight), "", SelectedTextStyle);

		GUILayout.Window(0, new Rect(screenPos.x - ( (_editorWindowWidth * 0.5f)), screenPos.y - (_editorWindowHeight * 0.25f), Screen.width - ( (_editorWindowWidth * 0.5f)), _editorWindowHeight), InfoWindow, "", GUI.skin.box);		
			
		
//GUILayout.BeginArea(new Rect(screenPos.x - ( (_editorWindowWidth * 0.5f)), screenPos.y - (_editorWindowHeight * 0.5f), Screen.width, _editorWindowHeight), "");		
//		
//		GUILayout.BeginHorizontal();
//		
//		//GUILayout.BeginVertical();
//		
//			//DrawInConnectPoints();		
//		
//		//GUILayout.EndVertical();	
//		GUILayout.FlexibleSpace();
//		GUILayout.Window(GetInstanceID(),new Rect(screenPos.x - ( (_editorWindowWidth * 0.5f)), screenPos.y - (_editorWindowHeight * 0.5f) + 16, _editorWindowWidth, _editorWindowHeight), InfoWindow,_target.GetType().ToString(), GrendelCustomStyles.CustomElement(GUI.skin.window, Color.grey, Color.yellow), GUILayout.Width(_editorWindowWidth));
//		GUILayout.FlexibleSpace();
//		GUILayout.BeginVertical();
//		
//		//DrawOutConnectPoints();
//		
//		GUILayout.EndVertical();		
//		
//		GUILayout.EndHorizontal();		
//		
//		DrawConnectionBoxes();		
//		
//		GUILayout.EndArea();
		
//		GUILayout.BeginArea(new Rect(screenPos.x - ( (_editorWindowWidth * 0.5f)), screenPos.y - (_editorWindowHeight * 0.5f), _editorWindowWidth, _editorWindowHeight), "");
//		
//		GUILayout.BeginHorizontal();
//			GUILayout.FlexibleSpace();
//			GUILayout.BeginVertical();
//			
//				DrawInConnectPoints();
//			
//			GUILayout.EndVertical();		
//		
//			
//			GUILayout.BeginVertical();
//				
//				GUILayout.Label(_target.GetType().ToString(), GrendelCustomStyles.CustomElement(GUI.skin.label, Color.grey, Color.yellow, TextAnchor.MiddleCenter), GUILayout.Width(_editorWindowWidth - 44));
//				
//				EditorGUILayout.PrefixLabel("Name: ");
//				_target.name = GUILayout.TextField(_target.name, GUILayout.Width(_editorWindowWidth - 44));
//				EditorGUILayout.PrefixLabel("Comment: ");
//				_target.Comment = GUILayout.TextField(_target.Comment, 512, GUILayout.Width(_editorWindowWidth - 44));
//		
//			GUILayout.EndVertical();				
//		
//		
//			GUILayout.BeginVertical();
//			
//				DrawOutConnectPoints();
//			
//			GUILayout.EndVertical();		
//		
//		GUILayout.EndHorizontal();
//		GUILayout.FlexibleSpace();
//		GUILayout.EndArea();
//			
						
		Handles.EndGUI();		
						
		DrawConnectionLines();	
		
		DrawSearchLine();
		
		
		
		EditorUtility.SetDirty(_target);
	}
	
	virtual public void InfoWindow(int windowID)
	{		
		
		GUILayout.BeginVertical();
		
		GUILayout.BeginArea(new Rect(0,0,_editorWindowWidth, _editorWindowHeight), _target.GetType().ToString(),GrendelCustomStyles.CustomElement(GUI.skin.window, Color.white, Color.yellow,TextAnchor.MiddleCenter,FontStyle.Bold) );
						
		GUILayout.BeginHorizontal();
			GUILayout.Label("Name: ", GUILayout.Width(64));
			_target.name = GUILayout.TextField(_target.name);	
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();			
			GUILayout.Label("Comment: ", GUILayout.Width(64));			
			GUI.skin.textField.wordWrap = true;
			_target.Comment = GUILayout.TextField(_target.Comment, 512, GUI.skin.textField, GUILayout.Height(64));			
		GUILayout.EndHorizontal();		
		
		
		GUILayout.EndArea();
	
		GUILayout.EndVertical();
		
		DrawConnectionBoxes();			
	}
	
//	virtual protected void DrawInConnectPoints()
//	{		
//		_inActivatePoint = new EditorConnectionPoint();
//		_inActivatePoint.DrawConnectionPoint(new EditorObjectConnectionType.MasterActivate(), ref _target.MasterActivateOpen);		
//		
//		GUILayout.Space(8);
//		
//		_inDeactivatePoint = new EditorConnectionPoint();
//		_inDeactivatePoint.DrawConnectionPoint(new EditorObjectConnectionType.MasterDeactivate(), ref _target.MasterDeactivateOpen);
//		
//		GUILayout.Space(8);
//		
//		_inTogglePoint = new EditorConnectionPoint();
//		_inTogglePoint.DrawConnectionPoint(new EditorObjectConnectionType.MasterToggle(), ref _target.MasterToggleOpen);
//	}
//	
//	virtual protected void DrawOutConnectPoints()
//	{		
//		_outActivatePoint = new EditorConnectionPoint();
//		_outActivatePoint.DrawConnectionPoint(new EditorObjectConnectionType.SubjectActivate(), ref _target.SubjectActivateOpen);		
//		
//		GUILayout.Space(8);
//		
//		_outDeactivatePoint = new EditorConnectionPoint();
//		_outDeactivatePoint.DrawConnectionPoint(new EditorObjectConnectionType.SubjectDeactivate(), ref _target.SubjectDeactivateOpen);
//		
//		GUILayout.Space(8);
//		
//		_outTogglePoint = new EditorConnectionPoint();
//		_outTogglePoint.DrawConnectionPoint(new EditorObjectConnectionType.SubjectToggle(), ref _target.SubjectToggleOpen);
//	}
	
	void SortConnections()
	{
		_subjectActivateList.Clear();
		_subjectDeactivateList.Clear();
		_subjectToggleList.Clear();
		_masterActivateList.Clear();
		_masterDeactivateList.Clear();
		_masterToggleList.Clear();
		
		foreach(EditorObjectConnection connection in _target.Connections)
		{			
			if (connection.ConnectedEditorObject == null)
			{
				continue;
			}		
			
			connection.SetColor();					
			
			switch(connection.ConnectionType)
			{
				case EditorObjectConnection.CONNECTION_TYPE.MASTER_ACTIVATE:
				
				_masterActivateList.Add(connection);				
				
				break;
				
				case EditorObjectConnection.CONNECTION_TYPE.MASTER_DEACTIVATE:
				
				_masterDeactivateList.Add(connection);
				
				break;
				
				case EditorObjectConnection.CONNECTION_TYPE.MASTER_TOGGLE:
				
				_masterToggleList.Add(connection);
								
				break;
				
				case EditorObjectConnection.CONNECTION_TYPE.SUBJECT_ACTIVATE:	
				
				_subjectActivateList.Add(connection);
							
				break;
				
				case EditorObjectConnection.CONNECTION_TYPE.SUBJECT_DEACTIVATE:
				
				_subjectDeactivateList.Add(connection);
								
				break;
				
				case EditorObjectConnection.CONNECTION_TYPE.SUBJECT_TOGGLE:
				
				_subjectToggleList.Add(connection);
								
				break;				
			}			
		}
	}
	
	virtual protected void DrawConnectionLines()
	{	
		foreach(EditorObjectConnection connection in _target.Connections)
		{
			
			if (connection.ConnectedEditorObject == null)
			{
				continue;
			}
			else if (connection.ConnectedEditorObject.HighlightHighlight)
			{
				Handles.color = Color.cyan;				
				DrawConnectionHelper(Target.transform.position, connection.ConnectedEditorObject.transform.position);
				continue;
			}		
				
			Handles.color = Selection.activeGameObject.GetInstanceID() == _target.GetInstanceID() ? connection.ConnectionColorDark : connection.ConnectionColor;			
			
			switch(connection.ConnectionType)
			{
				case EditorObjectConnection.CONNECTION_TYPE.MASTER_ACTIVATE:				
				
				DrawConnectionHelper(Target.transform.position, connection.ConnectedEditorObject.transform.position);				
				//DrawConnectionHelper(sp, _inActivatePoint.Position, connection.ConnectedEditorObject.transform.position, connection.ConnectedEditorObject.OutPoint);
				
				break;
				
				case EditorObjectConnection.CONNECTION_TYPE.MASTER_DEACTIVATE:				
				
				DrawConnectionHelper(Target.transform.position, connection.ConnectedEditorObject.transform.position);
				//DrawConnectionHelper(sp, _inDeactivatePoint.Position, connection.ConnectedEditorObject.transform.position, connection.ConnectedEditorObject.OutPoint);
				
				break;
				
				case EditorObjectConnection.CONNECTION_TYPE.MASTER_TOGGLE:				
				
				DrawConnectionHelper(Target.transform.position, connection.ConnectedEditorObject.transform.position);
				//DrawConnectionHelper(sp, _inTogglePoint.Position, connection.ConnectedEditorObject.transform.position, connection.ConnectedEditorObject.OutPoint);
				
				break;
				
				case EditorObjectConnection.CONNECTION_TYPE.SUBJECT_ACTIVATE:				
				
				connection.ConnectedEditorObject.ActivateHighlight = true;
				DrawConnectionHelper(_target.transform.position, connection.ConnectedEditorObject.transform.position);				
				//DrawConnectionHelper(sp, _outActivatePoint.Position, connection.ConnectedEditorObject.transform.position, connection.ConnectedEditorObject.InPoint);			
				
				break;
				
				case EditorObjectConnection.CONNECTION_TYPE.SUBJECT_DEACTIVATE:				
				
				connection.ConnectedEditorObject.DeactivateHighlight = true;
				DrawConnectionHelper(Target.transform.position, connection.ConnectedEditorObject.transform.position);
				//DrawConnectionHelper(sp, _outDeactivatePoint.Position, connection.ConnectedEditorObject.transform.position, connection.ConnectedEditorObject.InPoint);
				
				break;
				
				case EditorObjectConnection.CONNECTION_TYPE.SUBJECT_TOGGLE:				
				
				connection.ConnectedEditorObject.ToggleHighlight = true;
				DrawConnectionHelper(Target.transform.position, connection.ConnectedEditorObject.transform.position);
				//DrawConnectionHelper(sp, _outTogglePoint.Position, connection.ConnectedEditorObject.transform.position, connection.ConnectedEditorObject.InPoint);
				
				break;				
			}//	
			
			
			
			//if (_highlightedObject != null){ DrawConnectionHelper(Target.transform.position, _highlightedObject.transform.position); }
			
			//_highlightedObject = null;
		}
		
	}

	void DrawConnectionBoxes()
	{
		
		Handles.BeginGUI();
		
		GUILayoutOption[] buttonSize = new GUILayoutOption[] { GUILayout.Width(64), GUILayout.Height(64) };
		
		GUILayout.BeginVertical();
		
			GUILayout.BeginHorizontal();
			
			Vector3 origin = Vector3.zero;
			origin.x += _editorWindowWidth;	
					
				
			foreach(EditorObjectConnection eoc in _subjectActivateList)
			{
				DrawConnectionBox(Color.green, eoc, origin);
				origin.x += 192;
			}
		
			GUI.color = Color.green;
			_target.SubjectActivateOpen = GUI.Toggle(new Rect(origin.x, origin.y, 64, 64), _target.SubjectActivateOpen, "Add", GrendelCustomStyles.CustomElement(GUI.skin.button, Color.green, Color.white, TextAnchor.MiddleCenter, FontStyle.Bold));			
			if (_target.SubjectActivateOpen)
			{
				_target.SubjectDeactivateOpen = false;
				_target.SubjectToggleOpen = false;
			}
		
		
			GUILayout.EndHorizontal();		
				
			origin.y += 64;
			origin.x = _editorWindowWidth;
		
			GUILayout.BeginHorizontal();			
			
			foreach(EditorObjectConnection eoc in _subjectDeactivateList)
			{
				DrawConnectionBox(Color.red, eoc, origin);
				origin.x += 192;
			}
		
			GUI.color = Color.red;
			_target.SubjectDeactivateOpen = GUI.Toggle(new Rect(origin.x, origin.y, 64, 64), _target.SubjectDeactivateOpen, "Add", GrendelCustomStyles.CustomElement(GUI.skin.button, Color.red, Color.white, TextAnchor.MiddleCenter, FontStyle.Bold));
			if (_target.SubjectDeactivateOpen)
			{
				_target.SubjectActivateOpen = false;
				_target.SubjectToggleOpen = false;
			}	
		
			GUILayout.EndHorizontal();
		
			origin.y += 64;
			origin.x = _editorWindowWidth;
		
			GUILayout.BeginHorizontal();		
			
			foreach(EditorObjectConnection eoc in _subjectToggleList)
			{
				DrawConnectionBox(Color.yellow, eoc, origin);
				origin.x += 192;
			}
		
			GUI.color = Color.yellow;
			_target.SubjectToggleOpen = GUI.Toggle(new Rect(origin.x, origin.y, 64, 64), _target.SubjectToggleOpen, "Add", GrendelCustomStyles.CustomElement(GUI.skin.button, Color.yellow, Color.white, TextAnchor.MiddleCenter, FontStyle.Bold));
			if (_target.SubjectToggleOpen)
			{
				_target.SubjectActivateOpen = false;
				_target.SubjectDeactivateOpen = false;
			}
			
		
			GUILayout.EndHorizontal();
		
		
		GUILayout.EndVertical();
		
		Handles.EndGUI();		
//	
//		Event e = Event.current;
//		Vector3 mousePos = Vector3.zero;
//		
//		mousePos = ScreenToWorld(e.mousePosition);
//		//mousePos.y = Screen.height - mousePos.y;	
//		//mousePos.z = 10f;
//		//mousePos = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePos);			
		
	}	
	
	void DrawConnectionBox(Color color, EditorObjectConnection editorObjectConnection, Vector3 origin)
	{	
		int connectionWidth = 192;
		GUI.color = color;		
		
		Rect boxRect = new Rect(origin.x, origin.y, 192, 64);
		
		Event e = Event.current;
		
		if (boxRect.Contains(e.mousePosition))
		{			
			editorObjectConnection.ConnectedEditorObject.HighlightHighlight = true;			
		}
		
		if (editorObjectConnection.ConnectedEditorObject.HighlightHighlight)
		{
			GUI.color = Color.cyan;			
		}
		
		GUILayout.BeginArea(boxRect, editorObjectConnection.ConnectionType.ToString(),GrendelCustomStyles.CustomElement(GUI.skin.window, color, Color.white,TextAnchor.UpperCenter));
		
		GUILayout.BeginVertical();
			
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
				GUILayout.Label(editorObjectConnection.ConnectedEditorObject.GetType().ToString(), GrendelCustomStyles.CustomElement(GUI.skin.label, color, Color.white,TextAnchor.MiddleLeft));
				GUILayout.Label(editorObjectConnection.ConnectedEditorObject.name, GrendelCustomStyles.CustomElement(GUI.skin.label, color, Color.white,TextAnchor.MiddleLeft));
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
		
				if( GUILayout.Button("Del", GrendelCustomStyles.CustomElement(GUI.skin.button, color, Color.white, TextAnchor.MiddleCenter), GUILayout.Width(32)) )
				{
					_target.Connections.Remove(	editorObjectConnection );
				}
		
				if( GUILayout.Button("Sel", GrendelCustomStyles.CustomElement(GUI.skin.button, Color.grey, Color.white, TextAnchor.MiddleCenter), GUILayout.Width(32)) )
				{
					Selection.activeGameObject = editorObjectConnection.ConnectedEditorObject.gameObject;	
				}
		
			GUILayout.EndVertical();
		
			GUILayout.EndHorizontal();
		
		GUILayout.EndVertical();
		GUILayout.EndArea();		
		
		GUI.color = Color.white;
	}
	
	void DrawConnectionHelper(Vector3 sourcePosition, Vector3 targetPosition)
	{		
		Handles.DrawLine( sourcePosition, targetPosition);		
	}
	
	void DrawSearchLine()
	{		
		//_objectChosen = false;
		
		Event e = Event.current;
		Vector3 mousePos = e.mousePosition;
		mousePos.y = _currentCamera.pixelHeight - mousePos.y;	
		mousePos.z = 10f;
		mousePos = _currentCamera.camera.ScreenToWorldPoint(mousePos);
		
		if (_target.SubjectActivateOpen) 
		{ 
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
			Handles.color = Color.green;
			DrawConnectionHelper(_target.transform.position, mousePos);
			
		}
		else if (_target.SubjectDeactivateOpen)
		{
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
			Handles.color = Color.red;
			DrawConnectionHelper(_target.transform.position, mousePos);
		}
		else if (_target.SubjectToggleOpen)
		{
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
			Handles.color = Color.yellow;
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
//			_target.SubjectDeactivateOpen = false;
//			_target.SubjectToggleOpen = false;
//			_target.SubjectActivateOpen = _target.SubjectActivateOpen ? false : true;
//		}
//		else if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.D))
//		{
//			_target.SubjectActivateOpen = false;
//			_target.SubjectToggleOpen = false;
//			_target.SubjectDeactivateOpen = _target.SubjectDeactivateOpen ? false : true;
//		}
//		else if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.S))
//		{
//			_target.SubjectDeactivateOpen = false;
//			_target.SubjectActivateOpen = false;
//			_target.SubjectToggleOpen = _target.SubjectToggleOpen ? false : true;
//		}
	}
	
	void DrawQuickMenu()
	{
		Event e = Event.current;
		
		Vector3 pos = _currentCamera.WorldToScreenPoint(_currentHoveredObject.transform.position);
		pos.y = _currentCamera.pixelHeight - pos.y;
				
		Handles.BeginGUI();
		
		GUI.Box(new Rect(pos.x, pos.y, 100, 100), "QUICK!");
		
		Handles.EndGUI();
	}
	
	
	void CloseOpenConnections()
	{
		_target.SubjectActivateOpen = false;
		_target.SubjectDeactivateOpen = false;
		_target.SubjectToggleOpen = false;
	}
	
	void ChooseObject()
	{		
		
		EditorObject eo = _currentHoveredObject;
		
//		if (eo == null)
//		{
//			return;
//		}
//		else if (_target.SubjectActivateOpen)
//		{
//			if (_subjectDeactivateList.Contains(eo)) { _subjectDeactivateList.Remove(eo); }
//			if (_subjectToggleList.Contains(eo)) { _subjectToggleList.Remove(eo); }
//			if (!_subjectActivateList.Contains(eo)) { _subjectActivateList.Add(eo); }
//		}
//		else if (_target.SubjectDeactivateOpen)
//		{
//			if (_subjectActivateList.Contains(eo)) { _subjectActivateList.Remove(eo); }
//			if (_subjectToggleList.Contains(eo)) { _subjectToggleList.Remove(eo); }
//			if (!_subjectDeactivateList.Contains(eo)) { _subjectDeactivateList.Add(eo); }
//		}
//		else if (_target.SubjectToggleOpen)
//		{
//			if (_subjectActivateList.Contains(eo)) { _subjectActivateList.Remove(eo); }
//			if (_subjectDeactivateList.Contains(eo)) { _subjectDeactivateList.Remove(eo); }
//			if (!_subjectToggleList.Contains(eo)) { _subjectToggleList.Add(eo); }
//		}		
		
		if (eo == null)
		{
			return;
		}
		
		EditorObjectConnection newConnection = new EditorObjectConnection(EditorObjectConnection.CONNECTION_TYPE.SUBJECT_ACTIVATE);
		
		if (_target.SubjectActivateOpen)
		{
			newConnection.ConnectionType = EditorObjectConnection.CONNECTION_TYPE.SUBJECT_ACTIVATE;					
		}
		else if (_target.SubjectDeactivateOpen)
		{
			newConnection = new EditorObjectConnection(EditorObjectConnection.CONNECTION_TYPE.SUBJECT_DEACTIVATE);			
		}
		else if (_target.SubjectToggleOpen)
		{
			newConnection = new EditorObjectConnection(EditorObjectConnection.CONNECTION_TYPE.SUBJECT_TOGGLE);
		}
		else
		{
			return;
		}
		
		newConnection.ConnectedEditorObject = eo;
		newConnection.SetColor();
		
		EditorObjectConnection testConnection;
		
		for(int i = (_subjectActivateList.Count - 1); i >= 0; i--)
		{			
			testConnection = _subjectActivateList[i];
			
			if (newConnection.ConnectionType == EditorObjectConnection.CONNECTION_TYPE.SUBJECT_ACTIVATE)
			{
				if(testConnection.ConnectedEditorObject.GetInstanceID() == eo.GetInstanceID())
				{
					return;
				}
				else
				{
					//nada
				}
			}
			else
			{
				if(testConnection.ConnectedEditorObject.GetInstanceID() == eo.GetInstanceID())
				{
					_target.Connections.Remove(testConnection);
				}
			}
		}
		
		for(int i = (_subjectDeactivateList.Count - 1); i >= 0; i--)
		{			
			
			testConnection = _subjectDeactivateList[i];
			
			if (newConnection.ConnectionType == EditorObjectConnection.CONNECTION_TYPE.SUBJECT_DEACTIVATE)
			{
				if(testConnection.ConnectedEditorObject.GetInstanceID() == eo.GetInstanceID())
				{
					return;
				}
				else
				{
					//nada
				}
			}
			else
			{
				if(testConnection.ConnectedEditorObject.GetInstanceID() == eo.GetInstanceID())
				{					
					_target.Connections.Remove(testConnection);
				}
			}
		}
		
		for(int i = (_subjectToggleList.Count - 1); i >= 0; i--)
		{			
			testConnection = _subjectToggleList[i];
			
			if (newConnection.ConnectionType == EditorObjectConnection.CONNECTION_TYPE.SUBJECT_TOGGLE)
			{
				if(testConnection.ConnectedEditorObject.GetInstanceID() == eo.GetInstanceID())
				{
					return;
				}
				else
				{
					//nada
				}
			}
			else
			{
				if(testConnection.ConnectedEditorObject.GetInstanceID() == eo.GetInstanceID())
				{
					 _target.Connections.Remove(testConnection);
				}
			}
		}		
		
		//_target.Connections.Add(newConnection);
		_target.Connections.Add(newConnection);
		
		_target.SubjectActivateOpen = false;
		_target.SubjectDeactivateOpen = false;
		_target.SubjectToggleOpen = false;
		_currentHoveredObject = null;
	}	

	[DrawGizmo (GizmoType.NotSelected | GizmoType.Selected | GizmoType.Pickable)]
	static void DrawGizmos(EditorObject eo, GizmoType gizmoType)
	{				
		if (Application.isPlaying) { return; }
		
		Camera currentCamera = SceneView.currentDrawingSceneView.camera;
		
		if (eo.ActivateHighlight) { Gizmos.DrawIcon(eo.transform.position, "Gizmo_Activate_Ring"); }
		if (eo.DeactivateHighlight) { Gizmos.DrawIcon(eo.transform.position, "Gizmo_Deactivate_Ring"); }
		if (eo.ToggleHighlight) { Gizmos.DrawIcon(eo.transform.position, "Gizmo_Toggle_Ring"); }
		if (eo.HighlightHighlight) { Gizmos.DrawIcon(eo.transform.position, "Gizmo_Cyan_Ring"); Gizmos.DrawIcon(eo.transform.position, "Gizmo_Fill"); }
						
		eo.ActivateHighlight = false;
		eo.DeactivateHighlight = false;
		eo.ToggleHighlight = false;
		eo.HighlightHighlight = false;
		
		_currentHoveredObject = null;
				
		//SceneView.lastActiveSceneView.wantsMouseMove = true;
		
		if (Selection.activeGameObject != eo.gameObject)
		{
			
			HandleUtility.Repaint();
			//Vector3 pos = currentCamera.WorldToScreenPoint(eo.transform.position);
			//pos.y = Screen.height - pos.y;
			Vector3 centre = eo.transform.position;
			Vector3 right = (centre + (currentCamera.transform.right * 0.4f));
			Vector3 centreConverted = currentCamera.WorldToScreenPoint(centre);
			Vector3 rightConverted = currentCamera.WorldToScreenPoint(right);
			float dist = Mathf.Abs(centreConverted.x - rightConverted.x);
			
			centreConverted.y = currentCamera.pixelHeight - centreConverted.y;			
			
			//Rect labelRect = new Rect(pos.x - 32, pos.y - 64, 64, 64);		
			
			Rect labelRect = new Rect(centreConverted.x - dist, centreConverted.y - dist, dist * 2, dist * 2);
			
			#if DEBUG
			Handles.BeginGUI();
			GUI.Label(labelRect, "", GUI.skin.box);
			Handles.EndGUI();
			#endif
			Event e = Event.current;
			
			if (labelRect.Contains(e.mousePosition))
			{			
				eo.HighlightHighlight = true;
				try
				{
					EditorMessenger.Broadcast< EditorObject > ( "EditorObject Hovered", eo );
				}
				catch
				{
					
				}
				
					foreach(EditorObjectConnection connection in eo.Connections)
					{
					
					if (connection.ConnectedEditorObject == null)
					{
						continue;
					}
					else if (connection.ConnectedEditorObject.HighlightHighlight)
					{
						Handles.color = Color.cyan;
						Handles.DrawLine(eo.transform.position, connection.ConnectedEditorObject.transform.position);
						continue;
					}	
					
					Handles.color = connection.ConnectionColorDark;			
					
						switch(connection.ConnectionType)
						{
							case EditorObjectConnection.CONNECTION_TYPE.SUBJECT_ACTIVATE:				
							
							Handles.DrawLine(eo.transform.position, connection.ConnectedEditorObject.transform.position);				
							
							break;
							
							case EditorObjectConnection.CONNECTION_TYPE.SUBJECT_DEACTIVATE:				
							
							Handles.DrawLine(eo.transform.position, connection.ConnectedEditorObject.transform.position);
							
							break;
							
							case EditorObjectConnection.CONNECTION_TYPE.SUBJECT_TOGGLE:				
							
							Handles.DrawLine(eo.transform.position, connection.ConnectedEditorObject.transform.position);
							
							break;				
						}		
					}				
			}
			
			Vector3 pos = eo.transform.position;
			pos += (currentCamera.transform.right) * 0.5f;
			pos += (currentCamera.transform.up) * 0.15f;							
			
			Handles.color = eo.NameConflict == true ? Color.red : Color.white;				
		
			Handles.Label(pos, eo.name, GrendelCustomStyles.CustomElement(GUI.skin.label, Color.white, Handles.color));		
		
		}
		else
		{
			Gizmos.DrawIcon(eo.transform.position, "Gizmo_Selected");
			Gizmos.DrawIcon(eo.transform.position, "Gizmo_White_Ring");
		}	
		
		Gizmos.DrawIcon(eo.transform.position, eo.GizmoName);
		
		//if (eo.SubjectActivateOpen) { Gizmos.DrawLine(eo.transform.position, Camera.current (e.mousePosition) ); }				
	}
}//end class
