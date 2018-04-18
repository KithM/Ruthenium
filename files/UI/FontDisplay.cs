using UnityEngine.UI;
using UnityEngine;

public class FontDisplay : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Fonts.LoadFonts ();

		Text text_root = gameObject.GetComponent<Text> ();
		UpdateFont (text_root);

		Text[] text_children = gameObject.GetComponentsInChildren<Text> ();
		foreach (Text text_child in text_children) {
			UpdateFont (text_child);
		}
	}
	
	static void UpdateFont(Text t){
		if (t != null && (t.font.name == "ROBOTO-REGULAR" || t.font.name == "ROBOTO-MEDIUM")) {
			t.font = Fonts.GetDefaultFont();
		} else if (t != null && t.font.name == "ROBOTO-BOLD"){
			t.font = Fonts.GetDefaultBoldFont();
		} else if (t != null && t.font.name == "ROBOTO-BLACK"){
			t.font = Fonts.GetDefaultExtraBoldFont();
		} else if (t != null){
			t.font = Fonts.GetSystemFont();
		}
	}
}
