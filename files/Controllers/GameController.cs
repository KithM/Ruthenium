using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[Header("Text Elements")]
	public Text username;
	public Text password;

	public static Game current { get; protected set; }
	public static SaveLoad sl { get; protected set; }
	public static NotificationManager nm { get; protected set; }
	public static PermissionsManager pm { get; protected set; }
	public static LoginManager lm { get; protected set; }

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
		// Load the data files: check if the data.ruth, README.info, and system.info files exist
		// If not, create a new one
		sl.StartLoad ();
		current.WriteReadme ();
		Logger.WriteSystemInfo ();
	}

	public void OpenRepositoryURL(){
		Application.OpenURL ("https://github.com/KithM/Ruthenium/");
	}
}
