using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class PermissionsManager : MonoBehaviour {

	[Header("Panels")]
	public GameObject permissionsPanel;
	public GameObject bodyPanel;

	[Header("Prefabs")]
	public GameObject permissionPrefab;

	[Header("Text Elements")]
	public Text permissionAddText;

	[Header("Dropdown")]
	public Dropdown dd;

	[Header("Misc")]
	public int lastUserIndex;

	// Use this for initialization
	void Start () {
		HidePermissions ();
	}
	
	public void ShowPermissions(){
		RefreshPermissions ();

		for (int i = 0; i < dd.options.Count; i++) {
			if(dd.options[i].text == GameController.current.GetUsername()){
				dd.value = i;
				break;
			}
		}

		permissionsPanel.SetActive (true);
	}
	public void HidePermissions(){
		permissionsPanel.SetActive (false);
	}

	public void RefreshPermissions(){
		// This removes the permissions that were left when we closed the permissions (they might be updated)
		foreach (Transform child in bodyPanel.transform){
			Destroy (child.gameObject);
		}

		// TODO??
		ViewUserPermissions ();

		// This creates our dropdown list for selecting users, and it will be updated whenever we open the permissions manager
		// By default, when we close and open the permissions manager, we will be viewing OUR OWN permissions, not where we left off
		dd.ClearOptions ();
		List<string> usersList = new List<string> ();
		foreach (User user in GameController.current.dataBase) {
				usersList.Add (user.Username.ToString());
		}
		dd.AddOptions (usersList);

		// TODO: When we refresh from another user's permissions, we want to keep their permissions visible and not switch to our permissions
		for (int i = 0; i < dd.options.Count; i++) {
			if(dd.options[i].text == GameController.current.GetUsername()){
				dd.value = lastUserIndex; //i
				break;
			}
		}
	}

	// TODO: Works like a charm, but adding / removing permissions on any user only does it to the logged in user
	public void ViewUserPermissions(){

		// Delete our old permissions from the list, we want to see our selected user's permissions
		foreach (Transform child in bodyPanel.transform){
			Destroy (child.gameObject);
		}

		// Display our currently selected user's permissions
		foreach (User user in GameController.current.dataBase) {
			if (user == GameController.current.dataBase [dd.value]) {
				foreach (string perm in user.Permissions) {
					GameObject pGO = Instantiate (permissionPrefab, bodyPanel.transform);

					Text pGOt = pGO.GetComponentInChildren<Text> ();
					Button pGOb = pGO.GetComponentInChildren<Button> ();
					pGOb.onClick.AddListener (delegate {
						DeletePermission (perm);
					});
					pGOt.text = perm;
				}
			}
		}
	}

	public void DeletePermission(string perm){
		// Do we have permission to remove OUR OWN permissions?
		if(!GameController.current.permissions.Contains("permissions.remove.self") && ( GameController.current.GetUsername() == GameController.current.dataBase [dd.value].Username )){
			string log1 = "PermissionsManager::DeletePermission: Failed to delete permissions for \'" + GameController.current.dataBase [dd.value].Username + "\'. (insufficient permissions)";
			Logger.WriteLog (log1);
			GameController.nm.ShowNotification ("Insufficient permissions: permissions.remove.self.");
			return;
		}
		// Do we have permission to remove OTHERS permissions?
		if(!GameController.current.permissions.Contains("permissions.remove.others") && ( GameController.current.GetUsername() != GameController.current.dataBase [dd.value].Username )){
			string log1 = "PermissionsManager::DeletePermission: Failed to delete permissions for \'" + GameController.current.dataBase [dd.value].Username + "\'. (insufficient permissions)";
			Logger.WriteLog (log1);
			GameController.nm.ShowNotification ("Insufficient permissions: permissions.remove.others.");
			return;
		}

		foreach (User user in GameController.current.dataBase) {
			if(user.Username == GameController.current.dataBase [dd.value].Username){
				//GameController.current.permissions.Remove (perm);
				user.Permissions.Remove (perm);

				// Keep track of what user we have modified
				lastUserIndex = dd.value;

				RefreshPermissions ();
				GameController.sl.StartSave ();
			}
		}
	}

	public void AddPermission(){
		string perm = permissionAddText.text;

		// Does the permission exist? (case sensitive)
		if (Permissions.IsValidPermission(perm) == false) {
			Debug.LogError ("PermissionsManager::AddPermission: Invalid permission \'<b>" + perm + "</b>\'.");
			Logger.WriteLog ("PermissionsManager::AddPermission: Invalid permission \'" + perm + "\'.");
			GameController.nm.ShowNotification ("Invalid permission: " + perm + ".");
			return;
		}

		// Do we have permission to add to OUR OWN permissions?
		if(!GameController.current.permissions.Contains("permissions.add.self") && ( GameController.current.GetUsername() == GameController.current.dataBase [dd.value].Username )){
			string log1 = "PermissionsManager::DeletePermission: Failed to add permissions for \'" + GameController.current.dataBase [dd.value].Username + "\'. (insufficient permissions)";
			Logger.WriteLog (log1);
			GameController.nm.ShowNotification ("Insufficient permissions: permissions.add.self.");
			return;
		}
		// Do we have permission to add to OTHERS permissions?
		if(!GameController.current.permissions.Contains("permissions.add.others") && ( GameController.current.GetUsername() != GameController.current.dataBase [dd.value].Username )){
			string log1 = "PermissionsManager::DeletePermission: Failed to add permissions for \'" + GameController.current.dataBase [dd.value].Username + "\'. (insufficient permissions)";
			Logger.WriteLog (log1);
			GameController.nm.ShowNotification ("Insufficient permissions: permissions.add.others.");
			return;
		}

		foreach (User user in GameController.current.dataBase) {
			if(user.Username == GameController.current.dataBase [dd.value].Username){

				if(user.Permissions.Contains(perm)){
					Debug.Log ("PermissionsManager::AddPermission: Failed to add permission \'<b>" + perm + "</b>\' for user \'<b>" + GameController.current.dataBase [dd.value].Username + "</b>\'. (permission already exists)");
					Logger.WriteLog ("PermissionsManager::AddPermission: Failed to add permission \'" + perm + "\' for user \'" + GameController.current.dataBase [dd.value].Username + "\'. (permission already exists)");
					return;
				}
					
				// Add our permission to the USER, our current game's permissions will update when reloading or checking
				user.Permissions.Add (perm);

				Debug.Log ("PermissionsManager::AddPermission: Adding permission \'<b>" + perm + "</b>\' for user \'<b>" + GameController.current.dataBase [dd.value].Username + "</b>\'.");
				Logger.WriteLog ("PermissionsManager::AddPermission: Adding permission \'" + perm + "\' for user \'" + GameController.current.dataBase [dd.value].Username + "\'.");

				// Keep track of what user we have modified
				lastUserIndex = dd.value;

				RefreshPermissions ();
				GameController.sl.StartSave ();
			}
		}
	}
}
