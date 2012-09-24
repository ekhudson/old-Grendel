#define DEBUG_MODE
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Mar 12, 2012
	/// 
	/// Filename: Console.cs
	/// 
	/// Summary: Pops up the console and parses
	/// commands entered
	/// 
	/// </summary>

public class Console : Singleton<Console> 
{
	
	#region ConsoleLine Data Type
	//this class holds previous console text and Style
	public class ConsoleLine 
	{
		public string User = "none";
		public string CommandDateTime = "none";		
		public string TextString = "";
		public GUIStyle Style;
		
		public string DetailText
		{
			get 
			{								
				return Console.Instance.DetailView == true && User != "" ? System.String.Format("{0} {1}: ", StringTool.ForceLength(User, 8), CommandDateTime) : "";				
			}
		}		
	}
	#endregion
	
	#region PUBLIC VARIABLES	
	public List<ConsoleLine> ConsoleLineList = new List<ConsoleLine>(0); //list of previous console commands
	public int NumberOfCommandsToShow = 16; //number of previous commands to show
	public float ConsoleHeight = 400;
	public bool PauseGameWhenOpen = false;
	public bool DetailView = false; //detail view shows time, date and user of each command line
	public bool OutputLogOnQuit = false; //does the console output a log on quit?
	public string InputString = ""; //default console text, should be empty	
	public Color ConsoleColor = Color.white; //the colorization of the console;
	public GUIStyle Style_UserCurrent; //User Style, this is the Style used in the text field of the console
	public GUIStyle Style_UserPrevious; //The Style of previous console commands entered by the user
	public GUIStyle Style_Admin; //the Style of responses from the console
	public GUIStyle Style_Error; //the Style of errors from the console
	public GUIStyle Style_Detail; //the Style of details in the console	
	#endregion

	#region PRIVATE VARIABLES
	private bool _showConsole = false; //is the FPS Counter visible?
	private float _previousTimeScale = 1f; //the time scale before opening console
	private Vector2 _scrollPoint = Vector2.zero;
	private int _previousCommandCount = 0;

	//private delegate void ConsoleCommandDelegate(); 
	#endregion	
	
	//Toggles the visibility of the console
	public void ToggleConsole()
	{		
		if (!_showConsole)
		{			
			_showConsole = true;
			Screen.showCursor = true;
			if(PauseGameWhenOpen){_previousTimeScale = Time.timeScale; Time.timeScale = 0f;}
		}
		else
		{			
			_showConsole = false;
			if (GameOptions.Instance.HideDefaultCursor){ Screen.showCursor = false; }
			if(PauseGameWhenOpen){Time.timeScale = _previousTimeScale;}
		}
	
	}
	
	void OnGUI()
	{		
		//Check if the console should be showing
		if (_showConsole)
		{
			
			GUI.color = ConsoleColor;
			//GUI.Box(new Rect(0, 0,Screen.width, ConsoleHeight), new GUIContent("Console"), GrendelCustomStyles.CustomElement(GUI.skin.box, ConsoleColor, Color.white,TextAnchor.UpperCenter)); //define the console box
			
			if(_previousCommandCount < ConsoleLineList.Count)
			{
				_scrollPoint.y = 100000000; //gotta be a better solution to this
			}
			_scrollPoint = GUILayout.BeginScrollView(_scrollPoint, GrendelCustomStyles.CustomElement(GUI.skin.box, ConsoleColor, Color.white,TextAnchor.UpperCenter), new GUILayoutOption[]{ GUILayout.Height(ConsoleHeight), GUILayout.Width(Screen.width) }); //define the console box
				GUI.color = Color.white;				
				
				//Do our key input checks first, otherwise the Textfield below will eat our input
				
				//check for enter key input and run the command entered
				if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
				{				
					if (string.IsNullOrEmpty( InputString )) { return; }				
					OutputToConsole(InputString, Style_UserPrevious);				
					RunCommand(InputString); //attempt to run the command entered by the user
					InputString = ""; //reset the textfield string				
				}
				
				//check for UP ARROW input and cycle through previous commands
				if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.UpArrow))
				{					
					for (int i = ConsoleLineList.Count - 1; i >= 0; i--)
					{					
						if (ConsoleLineList[i].Style == Style_UserPrevious)
						{						
							InputString = ConsoleLineList[i].TextString;
							break;
						}
					}
				}
				
