using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct graph {
    public string name;
    public List<int> years;
    public List<float> extrapolatedCount;
}

public class Visualization : MonoBehaviour {
    [SerializeField]
    List<graph> birds;

    List<LineRenderer> lrs;
    public List<LineRenderer> GetLineRenderers() { return lrs; }

    // Start is called before the first frame update
    public Visualization() {
        birds = new List<graph>();
        lrs = new List<LineRenderer>();
    }

    public void Visualize(List<Organism> organisms) {
        foreach (Organism o in organisms) {
            List<int> years = new List<int>();
            List<float> extrapolatedCount = new List<float>();

            for (int i = 0; i < o.GetPopulationData().Count; i++) {
                years.Add(o.data[i].year);
                extrapolatedCount.Add(o.data[i].count / (float)o.data[i].numRoutes);
            }

            if (years.Count > 0) {

                graph b = new graph {
                    name = o.GetName(),
                    years = years,
                    extrapolatedCount = extrapolatedCount
                };
                birds.Add(b);
            }
        }

        foreach (graph b in birds) {
            // One line renderer per bird, so one gameObject per bird
            GameObject go = new GameObject(b.name);
            go.transform.parent = transform;

            LineRenderer lr = go.AddComponent<LineRenderer>();
            Color c = Random.ColorHSV(0, 1, 0, 1, 0, 1, 1, 1);

            List<Vector3> points = new List<Vector3>();
            for (int i = 0; i < b.years.Count; i++) {
                points.Add(new Vector3(b.years[i] % 1967, b.extrapolatedCount[i], 10));
            }

            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.widthMultiplier = 0.3f;

            lr.positionCount = b.years.Count;
            lr.SetPositions(points.ToArray());
            lr.startColor = c;
            lr.endColor = c;

            lrs.Add(lr);

        }
    }

}
