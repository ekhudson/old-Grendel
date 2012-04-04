using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Apr 2, 2012
	/// 
	/// Filename: AudioManager.cs
	/// 
	/// Summary: Handles playing sound effects / music in the scene
	///  
	/// </summary>

public class AudioManager : Singleton<AudioManager> 
{
	
	public float GlobalVolumeSFX = 1f;
	public float GlobalVolumeMusic = 1f;
	
	public int MaxPlayingMusicTracks = 3;
	
	private List<AudioClip> _currentPlayingMusicTracks = new List<AudioClip>();
	
	private Dictionary<int, AudioSource> AudioDictionary = new Dictionary<int, AudioSource>();
	
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void PlayMusicTrack(AudioClip musicTrack)
	{
		AudioSource source = gameObject.AddComponent<AudioSource>();	
		
		source.clip = musicTrack;
		source.volume = GlobalVolumeMusic;
		
		try
		{
			source.Play(1000);
			Console.Instance.OutputToConsole("Playing Music Track: " + musicTrack.name, Console.Instance.Style_Admin);
		}
		catch
		{
			Console.Instance.OutputToConsole("Error Playing Music Track: " + musicTrack.name, Console.Instance.Style_Error);
		}			
		
		AudioDictionary.Add(GetInstanceID(), source);
	}
}
