using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameObjectData
{
  
    public string callSign;
    public string iata;
    public string icao;
    public string arriveDepart;
    public string adIATA;
    public string planeType;
    public string priority;
    public float fuel;
    public float timeTillOnTime;
    public string planeAsset;
    public int maxPassengers;
    public char planeSize;
    public float minFuel;
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
    public string fileName;
    public GameObject prefab;

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
            GameObject obj = Instantiate(prefab, gameObjectData.callSign, gameObjectData.IATA, gameObjectData.ICAO, gameObjectData.PlaneIdent, gameObjectData.AD, gameObjectData.AD_IATA, gameObjectData.PlaneType, gameObjectData.PriorityEmergency, gameObjectData.Fuel, gameObjectData.TimeTillOntimeArriveDepart, gameObjectData.PlaneAsset, gameObjectData.MaxPassengers, gameObjectData.PlaneSize, gameObjectData.MinFuel, gameObjectData.timeToTerminal, gameObjectData.timeToRunway, gameObjectData.timeToLand, gameObjectData.timeToAir);
            obj.name = gameObjectData.name;
        }
    }
}
