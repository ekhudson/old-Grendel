using UnityEngine;
using System.Collections;

public class UserInput : MonoBehaviour 
{
	
	public float MouseSensitivityVertical = 1f;
	public float MouseSensitivityHorizontal = 1f;	
	

	// Use this for initialization
	void Start () 	
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{	
	
		if(Input.GetKeyDown(KeyCode.M))
		{			
			MapCamera.Instance.ToggleScript();			
		}
		
		if(Input.GetKeyDown(KeyCode.Equals))
		{			
			MapCamera.Instance.ZoomIn();	
		}
		
		if(Input.GetKeyDown(KeyCode.Minus))
		{			
			MapCamera.Instance.ZoomOut();	
		}
		
		if(Input.GetKeyDown(KeyCode.BackQuote))
		{
			if(GameOptions.Instance.DebugMode){ Console.Instance.ToggleConsole(); }
		}
		
	}
		
}
