﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Parse {

    /*
     * Parses CSV file delimmited by ~ and puts it in a Diorama object
     * Then matches each organism with their classification to allow sorting later
     */
    public static void ParseCSV(ref Diorama d, string data, string classification) {

        string[] fileData = System.IO.File.ReadAllLines(data);

        string name = null;
        string latinName = null;
        string habitat = null;
        string inTheScene = null;
        string didYouKnow = null;
        string family = null;

        for (int i = 0; i < fileData.Length; i++) {

            string s = fileData[i];

            // ~ is the delimitter, if we run into one by itself, we are starting a new Organism
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

        }


        /* 
         * Setting classifications of each Organism for future sorting
         */
        string[] classData = System.IO.File.ReadAllLines(classification);
        Classification curr = 0;

        for (int i = 0; i < classData.Length; i++) {

            string lineData = classData[i].Trim();

            // Early exit on blank lines
            if (lineData.Length == 0)
                continue;

            // Get our classification
            if (lineData.Contains("MAMMALS"))
                curr = Classification.mammal;
            else if (lineData.Contains("BIRDS"))
                curr = Classification.bird;
            else if (lineData.Contains("REPTILES"))
                curr = Classification.reptile;
            else if (lineData.Contains("FISH"))
                curr = Classification.fish;
            else if (lineData.Contains("INVERTEBRATES"))
                curr = Classification.invertebrate;
            else if (lineData.Contains("FUNGI"))
                curr = Classification.fungi;
            else if (lineData.Contains("PLANTS"))
                curr = Classification.plant;

            // Get our chunks of what to check against for determining class
            string[] lineChunks = lineData.Split(' ');

            // Run through organisms and set their Classifications
            for (int k = 0; k < d.organisms.Count; k++) {

                // If our last part is a number
                if (int.TryParse(lineChunks[lineChunks.Length-1], out int aou)) {

                    // Run through and see if all chunks are a part of the name. If any aren't, break out with a false flag
                    bool rightClass = true;
                    for (int m = 0; m < lineChunks.Length-1; m++) {
                        if (!d.organisms[k].GetName().Contains(lineChunks[m])) {
                            rightClass = false;
                            break;
                        }
                    }

                    // If we have the right class, this will still be true
                    if (rightClass) {
                        // If we made it here, our animal had a number at the end but we matched it anyways
                        // The number at the end was it's aou number, which is useful for other data
                        d.organisms[k].classification = curr;
                        d.organisms[k].aou = aou;
                    }

                } else { // No number

                    // Run through and see if all chunks are a part of the name. If any aren't, break out with a false flag
                    bool rightClass = true;
                    for (int m = 0; m < lineChunks.Length; m++) {
                        if (!d.organisms[k].GetName().Contains(lineChunks[m])) {
                            rightClass = false;
                            break;
                        }
                    }

                    // If we have the right class, this will be true
                    if (rightClass) {
                        // If we made it right here, we have the correct class for the animal and will thus set it
                        d.organisms[k].classification = curr;
                    }
                }
            }
        }
    }

    /* Runs through our data from BBS and determines counts of birds over
     * years and routes
     */
    public static void ParseBBSData(ref Diorama d, string filepath) {
        string[] fileData = System.IO.File.ReadAllLines(filepath);

        List<routeData> routes = new List<routeData>();
        int index;

        // Deep copy of organisms
        List<Organism> copyOfOrganisms = new List<Organism>();
        for (int i = 0; i < d.organisms.Count; i++) {
            copyOfOrganisms.Add(d.organisms[i].Clone());
        }

        // First line is text, so start on second
        for (int i = 1; i < fileData.Length; i++) {
            string s = fileData[i];

            string[] lineData = s.Trim().Split(',');

            // If we have no info on this route, add it to our list
            string routeId = lineData[2];

            // Get a Deep Copy of our static info
            List<Organism> currClone = new List<Organism>();
            for (int j = 0; j < copyOfOrganisms.Count; j++) {
                currClone.Add(copyOfOrganisms[j].Clone());
            }
            routeData r = new routeData {
                routeID = routeId,
                organisms = currClone
            };

            // Get our current species info
            int aou = int.Parse(lineData[4]);
            yearData p = new yearData {
                year = int.Parse(lineData[3]),
                numRoutes = 1,
                count = int.Parse(lineData[5])
            };

            // Find the animal with the matching aou so we can add data to it
            for (int k = 0; k < d.organisms.Count; k++) {
                if (d.organisms[k].aou == aou) {

                    // We have found the matching animal, now we need to see if it
                    // already has data for the year we are on
                    index = d.organisms[k].data.FindIndex(data => data.year == p.year);
                    if (index == -1) {

                        // It did not have existing data, so we add it
                        d.organisms[k].data.Add(p);
                    } else if (d.organisms[k].classification == Classification.bird) {

                        // The data already existed, so now we increment
                        int totalCounts = p.count + d.organisms[k].data[index].count;
                        int totalRoutes = d.organisms[k].data[index].numRoutes + 1;
                        d.organisms[k].data[index] = new yearData {
                            year = p.year,
                            numRoutes = totalRoutes,
                            count = totalCounts
                        };

                    }
                }
            }

            // See if we already have this route number, if not add it
            index = routes.FindIndex(route => string.Compare(route.routeID, routeId) == 0);
            if (index == -1) {
                routes.Add(r.Clone());
                index = routes.Count - 1;
            }

            // Get a reference to our routes data
            List<Organism> o = routes[index].organisms;
            int organismIndex = o.FindIndex(organism => organism.GetAOU() == aou);
            if (o[organismIndex].classification == Classification.bird) {

                // Add data on this bird for our route
                index = o[organismIndex].data.FindIndex(data => data.year == p.year);
                if (index == -1) {

                    // It did not have existing data, so we add it
                    o[organismIndex].data.Add(p);
                } else { 
                    // The data already existed, so now we increment
                    int totalCounts = p.count + o[organismIndex].data[index].count;
                    int totalRoutes = o[organismIndex].data[index].numRoutes + 1;
                    o[organismIndex].data[index] = new yearData {
                        year = p.year,
                        numRoutes = totalRoutes,
                        count = totalCounts
                    };

                }
            }
        }

        // Now that all data is stored in birds, sort them so they are sequential
        for (int i = 0; i < d.organisms.Count; i++) {
            d.organisms[i].data.Sort((d1, d2) => d1.year.CompareTo(d2.year));
        }

        // Sort our birds by year, per route
        foreach (routeData p in routes) {
            for (int i = 0; i < p.organisms.Count; i++) {
                p.organisms[i].data.Sort((d1, d2) => d1.year.CompareTo(d2.year));
            }
        }
        d.popByRoute = routes;
    }

    // Parse our routes and location data into a use able list/struct
    public static void ParseRouteData(ref Diorama d, string filepath) {
        string[] fileData = System.IO.File.ReadAllLines(filepath);

        // First line is headers
        for (int i = 1; i < fileData.Length; i++) {
            string s = fileData[i];

            string[] lineData = s.Trim().Split(',');

            string rId = lineData[2];
            float rLatitude = float.Parse(lineData[4]);
            float rLongitude = float.Parse(lineData[5]);

            // Find our already stored route data
            int index = d.popByRoute.FindIndex(r => r.routeID.CompareTo(rId) == 0);

            routeData route = new routeData {
                routeID = rId,
                organisms = d.popByRoute[index].organisms,
                latitude = rLatitude,
                longitude = rLongitude,
            };

            // Update our stored route with the lat/long info
            d.popByRoute[index] = route;
        }
    }
}
