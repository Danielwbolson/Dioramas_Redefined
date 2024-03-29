﻿using System.Collections;
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
    public Visualization() {}

    public Texture2D Visualize(Diorama d, Texture2D MNTexture2D, Texture2D colormap, int birdIndex, int year, int maxCount) {

        // Initialization
        popByYear = new List<extrapPopDataByYear>();
        popByRoute = new List<extrapPopDataByRoute>();

        List<routeData> rData = d.popByRoute;
        Texture2D tex = new Texture2D(MNTexture2D.width, MNTexture2D.height);
        Graphics.CopyTexture(MNTexture2D, tex);

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
            int x = ChangeScale_FtoI(0, tex.width - 1, minLong, maxLong, rData[i].longitude);
            int y = ChangeScale_FtoI(0, tex.height - 1, minLat, maxLat, rData[i].latitude);

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

        Color max = Color.white;
        Color min = Color.black;
        Color[] pixels = tex.GetPixels();
        Color[] newPixels = new Color[pixels.Length];

        float[] values = new float[popByRoute.Count];
        float[] inverseDistances = new float[popByRoute.Count];
        float[] ratios = new float[popByRoute.Count];

        List<float> routeCounts = new List<float>();
        // Get our max value found so that we can normalize our colormap
        for (int i = 0; i < popByRoute.Count; i++) {
            routeData r = rData[i];

            // Find the route
            int index = popByRoute.FindIndex(route => route.routeId.CompareTo(r.routeID) == 0);

            // Find yearly data in this route
            int yearIndex = popByRoute[index].popByYear[birdIndex].years.IndexOf(year);

            // If it fails, add 0, else add it's pop value
            if (yearIndex == -1) {
                routeCounts.Add(0f);
            } else {
                routeCounts.Add(popByRoute[index].popByYear[birdIndex].extrapolatedCount[yearIndex]);
            }
        }

        /*
         * Starting per-pixel calculations
         */

        for (int i = 0; i < pixels.Length; i++) {

            // We don't want to edit transparent sections of the image, black, or white
            if (pixels[i].a == 0 || (pixels[i].r < 0.1f && pixels[i].g < 0.1f && pixels[i].b < 0.1f)
                || (pixels[i].r > 0.9f && pixels[i].g > 0.9f && pixels[i].b > 0.9f)) {
                newPixels[i] = pixels[i];
                continue;
            }

            // Initialize pixel value for our Lerp
            float pixelVal = 0;

            // Getting our inverse distance relationship values
            // Further away routes have less impact on pixels color
            int imgWidth = tex.width;
            int imgHeight = tex.height;
            for (int k = 0; k < popByRoute.Count; k++) {

                int px = rData[k].pixelX;
                int py = rData[k].pixelY;
                int x = i % imgWidth;
                int y = (i - x) / (imgWidth - 1);
                float dist =
                    (px - x)*(px - x) + (py - y)*(py - y);

                if (dist == 0) {
                    dist = 1f;
                }
                inverseDistances[k] = (1.0f / (dist*dist));
            }
            float totalDist = SumArray(inverseDistances);

            // Turn these inverseDistances into ratios to multiply by the pixels
            // Normalize the distances
            float inverseTotalDist = 1.0f / totalDist;
            for (int k = 0; k < popByRoute.Count; k++) {
                ratios[k] = inverseDistances[k] * inverseTotalDist;
            }

            // Multiply our pre-computed route population count data by our ratio for this specific pixel
            for (int k = 0; k < popByRoute.Count; k++) {
                if (ratios[k] < 0.0001f) {
                    values[k] = 0.0f;
                    continue;
                }
                values[k] = (ratios[k] * routeCounts[k]);
            }

            // Need to sum it up and Lerp
            pixelVal = Mathf.Max(Mathf.Min(SumArray(values) / maxCount, 1), 0);
            newPixels[i] = colormap.GetPixel((colormap.width-1) - (int)(pixelVal * (colormap.width-1)), 0);
            //newPixels[i] = Color.Lerp(Color.black, Color.white, Mathf.Clamp(pixelVal, 0.001f, 0.999f));
        }

        // Apply our pixel colors to our textures
        tex.SetPixels(newPixels);
        tex.Apply();

        //    // Draw our route locations on our texture as well for debugging
        //    for (int i = 0; i < rData.Count; i++) {
        //        int x = rData[i].pixelX;
        //        int y = rData[i].pixelY;

        //        MNTexture2D.SetPixel(x, y, Color.black);
        //    }
        //    MNTexture2D.Apply();

        return tex;

    }


    /*
     * Helper functions
     */

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
