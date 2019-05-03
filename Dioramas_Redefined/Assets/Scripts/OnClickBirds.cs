using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine;

public class OnClickBirds : MonoBehaviour
{
    public GameObject Manager;
    private VisualizationManager vm;

    private Sprite[] ovenBirdimages;
    private Sprite[] greatBlueHeronImages;

    private void Start() {
        vm = Manager.GetComponent<VisualizationManager>();

        // Set up ovenBird stuff
        ovenBirdimages = new Sprite[51];
        for (int i = 1967; i < 2018; i++) {
            string filename = "Data/Ovenbird_155/" + i.ToString();
            Texture2D tex = Resources.Load<Texture2D>(filename);
            Rect r = new Rect(0, 0, tex.width, tex.height);
            Sprite s = Sprite.Create(tex, r, new Vector2(0.5f, 0.5f));

            ovenBirdimages[i % 1967] = s;
        }

        // Set up greatBlue Heron stuff
        greatBlueHeronImages = new Sprite[51];
        // Set our image array
        for (int i = 1967; i < 2018; i++) {
            string filename = "Data/Great_Blue_Heron_35/" + i.ToString();
            Texture2D tex = Resources.Load<Texture2D>(filename);
            Rect r = new Rect(0, 0, tex.width, tex.height);
            Sprite s = Sprite.Create(tex, r, new Vector2(0.5f, 0.5f));

            greatBlueHeronImages[i % 1967] = s;
        }
    }

    public void OnClick_Ovenbird() {
        // Set start image
        string filename = "Data/Ovenbird_155/1967";
        Texture2D startTex = Resources.Load<Texture2D>(filename);
        Rect r = new Rect(0, 0, startTex.width, startTex.height);
        Sprite start = Sprite.Create(startTex, r, new Vector2(0.5f, 0.5f));

        // Set end image
        filename = "Data/Ovenbird_155/2017";
        Texture2D endTex = Resources.Load<Texture2D>(filename);
        r = new Rect(0, 0, endTex.width, endTex.height);
        Sprite end = Sprite.Create(endTex, r, new Vector2(0.5f, 0.5f));

        vm.UpdateImages(start, end, ovenBirdimages, 155);
    }

    public void OnClick_GreatBlueHeron() {
        // Set start image
        string filename = "Data/Great_Blue_Heron_35/1967";
        Texture2D startTex = Resources.Load<Texture2D>(filename);
        Rect r = new Rect(0, 0, startTex.width, startTex.height);
        Sprite start = Sprite.Create(startTex, r, new Vector2(0.5f, 0.5f));

        // Set end image
        filename = "Data/Great_Blue_Heron_35/2017";
        Texture2D endTex = Resources.Load<Texture2D>(filename);
        r = new Rect(0, 0, endTex.width, endTex.height);
        Sprite end = Sprite.Create(endTex, r, new Vector2(0.5f, 0.5f));

        vm.UpdateImages(start, end, greatBlueHeronImages, 35);
    }


}