				//check for escape or tilde input and close the console
				if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Escape || Event.current.keyCode == KeyCode.BackQuote))
				{	
					ToggleConsole(); //toggle the console
					InputString = ""; //reset the textfield string
				}
			
				GUILayout.Label("", GUILayout.Height(ConsoleHeight)); //blank space to put new commands at the bottom of the console
							
				//check if we have previous commands to show
				if (ConsoleLineList.Count > 0)
				{
					//if so, cycle through those commands				
					for(int i = ConsoleLineList.Count - 1; i >= 0; i--)
					{
						int count = ConsoleLineList.Count - i; //set our count.				
						
						//Show Command Detail Text					
						GUILayout.BeginHorizontal();
							GUILayout.TextArea(ConsoleLineList[count - 1].DetailText, Style_Detail, GUILayout.Width(192));	
						
							//create previous commands as buttons, so the user can click on them to reuse previous commands
							if(GUILayout.Button(new GUIContent(ConsoleLineList[count - 1].TextString,
															   ConsoleLineList[count - 1].Style == Style_UserPrevious ? string.Format("Click to add '{0}' to input field", ConsoleLineList[count - 1].TextString) : ""),
															   ConsoleLineList[count - 1].Style))					
							{
								//only allow the user to reuse their commands (as opposed to reusing Admin or Error prompts)					
								if (ConsoleLineList[count -1].Style == Style_UserPrevious){ InputString = ConsoleLineList[count - 1].TextString; }			
							}	
						GUILayout.FlexibleSpace();
						GUILayout.EndHorizontal();
					}				
				}					

			GUILayout.EndScrollView();
			
			GUI.SetNextControlName("Console"); //set the name of the console
			
			InputString = GUILayout.TextField(InputString,  GrendelCustomStyles.CustomElement(GUI.skin.box, ConsoleColor, Style_UserCurrent.normal.textColor, TextAnchor.MiddleLeft)); //Set up the textfield for accepting user input		
			
			DetailView = GUILayout.Toggle(DetailView, "Detailed Console");	
			
			GUI.FocusControl("Console"); //focus on the textfield
			
			if (InputString == "`") { InputString = "";} //clear the tilde if it shows up in the input string (happens sometimes, probably damn onGUI
			
			_previousCommandCount = ConsoleLineList.Count;			
		}
	}//end OnGUI	
		
	void RunCommand(string command)
	{
		string[] splitCommand = command.Split(new char[] {' '}); //split up the user's input. Strings are split whenever a 'space' is encountered
				
		string newCommand = splitCommand[0];
		
		//assess special cases
		switch(newCommand)
		{
			case "?":
				newCommand = "ShowCommands";
			break;
			
			case "Help":
				newCommand = "ShowCommands";
			break;
			
			case "help":
				newCommand = "ShowCommands";
			break;
			
			default:
				//do nothing if no special case
			break;
		}
		
		object[] newCommandParameters = new object[splitCommand.Length];
		
		for(int i = 1; i < (splitCommand.Length); i++)
		{			
			newCommandParameters[i - 1] = splitCommand[i] as object;
		}
		
		ConsoleCommandRegistry.Instance.HandleCommand(newCommand, new ConsoleCommandParams(newCommandParameters));	
	}	
	
	//DebugInput
	//
	//Input directly related to debug tools
//	public void DebugInput()
//	{		
//		if (_bHotSpawnEnabled)
//		{			
//			
//			if (Input.GetKeyDown(KeyCode.Keypad0))
//			{				
//				HotSpawn("Enemy_Inquisitor", 1);
//			}
//			
//			if (Input.GetKeyDown(KeyCode.Keypad1))
//			{				
//				HotSpawn("Enemy_Fighter", 1);
//			}
//			
//			if (Input.GetKeyDown(KeyCode.Keypad2))
//			{				
//				HotSpawn("Enemy_Assassin", 1);
//			}
//			
//			if (Input.GetKeyDown(KeyCode.Keypad3))
//			{		
//				HotSpawn("Enemy_Frigate", 1);
//			}
//			
//			if (Input.GetKeyDown(KeyCode.Keypad4))
//			{		
//				HotSpawn("Enemy_Battleship", 1);
//			}
//			
//			if (Input.GetKeyDown(KeyCode.Keypad5))
//			{		
//				HotSpawn("Enemy_Macula", 1);
//			}
//			
//			if (Input.GetKeyDown(KeyCode.Keypad6))
//			{		
//				HotSpawn("Mine", 1);
//			}
//			
//			if (Input.GetKeyDown(KeyCode.Keypad7))
//			{		
//				HotSpawn("AU_Fighter_New", 1);
//			}
//			
//			if (Input.GetKeyDown(KeyCode.Keypad8))
//			{		
//				HotSpawn("Enemy_HUB", 1);
//			}
//			
//			if (Input.GetKeyDown(KeyCode.Keypad9))
//			{		
//				HotSpawn("Dragon", 1);
//			}
//		}		
//	}
//	
//	//HOTSPAWN
//	//
//	//HotSpawn allows the user to spawn an entity at the mouse position
//	//(requires _bHotSpawnEnabled to be true, set by console command "HotSpawn")
//	public void HotSpawn(string obj, int count)
//	{		
//		Vector3 tempLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//		tempLoc.y = 0;
//		
//		for (int i = 0; i < count; i++)
//		{
//			Debug.Log(obj + " Spawned");
//			GameObject SpawnedObject = (GameObject)Instantiate(Resources.Load("Prefabs/Entities/" + obj), tempLoc, Quaternion.identity);
//		}
//	}
	
	//OUTPUT TO CONSOLE
	//
	//This method can be called from anywhere to output a line to the console, useful for logging events	
	public void OutputToConsole(string text, GUIStyle style, bool showDetail)
	{
#if DEBUG_MODE
		Debug.Log(text);
#endif
		ConsoleLine line = new ConsoleLine();

		if (showDetail)
		{
			
			if (style == Style_Admin)
			{
				line.User = "Grendel";
			}
			else if(style == Style_Error)
			{
				line.User = "Grendel";
			}
			else
			{			
				line.User = Environment.UserName;			
			}		
			
			line.CommandDateTime = DateTime.Now.ToString("yy/mm/dd HH:mm:ss");	
		}
		else
		{
			line.User = "";
			line.CommandDateTime = "";
		}
		
		line.TextString =  text;
		line.Style = style;
		ConsoleLineList.Add(line);		
		
		//if the console isn't visible, push a notification to the screen
		if (!_showConsole && GameOptions.Instance.DebugMode)
		{
			new ScreenNotification(text, style.normal.textColor);
		}
	}
	
	public void OutputToConsole(string text,  GUIStyle style) 
	{
		OutputToConsole(text, style, true);
	}
	
	public void OnApplicationQuit()
	{
		OutputToConsole("Quitting", Style_Admin);
		if (OutputLogOnQuit){ ConsoleLog.OutputLog(ConsoleLineList, System.DateTime.Now.ToString("yymmdd HHmmss")); }
			
	}
	
	//PUBLIC ACCESSORS
	public bool ShowConsole
	{
		get { return _showConsole; }
		set { _showConsole = value; }
	}
}
