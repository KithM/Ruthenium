using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Permissions {
	// This class will be used to hold all the permissions in the game. 
	// TODO: Load from xml / text file.

	public static string[] list = { 
		"User.Type.Permanent", // User cannot be deleted
		"User.Type.Hidden", // User is invisible to everyone without Admin.See.Hidden
		"Settings.See.Hidden", // User can see users with that are hidden (User.Type.Hidden)
		"Settings.Manage.Self", // User can change their username, password, and open the settings menu
		"Settings.Manage.Others", // TODO: User can change other users' usernames, passwords, and change their settings
		"Permissions.Add.Self", // User can add their own permissions
		"Permissions.Remove.Self", // User can delete their own permissions
		"Permissions.Add.Others", // TODO: User can delete other users' permissions
		"Permissions.Remove.Others", // TODO: User can delete other users' permissions
		"Notifications.Off" // User cannot see notifications. Typically should never be turned off except for testing
	};

	public static bool IsValidPermission(string perm){
		foreach (string x in list) {
			if(x == perm){
				return true;
				break;
			}
		}

		return false;
	}

}
