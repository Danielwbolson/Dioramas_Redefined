using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diorama : MonoBehaviour {

    public Diorama() {
        organisms = new List<Organism>();
    }

    [SerializeField]
    public List<Organism> organisms;

}
