﻿using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class NotificationManager : MonoBehaviour {

	[Header("Panels")]
	public GameObject notificationPanel;

	[Header("Text Elements")]
	public Text notificationText;

	[Header("Notification")]
	public float alpha;
	public CanvasGroup cg;

	void Update(){
		if(alpha >= 0.075f * Time.deltaTime){
			alpha -= 0.075f * Time.deltaTime;
		}
		if(alpha < 0){
			alpha = 0;
		}
		cg.alpha = alpha;
	}

	public void ShowNotification(string notification){
		if(GameController.current.permissions != null && GameController.current.permissions.Contains("notifications.off")){
			return;
		}
		alpha = 1;
		notificationText.text = notification;
	}
}
