using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[Header("Text Elements")]
	public Text username;
	public Text password;

	public static Game current { get; protected set; }
	public static SaveLoad sl;
	public static NotificationManager nm;
	public static PermissionsManager pm;
	public static LoginManager lm;

	// Use this for initialization
	void Awake () {
		// Reference our controllers / managers
		sl = FindObjectOfType<SaveLoad> ();
		nm = FindObjectOfType<NotificationManager> ();
		pm = FindObjectOfType<PermissionsManager> ();
		lm = FindObjectOfType<LoginManager> ();

		// Create a new game
		current = new Game (null, null);
	}

	void Start(){
		// Create a unique identifier for the new user, even if it won't be used
		current.SetUserID(UnityEngine.Random.Range (1000,99999));

		sl.StartLoad ();
		current.WriteReadme ();
		Logger.WriteSystemInfo ();
	}
}
