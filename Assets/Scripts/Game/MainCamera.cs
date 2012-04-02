using UnityEngine;
using System.Collections;

public class MainCamera : Singleton<MainCamera>
{
	
	public float CameraUpperLimit = 180f;
	public float CameraLowerLimit = 10f;
	
	public Transform CamSetupFirstPerson;
	public Transform CamSetupThirdPerson;
	public Transform CamSetupTopdown;
	

	private Transform[] _cameraSetups = new Transform[3];
	private Transform _currentCamera;
	private int _currentCameraIndex = 1;
	private Vector3 _initialRotation = Vector3.zero;
	
	//TODO: Cam Setups should be a datatype that defines special
	//cam parameters, such as locking cam 'look' in RTS mode
	//and hiding the player in FPS mode
	
	
	// Use this for initialization
	void Start () 
	{
		_cameraSetups = new Transform[] {CamSetupFirstPerson, CamSetupThirdPerson, CamSetupTopdown};
		_currentCamera = _cameraSetups[_currentCameraIndex];
		
		_initialRotation.x = 90f;
	}
	
	// Update is called once per frame
	void Update () 
	{ 
	   		
		if(!Console.Instance.ShowConsole) //TODO: Replace this with a paused game state
		{
			Vector3 r = new Vector3(Input.GetAxis("Vertical") * UserInput.Instance.MouseSensitivityVertical, 0f, 0f); //vertical rotation  
	    	
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
	}
	
	public void CycleCameras()
	{
		_currentCameraIndex++;
		
		if(_currentCameraIndex > (_cameraSetups.Length - 1)) {_currentCameraIndex = 0;}
		
		_currentCamera = _cameraSetups[_currentCameraIndex];
		
		SwitchCamera(_currentCamera);		
	}
	
	public void SwitchCamera(Transform newSetup)
	{
		Console.Instance.OutputToConsole("Going to Camera Setup: " + _cameraSetups[_currentCameraIndex].name, Console.Instance.Style_Admin);
		transform.position = newSetup.position;		
	}
}//end class
