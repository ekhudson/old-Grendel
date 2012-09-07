using UnityEngine;
using System.Collections;

public class LevelManager : Singleton<LevelManager> 
{
	
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
		Console.Instance.OutputToConsole(string.Format("{0}: {1} loaded, calling music track", this.ToString(), Application.loadedLevelName), Console.Instance.Style_Admin);
		PlayBackgroundMusicTrack();		
	}
	
	// Update is called once per frame
//	void Update () 
//	{
//		Debug.Log(_musicTrackIndex);
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
	
	public void LoadLevel(string sceneName)
	{
		if (Application.loadedLevelName == sceneName)
		{
			Console.Instance.OutputToConsole(string.Format("{0}: Asked to load scene {1}, but that scene is already loaded.", this.ToString(), sceneName), Console.Instance.Style_Error);		
		}
		
		Console.Instance.OutputToConsole(string.Format("{0}: Loading scene {1}.", this.ToString(), sceneName), Console.Instance.Style_Admin);		
		
		try
		{			
			Application.LoadLevel(sceneName);
			GameManager.Instance.SetGameState(GameManager.GAMESTATE.LOADING);
		}
		catch
		{
			Console.Instance.OutputToConsole(string.Format("{0}: Attempted to load scene {1}, but a scene with that name was not found.", this.ToString(), sceneName), Console.Instance.Style_Error);					
		}
	}
}
