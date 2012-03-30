using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour 
{
	
	public float CameraUpperLimit = 180f;
	public float CameraLowerLimit = 10f;
	
	
	private Vector3 _initialRotation = Vector3.zero;
	
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{       

   		Vector3 r = new Vector3(Input.GetAxis("Vertical") * GameManager.UserInputRef.MouseSensitivityVertical, 0f, 0f); //vertical rotation  
    	
		_initialRotation += r;
		
		if (_initialRotation.x > CameraUpperLimit)
		{
			_initialRotation.x = CameraUpperLimit;
		}
		else if (_initialRotation.x < CameraLowerLimit)
		{
			_initialRotation.x = CameraLowerLimit;
		}
		else
		{			
			Vector3 testRot = transform.rotation.eulerAngles + r;
			transform.rotation = Quaternion.Euler(testRot);
		}		
				
		
	
	}
}//end class
