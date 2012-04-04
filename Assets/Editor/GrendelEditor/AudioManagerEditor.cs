using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor {
	
	public AudioManager _target;
	
	void OnEnable()
	{
		_target = (AudioManager)target;
	}
	
	// Override the GUI
	public override void OnInspectorGUI()
	{
		(target as AudioManager).GlobalVolumeMusic = EditorGUILayout.Slider("Global Music Volume: ", (target as AudioManager).GlobalVolumeMusic, 0f, 1f);
		(target as AudioManager).GlobalVolumeSFX = EditorGUILayout.Slider("Global Music Volume: ", (target as AudioManager).GlobalVolumeSFX, 0f, 1f);
	}
}
