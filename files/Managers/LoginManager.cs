using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class LoginManager : MonoBehaviour {

	[Header("Panels")]
	public GameObject loginPanel;
	public GameObject desktopPanel;

	[Header("InputFields")]
	public InputField username;
	public InputField password;

	[Header("Misc")]
	public Image usernameI;
	public Image passwordI;

	[Header("Text")]
	public Text warningText;
	public Text userText;

	// Use this for initialization
	void Start () {
		warningText.text = "";
		desktopPanel.SetActive (false);
	}

	public void Login(){
		if(username.text == "" && password.text == ""){
			warningText.text = "Username and password are required.";
		} else if(username.text == ""){
			warningText.text = "Username is required.";
		} else if(password.text == ""){
			warningText.text = "Password is required.";
		}

		int count = 0;

		for(int i = 0; i < GameController.current.dataBase.Count; i++){
			if(GameController.current.dataBase[i].Username == username.text) {
				count++;
			}
		}

		if(count == 0){
			warningText.text = "User does not exist.";

			string log = "LoginManager::Login: Failed to log in as \'" + username.text + "\'. (user not found)";
			Logger.WriteLog (log);
		}

		foreach(User user in GameController.current.dataBase){
			string databaseusername = user.Username;
			string databasepassword = user.Password;
			string databasebio = user.Bio;
			string databaseprivelages = user.UserGroup;
			List<string> databasepermissions = user.Permissions;

			if(username.text == databaseusername && password.text == databasepassword){
				GameController.nm.ShowNotification ("Welcome, " + databaseusername);
				Debug.Log ("LoginManager::Login: Login successful with username \'<b>" + databaseusername + "</b>\'.");
				warningText.text = "";
				userText.text = databaseusername;

				// Change our null username and password to the appropriate values
				GameController.current.SetUsername (databaseusername);
				GameController.current.SetPassword (databasepassword);
				GameController.current.SetBio (databasebio);
				GameController.current.SetUserGroup (databaseprivelages);

				if (databasepermissions != null) {
					foreach (string permission in databasepermissions) {
						GameController.current.AddPermission (permission);
					}
				}

				// Update our UserID to match our current username Hash Code
				GameController.current.SetUserID(GameController.current.GetUsername().GetHashCode());
				
				// At this point, all of our information is up to date
				// Switch from our login panel to the desktop screen
				loginPanel.SetActive (false);
				desktopPanel.SetActive (true);

				// Finally, write out some basic info to the debug.log file
				string log = "LoginManager::Login: Logged in as \'" + GameController.current.username + "\'.";
				Logger.WriteLog (log);
				return;

			} else if(username.text == databaseusername){
				warningText.text = "Incorrect password.";

				string log = "LoginManager::Login: Failed to log in as \'" + databaseusername + "\'. (incorrect password)";
				Logger.WriteLog (log);
			}

					
		}
	}

	public void Register(){
		string u = username.text;
		string p = password.text;

		if(u == "" && p == ""){
			warningText.text = "Username and password are required.";
			return;
		} else if(u == ""){
			warningText.text = "Username is required.";
			return;
		} else if(p == ""){
			warningText.text = "Password is required.";
			return;
		}

		if(u.Length > 25){
			warningText.text = "Username is too long (" + u.Length + " char).";
			return;
		}
		if(p.Length > 50){
			warningText.text = "Password is too long (" + p.Length + " char).";
			return;
		}

		int count = 0;

		for(int i = 0; i < GameController.current.dataBase.Count; i++){
			if(GameController.current.dataBase[i].Username == username.text) {
				count++;
			}
		}

		if(count > 0){
			warningText.text = "User already exists.";

			string log = "LoginManager::Register: Failed to create account with the username \'" + u + "\'. (user already exists)";
			Logger.WriteLog (log);

			return;
		}

		Debug.Log ("LoginManager::Register: Register successful with username \'<b>" + u + "</b>\'.");
		warningText.text = "";
		userText.text = u;

		// Change our null username and password to the appropriate values
		GameController.current.SetUsername (u);
		GameController.current.SetPassword (p);
		GameController.current.SetBio ("This user does not have a bio.");
		GameController.current.SetUserGroup ("User");

		string log1 = "LoginManager::Register: Register successful with username \'" + u + "\'.";
		Logger.WriteLog (log1);

		User user1 = new User (u, p);

		GameController.current.dataBase.Add (user1);
		GameController.sl.StartSave ();

		ShowDesktop ();
	}

	public void Logout(){
		string u = GameController.current.GetUsername ();

		GameController.nm.ShowNotification ("Goodbye, " + u);
		Debug.Log ("LoginManager::Logout: Logged out of the account \'<b>" + u + "</b>\'.");

		WindowManager.CloseAllWindows ();

		// Nullify our values so we don't have any conflicts
		GameController.current.SetUsername (null);
		GameController.current.SetPassword (null);
		GameController.current.SetBio (null);
		GameController.current.ClearPermissions ();
		GameController.current.SetUserGroup (null);

		string log1 = "LoginManager::Logout: Logged out of the account \'" + u + "\'.";
		Logger.WriteLog (log1);

		ShowLogin ();
	}

	public void ShowDesktop(){
		// Switch from our desktop screen to the login panel
		loginPanel.SetActive (false);
		desktopPanel.SetActive (true);
	}
	public void ShowLogin(){
		// Empty our Username and Password fields
		username.text = "";
		password.text = "";

		// Switch from our login panel to the desktop screen
		loginPanel.SetActive (true);
		desktopPanel.SetActive (false);
	}

	public void DeleteCurrentUser(){
		if(GameController.current.permissions.Contains("user.type.permanent")){
			string log1 = "LoginManager::DeleteCurrentUser: Failed to delete the account \'" + GameController.current.GetUsername() + "\'. (insufficient permissions)";
			Logger.WriteLog (log1);
			GameController.nm.ShowNotification ("Insufficient permissions: user.type.permanent users are not deletable.");
			return;
		}
		if(!GameController.current.permissions.Contains("settings.manage.self")){
			string log1 = "LoginManager::DeleteCurrentUser: Failed to delete the account \'" + GameController.current.GetUsername() + "\'. (insufficient permissions)";
			Logger.WriteLog (log1);
			GameController.nm.ShowNotification ("Insufficient permissions: settings.manage.self.");
			return;
		}

		for(int i = 0; i < GameController.current.dataBase.Count; i++){
			if(GameController.current.dataBase[i].Username == GameController.current.GetUsername()) {
				GameController.current.dataBase.Remove(GameController.current.dataBase[i]);
				GameController.sl.StartSave ();

				string log1 = "LoginManager::DeleteCurrentUser: Deleted the account \'" + GameController.current.GetUsername() + "\'.";
				Logger.WriteLog (log1);
				GameController.nm.ShowNotification ("Deleting  " + GameController.current.GetUsername());

				Logout ();
				break;
			}
		}

	}

	public void Reload(){
		WindowManager.CloseAllWindows ();
		ReloadGame ();
	}

	public void ReloadKeepWindows(){
		ReloadGame ();
	}

	void ReloadGame(){
		// Instead of creating a list of permissions equal to our old ones, we want to set our permissions equal to our exact user's permissions
		GameController.sl.StartLoad ();

		int count = 0;
		foreach(User user in GameController.current.dataBase){
			if(user.Username == GameController.current.GetUsername ()) {
				count++;

				// The user still exists so set our null permissions equal to our user's perms
				GameController.current.SetPermissions (user.Permissions);

				break;
			}
		}

		GameController.nm.ShowNotification ("Reloading.");

		if(count < 1){
			//The user no longer exists, so we should logout
			Logout ();
		}
	}
		
	// Here should be the normal "DeleteUser" function ,for deleting specific users

	public void ExitGame(){
		Application.Quit ();
	}
}
