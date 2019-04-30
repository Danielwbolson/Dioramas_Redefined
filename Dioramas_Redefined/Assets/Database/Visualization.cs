using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds information on extrapolated population data
[System.Serializable]
public struct extrapPopDataByYear {
    public string name;
    public List<int> years;
    public List<float> extrapolatedCount;
}

// Holds information, per route, of extrapolated population data
[System.Serializable]
public struct extrapPopDataByRoute {
    public string routeId;
    public List<extrapPopDataByYear> popByYear;
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

    public void Visualize(Diorama d, ref Texture2D MNTexture2D) {

        List<routeData> rData = d.popByRoute;

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

        // Get extrapolated population by routes
        for (int i = 0; i < d.popByRoute.Count; i++) {

            string rId = d.popByRoute[i].routeID;
            List<extrapPopDataByYear> routePop = new List<extrapPopDataByYear>();

            foreach (Organism o in d.popByRoute[i].organisms) {
                List<int> years = new List<int>();
                List<float> extrapolatedCount = new List<float>();

                for (int k = 0; k < o.GetPopulationData().Count; k++) {
                    years.Add(o.data[k].year);
                    extrapolatedCount.Add(o.data[k].count / (float)o.data[k].numRoutes);
                }

                extrapPopDataByYear b = new extrapPopDataByYear {
                    name = o.GetName(),
                    years = years,
                    extrapolatedCount = extrapolatedCount
                };
                routePop.Add(b);
            }

            extrapPopDataByRoute r = new extrapPopDataByRoute {
                routeId = rId,
                popByYear = routePop
            };

            popByRoute.Add(r);
        }

        // Get extrapolated pixel locations of each route
        for (int i = 0; i < rData.Count; i++) {
            int x = ChangeScale_FtoI(0, MNTexture2D.width - 1, minLong, maxLong, rData[i].longitude);
            int y = ChangeScale_FtoI(0, MNTexture2D.height - 1, minLat, maxLat, rData[i].latitude);

            rData[i] = new routeData {
                routeID = rData[i].routeID,
                organisms = rData[i].organisms,
                latitude = rData[i].latitude,
                longitude = rData[i].longitude,
                pixelX = x,
                pixelY = y
            };
        }

        // At this point, we now have stored population data by year
        // We have extrapolated our lat/longitude to pixel coordinates

        // Now, we want to change all pixels colors by the data at a specific year at all routes
        // Need a color map

        Color max = Color.blue;
        Color min = Color.grey * 0.5f;
        Color[] pixels = MNTexture2D.GetPixels();
        Color[] newPixels = new Color[pixels.Length];
        int year = 2015;

        float[] values = new float[popByRoute.Count];
        float[] inverseDistances = new float[popByRoute.Count];
        float[] ratios = new float[popByRoute.Count];

        for (int i = 0; i < pixels.Length; i++) {

            // We don't want to edit transparent sections of the image
            if (pixels[i].a == 0 || (pixels[i].r < 0.1f && pixels[i].g < 0.1f && pixels[i].b < 0.1f)) {
                newPixels[i] = pixels[i];
                continue;
            }

            float pixelVal = 0;

            for (int k = 0; k < popByRoute.Count; k++) {
                // i = row * width + column
                int x = i % MNTexture2D.width;
                int y = (i - x) / (MNTexture2D.height - 1);
                float dist =
                    (rData[k].pixelX - y) * (rData[k].pixelX - y) +
                    (rData[k].pixelY - x) * (rData[k].pixelY - x);

                if (dist == 0) {
                    dist = 0.1f;
                }
                inverseDistances[k] = (1.0f / dist);
            }

            float totalDist = SumArray(inverseDistances);

            // Inverse relationship. Things further away are less important
            for (int k = 0; k < popByRoute.Count; k++) {
                ratios[k] = inverseDistances[k] / totalDist;
            }

            // Going with bird 9, Ovenbird
            int birdIndex = 9;

            for (int k = 0; k < popByRoute.Count; k++) {
                routeData r = rData[k];

                // This will work
                int index = popByRoute.FindIndex(route => route.routeId.CompareTo(r.routeID) == 0);

                // This may fail
                int yearIndex = popByRoute[index].popByYear[birdIndex].years.IndexOf(year);

                // If it fails, add 0, else add it's pop value multiplied by its ratio
                if (yearIndex == -1) {
                    values[k] = 0.0f;
                } else {
                    values[k] = (ratios[k] * popByRoute[index].popByYear[birdIndex].extrapolatedCount[yearIndex]);
                }
            }

            // At this point, we have the 4 values of our nearest neighbors
            // Need to sum it up and Lerp
            pixelVal = SumArray(values) / popByRoute.Count;

            newPixels[i] = Color.Lerp(min, max, Mathf.Max(0, Mathf.Min(1, pixelVal)));

        }

        MNTexture2D.SetPixels(newPixels);
        MNTexture2D.Apply();

        //Change pixel color at each route location

        //width is 0->MNTexture2D.width
        //height is 0->MNTexture2D.height

        // We need to change:
        //(minLong, minLat) to(0,0)
        //(maxLong, maxLat) to(width, height)
        for (int i = 0; i < rData.Count; i++) {
            int x = ChangeScale_FtoI(0, MNTexture2D.width - 1, minLong, maxLong, rData[i].longitude);
            int y = ChangeScale_FtoI(0, MNTexture2D.height - 1, minLat, maxLat, rData[i].latitude);

            for (int j = x - 1; j < x + 2; j++) {
                for (int k = y - 1; k < y + 2; k++) {
                    MNTexture2D.SetPixel(j, k, Color.black);
                }
            }
        }
        MNTexture2D.Apply();

    }

    int ChangeScale_FtoI(int a, int b, float min, float max, float val) {
        return (int)(((b - a) * (val - min)) / (max - min) + a);
    }

    float SumArray(float[] array) {
        float total = 0;
        for (int i = 0; i < array.Length; i++) {
            total += array[i];
        }

        return total;
    }
}
