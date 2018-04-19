using UnityEngine.UI;
using UnityEngine;

public class DisplayFullVersion : MonoBehaviour {

	[Header("Text")]
	public Text versionText;

	// Use this for initialization
	void Start () {
		versionText.text = "Version " + Version.GetVersion ();
	}
}
