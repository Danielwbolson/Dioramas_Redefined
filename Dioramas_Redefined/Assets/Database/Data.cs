using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct populationDataByYear {
    public int year;
    public int numRoutes;
    public int count;

    public populationDataByYear Clone() {
        populationDataByYear p = new populationDataByYear {
            year = this.year,
            numRoutes = this.numRoutes,
            count = this.count
        };

        return p;
    }
}

[System.Serializable]
public struct populationDataByRoute {
    public string routeID;
    public List<Organism> organisms;

    public populationDataByRoute Clone() {

        List<Organism> o = new List<Organism>();
        for (int i = 0; i < this.organisms.Count; i++) {
            o.Add(organisms[i].Clone());
        }

        populationDataByRoute p = new populationDataByRoute {
            routeID = this.routeID,
            organisms = o
        };

        return p;
    }
}

[System.Serializable]
public struct routeData {
    public string routeID;
    public float latitude;
    public float longitude;
}
