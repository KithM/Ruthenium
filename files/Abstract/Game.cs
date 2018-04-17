using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

public class Game : IXmlSerializable {

	public string username { get; protected set; } // Our username is only filled in when we log in
	public string password { get; protected set; } // Our password is only filled in when we log in
	public int userID { get; protected set; } // Our userID holds our username hash
	public string bio { get; protected set; } // The Biography of the user, or whatever the user sets it to
	public string userGroup { get; protected set; } // TODO: Our user group. Can only be User or Admin
	public List<string> permissions { get; protected set; } // Our user privelages. Will hold all of our special permissions we can do when we are logged in

	// Our database is filled in every time we start the game using the XML file. It holds our username and our associated password, and
	// anything else we need for our user
	public List<User> dataBase;

	public Game (string username, string password){

	}

	// Default public constructor for saving the game
	public Game(){
		
	}

	////////////////////////////////////////////////////////////////////////////////
	////////////////////////////// SAVING AND LOADING //////////////////////////////
	////////////////////////////////////////////////////////////////////////////////

	public void LoadGame(string fileName) {
		Debug.Log("Game::LoadGame");

		dataBase = new List<User> ();
		permissions = new List<string> ();

		string xmlText = System.IO.File.ReadAllText (fileName);
		Debug.Log(xmlText);

		XmlTextReader reader = new XmlTextReader( new StringReader( xmlText ) );
		ReadXml (reader);
	}

	public XmlSchema GetSchema(){
		return null;
	}

