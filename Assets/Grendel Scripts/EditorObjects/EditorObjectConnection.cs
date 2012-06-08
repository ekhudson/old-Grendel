using UnityEngine;
using System.Collections;

[System.Serializable]
public class EditorObjectConnection
{	
	public enum CONNECTION_TYPE {SUBJECT_ACTIVATE, SUBJECT_DEACTIVATE, SUBJECT_TOGGLE, MASTER_ACTIVATE, MASTER_DEACTIVATE, MASTER_TOGGLE}
	public CONNECTION_TYPE ConnectionType;		
	public EditorObject ConnectedEditorObject;	
	//public float Delay = 0f; //probably going to use a node for this
	protected Color _connectionColor;
	protected Color _connectionColorDark;
	
	public Color ConnectionColor
	{
		get { return _connectionColor; }
	}
	
	public Color ConnectionColorDark
	{
		get { return _connectionColorDark; }
	}
	
	public EditorObjectConnection(CONNECTION_TYPE connectionType)
	{
		ConnectionType = connectionType;		
		SetColor();		
	}
	
	public void SetColor()
	{		
		switch(ConnectionType)
		{
				case CONNECTION_TYPE.SUBJECT_ACTIVATE:					
					_connectionColor =	Color.green;
					_connectionColorDark = GrendelColor.DarkGreen;
				break;
			
				case CONNECTION_TYPE.SUBJECT_DEACTIVATE:
					_connectionColor =	Color.red;
					_connectionColorDark = GrendelColor.DarkRed;
				break;
			
				case CONNECTION_TYPE.SUBJECT_TOGGLE:
					_connectionColor =	Color.yellow;
					_connectionColorDark = GrendelColor.DarkYellow;
				break;
			
				case CONNECTION_TYPE.MASTER_ACTIVATE:
					_connectionColor =	GrendelColor.DarkGreen; 
				break;
			
				case CONNECTION_TYPE.MASTER_DEACTIVATE:
					_connectionColor =	GrendelColor.DarkRed; 
				break;
			
				case CONNECTION_TYPE.MASTER_TOGGLE: 
					_connectionColor =	Color.yellow; //TODO: Change to dark colours
				break;
				
				default:					
					_connectionColor =	Color.grey; //TODO: Change to dark colours					
				break;
		}		
	}
}
