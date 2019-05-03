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

        data = new List<yearData>();
    }

    public Organism Clone() {

        Organism organism =
            new Organism(
                this.GetName(),
                this.GetLatinName(),
                this.GetHabitat(),
                this.GetInTheScene(),
                this.GetDidYouKnow(),
                this.GetFamily(),
                this.GetDistribution(),
                this.GetImage()
            );

        organism.data = new List<yearData>();
        for (int i = 0; i < this.data.Count; i++) {
            organism.data.Add(this.data[i].Clone());
        }

        organism.aou = this.aou;
        organism.classification = this.classification;

        return organism;
    }


    // Variables
    public Classification classification;
    public int aou;
    public List<yearData> data;

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
    public List<yearData> GetPopulationData() { return data; }
    public int GetAOU() { return aou; }
    public Texture2D GetDistribution() { return distribution; }
    public Texture2D GetImage() { return image; }

}
