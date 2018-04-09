using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Users : MonoBehaviour {

	[Header("Panels")]
	public GameObject usersPanel;

	[Header("Text Elements")]
	public Text usersList;

	// Use this for initialization
	void Start () {
		HideMenu ();
	}

	public void ShowMenu () {
		usersPanel.SetActive (true);

		usersList.text = "<b>Registered Users (Public only): </b>\n\n";
		int n = 0;

		foreach (User user in GameController.current.dataBase) {
			if(user.Permissions.Contains("User.Type.Hidden") && !GameController.current.permissions.Contains("Settings.See.Hidden")){
				continue;
			}
			if (n > 0 && user.Username != GameController.current.GetUsername ()) {
				usersList.text = usersList.text + ", " + user.Username + " (" + user.UserGroup + ")"; //•
			} else if (n > 0 && user.Username == GameController.current.GetUsername ()){
				usersList.text = usersList.text + ", <b>" + user.Username + "</b> (" + user.UserGroup + ")";
			} else if (user.Username != GameController.current.GetUsername ()){
				usersList.text = usersList.text + "" + user.Username + " (" + user.UserGroup + ")";
			} else if (user.Username == GameController.current.GetUsername ()){
				usersList.text = usersList.text + "<b>" + user.Username + "</b> (" + user.UserGroup + ")";
			}

			n++;
		}
	}
	public void HideMenu () {
		usersPanel.SetActive (false);
	}
}
