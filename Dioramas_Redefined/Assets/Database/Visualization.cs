using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct extrapPopDataByYear {
    public string name;
    public List<int> years;
    public List<float> extrapolatedCount;
}

[System.Serializable]
public struct extrapPopDataByRoute {
    public string routeId;
    public List<extrapPopDataByYear> extrapolatedCount;
}

public class Visualization : MonoBehaviour {
    [SerializeField]
    List<extrapPopDataByYear> popByYear;

    [SerializeField]
    List<extrapPopDataByRoute> popByRoute;

    float minLat = 43.5008f;
    float maxLat = 49.3877f;
    float minLong = -97.2304f;
    float maxLong = -89.4919f;

    // Start is called before the first frame update
    public Visualization() {
        popByYear = new List<extrapPopDataByYear>();
        popByRoute = new List<extrapPopDataByRoute>();
    }

    public void Visualize(Diorama d, ref Texture2D MNTexture2D, List<routeData> rData) {

        // Get our per-year population data
        foreach (Organism o in d.organisms) {
            List<int> years = new List<int>();
            List<float> extrapolatedCount = new List<float>();

            for (int i = 0; i < o.GetPopulationData().Count; i++) {
                years.Add(o.data[i].year);
                extrapolatedCount.Add(o.data[i].count / (float)o.data[i].numRoutes);
            }

            if (years.Count > 0) {

                extrapPopDataByYear b = new extrapPopDataByYear {
                    name = o.GetName(),
                    years = years,
                    extrapolatedCount = extrapolatedCount
                };
                popByYear.Add(b);
            }
        }

        // Change pixel color at each route location

        // width is 0 -> MNTexture2D.width
        // height is 0 -> MNTexture2D.height

        // We need to change:
        //  (minLong, minLat) to (0,0)
        //  (maxLong, maxLat) to (width, height)
        for (int i = 0; i < rData.Count; i++) {
            int x = ChangeScale_FtoI(0, MNTexture2D.width - 1, minLong, maxLong, rData[i].longitude);
            int y = ChangeScale_FtoI(0, MNTexture2D.height - 1, minLat, maxLat, rData[i].latitude);

            for (int j = x-1; j < x+2; j++) {
                for (int k = y-1; k < y+2; k++) {
                    MNTexture2D.SetPixel(j, k, Color.black);
                }
            }
        }
        MNTexture2D.Apply();
    }

    int ChangeScale_FtoI(int a, int b, float min, float max, float val) {
        return (int)(((b - a) * (val - min)) / (max - min) + a);
    }
}