	public void WriteXml(XmlWriter writer){
		Debug.Log ("Game::WriteXml");

		if (dataBase == null) {
			dataBase = new List<User> ();
		}

		if(permissions == null){
			permissions = new List<string> ();
		}

		// Version and game info, only put into the data file once
		writer.WriteAttributeString ("update", Version.GetVersion().ToString());

		if(dataBase.Count > 0){
			// WE HAVE ATLEAST 1 USER IN THE DATA.RUTH FILE
			foreach(User user in dataBase){
				string username = user.Username;
				string password = user.Password;
				string bio = user.Bio;
				string usergroup = user.UserGroup;

				// User Start
				writer.WriteStartElement("User");

				// Username
				writer.WriteStartElement("Username");
				writer.WriteString (username);
				writer.WriteEndElement ();

				// Password
				writer.WriteStartElement("Password");
				writer.WriteString (password);
				writer.WriteEndElement ();

				// Bio
				writer.WriteStartElement("Bio");
				writer.WriteString (bio);
				writer.WriteEndElement ();

				// Privelages
				writer.WriteStartElement("UserGroup");
				writer.WriteString (usergroup);
				writer.WriteEndElement ();

				// Permissions
				writer.WriteStartElement("Permissions");
				if (user.Permissions != null && user.Permissions.Count > 0) {
					foreach (string perm in user.Permissions) {
						writer.WriteStartElement ("Permission");
						writer.WriteString (perm);
						writer.WriteEndElement ();
					}
				}
				writer.WriteEndElement ();

				// User End
				writer.WriteEndElement();
			}
		} else {
			// DEFAULT (NO FILE)
			User user1 = new User ("default", "password");

			List<string> adminperms = new List<string> ();
			adminperms.Add ("permissions.add.self");
			adminperms.Add ("permissions.remove.self");
			adminperms.Add ("permissions.add.others");
			adminperms.Add ("permissions.remove.others");

			User user2 = new User ("admin", "password", "This user does not have a bio.", "Admin", adminperms);
			dataBase.Add (user1);
			dataBase.Add (user2);

			foreach (User user in dataBase) {
				string username = user.Username;
				string password = user.Password;
				string bio = user.Bio;
				string usergroup = user.UserGroup;

				// User Start
				writer.WriteStartElement ("User");

				// Username
				writer.WriteStartElement ("Username");
				writer.WriteString (username);
				writer.WriteEndElement ();

				// Password
				writer.WriteStartElement ("Password");
				writer.WriteString (password);
				writer.WriteEndElement ();

				// Bio
				writer.WriteStartElement ("Bio");
				writer.WriteString (bio);
				writer.WriteEndElement ();

				// Privelages
				writer.WriteStartElement ("UserGroup");
				writer.WriteString (usergroup);
				writer.WriteEndElement ();

				// Permissions
				writer.WriteStartElement ("Permissions");
				if (user.Permissions != null && user.Permissions.Count > 0) {
					foreach (string perm in user.Permissions) {
						writer.WriteStartElement ("Permission");
						writer.WriteString (perm);
						writer.WriteEndElement ();
					}
				}
				writer.WriteEndElement ();

				// User End
				writer.WriteEndElement ();
			}
		}
	}
	public void ReadXml(XmlReader reader){
		// TODO: EVERYTHING!!!!!
		bool doSaveAfterLoad = false;

		Debug.Log ("Game::ReadXml");
		reader.Read ();

		while( reader.Read () ){

			// Check the user's database version. Make sure it is up to date. If not, let them know
			string readVersion = reader.GetAttribute ("update");

			if (readVersion != Version.GetVersion ().ToString() && readVersion != null && readVersion != "") {
				Debug.LogError ("Game::ReadXml: Version \'<b>" + readVersion + "</b>\' is out of date. Make sure the data file is updated to most recent version.");
				Logger.WriteLog ("Game::ReadXml: Version \'" + readVersion + "\' is out of date. Version should be update \'" + Version.GetVersion ().ToString() + "\'.");
				GameController.nm.ShowNotification ("Ruthenium <b>" + readVersion + "</b> is out of date. Update to the newest version, <b>" + Version.GetVersion() + "</b>.");
				Version.SetCurrentVersion(readVersion);
			} else if (readVersion != null && readVersion != ""){
				Debug.Log ("Game::ReadXml: Version \'<b>" + readVersion + "</b>\' loaded successfully.");
				GameController.nm.ShowNotification ("Ruthenium <b>" + Version.GetVersion() + "</b> loaded successfully.");
				Version.SetCurrentVersion(readVersion);
			}

			switch (reader.Name) {
			case "User":
				// Each time we hit the <User> or </User> tag, we check if we have a following <Username> or <Password> tag.
				// If we don't, we simply just go to the next <User> tag.
				//Debug.Log (reader.Name);
				string xmlusername = null;
				string xmlpassword = null;
				string xmlbio = null;
				string xmlprivelages = null;
				List<string> xmlpermissions = new List<string> ();

				if (reader.ReadToDescendant ("Username")) {
					xmlusername = reader.ReadString ();
					if(xmlusername.Length > 25){
						Debug.LogError ("Game::ReadXml: The specified username \'<b>" + xmlusername + "</b>\' is too long (" + xmlusername.Length + " char).");
						Logger.WriteLog ("Game::ReadXml: The specified username \'" + xmlusername + "\' is too long (" + xmlusername.Length + " char).");
						xmlusername = xmlusername.Substring (0, 25);

						doSaveAfterLoad = true;
					}
				}
				if (reader.ReadToNextSibling ("Password")) {
					xmlpassword = reader.ReadString ();
					if(xmlpassword.Length > 50){
						Debug.LogError ("Game::ReadXml: The specified password for user \'<b>" + xmlusername + "</b>\' is too long (" + xmlpassword.Length + " char).");
						Logger.WriteLog ("Game::ReadXml: The specified password for user \'" + xmlusername + "\' is too long (" + xmlpassword.Length + " char).");
						xmlpassword = xmlpassword.Substring (0, 50);

						doSaveAfterLoad = true;
					}
				}
				if (reader.ReadToNextSibling ("Bio")) {
					xmlbio = reader.ReadString ();
					if(xmlbio.Length > 300){
						Debug.LogError ("Game::ReadXml: The specified bio for user \'<b>" + xmlusername + "</b>\' is too long (" + xmlbio.Length + " char).");
						Logger.WriteLog ("Game::ReadXml: The specified bio for user \'" + xmlusername + "\' is too long (" + xmlbio.Length + " char).");
						xmlbio = xmlbio.Substring (0, 300);

						doSaveAfterLoad = true;
					}
				}
				if (reader.ReadToNextSibling ("UserGroup")) {
					xmlprivelages = reader.ReadString ();
				}
				if (reader.ReadToNextSibling ("Permissions")) {

					if (reader.ReadToDescendant ("Permission")) {
						xmlpermissions.Add (reader.ReadString ());

						while(reader.ReadToNextSibling("Permission")){
							xmlpermissions.Add (reader.ReadString ());
						}
					}

					Debug.Log ("Game::ReadXml: " + xmlpermissions.Count + " permissions loaded.");

					User user = new User (xmlusername, xmlpassword, xmlbio, xmlprivelages, xmlpermissions);
					dataBase.Add (user);
					//Debug.Log ("Username: " + xmlusername + "\nPassword: " + reader.ReadString ());
				}

				// Continue the loop until we get all the users in the database
				reader.ReadToNextSibling ("User");
				continue;
			}

		}

		reader.Close ();

		if (doSaveAfterLoad == true) {
			// TODO: May need to move this?
			GameController.sl.StartSave ();
		}

		Debug.Log ("Game::ReadXml: Successfully loaded the data file \'<b>data.ruth</b>\' with " + dataBase.Count + " users.");

		string log1 = "Game::ReadXml: Successfully loaded the data file \'data.ruth\' with " + dataBase.Count + " users.";
		Logger.WriteLog (log1);

		foreach(User user in dataBase){
			string username1 = user.Username;
			string password1 = user.Password;

			Debug.Log ("Username: " + username1 + "\nPassword: " + password1);
		}
			
	}

