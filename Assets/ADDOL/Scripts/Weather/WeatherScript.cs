using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



public class WeatherScript : MonoBehaviour
{
    public Image weatherIcon;
    public Text CityName;
    //public Text Location;
    public Text Temp;
    public Text Weather;
    public SpriteRenderer spriteriner;
    public Sprite[] SpriteList;
    public Sprite nope;
    public string myCity;
    public string myIcon;
    public string myWeather;
    public string myTemp;
    public string myLocation;
    public GameObject Canvas;
    private DataList DataList = new DataList();
    private ImporterModel importerModel = new ImporterModel();

    // Use this for initialization
    void Start()
    {
        getCity();
        StartCoroutine(GetText());
    }

    private string getCity()
    {
        string path;
        string jsonFile;
        //path = Application.streamingAssetsPath + "/Location.json";
        path = Application.dataPath + "/ADDOL/StreamingAssets" + "/Location.json";
        Debug.Log(path);
        if (File.Exists(path))
        {
            jsonFile = File.ReadAllText(path);
        }
        else
        {
            Debug.Log("File does not exist.");
            jsonFile = null;
        }

        if (jsonFile != null)
        {
            importerModel = JsonUtility.FromJson<ImporterModel>(jsonFile);
            myCity = importerModel.City + "," + importerModel.Country;
        }
        else
        {
            Debug.Log("reverting to Hardcoded Location :)");
            myCity = "Portsmouth,UK";
        }
        return myCity;
    }


    IEnumerator GetText()
    {
        using (UnityWebRequest issou = UnityWebRequest.Get("http://api.openweathermap.org/data/2.5/weather?q=" + myCity + "&units=metric"))
        {
            issou.SetRequestHeader("x-api-key", "07e479cfae11ebfb7ba4602fb5705322");
            yield return issou.Send();

            if (issou.isNetworkError || issou.isHttpError)
            {
                Debug.Log(issou.error);
            }
            else
            {
                if (issou.isDone)
                {
                    // Or retrieve results as binary data
                    string jsonString = issou.downloadHandler.text;
                    DataList = JsonUtility.FromJson<DataList>(jsonString);

                    foreach (weather item in DataList.weather)
                    {
                        Debug.Log(item.icon);
                        myWeather = item.main + " \n" + item.description;
                        myIcon = item.icon;
                    }
                    Debug.Log(DataList.main.temp);
                    myTemp = DataList.main.temp + "°C";
                    //myLocation = "Lon:" + DataList.coord.lon + "; Lat :" + DataList.coord.lat;
                    //ChangeIcon(myIcon, myTemp, myCity, myLocation);
                    Temp.text = myTemp;
                    CityName.text = myCity;
                    Weather.text = myWeather;

                    spriteriner = gameObject.GetComponent<SpriteRenderer>();

                    switch (myIcon)
                    {
                        case "01d":
                            weatherIcon.overrideSprite = SpriteList[0];
                            break;
                        case "01n":
                            weatherIcon.overrideSprite = SpriteList[1];
                            break;
                        case "02d":
                            weatherIcon.overrideSprite = SpriteList[2];
                            break;
                        case "02n":
                            weatherIcon.overrideSprite = SpriteList[3];
                            break;
                        case "03d":
                            weatherIcon.overrideSprite = SpriteList[4];
                            break;
                        case "03n":
                            weatherIcon.overrideSprite = SpriteList[5];
                            break;
                        case "04d":
                            weatherIcon.overrideSprite = SpriteList[6];
                            break;
                        case "04n":
                            weatherIcon.overrideSprite = SpriteList[7];
                            break;
                        case "09d":
                            weatherIcon.overrideSprite = SpriteList[8];
                            break;
                        case "09n":
                            weatherIcon.overrideSprite = SpriteList[9];
                            break;
                        case "10d":
                            weatherIcon.overrideSprite = SpriteList[10];
                            break;
                        case "10n":
                            weatherIcon.overrideSprite = SpriteList[11];
                            break;
                        case "11d":
                            weatherIcon.overrideSprite = SpriteList[12];
                            break;
                        case "11n":
                            weatherIcon.overrideSprite = SpriteList[13];
                            break;
                        case "13d":
                            weatherIcon.overrideSprite = SpriteList[14];
                            break;
                        case "13n":
                            weatherIcon.overrideSprite = SpriteList[15];
                            break;
                        case "50d":
                            weatherIcon.overrideSprite = SpriteList[16];
                            break;
                        case "50n":
                            weatherIcon.overrideSprite = SpriteList[17];
                            break;
                        default:
                            weatherIcon.overrideSprite = nope;
                            break;
                    }


                }
            }
        }
    }
}


