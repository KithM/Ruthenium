using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class DatabaseManager : MonoBehaviour {

	[Header("Panels")]
	public GameObject mainMenu;
	public GameObject loadBodyPanel;
	public GameObject createBodyPanel;
	public GameObject loadDatabaseMenuPanel;
	public GameObject createDatabaseMenuPanel;

	[Header("Prefabs")]
	public GameObject databasePrefab;

	[Header("InputFields")]
	public InputField loadSelectedDatabase;
	public InputField createSelectedDatabase;

	[Header("Databases")]
	public DirectoryInfo[] databases;

	// Use this for initialization
	void Start () {
		ShowMainMenu ();
	}

	public void LoadAllDatabases(){
		loadSelectedDatabase.text = "";
		createSelectedDatabase.text = "";

		DirectoryInfo di = new DirectoryInfo(SaveLoad.DatabasePath ());
		databases = di.GetDirectories ();

		foreach (Transform child in createBodyPanel.transform){
			Destroy (child.gameObject);
		}
		foreach (Transform child in loadBodyPanel.transform){
			Destroy (child.gameObject);
		}

		foreach (DirectoryInfo database in databases) {
			GameObject dGO = Instantiate (databasePrefab, loadBodyPanel.transform);

			Text dGOt = dGO.GetComponentInChildren<Text> ();
			Button dGOb = dGO.GetComponentInChildren<Button> ();

			if (database.Name.Length > 29) {
				dGOt.text = database.Name.Substring(0,29) + "...";
			} else {
				dGOt.text = database.Name;
			}

			dGOb.onClick.AddListener (delegate {
				loadSelectedDatabase.text = database.Name;
			});

			GameObject d1GO = Instantiate (databasePrefab, createBodyPanel.transform);

			Text d1GOt = d1GO.GetComponentInChildren<Text> ();
			Button d1GOb = d1GO.GetComponentInChildren<Button> ();
			d1GOt.text = database.Name;

			d1GOb.onClick.AddListener (delegate {
				createSelectedDatabase.text = database.Name;
			});
		}
	}

	public void LoadDatabase(){
		// TODO: Load a list of folders (the names of the databases) inside of the Data folder
		// and let user select one to load

		int c = 0;
		foreach (DirectoryInfo database in databases) {
			if(database.Name == loadSelectedDatabase.text){
				c++;
			}
		}

		if(c == 0){
			loadSelectedDatabase.text = "";
			GameController.nm.ShowNotification ("Invalid database.");
			return;
		}

		GameController.current.SetDatabaseName (loadSelectedDatabase.text);
		GameController.sl.StartLoad ();

		HideMainMenu ();
		GameController.lm.ShowLogin ();
	}
	public void CreateNewDatabase(){
		int c = 0;
		foreach (DirectoryInfo database in databases) {
			if(database.Name == createSelectedDatabase.text){
				c++;
			}
		}

		if(c > 0){
			createSelectedDatabase.text = "";
			GameController.nm.ShowNotification ("Database already exists.");
			return;
		}
		if(string.IsNullOrEmpty(createSelectedDatabase.text)){
			createSelectedDatabase.text = "";
			GameController.nm.ShowNotification ("Invalid database name.");
			return;
		}

		GameController.current.SetDatabaseName (createSelectedDatabase.text);
		GameController.sl.StartLoad ();

		HideMainMenu ();
		GameController.lm.ShowLogin ();
	}


	public void ShowMainMenu(){
		loadDatabaseMenuPanel.SetActive (false);
		createDatabaseMenuPanel.SetActive (false);
		mainMenu.SetActive (true);
	}
	public void HideMainMenu(){
		mainMenu.SetActive (false);
		loadDatabaseMenuPanel.SetActive (false);
		createDatabaseMenuPanel.SetActive (false);
	}
	public void ShowLoadDatabaseMenu(){
		LoadAllDatabases ();

		mainMenu.SetActive (false);
		createDatabaseMenuPanel.SetActive (false);
		loadDatabaseMenuPanel.SetActive (true);
	}
	public void ShowCreateDatabaseMenu(){
		LoadAllDatabases ();

		mainMenu.SetActive (false);
		loadDatabaseMenuPanel.SetActive (false);
		createDatabaseMenuPanel.SetActive (true);
	}
}
