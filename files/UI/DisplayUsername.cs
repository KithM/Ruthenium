using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class DisplayUsername : MonoBehaviour {

	[Header("Text")]
	Text usernameText;

	// Use this for initialization
	void Start () {
		usernameText = gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.current.GetUsername () != usernameText.text && GameController.current.GetUsername () != "") {
			usernameText.text = GameController.current.GetUsername ().ToString();
		}
	}
}
