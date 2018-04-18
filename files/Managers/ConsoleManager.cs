using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class ConsoleManager : MonoBehaviour {

	[Header("Panels")]
	public GameObject commandConsole;

	[Header("InputFields")]
	public InputField commandList; // This is where we will see our sent commands and responses
	public InputField commandText; // This is where we will enter our commands

	private string defaultConsoleText;

	void Start () {
		HideConsole ();
		defaultConsoleText = GameController.current.GetUserGroup ().ToUpper () + " " + GameController.current.GetUsername () + "@" + GameController.current.GetUserID () + ": \n";
	}

	public void SendCommand(){
		string cmd = commandText.text;

		if(string.IsNullOrEmpty(cmd)){
			return;
		}

		commandList.text = defaultConsoleText;

		commandList.text = commandList.text + "\n> " + cmd;
		commandText.text = "";

		if (cmd.ToLower ().StartsWith ("help")) {
			SendResponse (
				"CONSOLE COMMANDS:" +
				"\n" + "help - display list of commands." +
				"\n" + "p help - display list of permission commands." +
				"\n" + "g help - display list of group commands." +
				"\n" + "c help - display list of cypher commands." +
				"\n" + "user list - display list of database users." +
				"\n" + "system - display list of system information." +
				"\n" + "version - display current Ruthenium build." +
				"\n" + "clear - clears the console."
			);
		} else if (cmd.ToLower ().StartsWith ("p help")) {
			SendResponse (
				"PERMISSION COMMANDS:" +
				"\n" + "p help - display list of permission commands." +
				"\n" + "p list <user> - display <user>'s permissions." +
				"\n" + "p add <user> <permission> - add permission to <user>." +
				"\n" + "p remove <user> <permission> - remove permission from <user>." +
				"\n" + "p compare <user1> <user2> - list permissions both <user1> and <user2> have."
			);
		} else if (cmd.ToLower ().StartsWith ("g help")) {
			SendResponse (
				"GROUP COMMANDS:" +
				"\n" + "g help - display list of group commands." +
				"\n" + "g promote <user> - promote <user> to Admin." +
				"\n" + "g demote <user> - demote <user> to User."
			);
		} else if (cmd.ToLower ().StartsWith ("c help")) {
			SendResponse (
				"CYPHER COMMANDS:" +
				"\n" + "c help - display list of cypher commands." +
				"\n" + "c password <length> - generates a secure password."
			);
		} else if (cmd.ToLower ().StartsWith ("p list")) {
			// Use regex to split the string into two parts
			string user = Regex.Replace (cmd, "(.+\\ .+\\ )", "");

			// Is the string null, empty?
			if (user == " " || user == null || user == "") {
				return;
			}

			int c = 0;
			foreach (User u in GameController.current.dataBase) {
				if (u.Username == user) {
					foreach (string perm in u.Permissions) {
						SendResponse (perm);
					}
					c++;
					return;
				}
			}

			// No user with that name exists
			if (c == 0) {
				SendResponse ("\'" + user + "\' is not a valid user.");
				return;
			}
		} else if (cmd.ToLower ().StartsWith ("p add")) {
			string pattern = "p add (.+)\\ (.+)"; //(\w+)
			string input = cmd;
			string substitution_user = "$1";
			string substitution_perm = "$2";

			Regex regex_user = new Regex(pattern);
			string user = regex_user.Replace(input, substitution_user);

			Regex regex_perm = new Regex(pattern);
			string perm = regex_perm.Replace(input, substitution_perm);

			// Does the permission exist? (case sensitive)
			if (Permissions.IsValidPermission(perm) == false) {
				Debug.LogError ("PermissionsManager::AddPermission: Invalid permission \'<b>" + perm + "</b>\'.");
				Logger.WriteLog ("PermissionsManager::AddPermission: Invalid permission \'" + perm + "\'.");
				SendResponse ("Invalid permission: " + perm + ".");
				return;
			}

			// Do we have permission to add to OUR OWN permissions?
			if(!GameController.current.permissions.Contains("permissions.add.self") && ( GameController.current.GetUsername() == user )){
				string log1 = "PermissionsManager::DeletePermission: Failed to add permissions for \'" + user + "\'. (insufficient permissions)";
				Logger.WriteLog (log1);
				SendResponse ("Insufficient permissions: permissions.add.self.");
				return;
			}
			// Do we have permission to add to OTHERS permissions?
			if(!GameController.current.permissions.Contains("permissions.add.others") && ( GameController.current.GetUsername() != user )){
				string log1 = "PermissionsManager::DeletePermission: Failed to add permissions for \'" + user + "\'. (insufficient permissions)";
				Logger.WriteLog (log1);
				SendResponse ("Insufficient permissions: permissions.add.others.");
				return;
			}

			int c = 0;
			foreach(User u in GameController.current.dataBase){
				if(u.Username == user){
					if (Permissions.IsValidPermission (perm) && u.Permissions.Contains (perm) == false) {
						u.Permissions.Add (perm);
						SendResponse ("Added permission \'" + perm + "\' to \'" + user + "\'.");

						GameController.sl.StartSave ();
						GameController.lm.ReloadKeepWindows ();
					} else if (Permissions.IsValidPermission (perm) == false) {
						SendResponse ("Invalid permission \'" + perm + "\'.");
					} else if (u.Permissions.Contains (perm)) {
						SendResponse ("\'" + user + "\' already has permission \'" + perm + "\'.");
					}
					c++;
					return;
				}
			}

			// No user with that name exists
			if (c == 0) {
				SendResponse ("\'" + user + "\' is not a valid user.");
				return;
			}
		} else if (cmd.ToLower ().StartsWith ("p remove")) {
			string pattern = "p remove (.+)\\ (.+)";
			string input = cmd;
			string substitution_user = "$1";
			string substitution_perm = "$2";

			Regex regex_user = new Regex(pattern);
			string user = regex_user.Replace(input, substitution_user);

			Regex regex_perm = new Regex(pattern);
			string perm = regex_perm.Replace(input, substitution_perm);

			// Do we have permission to remove OUR OWN permissions?
			if(!GameController.current.permissions.Contains("permissions.remove.self") && ( GameController.current.GetUsername() == user )){
				string log1 = "ConsoleManager::SendCommand: Failed to delete permissions for \'" + user + "\'. (insufficient permissions)";
				Logger.WriteLog (log1);
				SendResponse ("Insufficient permissions: permissions.remove.self.");
				return;
			}
			// Do we have permission to remove OTHERS permissions?
			if(!GameController.current.permissions.Contains("permissions.remove.others") && ( GameController.current.GetUsername() != user )){
				string log1 = "ConsoleManager::SendCommand: Failed to delete permissions for \'" + user + "\'. (insufficient permissions)";
				Logger.WriteLog (log1);
				SendResponse ("Insufficient permissions: permissions.remove.others.");
				return;
			}

			int c = 0;
			foreach(User u in GameController.current.dataBase){
				if(u.Username == user){
					if (Permissions.IsValidPermission (perm) && u.Permissions.Contains(perm)) {
						u.Permissions.Remove (perm);
						SendResponse ("Removed permission \'" + perm + "\' from \'" + user + "\'.");

						GameController.sl.StartSave ();
						GameController.lm.ReloadKeepWindows ();
					} else if (Permissions.IsValidPermission (perm) == false){
						SendResponse ("Invalid permission \'" + perm + "\'.");
					} else if(u.Permissions.Contains(perm) == false){
						SendResponse ("\'" + user + "\' does not have permission \'" + perm + "\'.");
					}
					c++;
					return;
				}
			}

			// No user with that name exists
			if (c == 0) {
				SendResponse ("\'" + user + "\' is not a valid user.");
				return;
			}
		} else if (cmd.ToLower ().StartsWith ("p compare")) {
			string pattern = "p compare (.+)\\ (.+)";
			string input = cmd;
			string substitution_user1 = "$1";
			string substitution_user2 = "$2";

			Regex regex_user1 = new Regex(pattern);
			string user1 = regex_user1.Replace(input, substitution_user1);

			Regex regex_user2 = new Regex(pattern);
			string user2 = regex_user2.Replace(input, substitution_user2);

			int c1 = 0;
			int c2 = 0;
			List<string> user1perms = new List<string>();
			List<string> user2perms = new List<string>();
			List<string> sameperms = new List<string>();
			foreach(User u in GameController.current.dataBase){
				if (u.Username == user1) {
					c1++;
					user1perms = u.Permissions;
				}
				if(u.Username == user2){
					c2++;
					user2perms = u.Permissions;
				}
			}

			for (int i = 0; i < user1perms.Count; i++) {
				if (user2perms.Contains (user1perms[i]) && sameperms.Contains(user1perms[i]) == false) {
					sameperms.Add (user1perms [i]);
					SendResponse (user1perms [i]);
				}
			}
			for (int i = 0; i < user2perms.Count; i++) {
				if (user1perms.Contains (user2perms[i]) && sameperms.Contains(user2perms[i]) == false) {
					sameperms.Add (user2perms [i]);
					SendResponse (user2perms [i]);
				}
			}

			// No user with that name exists
			if (c1 == 0) {
				SendResponse ("\'" + user1 + "\' is not a valid user.");
				return;
			}
			if (c2 == 0) {
				SendResponse ("\'" + user2 + "\' is not a valid user.");
				return;
			}
		} else if (cmd.ToLower ().StartsWith ("g promote")) {
			string pattern = "g promote (.+)";
			string input = cmd;
			string substitution_user1 = "$1";

			Regex regex_user1 = new Regex(pattern);
			string user1 = regex_user1.Replace(input, substitution_user1);

			// Do we have permission to promote others?
			if(!GameController.current.permissions.Contains("group.promote")){
				string log1 = "ConsoleManager::SendCommand: Failed to promote \'" + user1 + "\'. (insufficient permissions)";
				Logger.WriteLog (log1);
				SendResponse ("Insufficient permissions: group.promote.");
				return;
			}

			int c1 = 0;
			foreach(User u in GameController.current.dataBase){
				if (u.Username == user1) {
					c1++;
					if (u.UserGroup != "Admin") {
						u.UserGroup = "Admin";
						SendResponse ("\'" + user1 + "\' has been promoted to Admin.");
						GameController.sl.StartSave ();
						GameController.lm.ReloadKeepWindows ();
					} else {
						SendResponse ("\'" + user1 + "\' is already Admin.");
					}
				}
			}

			// No user with that name exists
			if (c1 == 0) {
				SendResponse ("\'" + user1 + "\' is not a valid user.");
				return;
			}
		} else if (cmd.ToLower ().StartsWith ("g demote")) {
			string pattern = "g demote (.+)";
			string input = cmd;
			string substitution_user1 = "$1";

			Regex regex_user1 = new Regex(pattern);
			string user1 = regex_user1.Replace(input, substitution_user1);

			// Do we have permission to promote others?
			if(!GameController.current.permissions.Contains("group.demote")){
				string log1 = "ConsoleManager::SendCommand: Failed to demote \'" + user1 + "\'. (insufficient permissions)";
				Logger.WriteLog (log1);
				SendResponse ("Insufficient permissions: group.demote.");
				return;
			}

			int c1 = 0;
			foreach(User u in GameController.current.dataBase){
				if (u.Username == user1) {
					c1++;
					if (u.UserGroup != "User") {
						u.UserGroup = "User";
						GameController.sl.StartSave ();
						GameController.lm.ReloadKeepWindows ();
						SendResponse ("\'" + user1 + "\' has been demoted to User.");
					} else {
						SendResponse ("\'" + user1 + "\' is already User.");
					}
				}
			}

			// No user with that name exists
			if (c1 == 0) {
				SendResponse ("\'" + user1 + "\' is not a valid user.");
				return;
			}

		} else if (cmd.ToLower ().StartsWith ("c password")) {
			string pattern = "c password (.+)";
			string input = cmd;
			string substitution_length = "$1";

			Regex regex_user1 = new Regex(pattern);
			string length = regex_user1.Replace(input, substitution_length);
			int l = int.Parse (length);

			string result = "";
			string[] characterList = { 
				"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", 
				"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", 
				"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "`", "-", "=", "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "+",
				"[", "]", "\\", ";", "\'", ",", ".", "/", "{", "}", ":", "\"", "<", ">", "?"
			};			

			for (int i = 0; i < l; i++) {
				result = result + characterList [Random.Range (0, characterList.Length)];
			}

			SendResponse (result);
			return;
				
		} else if (cmd.ToLower ().StartsWith ("system")) {
			SendResponse ("operatingSystem: " + SystemInfo.operatingSystem);
			SendResponse ("operatingSystemFamily: " + SystemInfo.operatingSystemFamily);
			SendResponse ("deviceUniqueIdentifier: " + SystemInfo.deviceUniqueIdentifier);
			SendResponse ("deviceName: " + SystemInfo.deviceName);
			SendResponse ("deviceModel: " + SystemInfo.deviceModel);
			SendResponse ("deviceType: " + SystemInfo.deviceType);
			SendResponse ("graphicsDeviceName: " + SystemInfo.graphicsDeviceName);
			SendResponse ("graphicsDeviceType: " + SystemInfo.graphicsDeviceType);
			SendResponse ("graphicsDeviceVendor: " + SystemInfo.graphicsDeviceVendor);
			SendResponse ("graphicsDeviceID: " + SystemInfo.graphicsDeviceID);
			SendResponse ("graphicsMemorySize: " + SystemInfo.graphicsMemorySize + " MB");
			SendResponse ("graphicsMultiThreaded: " + SystemInfo.graphicsMultiThreaded);
			SendResponse ("processorCount: " + SystemInfo.processorCount);
			return;
		} else if (cmd.ToLower ().StartsWith ("version")) {
			SendResponse (Version.GetVersion ());
			if(Version.GetCurrentVersion() != Version.GetVersion()){
				SendResponse ("Version \'" + Version.GetCurrentVersion() + "\' is out of date. You can get the latest version at https://github.com/KithM/Ruthenium/releases.");
			}
			return;
		} else if (cmd.ToLower ().StartsWith ("user list")) {
			foreach (User user in GameController.current.dataBase) {
				Debug.Log (user.Username);

				if(user.Permissions == null){
					user.Permissions = new List<string>();
				}
				if(user.Permissions.Contains("user.type.hidden") && !GameController.current.permissions.Contains("settings.see.hidden")){
					continue;
				}

				if (user.Username != GameController.current.GetUsername ()){
					SendResponse (user.Username + " (" + user.UserGroup + ")");
				} else if (user.Username == GameController.current.GetUsername ()){
					SendResponse ("[" + user.Username + "]" + " (" + user.UserGroup + ")");
				}

			}
		} else if (cmd.ToLower ().StartsWith ("clear")) {
			commandList.text = defaultConsoleText;
			return;
		} else {
			SendResponse ("\'" + cmd + "\' is not a valid command.");
			return;
		}
	}

	public void SendResponse(string res){
		commandList.Select ();
		commandList.text = commandList.text + "\n" + res;
	}

	public void ShowConsole(){
		commandList.text = defaultConsoleText;
		commandConsole.SetActive (true);
	}
	
	public void HideConsole(){
		commandText.text = "";
		commandConsole.SetActive (false);
	}
}
