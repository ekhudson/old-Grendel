using UnityEngine;
using System.Collections;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Mar 12, 2012
	/// 
	/// Filename: ConsoleCommands.cs
	/// 
	/// Summary: Defines all the available console commands
	/// 
	/// </summary>

public class ConsoleCommands : Singleton<ConsoleCommands>
{	
	void Quit(object[] parameters)
	{		
		Console.Instance.OutputToConsole("Quitting " + Application.absoluteURL, Console.Instance.Style_Admin);
		Application.Quit();
	}	
	
	void FPS(object[] parameters)
	{		
		if( FPSCounter.Instance.ShowFPS ) 
		{ 
			FPSCounter.Instance.ShowFPS = false;
			Console.Instance.OutputToConsole("FPS Counter Disabled" + Application.absoluteURL, Console.Instance.Style_Admin);
		} 
		else 
		{ 
			FPSCounter.Instance.ShowFPS = true;
			Console.Instance.OutputToConsole("FPS Counter Enabled" + Application.absoluteURL, Console.Instance.Style_Admin);
		}
	}	
	
	void Stats(object[] parameters)
	{
		FPS(parameters);
	}
	
	void NextTrack(object[] parameters)
	{
		AudioManager.Instance.IncrementMusicTrack(1);
		Console.Instance.OutputToConsole("Go to next music track.", Console.Instance.Style_Admin);
	}
	
	void PreviousTrack(object[] parameters)
	{
		AudioManager.Instance.IncrementMusicTrack(-1);
		Console.Instance.OutputToConsole("Go to previous music track.", Console.Instance.Style_Admin);
	}
	
	void PlayTrack(object[] parameters)
	{
		
	}
	
	void OutputLog(object[] parameters)
	{
		Debug.Log("here");
		ConsoleLog.OutputLog(Console.Instance.ConsoleCommandList, System.DateTime.Now.ToString("yymmdd HHmmss"));	
	}
	
	void Test(object[] parameters)
	{		
		if (parameters[0] == null || parameters[0] == " ")
		{
			Console.Instance.OutputToConsole("Parameter missing", Console.Instance.Style_Error);
		}
		else
		{
			Console.Instance.OutputToConsole("Success!", Console.Instance.Style_Admin);
		}
	}		
	
}//end class
