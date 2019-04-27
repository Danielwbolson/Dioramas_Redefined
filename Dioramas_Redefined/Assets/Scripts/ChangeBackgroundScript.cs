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
    private int counter = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        //myImageComponent = GetComponent<Image>();
        changeBackgroundImage();
    }

    public void changeBackgroundImage()
    {
        counter++;
        if (counter % 2 == 1) {
            myImageComponent.sprite = PostCardBG;
        } else {
            myImageComponent.sprite = PostCardBG;
        }
    }
}
