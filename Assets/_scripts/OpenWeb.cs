using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWeb : MonoBehaviour {

    public string WebURL_Aloe; //Website URL for Aloe
    public string WebURL_Pink; //Website URL for Pink
    public string WebURL_Basilica; //Website URL for Basilica
    public string WebURL_Mint; //Website URL for Mint
    public string WebURL_Parsley; //Website URL for Parsley
    public string WebURL_Rubber; //Website URL for Rubber
    public string WebURL_Spider; //Website URL for Spider

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetMouseButtonDown(0)) //If user clicks mouse
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null)
                {
                    OpenSite(hit.transform.gameObject.tag); //If user clicks website part area.
                }
            }
        }
    }

    void OpenSite(string stPlantName) //Open website for plants
    {
        switch(stPlantName)     //unique name for plants
        {
            case "Aloe":
                Application.OpenURL(WebURL_Aloe); //Open website for this plant
                break;
            case "Pink":
                Application.OpenURL(WebURL_Pink); //Open website for this plant
                break;
            case "Basilica":
                Application.OpenURL(WebURL_Basilica); //Open website for this plant
                break;
            case "Mint":
                Application.OpenURL(WebURL_Mint); //Open website for this plant
                break;
            case "Parsley":
                Application.OpenURL(WebURL_Parsley); //Open website for this plant
                break;
            case "Rubber":
                Application.OpenURL(WebURL_Rubber); //Open website for this plant
                break;
            case "Spider":
                Application.OpenURL(WebURL_Spider); //Open website for this plant
                break;
        }
    }
}
