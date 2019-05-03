using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImagetoMenu : MonoBehaviour
{
	public Button buttonMenu;
	
	void Start() {
		buttonMenu.onClick.AddListener(() => {
			SceneManager.LoadScene("NowMenuScene", LoadSceneMode.Single);
		});
	}
}
