using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public List<bool> active;
    List<LineRenderer> lrs;

    // Start is called before the first frame update
    void Start() {
        string data = "Bell_Dioramas_Touchscreen_FieldGuides_FINAL_CRiver-BigWoods_only.csv";
        string BBSData = "A2286.csv";
        string classification = "classifications.txt";

        Diorama diorama = gameObject.AddComponent<Diorama>();

        Parse.ParseCSV(
            ref diorama,
            Application.streamingAssetsPath + "/" + data, 
            Application.streamingAssetsPath + "/" + classification);

        Parse.ParseBBSData(
            ref diorama,
            Application.streamingAssetsPath + "/" + BBSData);

        Visualization visualization = gameObject.AddComponent<Visualization>();

        // Visualize all of our population data
        visualization.Visualize(diorama.organisms);
        lrs = visualization.GetLineRenderers();

        active = new List<bool>();
        for (int i = 0; i < lrs.Count; i++) {
            active.Add(true);
        }
        
    }

    private void Update() {
        for (int i = 0; i < lrs.Count; i++) {
            lrs[i].enabled = active[i];
        }
    }

}
