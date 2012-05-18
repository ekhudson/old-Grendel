using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor {
	
	public AudioList TheAudioList;
	public string[] MusicTracks = new string[0];	
	public LevelManager _target;
	
	void OnEnable()
	{
		if (!_target) { _target = (LevelManager)target; }		
	}
	
	// Override the GUI
	public override void OnInspectorGUI()
	{		
		if (Application.isPlaying) { return; }	
		
		GUI.changed = false;
		
		TheAudioList = GameObject.Find("GameManager").GetComponent<AudioList>();
		MusicTracks = new string[TheAudioList.MusicTracks.Count];		
			
		int i = 0;
		
		foreach(AudioClip clip in TheAudioList.MusicTracks)
		{			
			MusicTracks[i] = clip.name;
			i++;
		}			
		
		_target.RandomMusicTrack = EditorGUILayout.Toggle("Random Music Track:", _target.RandomMusicTrack);
		
		EditorGUI.BeginDisabledGroup (_target.RandomMusicTrack == true);	
		
		_target.MusicTrackIndex = EditorGUILayout.Popup( "Background Music:", _target.MusicTrackIndex, MusicTracks);		
		
		EditorGUI.EndDisabledGroup();	
		
		if (TheAudioList.MusicTracks.Count > 0)
		{
			_target.BackgroundMusicTrack = TheAudioList.MusicTracks[_target.MusicTrackIndex];
		}	
		
		if (GUI.changed){ EditorUtility.SetDirty( target ); }			
		
	}
}
