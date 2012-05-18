using UnityEngine;
using System.Collections;

public class EditorObjectConnectionType
{
	public enum CONNECTION_TYPE {SUBJECT_ACTIVATE, SUBJECT_DEACTIVATE, SUBJECT_TOGGLE, MASTER_ACTIVATE, MASTER_DEACTIVATE, MASTER_TOGGLE}
	protected CONNECTION_TYPE _connectionType;
	protected Color _connectionColor;
	
	public CONNECTION_TYPE ConnectionType 
	{
		get { return _connectionType; }
		set { _connectionType = value; }
	}
	
	public Color ConnectionColor
	{
		get { return _connectionColor; }
		set { _connectionColor = value; }
	}
	
	public class SubjectActivate : EditorObjectConnectionType
	{
		public SubjectActivate()
		{
			_connectionType = EditorObjectConnectionType.CONNECTION_TYPE.SUBJECT_ACTIVATE;
			_connectionColor = Color.green;
		}		
	}
	
	public class SubjectDeactivate : EditorObjectConnectionType
	{
		public SubjectDeactivate()
		{
			_connectionType = EditorObjectConnectionType.CONNECTION_TYPE.SUBJECT_DEACTIVATE;
			_connectionColor = Color.red;
		}		
	}
	
	public class SubjectToggle : EditorObjectConnectionType
	{
		public SubjectToggle()
		{
			_connectionType = EditorObjectConnectionType.CONNECTION_TYPE.SUBJECT_TOGGLE;
			_connectionColor = Color.yellow;
		}		
	}
	
	public class MasterActivate : EditorObjectConnectionType
	{
		public MasterActivate()
		{
			_connectionType = EditorObjectConnectionType.CONNECTION_TYPE.MASTER_ACTIVATE;
			_connectionColor = Color.green;
		}		
	}
	
	public class MasterDeactivate : EditorObjectConnectionType
	{
		public MasterDeactivate()
		{
			_connectionType = EditorObjectConnectionType.CONNECTION_TYPE.MASTER_DEACTIVATE;
			_connectionColor = Color.red;
		}		
	}
	
	public class MasterToggle : EditorObjectConnectionType
	{
		public MasterToggle()
		{
			_connectionType = EditorObjectConnectionType.CONNECTION_TYPE.MASTER_TOGGLE;
			_connectionColor = Color.yellow;
		}		
	}
}
