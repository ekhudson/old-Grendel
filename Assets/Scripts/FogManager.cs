using UnityEngine;
using System.Collections;

public class FogManager : Singleton<FogManager> 
{
	
	public GameObject ThePlayer;
	

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		float fogValue = (ThePlayer.transform.position.y + 1) * 30;
		
		Debug.Log(fogValue);
		RenderSettings.fogEndDistance = fogValue;
	}
}
