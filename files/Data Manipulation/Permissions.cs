using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Permissions {
	// This class will be used to hold all the permissions in the game. 
	// TODO: Load from xml / text file.

	public static string[] list = { 
		"user.type.permanent", // User cannot be deleted
		"user.type.hidden", // User is invisible to everyone without Admin.See.Hidden
		"settings.see.hidden", // User can see users with that are hidden (User.Type.Hidden)
		"settings.manage.self", // User can change their username, password, and open the settings menu
		"settings.manage.others", // TODO: User can change other users' usernames, passwords, and change their settings
		"permissions.add.self", // User can add their own permissions
		"permissions.remove.self", // User can delete their own permissions
		"permissions.add.others", // TODO: User can delete other users' permissions
		"permissions.remove.others", // TODO: User can delete other users' permissions
		"notifications.off" // User cannot see notifications. Typically should never be turned off except for testing
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
