using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        string filename = "Bell_Dioramas_Touchscreen_FieldGuides_FINAL_CRiver-BigWoods_only.csv";
        Diorama diorama = Parse.ParseCSV(Application.streamingAssetsPath + "/" + filename);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
