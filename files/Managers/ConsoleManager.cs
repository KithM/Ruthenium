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
				"\n" + "p remove <user> <permission> - remove permission from <user>."
			);
		} else if (cmd.ToLower ().StartsWith ("p list")) {
			// Use regex to split the string into two parts
			string ms = Regex.Replace (cmd, "(\\w+\\ \\w+\\ )", "");

			// Is the string null, empty?
			if (ms == " " || ms == null || ms == "") {
				return;
			}

			int c = 0;
			foreach (User u in GameController.current.dataBase) {
				if (u.Username == ms) {
					foreach (string perm in u.Permissions) {
						SendResponse (perm);
					}
					c++;
					return;
				}
			}

			// No user with that name exists
			if (c == 0) {
				SendResponse ("\'" + ms + "\' is not a valid user.");
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
		} else if (cmd.ToLower ().StartsWith ("version")) {
			SendResponse (Version.GetVersion ());
		} else if (cmd.ToLower ().StartsWith ("user list")) {
			foreach (User u in GameController.current.dataBase) {
				SendResponse (u.Username);
			}
		} else if (cmd.ToLower ().StartsWith ("clear")) {
			commandList.text = GameController.current.GetUserGroup().ToUpper() + " " + GameController.current.GetUsername() + "@" + GameController.current.GetUserID() + ": \n";
		} else {
			SendResponse ("\'" + cmd + "\' is not a valid command.");
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
