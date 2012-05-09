using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorConnectionPoint : Object
{
	public Color ConnectionColor = Color.green;
	public EditorObjectConnection.CONNECTION_TYPE ConnectionType = EditorObjectConnection.CONNECTION_TYPE.MASTER_ACTIVATE;
	
	protected float _width = 16;
	protected float _buffer = 16;
	protected GUIStyle _style;
	
	protected Rect _rect;
	
	public Vector3 Position
	{
		get
		{		
			return new Vector3(_rect.x + _rect.width, _rect.center.y, 0);			
		}		
	}	
	
	public float Buffer
	{
		get{ return _buffer; }
	}
	
	public float Width
	{
		get{ return _width; }
	}
	
	public float Offset
	{
		get{ return ( _buffer + (_width * 0.5f) ); } 
	}
	
	public void DrawConnectionPoint(Color color, Vector3 pos)
	{
		_style = new GUIStyle(GUI.skin.button);	
		
		GUI.Button(new Rect(pos.x, pos.y, _width, _width), "", _style);		
	}
	
	public void DrawConnectionPoint(EditorObjectConnection.CONNECTION_TYPE type, ref bool connectionBool)
	{
		_style = new GUIStyle(GUI.skin.button);	
		
		ConnectionType = type;
		
		connectionBool = GUILayout.Toggle(connectionBool, "", _style, GUILayout.Width(_width));	
		
		HandleUtility.Repaint();
		_rect = GUILayoutUtility.GetLastRect();			
		
	}
}
