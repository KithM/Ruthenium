using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public class SaveLoad : MonoBehaviour {

	public void StartSave(){
		// TODO:
		// Check to see if the save file already exists
		// if so ask for confirmation

		string fileName = "data";//gameObject.GetComponentInChildren<InputField> ().text;

		// TODO: Is the fileName valid? i.e. ban path delimiters? (\ / : . ?) etc

		// Right now fileName is just was in the dialog box. We need to pad this out
		// to the full path plus an extension
		// In the end we are looking for something that is looking like this
		// C:\Users\username\ApplicationData\Orion Games\Space Builder\Saves\SaveGame123.sav

		string filePath = System.IO.Path.Combine ( FileSaveBasePath(), fileName + ".ruth" );

		// At this point, filePath should look much like:
		// C:\Users\username\ApplicationData\Orion Games\Space Builder\Saves\SaveGame123.sav

		if(File.Exists(filePath) == true){
			// TODO: Do file override dialog box
			Debug.Log ("SaveLoad::StartSave: File already exists! File has been overriden.");
		}

		GameController.nm.ShowNotification ("Saving...");
		SaveGame (filePath);
	}

	public void SaveGame(string filePath) {
		// TODO:

		Debug.Log("SaveLoad::SaveGame: Saving game.");

		XmlSerializer serializer = new XmlSerializer( typeof(Game) );
		TextWriter writer = new StringWriter();
		serializer.Serialize(writer, GameController.current);
		writer.Close();

		Debug.Log( writer.ToString() );

		//PlayerPrefs.SetString("SaveGame00", writer.ToString());

		// Create/overwrite the save field with the xml text

		// Make sure the save folder exists
		if( Directory.Exists(FileSaveBasePath() ) == false){
			// NOTE: This can throw an exception if we can't
			// create the folder. But why would this ever happen?
			// We should by definition have the ability to write
			// to our persistent data folder unless something is
			// really broken with the PC / device
			Directory.CreateDirectory (FileSaveBasePath());
		}
		if( Directory.Exists(ImagesBasePath() ) == false){
			// NOTE: This can throw an exception if we can't
			// create the folder. But why would this ever happen?
			// We should by definition have the ability to write
			// to our persistent data folder unless something is
			// really broken with the PC / device
			Directory.CreateDirectory (ImagesBasePath());
		}

		File.WriteAllText ( filePath, writer.ToString() );
	}

	public void StartLoad(){

		Debug.Log("SaveLoad::StartLoad: Loading game.");

		string fileName = "data";

		// TODO: Is the fileName valid? i.e. ban path delimiters? (\ / : . ?) etc

		// Right now fileName is just was in the dialog box. We need to pad this out
		// to the full path plus an extension
		// In the end we are looking for something that is looking like this
		// C:\Users\username\ApplicationData\Orion Games\Space Builder\Saves\SaveGame123.sav

		string filePath = System.IO.Path.Combine ( FileSaveBasePath(), fileName + ".ruth" );

		// At this point, filePath should look much like:
		// C:\Users\username\ApplicationData\Orion Games\Space Builder\Saves\SaveGame123.sav

		if(File.Exists(filePath) == false){
			// The data.ruth file doesn't exist, so make a new one
			Debug.LogError ("SaveLoad::StartLoad: File doesn't exist! Creating one now.");

			// Set the username and password to a default so we can save the data.ruth file
			GameController.current.SetUsername("user" + GameController.current.userID.ToString());
			GameController.current.SetPassword("password");

			StartSave ();
			return;
		}

		GameController.nm.ShowNotification ("Loading...");
		LoadGame (filePath);
	}

	public void LoadGame(string filePath) {
		// This function simply grabs the current game and loads the data.ruth file that is passed to us
		GameController.current.LoadGame (filePath);

		if( Directory.Exists(FileSaveBasePath() ) == false){
			// NOTE: This can throw an exception if we can't
			// create the folder. But why would this ever happen?
			// We should by definition have the ability to write
			// to our persistent data folder unless something is
			// really broken with the PC / device
			Directory.CreateDirectory (FileSaveBasePath());
		}
		if( Directory.Exists(ImagesBasePath() ) == false){
			// NOTE: This can throw an exception if we can't
			// create the folder. But why would this ever happen?
			// We should by definition have the ability to write
			// to our persistent data folder unless something is
			// really broken with the PC / device
			Directory.CreateDirectory (ImagesBasePath());
		}
	}

	public static string FileSaveBasePath(){
		return System.IO.Path.Combine (Application.persistentDataPath, "Data");
	}
	public static string ImagesBasePath(){
		return System.IO.Path.Combine (Application.persistentDataPath, "Images");
	}
	public static string FontsBasePath(){
		return System.IO.Path.Combine (Application.persistentDataPath, "Data/fonts");
	}

}
