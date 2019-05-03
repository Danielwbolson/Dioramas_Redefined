using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VisualizationManager : MonoBehaviour
{
    public Texture2D MNTexture;

    public GameObject startImage;
    public GameObject endImage;
    public GameObject dataImage;
    public Sprite[] lerpSprites;

    public GameObject colorMap;
    public Texture2D colorMapTex;
    public Text colorMapMin;
    public Text colorMapMax;

    int max;
    int currIndex;

    private void Start() {
        // Set up colorMap
        Rect r = new Rect(0, 0, colorMapTex.width, colorMapTex.height);
        colorMap.GetComponent<Image>().sprite = Sprite.Create(colorMapTex, r, new Vector2(0.5f, 0.5f));
        Vector3 scale = colorMap.transform.localScale;
        colorMap.transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);

        lerpSprites = new Sprite[51];

        Image img = startImage.GetComponent<Image>();
        r = new Rect(0, 0, MNTexture.width, MNTexture.height);
        Sprite s = Sprite.Create(MNTexture, r, new Vector2(0.5f, 0.5f));
        img.sprite = s;
        img.useSpriteMesh = true;

        img = endImage.GetComponent<Image>();
        img.sprite = s;
        img.useSpriteMesh = true;

        img = dataImage.GetComponent<Image>();
        img.sprite = s;
        img.useSpriteMesh = true;

        for (int i = 0; i < 51; i++) {
            lerpSprites[i] = s;
        }
    }

    public void UpdateImages(Sprite start, Sprite end, Sprite[] sprites, int max) {
        // Update our start image
        Image img = startImage.GetComponent<Image>();
        img.sprite = start;
        img.useSpriteMesh = true;

        // Update our end image
        img = endImage.GetComponent<Image>();
        img.sprite = end;
        img.useSpriteMesh = true;

        // Update our array of data images
        lerpSprites = sprites;

        // Make sure we are set to the same slider value of our new image array
        img = dataImage.GetComponent<Image>();
        img.sprite = lerpSprites[currIndex];
        img.useSpriteMesh = true;

        this.max = max;

        colorMapMin.text = "0";
        colorMapMax.text = max.ToString();
    }
 

    public void OnSliderValueChanged(Slider s) {
        currIndex = (int)s.value % 1967;

        Image img = dataImage.GetComponent<Image>();
        img.sprite = lerpSprites[currIndex];
        img.useSpriteMesh = true;

    }

}
