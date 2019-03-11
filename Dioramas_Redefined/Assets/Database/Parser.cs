using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Parse {

    /*
     * Parses CSV file delimmited by ~ and puts it in a Diorama object
     */
    public static Diorama ParseCSV(string filepath) {
        Diorama d = new Diorama();

        string[] fileData = System.IO.File.ReadAllLines(filepath);

        string name = null;
        string latinName = null;
        string habitat = null;
        string inTheScene = null;
        string didYouKnow = null;
        string family = null;

        for (int i = 0; i < fileData.Length; i++) {

            string s = fileData[i];

            if (s == "~" || s.Length == 0) {

                // Every organism has a name, so we will use this to determine if we have a new organism
                if (name != null) {
                    Organism o = new Organism(name, latinName, habitat, inTheScene, didYouKnow, family);
                    d.organisms.Add(o);
                    name = null;
                }
                continue;
            }

            string[] lineData = s.Trim().Split('~');


            // Text parsing for each category of an Organism
            if (lineData[0].Contains("Name:")) {

                // Get regular name
                name = lineData[1].Substring(1, lineData[1].Length - 1);

                // Get latin name
                string nextS = fileData[++i];
                latinName = nextS.Substring(0, nextS.Length - 1);

            } else if (lineData[0].Contains("Habitat:")) {

                habitat = lineData[1];

            } else if (lineData[0].Contains("In the scene:")) {

                inTheScene = lineData[1];

            } else if (lineData[0].Contains("Did you know?:")) {

                didYouKnow = lineData[1];

            } else if (s.Contains("Classification:")) {

                family = lineData[1];

            }

            //Organism o = new Organism(name, latinName, habitat, inTheScene, didYouKnow, family, distribution, img);
        }

        return d;
    }
}
