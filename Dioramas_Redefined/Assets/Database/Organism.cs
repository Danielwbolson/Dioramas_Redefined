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

[System.Serializable]
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

    [SerializeField]
    private string name;
    [SerializeField]
    private string latinName;

    [SerializeField]
    private string habitat;
    [SerializeField]
    private string inTheScene;
    [SerializeField]
    private string didYouKnow;
    [SerializeField]
    private string family;

    [SerializeField]
    private Texture2D distribution;
    [SerializeField]
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
