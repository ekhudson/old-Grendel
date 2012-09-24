using System.Collections;

using UnityEngine;

public class ShellCasing : MonoBehaviour 
{
	
	public AudioClip[] ShellCasingSounds;
	public int MaxPlays = 4;
	
	// Use this for initialization
//	void Start () 
//	{
//	
//	}
//	
//	// Update is called once per frame
//	void Update () 
//	{
//	
//	}
	
	void OnParticleCollision()
	{		
		if (MaxPlays > 0)
		{
			AudioSource.PlayClipAtPoint(ShellCasingSounds[Random.Range(0, ShellCasingSounds.Length)], Vector3.zero + (Player.Instance.BaseTransform.position - transform.position), AudioManager.Instance.GlobalVolumeSFX);
			MaxPlays--;
		}
	}	
	
}
