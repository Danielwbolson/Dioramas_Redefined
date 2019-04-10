using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject PopUpWindow;

    public void Awake()
    {
        PopUpWindow = GameObject.FindWithTag("PopUp");
        PopUpWindow.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);

    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
	
	public void LoadDisplay(int index)
    {

        //PopUpWindow = GameObject.FindWithTag("PopUp");
        //PopUpWindow.SetActive(true);

        PopUpWindow.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);

        if (PopUpWindow.activeSelf)
        {
            Diorama dio = GameObject.FindObjectOfType<Diorama>();
            //Debug.Log(dio.organisms[index].GetName());
            Text popup_info = (Text)GameObject.Find("Name").GetComponent<Text>();
            popup_info.text = dio.organisms[index].GetName();

            popup_info = (Text)GameObject.Find("Latin Name").GetComponent<Text>();
            popup_info.text = dio.organisms[index].GetLatinName();

            popup_info = (Text)GameObject.Find("Habitat").GetComponent<Text>();
            popup_info.text = dio.organisms[index].GetHabitat();

            popup_info = (Text)GameObject.Find("In The Scene").GetComponent<Text>();
            popup_info.text = dio.organisms[index].GetInTheScene();

            popup_info = (Text)GameObject.Find("Did You Know").GetComponent<Text>();
            popup_info.text = dio.organisms[index].GetDidYouKnow();

            popup_info = (Text)GameObject.Find("Family").GetComponent<Text>();
            popup_info.text = dio.organisms[index].GetFamily();
        }
    }

    public void CloseWindow()
    {
        PopUpWindow.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        //PopUpWindow.SetActive(false);
    }
}
