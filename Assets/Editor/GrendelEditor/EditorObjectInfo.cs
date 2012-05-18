using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(EditorObject))]
[CanEditMultipleObjects] 
public class EditorObjectInfo<T> : Editor where T : class
{
	public static GUIStyle SelectedTextStyle = new GUIStyle();	
	
	protected EditorObject _target;
	protected int _editorWindowWidth = 256;
	protected int _editorWindowHeight = 128;	
	
	protected EditorConnectionPoint _inActivatePoint;
	protected EditorConnectionPoint _inDeactivatePoint;
	protected EditorConnectionPoint _inTogglePoint;
	protected EditorConnectionPoint _outActivatePoint;
	protected EditorConnectionPoint _outDeactivatePoint;
	protected EditorConnectionPoint _outTogglePoint;
	
	protected Vector3 panelPosition;
	
	virtual protected void OnEnable()
	{		
		if (!_target) { _target = target as EditorObject;}		
	}
	
	virtual protected void OnSceneGUI()
	{		
		DrawInfo();
		
		panelPosition = _target.transform.position;
		panelPosition.y += 2; //move panel above centre		
		
	}
	
	virtual protected void DrawInfo()
	{	
				
		SelectedTextStyle = GUI.skin.button;	
		Vector3 screenPos = SceneView.currentDrawingSceneView.camera.WorldToScreenPoint(panelPosition);
		screenPos.y = SceneView.currentDrawingSceneView.camera.pixelHeight - screenPos.y;
		
		SelectedTextStyle.normal.textColor = Color.yellow;
		SelectedTextStyle.alignment = TextAnchor.MiddleCenter;
		
		Handles.BeginGUI();			
		//GUI.Box(new Rect(screenPos.x - ( (_editorWindowWidth * 0.5f) - 20) , screenPos.y - (_editorWindowHeight * 0.5f), _editorWindowWidth - 36, _editorWindowHeight), "", SelectedTextStyle);
		GUILayout.BeginArea(new Rect(screenPos.x - ( (_editorWindowWidth * 0.5f)) - 20, screenPos.y - (_editorWindowHeight * 0.5f), _editorWindowWidth + 40, _editorWindowHeight), "");		
		
		GUILayout.BeginHorizontal();
		
		GUILayout.BeginVertical();
		
			DrawInConnectPoints();		
		
		GUILayout.EndVertical();	
		GUILayout.FlexibleSpace();
		GUILayout.Window(GetInstanceID(),new Rect(screenPos.x - ( (_editorWindowWidth * 0.5f)), screenPos.y - (_editorWindowHeight * 0.5f), _editorWindowWidth, _editorWindowHeight), InfoWindow,"", GUILayout.Width(_editorWindowWidth));
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		
		DrawOutConnectPoints();
		
		GUILayout.EndVertical();		
		
		GUILayout.EndHorizontal();
		
		GUILayout.EndArea();
		
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
		
		DrawConnections();		
		
		DestroyConnectPoints();
		
		EditorUtility.SetDirty(_target);
	}
	
	virtual public void InfoWindow(int windowID)
	{		
		GUILayout.BeginVertical();
			
			GUILayout.Label(_target.GetType().ToString(), GrendelCustomStyles.CustomElement(GUI.skin.label, Color.grey, Color.yellow, TextAnchor.MiddleCenter));				
		
			GUILayout.Label("Name: ");
			_target.name = GUILayout.TextField(_target.name);			
			GUILayout.Label("Comment: ");
			_target.Comment = GUILayout.TextField(_target.Comment, 512);
	
		GUILayout.EndVertical();				
	}
	
	virtual protected void DrawInConnectPoints()
	{		
		_inActivatePoint = new EditorConnectionPoint();
		_inActivatePoint.DrawConnectionPoint(new EditorObjectConnectionType.MasterActivate(), ref _target.MasterActivateOpen);		
		
		GUILayout.Space(8);
		
		_inDeactivatePoint = new EditorConnectionPoint();
		_inDeactivatePoint.DrawConnectionPoint(new EditorObjectConnectionType.MasterDeactivate(), ref _target.MasterDeactivateOpen);
		
		GUILayout.Space(8);
		
		_inTogglePoint = new EditorConnectionPoint();
		_inTogglePoint.DrawConnectionPoint(new EditorObjectConnectionType.MasterToggle(), ref _target.MasterToggleOpen);
	}
	
