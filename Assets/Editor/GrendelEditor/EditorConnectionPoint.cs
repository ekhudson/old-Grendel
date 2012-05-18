using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorConnectionPoint : Object
{
	public Color ConnectionColor = Color.green;	
	public EditorObjectConnectionType ConnectionType = new EditorObjectConnectionType.MasterActivate();	
	
	protected float _width = 16;
	protected float _buffer = 16;
	protected GUIStyle _style;
	
	protected Rect _rect;
	
	public Vector3 Position
	{
		get
		{		
			return new Vector3(_rect.x, _rect.center.y, 0);			
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

	
	public void DrawConnectionPoint(EditorObjectConnectionType type, ref bool connectionBool)
	{
		_style = new GUIStyle(GUI.skin.button);	
		
		ConnectionType = type;
		
		GUI.color = type.ConnectionColor;		
		connectionBool = GUILayout.Toggle(connectionBool, "", _style, GUILayout.Width(_width));		
		GUI.color = Color.white;
		
		HandleUtility.Repaint();
		_rect = GUILayoutUtility.GetLastRect();			
	}
}
