using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UserInput : Singleton<UserInput> 
{
	
	public float MouseSensitivityVertical = 1f;
	public float MouseSensitivityHorizontal = 1f;	
	
	private List<KeyCode> _blockedKeys = new List<KeyCode>();
	
	public List<KeyCode> BlockedKeys
	{
		get
		{
			return _blockedKeys;
		}
	}		
	
	public void SetBlocking(KeyCode key, bool isBlocked)
	{
		if (_blockedKeys.Contains(key) && !isBlocked)
		{
			_blockedKeys.Remove(key); 
		}
		else if (!_blockedKeys.Contains(key) && isBlocked)
		{
			_blockedKeys.Add(key);
		}
	}
	
	public void SetBlocking(KeyCode[] keys, bool areBlocked)
	{
		foreach(KeyCode key in keys)
		{
			SetBlocking(key, areBlocked);
		}
	}
	
	
	// Update is called once per frame
	void Update () 
	{	
	
		if(Input.GetKeyDown(KeyCode.C) && !_blockedKeys.Contains(KeyCode.C))
		{			
			//MainCamera.Instance.CycleCameras();
			EventManager.Instance.Post(new UserInputKeyEvent(UserInputKeyEvent.TYPE.KEYDOWN, KeyCode.C, Vector3.zero, this));
		}
		
		if(Input.GetKeyDown(KeyCode.M))
		{			
			MapCamera.Instance.ToggleScript();			
		}
		
		if(Input.GetKeyDown(KeyCode.Equals))
		{			
			AudioManager.Instance.VolumeUp();
		}
		
		if(Input.GetKeyDown(KeyCode.Minus))
		{			
			AudioManager.Instance.VolumeDown();	
		}
		
		if(Input.GetKeyDown(KeyCode.BackQuote))
		{
			if(GameOptions.Instance.DebugMode){ Console.Instance.ToggleConsole(); }
		}
		
		if(Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			MapCamera.Instance.ZoomIn();
		}
		
		if(Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			MapCamera.Instance.ZoomOut();
		}		
		
	}
		
}