	virtual protected void DrawOutConnectPoints()
	{		
		_outActivatePoint = new EditorConnectionPoint();
		_outActivatePoint.DrawConnectionPoint(new EditorObjectConnectionType.SubjectActivate(), ref _target.SubjectActivateOpen);		
		
		GUILayout.Space(8);
		
		_outDeactivatePoint = new EditorConnectionPoint();
		_outDeactivatePoint.DrawConnectionPoint(new EditorObjectConnectionType.SubjectDeactivate(), ref _target.SubjectDeactivateOpen);
		
		GUILayout.Space(8);
		
		_outTogglePoint = new EditorConnectionPoint();
		_outTogglePoint.DrawConnectionPoint(new EditorObjectConnectionType.SubjectToggle(), ref _target.SubjectToggleOpen);
	}
	
	virtual protected void DrawConnections()
	{
		//get position of panel on screen
		Vector3 sp = SceneView.currentDrawingSceneView.camera.WorldToScreenPoint(panelPosition);
		sp.y = SceneView.currentDrawingSceneView.camera.pixelHeight - sp.y;
		sp.x -= (_editorWindowWidth * 0.5f);
		sp.y -= (_editorWindowHeight * 0.5f);	
		
		foreach(EditorObjectConnection connection in _target.Connections)
		{
			
			connection.SetColor();	
			Handles.color = connection.ConnectionColor;
			
			switch(connection.ConnectionType)
			{
				case EditorObjectConnection.CONNECTION_TYPE.MASTER_ACTIVATE:
				
				DrawConnectionHelper(sp, _inActivatePoint.Position, connection.ConnectedEditorObject.transform.position, connection.ConnectedEditorObject.OutPoint);
				
				break;
				
				case EditorObjectConnection.CONNECTION_TYPE.MASTER_DEACTIVATE:
				
				DrawConnectionHelper(sp, _inDeactivatePoint.Position, connection.ConnectedEditorObject.transform.position, connection.ConnectedEditorObject.OutPoint);
				
				break;
				
				case EditorObjectConnection.CONNECTION_TYPE.MASTER_TOGGLE:
				
				DrawConnectionHelper(sp, _inTogglePoint.Position, connection.ConnectedEditorObject.transform.position, connection.ConnectedEditorObject.OutPoint);
				
				break;
				
				case EditorObjectConnection.CONNECTION_TYPE.SUBJECT_ACTIVATE:					
				
				DrawConnectionHelper(sp, _outActivatePoint.Position, connection.ConnectedEditorObject.transform.position, connection.ConnectedEditorObject.InPoint);			
				
				break;
				
				case EditorObjectConnection.CONNECTION_TYPE.SUBJECT_DEACTIVATE:
				
				DrawConnectionHelper(sp, _outDeactivatePoint.Position, connection.ConnectedEditorObject.transform.position, connection.ConnectedEditorObject.InPoint);
				
				break;
				
				case EditorObjectConnection.CONNECTION_TYPE.SUBJECT_TOGGLE:
				
				DrawConnectionHelper(sp, _outTogglePoint.Position, connection.ConnectedEditorObject.transform.position, connection.ConnectedEditorObject.InPoint);
				
				break;				
			}			
		}		
		
		Handles.color = Color.green;
		Event e = Event.current;
		Vector3 mousePos = e.mousePosition;
		mousePos.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePos.y;	
		mousePos.z = 10f;
		mousePos = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePos);		
		
		if (_target.SubjectActivateOpen) { DrawConnectionHelper(sp, _outActivatePoint.Position, mousePos, Vector3.zero); }
	}
	
	void DrawConnectionHelper(Vector3 panelPosition, Vector3 pointPosition, Vector3 targetPosition, Vector3 targetConnectionPoint)
	{
		Vector3 pos = panelPosition + pointPosition;
		pos.y = SceneView.currentDrawingSceneView.camera.pixelHeight - pos.y;		
		pos = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(pos);
				
		targetPosition = SceneView.currentDrawingSceneView.camera.WorldToScreenPoint(targetPosition);
		targetPosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - targetPosition.y;
		
		Vector3 targetPos =  targetPosition + targetConnectionPoint;		
		targetPos.y = SceneView.currentDrawingSceneView.camera.pixelHeight - targetPos.y;
		targetPos = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(targetPos);
				
		Handles.DrawLine( pos, targetPos);
	}
	
	void DestroyConnectPoints()
	{
		DestroyImmediate(_inActivatePoint);
		DestroyImmediate(_inDeactivatePoint);
		DestroyImmediate(_inTogglePoint);
		DestroyImmediate(_outActivatePoint);
		DestroyImmediate(_outDeactivatePoint);
		DestroyImmediate(_outTogglePoint);
	}
}
