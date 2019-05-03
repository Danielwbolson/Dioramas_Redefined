using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct yearData {
    public int year;
    public int numRoutes;
    public int count;

    public yearData Clone() {
        yearData p = new yearData {
            year = this.year,
            numRoutes = this.numRoutes,
            count = this.count
        };

        return p;
    }
}

[System.Serializable]
public struct routeData {
    public string routeID;
    public List<Organism> organisms;
    public float latitude;
    public float longitude;
    public int pixelX;
    public int pixelY;

    public routeData Clone() {

        List<Organism> o = new List<Organism>();
        for (int i = 0; i < this.organisms.Count; i++) {
            o.Add(organisms[i].Clone());
        }

        routeData p = new routeData {
            routeID = routeID,
            organisms = o,
            latitude = latitude,
            longitude = longitude,
            pixelX = pixelX,
            pixelY = pixelY
        };

        return p;
    }
}
