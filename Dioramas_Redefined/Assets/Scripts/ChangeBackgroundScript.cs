using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ChangeBackgroundScript : MonoBehaviour
{
    public Image myImageComponent;
    public Sprite CurrentBG;
    public Sprite PostCardBG;
    public Sprite NovemberBG;
    public Sprite OldAprilBG;
    
    // Start is called before the first frame update
    void Start()
    {
        //myImageComponent = GetComponent<Image>();
        //changeBackgroundImage();
    }

    public void changeBackgroundImage(int i)
    {
        switch (i)
        {
            case 0:
                myImageComponent.sprite = PostCardBG;
                break;
            case 1:
                myImageComponent.sprite = OldAprilBG;
                break;
            case 2:
                myImageComponent.sprite = NovemberBG;
                break;
            case 3:
                myImageComponent.sprite = CurrentBG;
                break;
            default:
                Debug.Log("Invalid case number: " + i);
                break;
        }
    }
}
