using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Manager : MonoBehaviour {

    //public GameObject MNGameObject;
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

        //// Visualize all of our population data
        //Visualization visualization = gameObject.AddComponent<Visualization>();


        /*
         * Save our created textures for real-time use
         */
        //for (int i = 0; i < diorama.organisms.Count; i++) {

        //    // Make sure we are only working with birds
        //    if (diorama.organisms[i].classification != Classification.bird) {
        //        continue;
        //    }

        //    // Calculate maxCount at any route
        //    List<float> extrapCounts = new List<float>();
        //    for (int j = 0; j < diorama.popByRoute.Count; j++) { // Route populations
        //        for (int k = 0; k < diorama.popByRoute[j].organisms[i].data.Count; k++) { // years
        //            int count = diorama.popByRoute[j].organisms[i].data[k].count;
        //            int numRoutes = diorama.popByRoute[j].organisms[i].data[k].numRoutes;

        //            extrapCounts.Add(count / (float)numRoutes);
        //        }
        //    }

        //    // Get a ceiling to the nearest 50
        //    int maxCount = Mathf.CeilToInt(Mathf.Max(extrapCounts.ToArray()) / 5.0f) * 5;

        //    // Get a useable save name
        //    string name = diorama.organisms[i].GetName().Trim();
        //    name = name.Replace(" ", "_");
        //    name = name.Replace(",", "");

        //    // Get directory for animal
        //    string dirPath = Application.dataPath + "/Data/" + name + "_" + maxCount.ToString() + "/";
        //    if (!Directory.Exists(dirPath)) {
        //        Directory.CreateDirectory(dirPath);
        //    }

        //    // Run through all years of data on birds
        //    for (int k = 1967; k <= 2017; k++) {
        //        // i is bird index, k is year
        //        Texture2D tex = visualization.Visualize(diorama, MNTexture2D, Colormap, i, k, maxCount);

        //        File.WriteAllBytes(dirPath + k.ToString() + ".png", tex.EncodeToPNG());
        //    }                
        //}

        //// Set our sprite now
        //Rect r = new Rect(0, 0, tex.width, tex.height);
        //MNGameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, r, new Vector2(0.5f, 0.5f));

    }

}
