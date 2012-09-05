using UnityEngine;
using System.Collections;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Feb 15, 2012
	/// 
	/// Filename: GameManager.cs
	/// 
	/// Summary: Essentially holds information that might need to be
	/// accessed by a variety of objects in the scene, as well as maintains
	/// the current state of the Game
	///  
	/// </summary>

public class GameManager : Singleton<GameManager>
{
	#region PUBLIC VARIABLES
	public string ApplicationTitle = "Grendel";
	public string ApplicationVersion = "1.0";
	public bool DebugBuild = true;
	
	public enum GAMESTATE
	{
		LOADING,
		INTRO,
		MAINMENU,
		OPTIONS,
		RUNNING,
		PAUSED,
		CREDITS,
	}	
	#endregion
	
	#region PRIVATE VARIABLES
	
	private GAMESTATE _gameState = GAMESTATE.LOADING;
	
	#endregion
	
	protected override void Awake()
	{		
		base.Awake();		
	}

	// Use this for initialization
	void Start () 
	{				
		Console.Instance.OutputToConsole(string.Format("Starting up {0} {1}", ApplicationTitle, ApplicationVersion), Console.Instance.Style_Admin);
		ConnectionRegistry.Instance.BuildConnections();
	}
	
	public void SetGameState(GAMESTATE state)
	{
		switch(state)
		{
			case GAMESTATE.LOADING:
			
			break;
			
			case GAMESTATE.INTRO:
			
			break;
			
			case GAMESTATE.MAINMENU:
			
			break;
			
			case GAMESTATE.OPTIONS:
			
			break;
			
			case GAMESTATE.RUNNING:
			
			break;
			
			case GAMESTATE.PAUSED:
			
			break;
			
			case GAMESTATE.CREDITS:
			
			break;			
			
			default:
			
				_gameState = state;
			
			break;
		}
	}
}
