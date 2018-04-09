using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User {
	
	/// Username of the user.
	public string Username;

	/// Password of the user.
	public string Password;

	/// Privelages for the user. Can be either User or Admin and defaults to User.
	public string UserGroup;

	/// Permissions of the user. Should be located in the README file for reference.
	public List<string> Permissions;

	/// Personal bio or information about the user.
	public string Bio;

	/// Initializes a new instance of the User class.
	public User (string username, string password, string bio = "This user does not have a bio.", string usergroup = "User", List<string> permissions = null)
	{
		Debug.Log ("<b>User created. </b> Username: " + username + ", Password: " + password + ".");

		this.Username = username;
		this.Password = password;
		this.Bio = bio;
		this.UserGroup = usergroup;
		this.Permissions = permissions;
	}

}
