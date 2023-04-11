using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// class that represents the sky where all arriving planes originate from (copy of terminal)
// In the future we may combine this into one class and have an arrival or departure variable
public class Sky : MonoBehaviour
{
    public Airplane[] models = new Airplane[10]; //airplane prefab collection
    public List<Airplane> _planes = new List<Airplane>();   // list of all arriving airplanes
    public ArrivalButton button; //button prefab
    public List<ArrivalButton> _buttons = new List<ArrivalButton>();
    public ATC atc;

    private float nextActionTime = 0.0f;    // current time
    private float period = 15.0f;   // time in seconds between spawning airplanes
    void Update() {
        if (GameManager.state == GameState.AirTrafficControl) {
            if (Time.time > nextActionTime) {
                nextActionTime += period;
                period = Random.Range(20,35);
                createButton();
                createPlane();
            }
        }
    }

    Airplane selectAirplane(string model_name){
        switch (model_name){
            case "frachtmaschine_757_static":
                return models[0];
            case "frachtmaschine_antonov_static":
                return models[1];
            case "passagiermaschine_707_static":
                return models[2];
            case "passagiermaschine_747_static":
                return models[3];
            case "passagiermaschine_a320_static":
                return models[4];
            case "passagiermaschine_bae146_static":
                return models[5];
            case "privatjet1_static":
                return models[6];
            case "privatjet2_static":
                return models[7];
            case "sportflugzeug_beech_static":
                return models[8];
            case "sportflugzeug_cessna_static":
                return models[9];
            default:
                return models[0];
        }
    }

    // creates an airplane, initializes it, and adds to the list
    void createPlane() {
        // if user has chosen all planes, don't add any more planes => game ends:
        if(atc.chosenPlanes.Count == atc.allPlaneData.Count){
            return;
        }
        
        List<PlaneData> planeData = atc.allPlaneData;
        int index = Random.Range(0, planeData.Count);
        // find an index that the user has not chosen and choose that plane
        while(atc.chosenPlanes.Contains(index)){
            index = Random.Range(0, planeData.Count);
        }

        PlaneData chosenPlane = planeData[index];
        Airplane newPlane = Instantiate(selectAirplane(chosenPlane.planeAsset), new Vector3(0, 1, 0), Quaternion.identity);
        atc.chosenPlanes.Add(index);

        if(chosenPlane.iata.Equals("N/A")) {
            newPlane._flightName = chosenPlane.callSign + chosenPlane.adIATA; 
        }
        else {
            newPlane._flightName = chosenPlane.callSign + chosenPlane.iata; 
        }
          
        newPlane._flightIndex = _planes.Count;
        newPlane.status = PlaneStatus.Circling;
        newPlane.departure = false;
        newPlane.baseValue = newPlane.basePointMap[chosenPlane.planeSize];
        
        if(chosenPlane.priority.Equals("Priority")){
            newPlane.priorityMultiplier = 2;
            newPlane.priority = "Higher Priority";
        }
        else if(chosenPlane.priority.Equals("Emergency")){
            newPlane.priorityMultiplier = 3;
            newPlane.priority = "Emergency";
        }
        else{
            newPlane.priorityMultiplier = 1;
            newPlane.priority = "Normal";
        }

        newPlane.fuelLevel = chosenPlane.fuel;
        newPlane.planeClass = chosenPlane.planeSize;
        newPlane.planeType = chosenPlane.planeType;
        newPlane.passengersOnBoard = chosenPlane.maxPassengers;
        newPlane.planeAsset = chosenPlane.planeAsset; 
        newPlane.waitingTime = 0;
        
        // sky specific property assignment:
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

