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
                    howsItDoingText = "The Ovenbird has remained stable over the years. However, Ovenbirds are sensitive to habitat destruction. In addition, the Dutch elm disease affected populations in Minnesota, because when the dying trees let in more light, more vegetation grows on the ground, and Ovenbird numbers decline.";
                    break;
                case 21:
                    howsItDoingText = "The Great Blue Heron population has remained stable throughout the years, but has experienced decreases in the past through habitat loss due to chemicals and other human intrusions. Conservation efforts have been made, but newer industrial chemicals continue to affect heron habitats. This leads to reduced nest site attendance.";
                    break;
                case 22:
                    howsItDoingText = "The Cerulean Warbler is currently a vulnerable species, with declines of 72% since 1970, with a projected further decline of 50% by 2041. Once abundant near the Mississippi River in the nineteenth century, habitat loss has caused them to stop breeding due to factors such as degrading breeding grounds from human and environmental factors.";
                    break;
                case 23:
                    howsItDoingText = "The Mourning Warbler population has decreased by around 43% since 1966. Despite this, it is not on a watch list. The Mourning Warbler prefers forests disturbed by human activity, so they are benefiting from activities that are detrimental to other birds.";
                    break;
                default:
                    break;
            }
            Text popup_info = (Text)GameObject.Find("Name").GetComponent<Text>();
            popup_info.text = dio.organisms[index].GetName();

            popup_info = (Text)GameObject.Find("Latin Name").GetComponent<Text>();
            popup_info.text = dio.organisms[index].GetLatinName();

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
