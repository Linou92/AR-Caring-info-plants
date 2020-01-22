using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net.NetworkInformation;

public class GetMyWeather : MonoBehaviour
{
    public string currentIP; //current my IP online
    public string currentCity; //current my city

    public string stCity; //Current city
    public string stTemperature; //Current temperature data
    public string stConditionContent; //Current condition description data
    public string stConditionImage; //Current condition image URL

    public List<MeshRenderer> AllConditionImageList = new List<MeshRenderer>();  //condition image
    public List<TextMesh> AllTemperatureTextList = new List<TextMesh>(); //temperature
    public List<TextMesh> AllConditionDataList = new List<TextMesh>(); //condition description
    public List<TextMesh> AllCityDataList = new List<TextMesh>(); //city

    private float currentLongitude = 0f;  //newest longitude
    private float currentLatitude = 0f; //newest latitude

    public float originalLongitude; //original longitude
    public float originalLatitude; //original latitude

    public GameObject ErrorDlg1;
    public GameObject ErrorDlg2;
    public GameObject ErrorDlg3;

    void Start()
    {
#if UNITY_EDITOR
        StartCoroutine(SendRequestforPC()); //Get current weather data on PC
#elif UNITY_ANDROID || UNITY_IOS
        StartCoroutine (GetCoordinates()); //Get current GPS data on mobile
#endif
    }

    IEnumerator GetCoordinates()  //Get current GPS data on mobile
    {
        while (true)
        {
            if (!Input.location.isEnabledByUser) //if GPS service is disabled, exit
            {
                Debug.Log("Location is not enabled by user.");
                ErrorDlg1.SetActive(true);
                yield break;
            }

            Input.location.Start(1f, .1f);  //accuracy : 1 meter, update GPS data/0.1 meter
            int maxwait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxwait > 0) //enable GPS servcie
            {
                yield return new WaitForSeconds(1);
                maxwait--;
            }

            if (maxwait <= 0)
            {
                Debug.Log("Timed Out.");
                ErrorDlg2.SetActive(true);
                yield break;
            }

            if (Input.location.status == LocationServiceStatus.Failed) //if GPS service is fail, exit
            {
                Debug.Log("Unable to determine location.");
                ErrorDlg3.SetActive(true);
                yield break;
            }
            else
            {
                currentLatitude = Input.location.lastData.latitude; //get current latitude
                currentLongitude = Input.location.lastData.longitude; //get current langitude
            }
        }
    }

    public void Update()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID || UNITY_IOS

        float a = Mathf.Abs(currentLatitude - Input.location.lastData.latitude); //get delta between prev lat and new lat data
        float b = Mathf.Abs(currentLongitude - Input.location.lastData.longitude); //get delta between prev lon and new lon data

        if(a > 0.001f || b > 0.001f) //if delta is too big, update GPS data
        {
            currentLatitude = Input.location.lastData.latitude; //update latitude
            currentLongitude = Input.location.lastData.longitude; //update longitude
            StartCoroutine(SendRequestforMobile()); //update weather data
        }
