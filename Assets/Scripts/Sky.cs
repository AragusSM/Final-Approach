using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// class that represents the sky where all arriving planes originate from (copy of terminal)
// In the future we may combine this into one class and have an arrival or departure variable
public class Sky : MonoBehaviour
{
    public Airplane airplane; // airplane prefab
    public List<Airplane> _planes = new List<Airplane>();   // list of all arriving airplanes
    public ArrivalButton button; //button prefab
    public List<ArrivalButton> _buttons = new List<ArrivalButton>();
    public ATC atc;

    private float nextActionTime = 0.0f;    // current time
    private float period = 15.0f;   // time in seconds between spawning airplanes
    void Update() {
        if (Time.time > nextActionTime) {
            nextActionTime += period;
            createButton();
            createPlane();
        }
    }

    // creates an airplane, initializes it, and adds to the list
    void createPlane() {
        Airplane newPlane = Instantiate(airplane, new Vector3(0, 1, 0), Quaternion.identity);
        List<PlaneData> planeData = atc.allPlaneData;
        int index = Random.Range(0, planeData.Count);
        // if user has selected all planes, clear the list:
        if(atc.chosenPlanes.Count == atc.allPlaneData.Count){
            atc.chosenPlanes.Clear();
        }
        // otherwise, find an index that the user has not chosen and choose that plane
        while(atc.chosenPlanes.Contains(index)){
            index = Random.Range(0, planeData.Count);
        }

        PlaneData chosenPlane = planeData[index];

        newPlane._flightName = chosenPlane.callSign + chosenPlane.iata;    // currently the flight name is just the airplane number in the order it was spawned
        newPlane._flightIndex = _planes.Count;
        newPlane.status = PlaneStatus.Circling;
        newPlane.departure = false;
        newPlane.baseValue = newPlane.basePointMap[chosenPlane.planeType[0]]; 
        if(chosenPlane.priority.Equals("Priority")){
            newPlane.priorityMultiplier = 2;
        }
        else if(chosenPlane.priority.Equals("Emergency")){
            newPlane.priorityMultiplier = 3;
        }
        else{
            newPlane.priorityMultiplier = 1;
        }

        newPlane.fuelLevel = chosenPlane.fuel;
        // logic for assigning temp plane class: 
        newPlane.planeClass = chosenPlane.planeSize; 
        newPlane.waitingTime = 0;
        newPlane.sky = this;
        newPlane.atcRef = this.atc;
        _planes.Add(newPlane);
        Debug.Log("Created new plane: " + newPlane._flightName + ". There are " + _planes.Count + " planes in the list.");
    }

    void createButton(){
        ArrivalButton newbutton = Instantiate(button, new Vector3(0, 0, 0), Quaternion.identity);
        newbutton.transform.parent = GameObject.Find("ArrScreen").transform;
        RectTransform rt = newbutton.GetComponent<RectTransform>();
        newbutton.sky = this;
        newbutton.atc = atc;
        newbutton.buttonNumber = _planes.Count;
        //change the position of the button and add the onclick function
        rt.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        newbutton.transform.Rotate(new Vector3(25.0f, 180.0f, 0.0f));
        newbutton.GetComponent<Button>().onClick.AddListener(delegate {atc.selectArrivingAirplane(newbutton.buttonNumber); });
        _buttons.Add(newbutton);
    }
}

