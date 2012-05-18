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
	
	public Color ConnectionColor
	{
		get { return _connectionColor; }
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
				break;
			
				case CONNECTION_TYPE.SUBJECT_DEACTIVATE:
					_connectionColor =	Color.red;
				break;
			
				case CONNECTION_TYPE.SUBJECT_TOGGLE:
					_connectionColor =	Color.yellow;
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
