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

	// Use this for initialization
	void Start () {
		HidePermissions ();
	}
	
	public void ShowPermissions(){
		RefreshPermissions ();
		permissionsPanel.SetActive (true);
	}
	public void HidePermissions(){
		permissionsPanel.SetActive (false);
	}

	public void RefreshPermissions(){
		foreach (Transform child in bodyPanel.transform){
			Destroy (child.gameObject);
		}

		foreach (User u in GameController.current.dataBase) {	
			if (u.Username == GameController.current.GetUsername ()) {
				GameController.current.SetPermissions (u.Permissions);
			}
		}

		Debug.Log(GameController.current.permissions.Count + " permissions loaded.");

		foreach (string perm in GameController.current.permissions) {
			GameObject pGO = Instantiate (permissionPrefab, bodyPanel.transform);

			Text pGOt = pGO.GetComponentInChildren<Text> ();
			Button pGOb = pGO.GetComponentInChildren<Button> ();
			pGOb.onClick.AddListener(delegate{ DeletePermission(perm); }); //AddListener (DeletePermission(perm));
			pGOt.text = perm;
		}
			
		dd.ClearOptions ();

		List<string> usersList = new List<string> ();

		foreach (User user in GameController.current.dataBase) {
				usersList.Add (user.Username.ToString());
		}

		dd.AddOptions (usersList);

		for (int i = 0; i < dd.options.Count; i++) {
			if(dd.options[i].text == GameController.current.GetUsername()){
				dd.value = i;
				break;
			}
		}
	}

	public void DeletePermission(string perm){
		if(!GameController.current.permissions.Contains("Permissions.Remove.Self")){
			string log1 = "PermissionsManager::DeletePermission: Failed to delete permissions for \'" + GameController.current.GetUsername() + "\'. (insufficient permissions)";
			Logger.WriteLog (log1);
			GameController.nm.ShowNotification ("Insufficient permissions: Permissions.Remove.Self.");
			return;
		}

		foreach (User user in GameController.current.dataBase) {
			if(user.Username == GameController.current.GetUsername()){
				GameController.current.permissions.Remove (perm);
				user.Permissions.Remove (perm);

				RefreshPermissions ();
				GameController.sl.StartSave ();
			}
		}
	}

	public void AddPermission(){
		string perm = permissionAddText.text;

		if (Permissions.IsValidPermission(perm) == false) {
			Debug.LogError ("PermissionsManager::AddPermission: Invalid permission \'<b>" + perm + "</b>\'.");
			Logger.WriteLog ("PermissionsManager::AddPermission: Invalid permission \'" + perm + "\'.");
			GameController.nm.ShowNotification ("Invalid permission: " + perm + ".");
			return;
		}

		if(!GameController.current.permissions.Contains("Permissions.Add.Self")){
			string log1 = "PermissionsManager::DeletePermission: Failed to add permissions for \'" + GameController.current.GetUsername() + "\'. (insufficient permissions)";
			Logger.WriteLog (log1);
			GameController.nm.ShowNotification ("Insufficient permissions: Permissions.Add.Self.");
			return;
		}

		foreach (User user in GameController.current.dataBase) {
			if(user.Username == GameController.current.GetUsername()){

				if(user.Permissions.Contains(perm)){
					Debug.Log ("PermissionsManager::AddPermission: Failed to add permission \'<b>" + perm + "</b>\' for user \'<b>" + GameController.current.GetUsername() + "</b>\'. (permission already exists)");
					Logger.WriteLog ("PermissionsManager::AddPermission: Failed to add permission \'" + perm + "\' for user \'" + GameController.current.GetUsername() + "\'. (permission already exists)");
					return;
				}

				GameController.current.permissions.Add (perm);
				user.Permissions.Add (perm);

				Debug.Log ("PermissionsManager::AddPermission: Adding permission \'<b>" + perm + "</b>\' for user \'<b>" + GameController.current.GetUsername() + "</b>\'.");
				Logger.WriteLog ("PermissionsManager::AddPermission: Adding permission \'" + perm + "\' for user \'" + GameController.current.GetUsername() + "\'.");

				RefreshPermissions ();
				GameController.sl.StartSave ();
			}
		}
	}
}
