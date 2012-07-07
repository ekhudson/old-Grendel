using UnityEngine;
using System.Collections;
using System;

public class GrendelToolbarButton : MonoBehaviour 
{
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct)]
	public class ToolbarButtonAttribute : Attribute
	{
		public ToolbarButtonAttribute(string itemName) 
		{ 
			this.name = itemName;			
		}		
		
		string name;
	}
	
	public static void Main() 
	{
		System.Reflection.MemberInfo info = typeof(GrendelToolbarButton);
		object[] attributes = info.GetCustomAttributes(true);
		
		for (int i = 0; i < attributes.Length; i ++)
		{
			System.Console.WriteLine(attributes[i]);
		}
	} 
	
	
	
	
//	// Use this for initialization
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
}