	////////////////////////////////////////////////////////////////////////////////
	///////////////////////////// GETTING AND SETTING //////////////////////////////
	////////////////////////////////////////////////////////////////////////////////

	public string GetUsername(){
		return username;
	}
	public string GetBio(){
		return bio;
	}
	public string GetUserGroup(){
		return userGroup;
	}
	public string GetPassword(){
		return password;
	}
	public int GetUserID(){
		return userID;
	}
	public List<string> GetPermissions(){
		return permissions;
	}

	public void SetUsername(string input){
		username = input;
		Debug.Log ("Game::SetUsername: Setting username to \'<b>" + input + "</b>\'.");
	}
	public void SetPassword(string input){
		password = input;
		Debug.Log ("Game::SetPassword: Setting password to \'<b>" + input + "</b>\'.");
	}
	public void SetBio(string input){
		bio = input;
		Debug.Log ("Game::SetBio: Setting bio to \'<b>" + input + "</b>\'.");
	}
	public void SetUserGroup(string input){
		if (input == "User" || input == "Admin") {
			userGroup = input;
			Debug.Log ("Game::SetUserGroup: Setting userGroup to \'<b>" + userGroup + "</b>\'.");
		} else {
			userGroup = "User";
			Debug.Log ("Game::SetUserGroup: Setting userGroup to \'<b>" + userGroup + "</b>\'. (UserGroup was not either User or Admin when loading.)");

			string log1 = "Game::SetUserGroup: Setting userGroup to \'" + userGroup + "\'. (UserGroup was not either User or Admin when loading.)";
			Logger.WriteLog (log1);
		}
	}
	public void SetUserID(int id){
		userID = id;
		Debug.Log ("Game::SetUserID: Setting userID to \'<b>" + id + "</b>\'.");
	}

