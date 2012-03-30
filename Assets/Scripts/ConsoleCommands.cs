using UnityEngine;
using System.Collections;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Mar 12, 2012
	/// 
	/// Filename: ConsoleCommands.cs
	/// 
	/// Summary: Defines the ConsoleCommand Data Type and 
	/// holds all the available console commands
	/// 
	/// </summary>

public class ConsoleCommands : MonoBehaviour
{		
	public class ConsoleCommand
	{
		public string Command;
		public string[] Parameters = new string[4];		
	}
	
	private static string _responseText = ""; //the reponse given when a command is entered
	private static GUIStyle _responseStyle;
	private static ConsoleCommand _currentCommand;
	private static ConsoleCommands _instance;
	
	static public ConsoleCommands Instance
	{
		get{ return _instance; }
	}
	
	void Awake()
	{
		_instance = this;
	}
	
	
	
	public void RunCommand(string command)
	{
		string[] splitCommand = command.Split(new char[] {' '}); //split up the user's input. Strings are split whenever a 'space' is encountered
		_currentCommand = new ConsoleCommand();
		_currentCommand.Command = splitCommand[0];
			
		for(int i = 1; i < (splitCommand.Length - 1); i++)
		{
			_currentCommand.Parameters[i - 1] = splitCommand[i];
		}
		
		_responseText = "I'm sorry Dave, I can't do that."; //set default text
		_responseStyle = Console.Instance.Style_Error; //set default style		
		
		try
		{
		 	Invoke( _currentCommand.Command, 0f);
		}
		catch
		{
			Console.Instance.OutputToConsole(_responseText, _responseStyle);
		}
		
	}
	
	void Quit()
	{
		Console.Instance.OutputToConsole("Quitting " + Application.absoluteURL, Console.Instance.Style_Admin);
		Application.Quit();
	}
	
	
		
}//end class
