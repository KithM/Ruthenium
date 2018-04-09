using UnityEngine.UI;
using UnityEngine;

public class DisplayVersion : MonoBehaviour {

	[Header("Text")]
	public Text versionText;

	// Use this for initialization
	void Start () {
		versionText.text = Version.GetVersion ();
	}
}
