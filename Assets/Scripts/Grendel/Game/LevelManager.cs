using UnityEngine;
using System.Collections;

public class LevelManager : Singleton<LevelManager> {
	
	public AudioClip BackgroundMusicTrack;	
	
	// Use this for initialization
	void Start () 
	{
		PlayBackgroundMusicTrack();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void PlayBackgroundMusicTrack()
	{
		AudioManager.Instance.PlayMusicTrack(BackgroundMusicTrack);
	}
}
