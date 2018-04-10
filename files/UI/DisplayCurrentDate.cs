using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class DisplayCurrentDate : MonoBehaviour {

	[Header("Panels")]
	public GameObject datePanel;

	[Header("Text Elements")]
	public Text dateTimeText;

	void Start () {
		HideFullDate ();
	}

	void Update(){
		dateTimeText.text = System.DateTime.Now.Month + "." + System.DateTime.Now.Day + "." + System.DateTime.Now.Year + 
			"\n" + System.DateTime.Now.ToLongTimeString();
	}

	public void ToggleFullDate(){
		if(datePanel.activeSelf == true){
			HideFullDate ();
		} else {
			ShowFullDate ();
		}
	}
	public void ShowFullDate(){
		datePanel.SetActive (true);
	}
	public void HideFullDate(){
		datePanel.SetActive (false);
	}
}
