using UnityEngine;
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


public class Console : MonoBehaviour {
	
	//Console Line Data Class
	//this class holds previous console text and Style
	public class ConsoleLine
	{
		public string TextString = "";
		public GUIStyle Style;
	}
	
//	public class ConsoleCommand
//	{
//		public string Command;
//		public string[] parameters;		
//	}
	
	//PUBLIC VARIABLES	
	public List<ConsoleLine> ConsoleCommandList = new List<ConsoleLine>(0); //list of previous console commands
	public int NumberOfCommandsToShow = 8; //number of previous commands to show
	public float ConsoleHeight = 400;
	public bool PauseGameWhenOpen = false;
	public string InputString = ""; //default console text, should be empty	
	public GUIStyle Style_UserCurrent; //User Style, this is the Style used in the text field of the console
	public GUIStyle Style_UserPrevious; //The Style of previous console commands entered by the user
	public GUIStyle Style_Admin; //the Style of responses from the console
	public GUIStyle Style_Error; //the Style of errors from the console
	
	//FPS Counter Variables
	//public float FPSupdateInterval = 0.01F;
	//public Font FPSFont;
	
	//PRIVATE VARIABLES
	//private bool _bShowFPS = false; //is the FPS Counter visible?
	private bool _showConsole = false; //is the FPS Counter visible?
	private float _previousTimeScale = 1f; //the time scale before opening console
	//private bool _bHotSpawnEnabled = true; //is hot spawning enable?
	//private bool _bCameraScrollEnabled = false; //is camera scrolling enabled?
	
	private static Console _instance;
	
	static public Console Instance
	{
		get {return _instance;}
	}
		
 	//fps variables
	//private GameObject _fpsGO;
	//private GUIText FPSText = new GUIText();
	//private float accum   = 0; // FPS accumulated over the interval
	//private int   frames  = 0; // Frames drawn over the interval
	//private float timeleft; // Left time for current interval
	
	
	void Awake()
	{
		_instance = this;
	}
	
	// Update is called once per frame
//	void Update () 
//	{		
//		DebugInput();
//	}
	
//	void CreateFPSCounter()
//	{   
//		_fpsGO = new GameObject("FPSGameObject");		
//		timeleft = FPSupdateInterval; 
//		FPSText = _fpsGO.AddComponent<GUIText>();
//		FPSText.font = FPSFont;
//		FPSText.fontSize = 12;
//		FPSText.pixelOffset = new Vector2(10, Screen.height);
//	}
	
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
			GUI.Box(new Rect(0, 0,Screen.width, ConsoleHeight), new GUIContent("Console")); //define the console box
			
			//Do our key input checks first, otherwise the Textfield below will eat our input
			
			//check for enter key input and run the command entered
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
			{				
				ConsoleLine line = new ConsoleLine(); //create an empty console line object
				line.TextString = InputString; //set the console line text to the text entered by the user
				line.Style = Style_UserPrevious; //set the Style to User Previous Style				
				ConsoleCommandList.Add(line); //add the line to the previous console command array
				ConsoleCommands.Instance.RunCommand(InputString); //attempt to run the command entered by the user
				InputString = ""; //reset the textfield string				
			}
			
