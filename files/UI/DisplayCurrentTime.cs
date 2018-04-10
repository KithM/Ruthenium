using UnityEngine;
using UnityEngine.UI;

public class DisplayCurrentTime : MonoBehaviour {

	Text timeText;

	// Use this for initialization
	void Start () {
		timeText = gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		timeText.text = System.DateTime.Now.ToShortTimeString ();// + "\n" + System.DateTime.Now.Date.DayOfWeek.ToString ();//System.DateTime.Now.ToShortDateString();
	}
}
