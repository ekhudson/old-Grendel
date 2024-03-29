using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	/// <summary>
	/// Title: Grendel Engine
	/// Author: Elliot Hudson
	/// Date: Apr 1, 2012
	/// 
	/// Filename: ScreenNotification.cs
	/// 
	/// Summary: Manages which notification is currently
	/// displayed, and how that notication fades out
	/// 
	/// TODO: Make notifications stack beneath each other if there
	/// is more than one
	/// 
	/// </summary>

public class ScreenNotificationManager : Singleton<ScreenNotificationManager> 
{
	
	private List<ScreenNotification> _notificationsList = new List<ScreenNotification>();		
	private ScreenNotification _currentNotification;
	private const float kNotificationHeight = 16f;
	private const float kSlideThreshold = 0.55f; //the alpha threshold below which the next notification begins to slide overtop of this one.
	
	
	public List<ScreenNotification> NotificationList
	{
		get {return _notificationsList;}
		set {_notificationsList = value;}
	}
	
	public void AddNotification(ScreenNotification notification)
	{		
		_notificationsList.Add(notification);	
	}
	
	public void RemoveNotification(ScreenNotification notification)
	{
		_notificationsList.Remove(notification);		
		_currentNotification = null;		
	}
	
	void OnGUI()
	{
		if (!GameManager.Instance.DebugBuild)
		{
			return;
		}
		
		if (_currentNotification == null && _notificationsList.Count > 0)
		{
			_currentNotification = _notificationsList[0];
			StartCoroutine( "HoldNotification" );
		}
		
		if (_currentNotification != null)
		{			
			if (!Console.Instance.ShowConsole)
			{ 
				//_currentNotification.DisplayNotification();
				foreach(ScreenNotification notification in NotificationList)
				{					
					GUILayout.Label(notification.Text, notification.NotificationStyle, 
					                new GUILayoutOption[]{ GUILayout.Width( (Screen.width * 0.50f) ),
						            GUILayout.Height( notification.NotificationStyle.normal.textColor.a <= 0.55f ? kNotificationHeight * (notification.NotificationStyle.normal.textColor.a / 0.55f) : kNotificationHeight)});					
				}
				
			}
		}
		else
		{
			StopCoroutine( "HoldNotification" );
			StopCoroutine( "FadeNotification" );
		}
	}
	
	IEnumerator HoldNotification()
	{		
		yield return new WaitForSeconds(NotificationList.Count > 1 ? _currentNotification.HoldTime * 0.25f : _currentNotification.HoldTime);
		StartCoroutine ( "FadeNotification" );
	}
	
	IEnumerator FadeNotification()
	{
		float fadeAmt = 1 / (_currentNotification.FadeTime * 60);		
		
		while(_currentNotification.BoxColor.a > 0)
		{			
			Color textColor = _currentNotification.NotificationStyle.normal.textColor;
			Color bgColor = _currentNotification.BoxColor;			
			
			textColor.a -= fadeAmt;
			bgColor.a -= fadeAmt;				
			
			_currentNotification.BoxTexture.SetPixel(1,1, bgColor);
			_currentNotification.BoxTexture.Apply();
			//_currentNotification.NotificationStyle.normal.background = _defaultTexture;
			_currentNotification.BoxColor = bgColor;	
			_currentNotification.NotificationStyle.normal.textColor = textColor;			
			
			yield return new WaitForSeconds(Time.deltaTime);
		}
		
		RemoveNotification(_currentNotification);	
	}
}
