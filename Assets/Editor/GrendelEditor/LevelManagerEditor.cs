using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor {
	
	public AudioList TheAudioList;
	public string[] MusicTracks = new string[0];
		
	// Override the GUI
	public override void OnInspectorGUI()
	{		
		TheAudioList = GameObject.Find("GameManager").GetComponent<AudioList>();
		MusicTracks = new string[TheAudioList.MusicTracks.Length];
		
		for (int i = 0; i < (TheAudioList.MusicTracks.Length); i++)
		{
			MusicTracks[i] = TheAudioList.MusicTracks[i].name;
		}
		
		int index = 0;
		index = EditorGUILayout.Popup( "Background Music:", index, MusicTracks);
		GameObject.Find("GameManager").GetComponent<LevelManager>().BackgroundMusicTrack =  TheAudioList.MusicTracks[index];
	}
}
