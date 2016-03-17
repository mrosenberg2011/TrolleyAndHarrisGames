using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public void StartButton() {
		Application.LoadLevel ("Trolley");
	}

	public void QuitButton() {
		Application.Quit ();
	}
}