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
			if(user.Permissions == null){
				user.Permissions = new List<string>();
			}
			if(user.Permissions.Contains("user.type.hidden") && !GameController.current.permissions.Contains("settings.see.hidden")){
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
