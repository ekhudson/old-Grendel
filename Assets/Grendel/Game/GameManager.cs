using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	[SerializeField]
	public string MainMenuScene = null;
	
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
	
	#region PROPERTIES
	public GAMESTATE GameState
	{
		get{return _gameState;}
		
	}
	#endregion
	
	protected override void Awake()
	{		
		base.Awake();		
	}

	// Use this for initialization
	void Start () 
	{				
		Console.Instance.OutputToConsole(string.Format("{0}: Starting up {1} {2}",  this.ToString(), ApplicationTitle, ApplicationVersion), Console.Instance.Style_Admin);
	}
	
	void Update()
	{
		switch (_gameState)
		{
			case GAMESTATE.LOADING:
			
				if (ComponentsLoaded())
				{
					SetGameState(GAMESTATE.RUNNING);
				}
									
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
		
			break;
		}
	}
	
	public void SetGameState(GAMESTATE state)
	{
		if (state == _gameState)
		{
			Console.Instance.OutputToConsole(string.Format("{0}: Attempted to set GameState to {1}, but GameState is already set to {2}", this.ToString(), state.ToString(), _gameState.ToString()), Console.Instance.Style_Admin);
			return;
		}
		
		switch(state)
		{
			case GAMESTATE.LOADING:
			
				switch (_gameState)
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
				
					break;
				}
			
			break;
			
			case GAMESTATE.INTRO:
			
				switch (_gameState)
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
				
					break;
				}
			
			break;
			
			case GAMESTATE.MAINMENU:
			
				switch (_gameState)
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
				
					break;
				}
			
			LevelManager.Instance.LoadLevel(MainMenuScene);
			
			break;
			
			case GAMESTATE.OPTIONS:
			
				switch (_gameState)
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
				
					break;
				}
			
			break;
			
			case GAMESTATE.RUNNING:
			
				switch (_gameState)
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
				
					break;
				}
			
			break;
			
			case GAMESTATE.PAUSED:
			
				switch (_gameState)
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
				
					break;
				}
			
			break;
			
			case GAMESTATE.CREDITS:
			
				switch (_gameState)
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
				
					break;
				}
			
			break;			
			
			default:				
			
			break;
		}
		
		
		Console.Instance.OutputToConsole(string.Format("{0}: Setting GameState to {1}. Previous state: {2}", this.ToString(), state.ToString(), _gameState.ToString()), Console.Instance.Style_Admin);
		EventManager.Instance.Post(new GameStateEvent(state, _gameState, this));
		_gameState = state;
		
		
	}
	
	private bool ComponentsLoaded()
	{		
		object[] singletons = FindObjectsOfType(typeof(Singleton<>));
		
		foreach(object st in singletons)
		{
			if (st == null)
			{
				return false;
			}
		}
		
		return true;
	}	
}
