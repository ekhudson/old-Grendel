using UnityEngine;
using System.Collections;

public class LevelManager : Singleton<LevelManager> {
	
	public bool RandomMusicTrack = false;
	public AudioClip BackgroundMusicTrack;
	
	private int _musicTrackIndex; //the index of the currently set music track
	
	[SerializeField]
	public int MusicTrackIndex
	{
		get{ return _musicTrackIndex; }
		set{ _musicTrackIndex = value; }
	}
	
	// Use this for initialization
	void Start () 
	{
		PlayBackgroundMusicTrack();		
	}
	
	// Update is called once per frame
//	void Update () 
//	{
//	
//	}
	
	void PlayBackgroundMusicTrack()
	{
		if (RandomMusicTrack) 
		{ 
			AudioManager.Instance.PlayMusicTrack( AudioList.Instance.MusicTracks[Random.Range(0, AudioList.Instance.MusicTracks.Count)] ); 
		}
		else
		{			
			AudioManager.Instance.PlayMusicTrack(BackgroundMusicTrack);
		}
	}
}
