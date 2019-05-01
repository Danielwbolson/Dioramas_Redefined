using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public GameObject MNGameObject;
    public Texture2D MNTexture2D;
    public Texture2D Colormap;

    public List<bool> active;

    // Start is called before the first frame update
    void Start() {
        string data = "Bell_Dioramas_Touchscreen_FieldGuides_FINAL_CRiver-BigWoods_only.csv";
        string BBSData = "A2286.csv";
        string RouteData = "I2286.csv";
        string classification = "classifications.txt";

        Diorama diorama = gameObject.AddComponent<Diorama>();

        Parse.ParseCSV(
            ref diorama,
            Application.streamingAssetsPath + "/" + data,
            Application.streamingAssetsPath + "/" + classification);

        Parse.ParseBBSData(
            ref diorama,
            Application.streamingAssetsPath + "/" + BBSData);

         Parse.ParseRouteData(
             ref diorama,
             Application.streamingAssetsPath + "/" + RouteData);

        // Visualize all of our population data
        Visualization visualization = gameObject.AddComponent<Visualization>();
        visualization.Visualize(diorama, ref MNTexture2D, Colormap);

        // Set our sprite now
        Rect r = new Rect(0, 0, MNTexture2D.width, MNTexture2D.height);
        MNGameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(MNTexture2D, r, new Vector2(0.5f, 0.5f));

    }

}
