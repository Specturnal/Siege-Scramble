using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	[SerializeField]private GameObject ShopPanel, TintPanel;
	public bool PanelOpened;
	private GameObject[] ProductPanel = new GameObject[4];
	[SerializeField]private GameObject[] Knight, Wizard, Archer, Musketeer;
	private bool Filtered;

	public Text Name, Price, Description, ButtonText;
	public Button BuyButton;
	[SerializeField]private Text MoneyDisplay;

	public GameObject WarningPanel;
	public Text WarningText;

	private Profile Profile;
	private bool PlayerDetected;

	private GameObject PopUp;
	[SerializeField]private AudioSource audios;

	private void Start(){
		Profile = FindObjectOfType<Profile> ();
		for (int i = 0; i < 4; i++) {
			ProductPanel [i] = ShopPanel.transform.GetChild (0).GetChild (i).gameObject;
		}
		PopUp = transform.GetChild (0).gameObject;
	}
		
	/*private void Update(){
		MoneyDisplay.text = Profile.Money + "G";

		if (PlayerDetected && !Profile.PanelOpened) {
			if (!PanelOpened && Input.GetKeyDown (KeyCode.Q)) {
				PanelOpened = true;
				ShopPanel.SetActive (true);
				TintPanel.SetActive (true);
				Time.timeScale = 0;
				if (!Filtered) {
					switch (Profile.ClassTypeToInt) {
					case 0:
						for (int i = 0; i < Knight.Length; i++) {
							Knight [i].SetActive (true);
						}
						break;
					case 1:
						for (int i = 0; i < Wizard.Length; i++) {
							Wizard [i].SetActive (true);
						}
						break;
					case 2:
						for (int i = 0; i < Archer.Length; i++) {
							Archer [i].SetActive (true);
						}
						break;
					case 3:
						for (int i = 0; i < Musketeer.Length; i++) {
							Musketeer [i].SetActive (true);
						}
						break;
					default:
						break;
					}
					Filtered = true;
					audios.Play ();
				}
			} else if (Input.GetKeyDown (KeyCode.Q) && PanelOpened && WarningPanel.activeSelf || Input.GetKeyDown (KeyCode.Escape) && PanelOpened && WarningPanel.activeSelf) {
				WarningPanel.SetActive (false);
				audios.Play ();
			} else if (Input.GetKeyDown (KeyCode.Q) && PanelOpened || Input.GetKeyDown (KeyCode.Escape) && PanelOpened) {
				PanelOpened = false;
				ShopPanel.SetActive (false);
				TintPanel.SetActive (false);
				Time.timeScale = 1;
				audios.Play ();
			}
		}
	}*/

	public void SwitchProductPanel(int Page){
		for (int i = 0; i < 4; i++) {
			if (ProductPanel [i].activeSelf) {
				if (i != Page)
					ProductPanel [i].SetActive (false);
			} else if (i == Page)
				ProductPanel [i].SetActive (true);
		}
	}

	private void OnTriggerStay2D(Collider2D other){
		if (other.CompareTag ("Player")) {
			PlayerDetected = true;
			if (!PopUp.activeSelf)
				PopUp.SetActive (true);
		}
	}

	private void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("Player")) {
			PlayerDetected = false;
			PopUp.SetActive (false);
		}
	}
}
