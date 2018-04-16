using System.Collections;
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

	void Start () {
		HideConsole ();
	}

	public void SendCommand(){
		string cmd = commandText.text;

		if(cmd == null || cmd == ""){
			return;
		}

		commandList.text = commandList.text + "\n> " + cmd;
		commandText.text = "";

		if (cmd.ToLower ().StartsWith ("help")) {
			SendResponse (
				"CONSOLE COMMANDS:" +
				"\n" + "help - display list of commands." +
				"\n" + "p help - display list of permission commands." +
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
		} else if (cmd.ToLower ().StartsWith ("p list")) {
			// Use regex to split the string into two parts
			string user = Regex.Replace (cmd, "(\\w+\\ \\w+\\ )", "");

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

			int c = 0;
			foreach(User u in GameController.current.dataBase){
				if(u.Username == user){
					if (Permissions.IsValidPermission (perm) && u.Permissions.Contains (perm) == false) {
						u.Permissions.Add (perm);
						SendResponse ("Added permission \'" + perm + "\' to \'" + user + "\'.");
						GameController.sl.StartSave ();
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

			int c = 0;
			foreach(User u in GameController.current.dataBase){
				if(u.Username == user){
					if (Permissions.IsValidPermission (perm) && u.Permissions.Contains(perm)) {
						u.Permissions.Remove (perm);
						SendResponse ("Removed permission \'" + perm + "\' from \'" + user + "\'.");
						GameController.sl.StartSave ();
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

		} else if (cmd.ToLower ().StartsWith ("system")) {
			SendResponse ("operatingSystem: " + SystemInfo.operatingSystem.ToString());
			SendResponse ("operatingSystemFamily: " + SystemInfo.operatingSystemFamily);
			SendResponse ("deviceUniqueIdentifier: " + SystemInfo.deviceUniqueIdentifier.ToString());
			SendResponse ("deviceName: " + SystemInfo.deviceName.ToString());
			SendResponse ("deviceModel: " + SystemInfo.deviceModel.ToString());
			SendResponse ("deviceType: " + SystemInfo.deviceType.ToString());
			SendResponse ("graphicsDeviceName: " + SystemInfo.graphicsDeviceName.ToString());
			SendResponse ("graphicsDeviceType: " + SystemInfo.graphicsDeviceType.ToString());
			SendResponse ("graphicsDeviceVendor: " + SystemInfo.graphicsDeviceVendor.ToString());
			SendResponse ("graphicsDeviceID: " + SystemInfo.graphicsDeviceID);
			SendResponse ("graphicsMemorySize: " + SystemInfo.graphicsMemorySize + " MB");
			SendResponse ("graphicsMultiThreaded: " + SystemInfo.graphicsMultiThreaded);
			SendResponse ("processorCount: " + SystemInfo.processorCount.ToString());
			return;
		} else if (cmd.ToLower ().StartsWith ("version")) {
			SendResponse (Version.GetVersion ());
			Application.OpenURL("https://github.com/KithM/Ruthenium/releases");
			return;
		} else if (cmd.ToLower ().StartsWith ("user list")) {
			foreach (User u in GameController.current.dataBase) {
				SendResponse (u.Username);
				return;
			}
		} else if (cmd.ToLower ().StartsWith ("clear")) {
			commandList.text = GameController.current.GetUserGroup().ToUpper() + " " + GameController.current.GetUsername() + "@" + GameController.current.GetUserID() + ": \n";
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
		commandList.text = GameController.current.GetUserGroup().ToUpper() + " " + GameController.current.GetUsername() + "@" + GameController.current.GetUserID() + ": \n";
		commandConsole.SetActive (true);
	}
	
	public void HideConsole(){
		commandText.text = "";
		commandConsole.SetActive (false);
	}
}
