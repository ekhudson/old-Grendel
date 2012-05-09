using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class EditorObject : MonoBehaviour
{
	public bool DebugMode = false;
	public string Comment = "";
	public List<EditorObjectConnection> Connections = new List<EditorObjectConnection>();	
	
	protected Transform _transform;
	protected GameObject _gameObject;
	protected GUIStyle SelectedTextStyle = new GUIStyle();
	protected int _labelWidth = 128;
	protected int _labelHeight = 32;
	
	
	[HideInInspector]
	public bool SubjectActivateOpen = false;
	[HideInInspector]
	public bool SubjectDeactivateOpen = false;
	[HideInInspector]
	public bool SubjectToggleOpen = false;
	
	[HideInInspector]
	public bool MasterActivateOpen = false;
	[HideInInspector]
	public bool MasterDeactivateOpen = false;
	[HideInInspector]
	public bool MasterToggleOpen = false;
	
	virtual protected void Awake ()
	{
	
	}
	
	// Use this for initialization
	virtual protected void Start () 
	{
	
	}
	
	// Update is called once per frame
	virtual protected void Update () 
	{
	
	}	
	
	virtual public void OnSceneGUI()
	{		
		
		//Debug.Log("Scene view in : " + name);
		DrawSimpleLabel(Camera.main);
		
	}
	
	public void DrawSimpleLabel(Camera cameraToUse)
	{
		SelectedTextStyle = GUI.skin.button;
		SelectedTextStyle.normal.textColor = Color.yellow;
		Vector2 screenCoords = cameraToUse.WorldToScreenPoint(transform.position);
		screenCoords.y = cameraToUse.pixelHeight - screenCoords.y;
		
		if( GUI.Button(new Rect(screenCoords.x - (_labelWidth * 0.5f), screenCoords.y - (_labelHeight * 0.5f), _labelWidth, _labelHeight), name, SelectedTextStyle) )
		{
			Selection.activeGameObject = this.gameObject;
		}
	}
	
	virtual protected void OnDrawGizmos()
	{
		if (!DebugMode) { return; }
		//if (Connections.Count > 0){	DrawConnections(); }
		
	}
	
//	virtual protected void DrawConnections()
//	{		
//		foreach(EditorObjectConnection connection in Connections)
//		{			
//					
//			Gizmos.color = connection.ConnectionColor;
//			Gizmos.DrawLine(transform.position, connection.ConnectedEditorObject.transform.position);			
//		}		
//	}
	
	
}
