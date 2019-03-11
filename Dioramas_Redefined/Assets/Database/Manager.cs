using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        string data = "Bell_Dioramas_Touchscreen_FieldGuides_FINAL_CRiver-BigWoods_only.csv";
        string classification = "classifications.txt";

        Diorama diorama = gameObject.AddComponent<Diorama>();

        Parse.ParseCSV(
            ref diorama,
            Application.streamingAssetsPath + "/" + data, 
            Application.streamingAssetsPath + "/" + classification);
        
    }

}
