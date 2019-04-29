using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject PopUpWindow;

    public void Awake()
    {
        //PopUpWindow = GameObject.FindWithTag("PopUp");
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

            string howsItDoingText = "";

            switch (index)
            {
                case 9:
                    howsItDoingText = "Ovenbird Stuff";
                    break;
                case 21:
                    howsItDoingText = "Blue Hero Stuff";
                    break;
                case 22:
                    howsItDoingText = "Cerulean Warbler Stuff";
                    break;
                case 23:
                    howsItDoingText = "Mourning Warbler Stuff";
                    break;
                default:
                    break;
            }
            Text popup_info = (Text)GameObject.Find("Name").GetComponent<Text>();
            popup_info.text = "Name: " + dio.organisms[index].GetName();

            popup_info = (Text)GameObject.Find("Latin Name").GetComponent<Text>();
            popup_info.text = "Latin Name: " + dio.organisms[index].GetLatinName();

            popup_info = (Text)GameObject.Find("Habitat").GetComponent<Text>();
            popup_info.text = "Habitat: " + dio.organisms[index].GetHabitat();

            popup_info = (Text)GameObject.Find("In The Scene").GetComponent<Text>();
            popup_info.text = "In the Scene: " + dio.organisms[index].GetInTheScene();

            popup_info = (Text)GameObject.Find("Hows It Doing").GetComponent<Text>();
            popup_info.text = "Hows it doing?: " + howsItDoingText;

            popup_info = (Text)GameObject.Find("Family").GetComponent<Text>();
            popup_info.text = "Family: " + dio.organisms[index].GetFamily();
        }
    }

    public void CloseWindow()
    {
        PopUpWindow.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        //PopUpWindow.SetActive(false);
    }
}
