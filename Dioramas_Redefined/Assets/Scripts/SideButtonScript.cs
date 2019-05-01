using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideButtonScript : MonoBehaviour
{

    public GameObject DioPanel;
    public GameObject VizPanel;

    // Start is called before the first frame update
    void Start()
    {
        ChangeView(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeView(int choice)
    {
        switch (choice)
        {
            case 0:
                DioPanel.SetActive(true);
                VizPanel.SetActive(false);
                break;
            case 1:
                VizPanel.SetActive(true);
                DioPanel.SetActive(false);
                break;
            default:
                break;

        }
    }
}