			//check for escape or tilde input and close the console
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Escape || Event.current.keyCode == KeyCode.BackQuote))
			{	
				ToggleConsole(); //toggle the console
				InputString = ""; //reset the textfield string
			}
			
			//check for UP ARROW input and cycle through previous commands
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.UpArrow))
			{					
				for (int i = ConsoleCommandList.Count - 1; i >= 0; i--)
				{					
					if (ConsoleCommandList[i].Style == Style_UserPrevious)
					{						
						InputString = ConsoleCommandList[i].TextString;
						break;
					}
				}
			}
			
			//check if we have previous commands to show
			if (ConsoleCommandList.Count > 0)
			{
				//if so, cycle through those commands
				for (int i = 0; i < ConsoleCommandList.Count && i < NumberOfCommandsToShow; i++)
				{
					int count = ConsoleCommandList.Count - i; //set our count. We will be iterating backwards through the array of previous commands				
					//GUI.TextArea(new Rect(0, (ConsoleHeight - 50) - i * 20,Screen.width, 20), ConsoleCommandList[count - 1].TextString, ConsoleCommandList[count - 1].Style); //display previous command with appropriate Style				
					if(GUI.Button(new Rect(0, (ConsoleHeight - 50) - i * 20, Screen.width, 20), ConsoleCommandList[count - 1].TextString, ConsoleCommandList[count - 1].Style))
					{
						InputString = ConsoleCommandList[count - 1].TextString;
					}
				}
			}
						
			GUI.SetNextControlName("Console"); //set the name of the console
			InputString = GUI.TextField(new Rect(0, ConsoleHeight - 20,Screen.width, 20), InputString, Style_UserCurrent); //Set up the textfield for accepting user input
			GUI.FocusControl("Console"); //focus on the textfield

			
		}
		
//		//FPS check
//		if (_bShowFPS)
//		{
//			DrawFPSCounter();
//		}
//		else
//		{
//			if (FPSText){ FPSText.enabled = false; }
//		}
	}//end OnGUI
	
	/// <summary>
	/// DrawFPSCounter
	/// 
	/// Draws a simple FPS counter to the GUI.
	/// Useful when testing builds outside the editor
	/// </summary>	
