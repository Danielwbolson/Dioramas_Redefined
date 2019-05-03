using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DioramaToday : MonoBehaviour
{
	public Button button360;
	
	void Start() {
		button360.onClick.AddListener(() => {
			SceneManager.LoadScene("Diorama Today", LoadSceneMode.Single);
		});
	}
}
