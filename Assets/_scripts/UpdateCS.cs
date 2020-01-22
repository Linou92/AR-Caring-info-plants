using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UpdateCS : MonoBehaviour {

    public int m_Index;   //unique index for plants

    public InputField LastData;  //input new water-date.
    public Text UpdateTxt;  //next water-date.

    public GameObject Warining;

    // Use this for initialization
    void Start () {

        GetDateData();  //Get next water-date.
     }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateButtonClick() //When click update button
    {
        DateTime d_UpdateDate;

        if (DateTime.TryParse(LastData.text, out d_UpdateDate))  //Check whether the input data is in date format or not.
        {
            String.Format("{0:mm/dd/yyyy}", d_UpdateDate);

            SetDateData(d_UpdateDate.ToShortDateString());  
        }
        else
        {
            Warining.SetActive(true);  //If input data isn't in date format, displays warning message.
        }
    }

    bool isValidData(string stNew) //Check whether the input data is in number and one symbol(/)
    {
        string stReguar = "0123456789/";

        for (int i = 0; i < stNew.Length; i++)
        {
            if (!stReguar.Contains(stNew[i].ToString()))
            {
                return false; //If the input data includes other character, returns false;
            }
        }
        return true;
    }

    public void OnChange(string stNew) //Check input date whenever user input data.
    {
        if (stNew.Length > 0)
        {
            if (!isValidData(stNew))
            {
                Warining.SetActive(true); //If the input data includes other character, display warning message;
            }
        }
    }

    void GetDateData() //Displays updated date.
    {
        switch (m_Index)    //unique index for plants
        {
            case 1: //Aloe vera plant
                if (PlayerPrefs.HasKey("Aloe"))
                {
                    UpdateTxt.text = PlayerPrefs.GetString("Aloe"); //Display stored next water-date
                }
                break;
            case 2: //Pink plant
                if (PlayerPrefs.HasKey("Pink"))
                {
                    UpdateTxt.text = PlayerPrefs.GetString("Pink"); //Display stored next water-date
                }
                break;
            case 3: //Basilica
                if (PlayerPrefs.HasKey("Basilica"))
                {
                    UpdateTxt.text = PlayerPrefs.GetString("Basilica"); //Display stored next water-date
                }
                break;
            case 4: //Mint
                if (PlayerPrefs.HasKey("Mint"))
                {
                    UpdateTxt.text = PlayerPrefs.GetString("Mint"); //Display stored next water-date
                }
                break;
            case 5: //Parsley
                if (PlayerPrefs.HasKey("Parsley"))
                {
                    UpdateTxt.text = PlayerPrefs.GetString("Parsley"); //Display stored next water-date
                }
                break;
            case 6: //Rubber
                if (PlayerPrefs.HasKey("Rubber"))
                {
                    UpdateTxt.text = PlayerPrefs.GetString("Rubber"); //Display stored next water-date
                }
                break;
            case 7: //Spider
                if (PlayerPrefs.HasKey("Spider"))
                {
                    UpdateTxt.text = PlayerPrefs.GetString("Spider"); //Display stored next water-date
                }
                break;
        }
    }

    void SetDateData(string stUpdatedData) //Calculate next water date.
    {
        DateTime d_OldDate = DateTime.Parse(stUpdatedData); //Check input data.

        bool isWinter;
        if (d_OldDate.Month <= 2 || d_OldDate.Month >= 11) //If input date is in winter period.
        {
            isWinter = true;
        }
        else
        {
            isWinter = false;
        }

        switch (m_Index) //unique index for plants
        {
            case 1: //Aloe vera
                if (isWinter) //If winter, once/per 4 weeks
                {
                    d_OldDate = d_OldDate.AddDays(4f * 7f);
                }
                else //If no winter, once/3 weeks
                {
                    d_OldDate = d_OldDate.AddDays(3f * 7f);
                }

                PlayerPrefs.SetString("Aloe", d_OldDate.ToShortDateString()); //Store next water-date
                UpdateTxt.text = d_OldDate.ToShortDateString(); //Displays next water-date
                break;
            case 2: //Pink
                {
                    d_OldDate = d_OldDate.AddDays(1f * 7f); //once a week
                }

                PlayerPrefs.SetString("Pink", d_OldDate.ToShortDateString()); //Store next water-date
                UpdateTxt.text = d_OldDate.ToShortDateString(); //Displays next water-date
                break;
            case 3: //Basilica
                {
                    d_OldDate = d_OldDate.AddDays(1f * 7f); //once a week
                }

                PlayerPrefs.SetString("Basilica", d_OldDate.ToShortDateString()); //Store next water-date
                UpdateTxt.text = d_OldDate.ToShortDateString(); //Displays next water-date
                break;
            case 4: //Mint
                {
                    d_OldDate = d_OldDate.AddDays(3.5f); //once 3 ~ 4days of a week
                }

                PlayerPrefs.SetString("Mint", d_OldDate.ToShortDateString()); //Store next water-date
                UpdateTxt.text = d_OldDate.ToShortDateString(); //Displays next water-date
                break;
            case 5: //Parsley
                {
                    d_OldDate = d_OldDate.AddDays(3.5f); //once 3 ~ 4days of a week
                }

                PlayerPrefs.SetString("Parsley", d_OldDate.ToShortDateString()); //Store next water-date
                UpdateTxt.text = d_OldDate.ToShortDateString(); //Displays next water-date
                break;
            case 6: //Rubber
                if (isWinter)
                {
                    d_OldDate = d_OldDate.AddDays(2.5f * 7f); //If winter, once in 2 ~ 3 weeks
                }
                else
                {
                    d_OldDate = d_OldDate.AddDays(1f * 7f);//If no winter, once a week
                }

                PlayerPrefs.SetString("Rubber", d_OldDate.ToShortDateString()); //Store next water-date
                UpdateTxt.text = d_OldDate.ToShortDateString(); //Displays next water-date
                break;
            case 7: //Spider
                {
                    d_OldDate = d_OldDate.AddDays(1f * 7f); //once a week
                }

                PlayerPrefs.SetString("Spider", d_OldDate.ToShortDateString()); //Store next water-date
                UpdateTxt.text = d_OldDate.ToShortDateString(); //Displays next water-date
                break;
        }
    }
}