//	void DrawFPSCounter()
//	{
//		if (!FPSText)
//		{
//			CreateFPSCounter();
//		}
//		
//		FPSText.enabled = true;
//		timeleft -= GameManager.RealDeltaTime;
//		accum += Time.timeScale/Time.deltaTime;
//		++frames;
//
//		// Interval ended - update GUI text and start new interval
//		if( timeleft <= 0.0 )
//		{
//    		// display two fractional digits (f2 format)
//			float fps = accum/frames;
//			string format = System.String.Format("{0:F2} FPS",fps);
//			 FPSText.text = format;
//
//			if(fps < 30)
//    			FPSText.material.color = Color.yellow;
//			else 
//    		if(fps < 10)
//        		FPSText.material.color = Color.red;
//   			 else
//        		FPSText.material.color = Color.green;
//
//			//  DebugConsole.Log(format,level);
//   			timeleft = FPSupdateInterval;
//    		accum = 0.0F;
//    		frames = 0;
//		}
//	}
//	
	
	/// <summary>
	/// RUN COMMAND
	/// 
	/// Attempts to run the command just entered by the user. Returns an error message to the console display
	/// if no matching command is found
	/// </summary>
	/// <param name="command">
	/// A <see cref="System.String"/>
	/// </param>
	void RunCommand(string command)
	{
//		string[] splitCommand = command.Split(new char[] {' '}); //split up the user's input. Strings are split whenever a 'space' is encountered
//		string responseText = "I'm sorry Dave, I can't do that."; //set default text
//		GUIStyle responseStyle = Style_Error; //set default Style
		
	 
		
				
//		//QUIT
//		if (splitCommand[0] == "Quit" || splitCommand[0] == "quit")
//		{
//			Application.Quit();	
//		}
//		
//		//GOD MODE
//		if (splitCommand[0] == "God" || splitCommand[0] == "god")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.PlayerRef.PlayerShip.GodMode = true;
//				responseStyle = Style_Admin;
//				responseText = "God Mode Enabled";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.PlayerRef.PlayerShip.GodMode = false;
//				responseStyle = Style_Admin;
//				responseText = "God Mode Disabled";
//			}
//			else
//			{
//				if (!GameManager.PlayerRef.PlayerShip.GodMode)
//				{
//					GameManager.PlayerRef.PlayerShip.GodMode = true;
//					responseStyle = Style_Admin;
//					responseText = "God Mode Enabled";
//				}
//				else
//				{
//					GameManager.PlayerRef.PlayerShip.GodMode = false;
//					responseStyle = Style_Admin;
//					responseText = "God Mode Disabled";
//				}
//			}			
//		}
//		
//		//UNLOCK CAMERA
//		if (splitCommand[0] == "UnlockCam" || splitCommand[0] == "unlockcam")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.CameraRef.UnlockedCam = true;
//				responseStyle = Style_Admin;
//				responseText = "Camera Unlocked";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.CameraRef.UnlockedCam = false;
//				responseStyle = Style_Admin;
//				responseText = "Camera Locked";
//			}
//			else
//			{
//				if (!GameManager.CameraRef.UnlockedCam)
//				{
//					GameManager.CameraRef.UnlockedCam = true;
//					responseStyle = Style_Admin;
//					responseText = "Camera Unlocked";
//				}
//				else
//				{
//					GameManager.CameraRef.UnlockedCam = false;
//					responseStyle = Style_Admin;
//					responseText = "Camera Locked";
//				}
//			}			
//		}
//		
//		
//		
//		//LOAD LEVEL
//		if (splitCommand[0] == "LoadLevel" || splitCommand[0] == "loadlevel")
//		{
//			
//			if (splitCommand.Length > 1)
//			{
//				responseStyle = Style_Admin;
//				responseText = "LoadLevel " + splitCommand[1];
//				GameManager.LoadLevel(splitCommand[1]);
//			}
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Scene name not specified. Command syntax is 'LoadLevel SceneName'";
//			}                      
//			
//		}
//		
//		
//		
//		//RESTART LEVEL
//		if (splitCommand[0] == "RestartLevel" || splitCommand[0] == "restartlevel")
//		{
//			
//			if (splitCommand.Length > 1)
//			{
//				responseStyle = Style_Error;
//				responseText = "Unwanted parameter: " + splitCommand[1] + ". RestartLevel does not require any parameters";
//			}
//			else
//			{
//				responseStyle = Style_Admin;
//				responseText = "Restarting level.";
//				GameManager.LoadLevel(Application.loadedLevelName);
//			}                      
//			
//		}
//		
//		//MUTE ALL
//		if (splitCommand[0] == "MuteAll" || splitCommand[0] == "muteall")
//		{
//			AudioManager.GlobalVolume = 0;			
//			responseStyle = Style_Admin;
//			responseText = "All sounds muted";
//		}
//		
//		//INFINITE ENERGY
//		if (splitCommand[0] == "Energy" || splitCommand[0] == "energy")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.PlayerRef.InfiniteEnergy = true;
//				responseStyle = Style_Admin;
//				responseText = "Infinite Energy";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.PlayerRef.InfiniteEnergy = false;
//				responseStyle = Style_Admin;
//				responseText = "Regular Energy";
//			}
//			else
//			{
//				if (!GameManager.PlayerRef.InfiniteEnergy)
//				{
//					GameManager.PlayerRef.InfiniteEnergy = true;
//					responseStyle = Style_Admin;
//					responseText = "Infinite Energy";
//				}
//				else
//				{
//					GameManager.PlayerRef.InfiniteEnergy = false;
//					responseStyle = Style_Admin;
//					responseText = "Regular Energy";
//				}
//			}			
//		}
//		
//		//INFINITE SLOWMO
//		if (splitCommand[0] == "Slowmo" || splitCommand[0] == "slowmo")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.PlayerRef.InfiniteSlomo = true;
//				responseStyle = Style_Admin;
//				responseText = "Infinite Slowmo";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.PlayerRef.InfiniteSlomo = false;
//				responseStyle = Style_Admin;
//				responseText = "Regular Slowmo";
//			}
//			else
//			{
//				if (!GameManager.PlayerRef.InfiniteSlomo)
//				{
//					GameManager.PlayerRef.InfiniteSlomo = true;
//					responseStyle = Style_Admin;
//					responseText = "Infinite Slowmo";
//				}
//				else
//				{
//					GameManager.PlayerRef.InfiniteSlomo = false;
//					responseStyle = Style_Admin;
//					responseText = "Regular Slowmo";
//				}
//			}			
//		}
//		
//		//INFINITE EVERYTHING
//		if (splitCommand[0] == "Impulse" || splitCommand[0] == "impulse")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.PlayerRef.PlayerShip.GodMode = true;
//				GameManager.PlayerRef.InfiniteEnergy = true;
//				GameManager.PlayerRef.InfiniteSlomo = true;
//				responseStyle = Style_Admin;
//				responseText = "Infinite Slowmo, Infinite Energy, God Mode Enabled";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.PlayerRef.PlayerShip.GodMode = false;
//				GameManager.PlayerRef.InfiniteEnergy = false;
//				GameManager.PlayerRef.InfiniteSlomo = false;
//				responseStyle = Style_Admin;
//				responseText = "Regular Slowmo";
//			}
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'Impulse [0]' or 'Impulse [1]'";
//			}			
//		}
//		
//		// FILL OVERCHARGE BAR
//		if (splitCommand[0] == "FullOvercharge" || splitCommand[0] == "fullovercharge" || splitCommand[0] == "fullOvercharge")
//		{
//			GameManager.PlayerRef.CurrentOvercharge = GameManager.PlayerRef.MaxOvercharge; // Max overcharge
//			responseStyle = Style_Admin;
//			responseText = "Overcharge meter filled to full.";
//		}
//		
//		//SPAWN OBJECT
//		if (splitCommand[0] == "Spawn" || splitCommand[0] == "spawn")
//		{
//			
//			if (splitCommand.Length > 1)
//			{
//				int count = 1;
//				float spread = 1f;
//				Vector3 location = GameManager.PlayerRef.Position;
//				
//				if (splitCommand.Length > 2)
//				{
//					count = System.Convert.ToInt32(splitCommand[2]);
//					spread *= count;
//				}
//				
//				responseText = "";
//				
//				for (int i = 0; i < count; i++)
//				{						
//					Vector3 tempLoc = new Vector3(location.x + Random.Range(-spread, spread), 0, location.z + Random.Range(-spread, spread));
//					GameObject SpawnedObject = (GameObject)Instantiate(Resources.Load("Prefabs/" + splitCommand[1]), tempLoc, Quaternion.identity);
//					responseStyle = Style_Admin;
//					responseText += splitCommand[1] + " spawned @ " + tempLoc + " | ";
//				}
//			}
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'Spawn [prefab path/name] [count]'";
//			}
//		}
//		
//		//TOGGLE FPS
//		if (splitCommand[0] == "ShowFPS" || splitCommand[0] == "showfps")
//		{
//			
//			if (_bShowFPS != true)
//			{
//				_bShowFPS = true;
//				responseStyle = Style_Admin;
//				responseText = "Showing FPS";
//			}
//			else
//			{
//				_bShowFPS = false;
//				responseStyle = Style_Admin;
//				responseText = "Hiding FPS";
//			}
//		}
//		
//		//Adjust Bullet Speed
//		if (splitCommand[0] == "BulletSpeed" || splitCommand[0] == "bulletspeed")
//		{
//			
//			if (splitCommand.Length > 1)
//			{				
//				GameManager.GlobalBulletSpeedModifier = float.Parse(splitCommand[1]);
//				responseStyle = Style_Admin;
//				responseText = "Global Bullet Speed Modifier set to: " + splitCommand[1];
//			}			
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'BulletSpeed [float Multiplier]'";
//			}			
//		}
//		
//		//Adjust Global Damage
//		if (splitCommand[0] == "GlobalDamage" || splitCommand[0] == "globaldamage")
//		{
//			
//			if (splitCommand.Length > 1)
//			{				
//				GameManager.GlobalDamageModifier = float.Parse(splitCommand[1]);
//				responseStyle = Style_Admin;
//				responseText = "Global Damage Modifier set to: " + splitCommand[1];
//			}			
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'GlobalDamage [float Multiplier]'";
//			}			
//		}
//		
//		//Toggle Rick Mode
////		if (splitCommand[0] == "RickMode" || splitCommand[0] == "rickmode")
////		{
////			
////			if (GameManager.GlobalBulletSpeedModifier == 1)
////			{				
////				GameManager.CameraRef.RickMode = true;
////				AudioManager.GotoLuciMusic();
////				GameManager.GlobalBulletSpeedModifier = 0.25f;
////				GameManager.GlobalDamageModifier = 5;
////				responseStyle = Style_Admin;
////				responseText = "RickMode Activated. All glory to Her Majesty's Jewel of the South, the Commonwealth of Australia.";
////			}			
////			else
////			{
////				GameManager.CameraRef.RickMode = false;
////				AudioManager.GotoDefaultMusic();
////				GameManager.GlobalBulletSpeedModifier = 1;
////				GameManager.GlobalDamageModifier = 1;
////				responseStyle = Style_Admin;
////				responseText = "RickMode Deactivated. Not my bowl of rice.";
////			}			
////		}
////		
////		//Toggle NyanMode
////		if (splitCommand[0] == "NyanMode" || splitCommand[0] == "nyanmode")
////		{
////			if (!GameManager.NyanMode)
////			{
////				GameManager.NyanMode = true;
////				GameManager.PlayerRef.Sprite.PlayAnim(4);
////				AudioManager.PlayUnmanagedSound(GameManager.LevelManagerRef.NyanMusic, 2.0f, true);
////				responseStyle = Style_Admin;
////				responseText = "NYAN! NYAN! NYAN!";
////			}
////			else
////			{
////				GameManager.NyanMode = false;
////				GameManager.PlayerRef.Sprite.PlayAnim(1);
////				responseStyle = Style_Admin;
////				responseText = "NyanMode deactivated";
////			}
////		}
////		
////		//Toggle NyanMode
////		if (splitCommand[0] == "DragonMode" || splitCommand[0] == "dragonmode")
////		{
////			if (!GameManager.DragonMode)
////			{
////				GameManager.DragonMode = true;						
////			
////				AudioManager.PlayCombatMusic(GameManager.LevelManagerRef.DragonMusic, 2, true);
////				AudioManager.BGMusicAudioSource.Stop();
////				
////				for (int i = 0; i < 100; i++)
////				{					
////					Vector3 tempLoc = new Vector3(GameManager.PlayerRef.Position.x + Random.Range(-1000, 1000), 0, GameManager.PlayerRef.Position.z + Random.Range(-1000, 1000));
////					GameObject SpawnedObject = (GameObject)Instantiate(Resources.Load("Prefabs/Entities/Dragon"), tempLoc, Quaternion.identity);
////				}
////				
////				responseStyle = Style_Admin;
////				responseText = "You are the last dragon.";
////			}
////			else
////			{
////				GameManager.DragonMode = false;				
////				responseStyle = Style_Admin;
////				responseText = "You took an arrow to the knee...";
////			}
////		}
//		
//		//Toggle HotSpawning
//		if (splitCommand[0] == "HotSpawn" || splitCommand[0] == "hotspawn")
//		{
//			if (!_bHotSpawnEnabled)
//			{				
//				_bHotSpawnEnabled = true;
//				responseStyle = Style_Admin;
//				responseText = "Hot Spawning Enabled";
//			}
//			else
//			{				
//				_bHotSpawnEnabled = false;
//				responseStyle = Style_Admin;
//				responseText = "Hot Spawning Disabled";
//			}
//		}
//		
//		//Toggle DebugDrones
//		if (splitCommand[0] == "DebugDrones" || splitCommand[0] == "debugdrones")
//		{
//			if (!GameManager.UserInputRef.EnableDebugDrones)
//			{				
//				GameManager.UserInputRef.EnableDebugDrones = true;
//				responseStyle = Style_Admin;
//				responseText = "Debug Drones Enabled";
//			}
//			else
//			{				
//				GameManager.UserInputRef.EnableDebugDrones = false;
//				responseStyle = Style_Admin;
//				responseText = "Debug Drones Disabled";
//			}
//		}
//		
//		//Toggle HotSpawning
//		if (splitCommand[0] == "Zoom" || splitCommand[0] == "zoom")
//		{
//			if (!_bCameraScrollEnabled)
//			{				
//				_bCameraScrollEnabled = true;
//				responseStyle = Style_Admin;
//				responseText = "Camera Scrolling Enabled";
//			}
//			else
//			{				
//				_bCameraScrollEnabled = false;
//				responseStyle = Style_Admin;
//				responseText = "Camera Scrolling Disabled";
//			}
//		}
//		
//		//DISABLE/ENABLE IMAGE EFFECTS
//		if (splitCommand[0] == "ImageEffects" || splitCommand[0] == "imageeffects")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.DisableImageEffects = false;
//				GameManager.CheckQualitySettings();
//				responseStyle = Style_Admin;
//				responseText = "Image Effects Enabled";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.DisableImageEffects = true;
//				GameManager.CheckQualitySettings();
//				responseStyle = Style_Admin;
//				responseText = "Image Effects Disabled";
//			}
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'ImageEffects [1 = On / 0 = Off]'";
//			}			
//		}
//		
//			
//		//DISABLE/ENABLE PARTICLES
//		if (splitCommand[0] == "Particles" || splitCommand[0] == "particles")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.DisableParticles = false;
//				GameManager.CheckQualitySettings();
//				responseStyle = Style_Admin;
//				responseText = "Particles Enabled";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.DisableParticles = true;
//				GameManager.CheckQualitySettings();
//				responseStyle = Style_Admin;
//				responseText = "Particles Disabled";
//			}
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'Particles [1 = On / 0 = Off]'";
//			}			
//		}
//		
//		//DISABLE/ENABLE PARTICLES
//		if (splitCommand[0] == "JetStreams" || splitCommand[0] == "jetstreams")
//		{
//			
//			if (splitCommand.Length > 1 && splitCommand[1] == "1")
//			{
//				GameManager.DisableJetStreams = false;
//				GameManager.CheckQualitySettings();
//				responseStyle = Style_Admin;
//				responseText = "JetStreams Enabled";
//			}
//			else if (splitCommand.Length > 1 && splitCommand[1] == "0")
//			{
//				GameManager.DisableJetStreams = true;				
//				GameManager.CheckQualitySettings();
//				responseStyle = Style_Admin;
//				responseText = "JetStreams Disabled";
//			}
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'JetStreams [1 = On / 0 = Off]'";
//			}			
//		}
//		
//		
//		//Ortho Size
//		if (splitCommand[0] == "Orthosize" || splitCommand[0] == "orthosize")
//		{
//			
//			if (splitCommand.Length > 1)
//			{				
//				Camera.main.orthographicSize = float.Parse(splitCommand[1]);
//				responseStyle = Style_Admin;
//				responseText = "Camera Orthographic Size Set To: " + splitCommand[1];
//			}			
//			else
//			{
//				responseStyle = Style_Error;
//				responseText = "Missing parameter. Correct syntax is 'Orthosize [float Orthographic Size]'";
//			}			
//		}		
//				
//		ConsoleLine line = new ConsoleLine(); //create an empty console line object
//		line.Style = responseStyle; //set the Style
//		line.TextString = responseText; //set the text
//		ConsoleCommandList.Add(line); //add the line to the console output		
//	
	}//end RunCommand
	
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
	public void OutputToConsole(string text, GUIStyle Style)
	{
		ConsoleLine line = new ConsoleLine();
		line.TextString = text;
		line.Style = Style;
		ConsoleCommandList.Add(line);		
	}
	
//	
//	public bool CameraScrollingEnabled
//	{
//		get{return _bCameraScrollEnabled;}		
//	}
	
	
	//PUBLIC ACCESSORS
	public bool ShowConsole
	{
		get { return _showConsole; }
		set { _showConsole = value; }
	}
}
