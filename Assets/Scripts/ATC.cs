using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using System.IO;


 // PlaneData Class
public class PlaneData
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

// class that handles the atc game logic
public class ATC : MonoBehaviour
{
    public Terminal terminal;   // terminal object where all departing planes originate from
    public Sky sky; // sky object where all arriving planes originate from
    Airplane selectedAirplane;  // the airplane that the user has currently selected
    public int selectedButton = -1; // the button that the user has currently selected, -1 if none
    public bool isDepartureButton = true; //notifies the aesthetic renderer if the button selected was a runwaya or departure button
    GameObject panel; //display panel object
    GameObject[] panelElements; //display panel elements
   
    public List<PlaneData> allPlaneData; // list of planeObject data pulled from csv

    public HashSet<int> chosenPlanes; // indices of the chosen planes (shared across sky and terminal)

    // called when a plane is selected from the departures UI
    void Start() {
        GameObject g = FindInActiveObjectByName("FlightDisplay");
        GameObject ui = g.transform.GetChild(0).gameObject;
        GameObject selection = ui.transform.GetChild(0).gameObject;
        panel = selection.transform.GetChild(0).gameObject;
        GameObject flightText = panel.transform.Find("FlightNumberText").gameObject;
        GameObject fuelText = panel.transform.Find("FuelText").gameObject;
        GameObject statusText = panel.transform.Find("StatusText").gameObject;
        GameObject waitText = panel.transform.Find("FlightNumberText").gameObject;
        GameObject description = panel.transform.Find("Description").gameObject;
        panelElements = new GameObject[5];
        panelElements[0] = flightText;
        panelElements[1] = fuelText;
        panelElements[2] = statusText;
        panelElements[3] = waitText;
        panelElements[4] = description;
        allPlaneData = new List<PlaneData>();
        chosenPlanes = new HashSet<int>();
        // chnage this path based on where you put the csv file
        readPlaneData("/Users/neal/final-approach-vr/Assets/VRPlaneInfo.csv");

    }

    // read plane data from csv file
    public void readPlaneData(string fileName) {
        StreamReader strReader = new StreamReader(fileName);
        bool endOfFile = false; 
        // while not end of file use ReadLine method to read comma delimited data
        while (!endOfFile) {
            string dataString = strReader.ReadLine();
            if (dataString == null) {
                endOfFile = true;
                break;
            }
            string[] dataValues = dataString.Split(',');
            PlaneData planeData = new PlaneData();
            // assign data values to plane data
            planeData.callSign = dataValues[0];
            planeData.iata = dataValues[1];
            planeData.icao = dataValues[2];
            planeData.arriveDepart = dataValues[3];
            planeData.adIATA = dataValues[4];
            planeData.planeType = dataValues[5];
            planeData.priority = dataValues[6];
            planeData.fuel = float.Parse(dataValues[7]);
            planeData.timeTillOnTime = float.Parse(dataValues[8]);
            planeData.planeAsset = dataValues[9];
            planeData.maxPassengers = int.Parse(dataValues[10]);
            planeData.planeSize = dataValues[11][0];
            planeData.minFuel = float.Parse(dataValues[12]);
            planeData.timeToTerminal = float.Parse(dataValues[13]);
            planeData.timeToRunway = float.Parse(dataValues[14]);
            planeData.timeToLand = float.Parse(dataValues[15]);
            planeData.timeToAir = float.Parse(dataValues[16]);
            allPlaneData.Add(planeData);
        }
    }


    public void selectDepartingAirplane(int index) {
        if (selectedAirplane == terminal._planes[index]) {  // deselect the currently selected plane
            selectedAirplane = null;
            selectedButton = -1;
            isDepartureButton = true;
            display(false, selectedAirplane);
        }else{  // select the selected plane
            selectedAirplane = terminal._planes[index];
            selectedButton = index;
            isDepartureButton = true;
            display(true, selectedAirplane);
        }
    }

    // called when a plane is selected from the arrivals UI
    public void selectArrivingAirplane(int index) {
        if (selectedAirplane == sky._planes[index]) {  // deselect the currently selected plane
            selectedAirplane = null;
            selectedButton = -1;
            isDepartureButton = false;
            display(false, selectedAirplane);
        }else{  // select the selected plane
            selectedAirplane = sky._planes[index];
            selectedButton = index;
            isDepartureButton = false;
            display(true, selectedAirplane);
        }
    }


    // called when a runway is selected from the runway UI (takes in a runway and a boolean representing whether the plane is 
    // departing or arriving)
    public void selectRunway(Runway runway) {
        if (selectedAirplane != null && (selectedAirplane.status == PlaneStatus.Terminal || 
        selectedAirplane.status ==  PlaneStatus.Circling)) {
              // ensure that the user has selected a plane and that the plane is at the terminal or circling
            if (runway.open) {
                selectedButton = -1;
                if(selectedAirplane.status == PlaneStatus.Terminal){
                    selectedAirplane.status = PlaneStatus.Taxiing;
                }else{
                    selectedAirplane.status = PlaneStatus.Landing;
                }
                runway.plane = selectedAirplane;
                runway.open = false;
            } else {
                // there is already a plane on that runway!
            }
            
        } else {
            // you must selected a plane first!
        }
       
    }
    //display the information on the monitor
    public void display(bool Display, Airplane a){
            GameObject g = FindInActiveObjectByName("FlightDisplay");
        if(Display){
            g.SetActive(true);
        }else{
            g.SetActive(false);
        }
    }

    //mainly to initialize the panel elements so we don't have to find them every time
    
    void Update(){
        if(selectedAirplane != null){
            panelElements[0].GetComponent<TMPro.TextMeshProUGUI>().text = selectedAirplane._flightName;
            panelElements[2].GetComponent<TMPro.TextMeshProUGUI>().text = selectedAirplane.status.ToString();
        }
    }

    //find object
    public static GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}

