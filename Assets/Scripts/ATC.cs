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
    public TextAsset textAssetData;
    public AudioSource planeSound2;

    // called when a plane is selected from the departures UI
    void Start() {
        GameObject g = FindInActiveObjectByName("FlightDisplay");
        GameObject ui = g.transform.GetChild(0).gameObject;
        GameObject selection = ui.transform.GetChild(0).gameObject;
        panel = selection.transform.GetChild(0).gameObject;
        GameObject flightText = panel.transform.Find("FlightNumberText").gameObject;
        GameObject fuelText = panel.transform.Find("FuelText").gameObject;
        GameObject statusText = panel.transform.Find("StatusText").gameObject;
        GameObject waitText = panel.transform.Find("WaitText").gameObject;
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
        string csvString = "Air Force ONE,N/A,N/A,Andrews Air Force Base,AFB,Boeing 747,Priority,28,230,passagiermaschine_747_static,416,A,3,20,20,15,15\nKorean Air 539,KAL539,KE539,Seoul,ICN,Boeing 747,N/A,36,-325,passagiermaschine_747_static,416,A,3,20,20,15,15\nLufthansa 419,DLH419,LH419,Frankfurt,FRA,Boeing 747,N/A,25,120,passagiermaschine_747_static,416,A,3,20,20,15,15\nQatar Airways 8830,QTR8830,QR8830,Melbourne,MEL,Boeing 747,N/A,38,215,passagiermaschine_747_static,416,A,3,20,20,15,15\nCathay Pacific 3081,CPA3081,CX3081,Hong Kong,HKG,Boeing 747,N/A,36,295,passagiermaschine_747_static,416,A,3,20,20,15,15\nSingapore Airlines 7290,SIA7290,SQ7290,Singapore,SIN,Boeing 747,N/A,6,-3,passagiermaschine_747_static,416,A,3,20,20,15,15\nAir Force ABYSS,N/A,N/A,Ramstein Air Base,RMS,Lockheed C-130,Priority,17,-498,frachtmaschine_antonov_static,124,A,3,20,20,15,15\nAir Force KILO,N/A,N/A,RAF Mildenhall,MHZ,Lockheed C-130,Priority,7,-529,frachtmaschine_antonov_static,124,A,3,20,20,15,15\nAir Force SCRON,N/A,N/A,Peterson Air Force Base,COS,Lockheed C-130,Priority,38,-199,frachtmaschine_antonov_static,124,A,3,20,20,15,15\nAir Force EDGAR,N/A,N/A,Bagram Air Base,OAI,Lockheed C-130,Priority,22,-509,frachtmaschine_antonov_static,124,A,3,20,20,15,15\nIcelandair 614,ICE614,FI614,New York JFK,JFK,Boeing 757,N/A,17,77,frachtmaschine_757_static,239,B,4,20,20,15,15\nDelta 937,DAL937,DL937,Denver,DEN,Boeing 757,N/A,52,272,frachtmaschine_757_static,239,B,4,20,20,15,15\nUnited 1976,UAL1976,UA1976,San Francisco,SFO,Boeing 757,N/A,47,89,frachtmaschine_757_static,239,B,4,20,20,15,15\nDelta 947,DAL947,DL947,Atlanta,ATL,Boeing 757,N/A,36,231,frachtmaschine_757_static,239,B,4,20,20,15,15\nUnited 1432,UAL1432,UA1432,Los Angeles,LAX,Boeing 757,N/A,18,-489,frachtmaschine_757_static,239,B,4,20,20,15,15\nUnited 714,UAL714,UA714,Chicago,ORD,Boeing 757,N/A,12,-124,frachtmaschine_757_static,239,B,4,20,20,15,15\nUnited 724,UAL724,UA724,Honolulu,HNL,Boeing 757,N/A,45,155,frachtmaschine_757_static,239,B,4,20,20,15,15\nDelta 2120,DAL2120,DL2120,Las Vegas,LAS,Boeing 757,N/A,11,88,frachtmaschine_757_static,239,B,4,20,20,15,15\nUnited 2310,UAL2310,UA2310,Boston,BOS,Boeing 757,N/A,47,-428,frachtmaschine_757_static,239,B,4,20,20,15,15\nUnited 8116,UAL8116,UA8116,Newark,EWR,Boeing 757,N/A,22,-104,frachtmaschine_757_static,239,B,4,20,20,15,15\nDelta 1426,DAL1426,DL1426,Minneapolis,MSP,Boeing 757,N/A,29,-297,frachtmaschine_757_static,239,B,4,20,20,15,15\n";
        
        string[] csvStringDelimed = csvString.Split('\n');
        // string[] newCSVArr = textAssetData.Split(new string{"\n"}, System.StringSplitOptions.None);

    for (int i = 0; i < csvStringDelimed.Length; i++) {
        string[] dataValues = csvStringDelimed[i].Split(',');
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
                    if(runway.index == 1){
                        selectedAirplane.GetComponent<Animator>().Play("Taxiing");
                    }else{
                        selectedAirplane.GetComponent<Animator>().Play("Taxiing2");
                    }
                    
                }else{
                    selectedAirplane.status = PlaneStatus.Landing;
                    if(runway.index == 1){
                        selectedAirplane.GetComponent<Animator>().Play("Landing2");
                    }else{
                        selectedAirplane.GetComponent<Animator>().Play("Landing");
                    }
                    planeSound2.time = 12.0f;
                    planeSound2.Play();
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
            GameObject g2 = FindInActiveObjectByName("ProjectorLightLeft");
            GameObject g3 = FindInActiveObjectByName("ProjectorLightRight");
        if(Display){
            g.SetActive(true);
            g2.SetActive(true);
            g3.SetActive(true);
        }else{
            g.SetActive(false);
            g2.SetActive(false);
            g3.SetActive(false);
        }
    }

    //mainly to initialize the panel elements so we don't have to find them every time
    
    void Update(){
        if(selectedAirplane != null){
            panelElements[0].GetComponent<TMPro.TextMeshProUGUI>().text = selectedAirplane._flightName;
            int Fuel = (int) selectedAirplane.fuelLevel;
            panelElements[1].GetComponent<TMPro.TextMeshProUGUI>().text = Fuel.ToString() + "%";
            panelElements[2].GetComponent<TMPro.TextMeshProUGUI>().text = selectedAirplane.status.ToString();
            panelElements[3].GetComponent<TMPro.TextMeshProUGUI>().text = selectedAirplane.waitingTimeSeconds.ToString();
            panelElements[4].GetComponent<TMPro.TextMeshProUGUI>().text = "Passengers: " + selectedAirplane.passengersOnBoard + ", PlaneType: " + selectedAirplane.planeType + ", Priority: " + selectedAirplane.priority;
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
