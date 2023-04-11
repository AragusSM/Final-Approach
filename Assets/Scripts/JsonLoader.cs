using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameObjectData
{
  
    public string Callsign;
    public string IATA;
    public string ICAO;
    public string PlaneIdent;
    public string AD;
    public string AD_IATA;
    public string PlaneType;
    public string PriorityEmergency;
    public float Fuel;
    public float TimeTillOntimeArriveDepart;
    public string PlaneAsset;
    public int MaxPassengers;
    public char PlaneSize;
    public float MinFuel;
    public float timeToTerminal;
    public float timeToRunway;
    public float timeToLand;
    public float timeToAir;
}

[System.Serializable]
public class GameObjectList
{
    public List<GameObjectData> gameObjects;
}

public class JsonLoader : MonoBehaviour
{
    public string csvjson;
    public GameObject DaddyPlane;

    void Start()
    {
        LoadJsonData();
    }

    private void LoadJsonData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(csvjson);
        GameObjectList gameObjectList = JsonUtility.FromJson<GameObjectList>(jsonFile.text);

        foreach (GameObjectData gameObjectData in gameObjectList.gameObjects)
        {
            GameObject obj = Instantiate(DaddyPlane, gameObjectData.Callsign, gameObjectData.IATA, gameObjectData.ICAO, gameObjectData.PlaneIdent, gameObjectData.AD, gameObjectData.AD_IATA, gameObjectData.PlaneType, gameObjectData.PriorityEmergency, gameObjectData.Fuel, gameObjectData.TimeTillOntimeArriveDepart, gameObjectData.PlaneAsset, gameObjectData.MaxPassengers, gameObjectData.PlaneSize, gameObjectData.MinFuel, gameObjectData.timeToTerminal, gameObjectData.timeToRunway, gameObjectData.timeToLand, gameObjectData.timeToAir);
           

        }
    }
}
