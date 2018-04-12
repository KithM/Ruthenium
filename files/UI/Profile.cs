using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class Profile : MonoBehaviour {

	[Header("Panels")]
	public GameObject profilePanel;
	public GameObject editModeProfilePanel;

	[Header("Images")]
	public Image profilePicture;

	[Header("Text Elements")]
	public Text bioText;
	public Text userGroupText;

	[Header("EDITMODE")]
	public InputField EDITMODE_biotext;

	// Use this for initialization
	void Start () {
		HideProfile ();
	}
	
	public void ShowProfile(){

		if (GameController.current.GetBio () != "" && GameController.current.GetBio () != null) {
			// We have all of our profile information, don't substitute anything
			bioText.text = "<b>Bio:</b> " + GameController.current.GetBio ();	
		} else {
			// We dont have a bio right now, so substitute it with "This user does not have a bio.", as it is default anyway
			bioText.text = "<b>Bio:</b> " + "This user does not have a bio.";
		}

		if (GameController.current.GetUserGroup () != "" && GameController.current.GetUserGroup () != null) {
			// We have all of our profile information, don't substitute anything
			userGroupText.text = GameController.current.GetUserGroup () + "\n(" + GameController.current.GetUserID () + ")";
		} else {
			// We dont have a uID right now, so substitute it with User, as it is default anyway
			userGroupText.text = "User";
		}

		//TODO:
		string filePath = System.IO.Path.Combine ( SaveLoad.ImagesBasePath(), GameController.current.GetUsername() + ".png" );

		Texture2D ppT = LoadTextureFromFile (filePath);
		if (ppT != null) {
			Rect ppR = new Rect (0, 0, ppT.width, ppT.height);
			Sprite ppS = Sprite.Create (ppT, ppR, profilePicture.rectTransform.pivot);
			profilePicture.sprite = ppS;
		}

		profilePanel.SetActive (true);
	}
	public void HideProfile(){
		bioText.text = "This user is offline.";

		profilePanel.SetActive (false);
		editModeProfilePanel.SetActive (false);
	}

	public void EditProfile(){
		EDITMODE_biotext.text = GameController.current.GetBio ();

		profilePanel.SetActive (false);
		editModeProfilePanel.SetActive (true);
	}
	public void CancelEditProfile(){
		EDITMODE_biotext.text = GameController.current.GetBio ();

		profilePanel.SetActive (true);
		editModeProfilePanel.SetActive (false);
	}
	public void SaveEditProfile(){
		for (int i = 0; i < GameController.current.dataBase.Count; i++) {
			if (GameController.current.dataBase [i].Username == GameController.current.GetUsername ()) {
				GameController.current.dataBase [i].Bio = EDITMODE_biotext.text;
				GameController.current.SetBio(EDITMODE_biotext.text);
				GameController.sl.StartSave ();
			}
		}

		profilePanel.SetActive (false);
		editModeProfilePanel.SetActive (false);
		ShowProfile ();
	}

	public Texture2D LoadTextureFromFile(string filePath) {

		Texture2D tex = null;
		byte[] fileData;

		// default blank texture used for saving, if loading it will just be overrided
		tex = new Texture2D(100, 100, TextureFormat.ARGB32, false);

		if (File.Exists(filePath))     {
			fileData = File.ReadAllBytes(filePath); 

			tex.LoadImage(fileData); //..this will auto-resize the texture dimensions
			Debug.Log ("Profile::LoadTextureFromFile: Loaded file \'<b>" + filePath + "</b>\'.");
		} else {
			Debug.Log ("Profile::LoadTextureFromFile: Failed to load file \'<b>" + filePath + "</b>\'.");
			SaveTextureToFile (tex, filePath);
		}
		return tex;
	}

	public void SaveTextureToFile(Texture2D tex, string filePath){
		var bytes = tex.EncodeToPNG();

		FileStream file = File.Create (filePath);
		BinaryWriter binary = new BinaryWriter (file);

		binary.Write(bytes);
		file.Close();

		Debug.Log ("Profile::SaveTextureToFile: Created file \'<b>" + filePath + "</b>\'.");
	}
}
