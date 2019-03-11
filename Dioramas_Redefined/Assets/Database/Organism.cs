using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Classification {
    mammal,
    bird,
    reptile,
    fish,
    invertebrate,
    fungi,
    plant
}

public class Organism {

    public Organism() { }

    public Organism(string n, string ln, string h, string i, string d, string f, Texture2D distr = null, Texture2D img = null) {
        name = n;
        latinName = ln;

        habitat = h;
        inTheScene = i;
        didYouKnow = d;
        family = f;

        distribution = distr;
        image = img;
    }


    // Variables
    public Classification classification;

    private string name;
    private string latinName;

    private string habitat;
    private string inTheScene;
    private string didYouKnow;
    private string family;

    private Texture2D distribution;
    private Texture2D image;


    // Getters
    public Classification GetClassification() { return classification; }
    public string GetName() { return name; }
    public string GetLatinName() { return latinName; }
    public string GetHabitat() { return habitat; }
    public string GetInTheScene() { return inTheScene; }
    public string GetDidYouKnow() { return didYouKnow; }
    public string GetFamily() { return family; }
    public Texture2D GetDistribution() { return distribution; }
    public Texture2D GetImage() { return image; }

}
