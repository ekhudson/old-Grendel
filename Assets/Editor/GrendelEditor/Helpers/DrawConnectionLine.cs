using System.Collections;

using UnityEngine;
using UnityEditor;

public class DrawConnectionLine : ScriptableObject 
{
	private const float kArrowSize = 1f;
	private const float kArrowSpacing = 1f;
	
	
	public static void DrawLine(GameObject source, GameObject target, Color color)
	{
		Vector3 sourcePosition = source.transform.position;
		Vector3 targetPosition = target.transform.position;			
		
		float arrowSize = kArrowSize;
		Vector3 direction = (sourcePosition - targetPosition).normalized;		
		Quaternion rotation = Quaternion.LookRotation(direction);
		rotation *= Quaternion.Euler(0, 180, 0);
		float amt = ((sourcePosition - targetPosition).magnitude - (kArrowSpacing + kArrowSize));
		
		Handles.color = color;
		
		for(float i = 0; i < amt; i += kArrowSpacing)
		{
			Handles.ArrowCap(0, targetPosition + ((( direction * arrowSize) / 0.5f) + (direction * i)), rotation, arrowSize);
		}
		
		Handles.DrawLine( sourcePosition,  targetPosition + (( direction * arrowSize) / 0.5f));
		
		Handles.color = Color.white;
	}	
	
	public static void DrawLine(EditorObject source, EditorObject target, Color color)
	{
		DrawLine(source.gameObject, target.gameObject, color);
	}
	
}

