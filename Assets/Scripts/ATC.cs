using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