#endif
    }

    IEnumerator SendRequestforMobile() //Get weather data for mobile
    {
        string q = currentLatitude + "," + currentLongitude; //make parameters with current latitude and longitude

        //Get weather from weather site
        WWW www = new WWW("https://api.apixu.com/v1/current.json?key=9c7878a483664ef6928185252170905&q=" + q);
        yield return www;

        if (www.error == null)
        {
            var WeatherData = JSON.Parse(www.text);
            stCity = WeatherData["location"]["name"]; //Get current location
            stTemperature = WeatherData["current"]["temp_c"]; //Get current temperature
            stConditionContent = WeatherData["current"]["condition"]["text"]; //Get current condition description
            stConditionImage = WeatherData["current"]["condition"]["icon"]; //Get current condition image

            for (int i = 0; i < AllTemperatureTextList.Count; i++) //Display current temperature for all plants
            {
                AllTemperatureTextList[i].text = stTemperature + "°C ";
            }
            for (int i = 0; i < AllConditionDataList.Count; i++) //Display current condition description for all plants
            {
                AllConditionDataList[i].text = stConditionContent;
            }

            //Get city from mapbox site
            WWW city = new WWW("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + q
                + "&sensor=true&key=AIzaSyBIM-m-w_J9jKndAaprRR-aonk4VatgIMs");
            yield return city;

            if (city.error == null)
            {
                var cityData = JSON.Parse(city.text);

                string stCity = "";
                if (cityData["results"][0]["address_components"][1]["types"][0].Value == "postal_town")
                {
                    stCity = cityData["results"][0]["address_components"][1]["long_name"];
                }

                if (cityData["results"][0]["address_components"][2]["types"][0].Value == "postal_town")
                {
                    stCity = cityData["results"][0]["address_components"][2]["long_name"];
                }

                if (cityData["results"][0]["address_components"][3]["types"][0].Value == "postal_town")
                {
                    stCity = cityData["results"][0]["address_components"][3]["long_name"];
                }

                for (int i = 0; i < AllCityDataList.Count; i++) //Display current city for all plants
                {
                    AllCityDataList[i].text = stCity;
                }
            }
            else
            {
                Debug.Log("WWW error: " + city.error);
            }
        }
        else
        {
            Debug.Log("WWW error: " + www.error);
        }

        //Get weather image
        WWW conditionRequest = new WWW("https:" + stConditionImage);
        yield return conditionRequest;

        if (conditionRequest.error == null || conditionRequest.error == "")
        {
            for (int i = 0; i < AllConditionImageList.Count; i++)  //Display current condition image for all plants
            {
                Material mat = AllConditionImageList[i].material;
                mat.mainTexture = conditionRequest.texture;
                AllConditionImageList[i].material = mat;
            }
        }
        else
        {
            Debug.Log("WWW error: " + conditionRequest.error);
        }
    }

    IEnumerator SendRequestforPC() //get weather data for PC
    {
        //Get my ip
        WWW IPRequest = new WWW("https://ipapi.co/json/");
        yield return IPRequest;

        if (IPRequest.error == null || IPRequest.error == "")
        {
            var IPData = JSON.Parse(IPRequest.text);
            currentIP = IPData["ip"];  //Get my current ip
            currentCity = IPData["city"]; //Get my current city
        }
        else
        {
            Debug.Log("WWW error: " + IPRequest.error);
        }

        //Get weather from weather site
        WWW www = new WWW("https://api.apixu.com/v1/current.json?key=9c7878a483664ef6928185252170905&q=" + currentIP);
        //        WWW www = new WWW("https://api.apixu.com/v1/current.json?key=9c7878a483664ef6928185252170905&q=" + currentCity);
        yield return www;

        if (www.error == null)
        {
            var WeatherData = JSON.Parse(www.text);
            stCity = WeatherData["location"]["name"]; //Get current location
            stTemperature = WeatherData["current"]["temp_c"]; //Get current temperature
            stConditionContent = WeatherData["current"]["condition"]["text"]; //Get current condition description
            stConditionImage = WeatherData["current"]["condition"]["icon"]; //Get current condition image

            for (int i = 0; i < AllTemperatureTextList.Count; i++) //Display current temperature for all plants
            {
                AllTemperatureTextList[i].text = stTemperature + "°C ";
            }
            for (int i = 0; i < AllConditionDataList.Count; i++) //Display current condition description for all plants
            {
                AllConditionDataList[i].text = stConditionContent;
            }
            for (int i = 0; i < AllCityDataList.Count; i++) //Display current city for all plants
            {
                AllCityDataList[i].text = currentCity;
            }
        }
        else
        {
            Debug.Log("WWW error: " + www.error);
        }

        //Get weather image
        WWW conditionRequest = new WWW("https:" + stConditionImage);
        yield return conditionRequest;

        if (conditionRequest.error == null || conditionRequest.error == "")
        {
            for (int i = 0; i < AllConditionImageList.Count; i++)  //Display current condition image for all plants
            {
                Material mat = AllConditionImageList[i].material;
                mat.mainTexture = conditionRequest.texture;
                AllConditionImageList[i].material = mat;
            }
        }
        else
        {
            Debug.Log("WWW error: " + conditionRequest.error);
        }
    }
}