	// PERMISSIONS
	public void SetPermissions(List<string> newpermissions){
		permissions = newpermissions;
	}
	public void AddPermission(string permission){
		// This first if statement will check if our Permissions class holds our permission (case sensitive)
		if (Permissions.IsValidPermission(permission) == false) {
			Debug.LogError ("Game::AddPermission: Invalid permission \'<b>" + permission + "</b>\'.");
			Logger.WriteLog ("Game::AddPermission: Invalid permission \'" + permission + "\'.");
			GameController.nm.ShowNotification ("Invalid permission: " + permission + ".");
			return;
		}

		if(permissions.Contains(permission)){
			Debug.Log ("Game::AddPermission: Failed to add permission \'<b>" + permission + "</b>\' for user \'<b>" + GetUsername() + "</b>\'. (permission already exists)");
			Logger.WriteLog ("Game::AddPermission: Failed to add permission \'" + permission + "\' for user \'" + GetUsername() + "\'. (permission already exists)");
			return;
		}
		permissions.Add(permission);
		Debug.Log ("Game::AddPermission: Adding permission \'<b>" + permission + "</b>\' for user \'<b>" + GetUsername() + "</b>\'.");
		Logger.WriteLog ("Game::AddPermission: Adding permission \'" + permission + "\' for user \'" + GetUsername() + "\'.");
	}
	public void ClearPermissions(){
		Debug.Log ("Game::ClearPermissions: Removing all " + permissions.Count + " permissions for user \'<b>" + GetUsername() + "</b>\'.");
		Logger.WriteLog ("Game::ClearPermissions: Removing all " + permissions.Count + " permissions for user \'" + GetUsername() + "\'.");
		permissions.Clear();
	}

	/////// WRITING EXTRA INFO ///////

	public void WriteInfo(string info){

		string filePath = System.IO.Path.Combine ( Application.persistentDataPath, "README.info" );

		if(File.Exists(filePath) && File.ReadAllText(filePath).Contains(info)){
			//The file already exists and the information is up to date, so we can return
			return;
		}

		// At this point, filePath should look much like:
		// C:\Users\username\ApplicationData\Orion Games\Space Builder\Saves\SaveGame123.sav

		StreamWriter writer1 = new StreamWriter(filePath, false);
		//File.WriteAllText (filePath, info);

		if (info != null) {
			writer1.WriteLine (info);
		}

		writer1.Close();
	}

	public void WriteReadme(){
		string info = 
			"README CONTENTS (Version " + Version.GetVersion().ToString() + ")\r\n" + 
			"1. Modification\r\n" + 
			"2. User Permissions\r\n" +
			"3. User Groups\r\n\r\n" + 
			"GitHub Repository: https://github.com/KithM/Ruthenium\r\n" +
			"Latest Build: https://github.com/KithM/Ruthenium/releases\r\n" +
			"Latest Stable: https://github.com/KithM/Ruthenium/releases/latest\r\n\r\n" +
			"MODIFICATION\r\n" + 
			"To modify the game, open any .ruth file (located in the Data folder) in a text editor and change any values as you wish. " +
			"However, note that modifications could cause the game to crash or stop working, so make sure to back up the game first. " +
			"For debugging purposes, you can find and see what the game is doing by reading the debug.log file. " +
			"You can always delete the file if it gets too large.\r\n\r\n" + 
			"USER PERMISSIONS\r\n" + 
			"Permissions for a user can be set by opening the permissions start tag and adding an end tag, " +
			"similar in how the Username and Password appear in the file. " +
			"You can then add in as many permissions that are available in the game.\r\n\r\n" + 
			"Permissions: " + PermissionsToList() + "\r\n\r\n" + 
			"USER GROUPS\r\n" + 
			"UserGroups are simple groups that allow or restrict certain actions performed by the user, similar to user permissions. " +
			"By default, only the UserGroups 'User' and 'Admin' exist. Users with the 'User' UserGroup are given basic permissions, " +
			"while 'Admin' Users are given administrative permissions, such as deleting accounts, changing passwords, and so on.\r\n\r\n" +
			"Created by Orion Games (c) " + System.DateTime.Now.Year + ".";

		WriteInfo (info);
	}

	public string PermissionsToList(){
		string permissionsstring = null;

		permissionsstring = "";
		foreach (string perm in Permissions.list) {
			if (permissionsstring != "") {
				permissionsstring = permissionsstring + ", " + perm;
			} else {
				permissionsstring = perm;
			}
		}

		return permissionsstring;
	}
}
