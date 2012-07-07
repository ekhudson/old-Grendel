using UnityEngine;
using System.Collections;

interface IEditorObject 
{	
	void OnActivate(EditorObject caller); //called when the editor object is activated
	
	void OnDeactivate(EditorObject caller); //called when the editor object is deactivated
	
	void OnToggle(EditorObject caller); //called when the editor object is toggled
	
	void OnEnabled(EditorObject caller); //Activates other editor objects
	
	void OnDisabled(EditorObject caller); //Deactivates other editor objects
		
	void CallSubjects(); //run through connections and call subjects accordingly
	
	void Call(EditorObject.EditorObjectMessage message, EditorObject caller); //Get called		
	
}