using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// class that represents the terminal where all departing planes originate from
public class Terminal : MonoBehaviour
{
    public Airplane[] models = new Airplane[10]; //airplane prefab collection
    public DepartureButton button; //button prefab
    public List<Airplane> _planes = new List<Airplane>();   // list of all departing airplanes
    public List<DepartureButton> _buttons = new List<DepartureButton>();

    public ATC atc;

    private float nextActionTime = 0.0f;    // current time
    private float period = 15.0f;   // time in seconds between spawning airplanes
    void Update() {
        if (GameManager.state == GameState.AirTrafficControl) { // make sure we're in gameplay state
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
        
        newPlane._flightName = chosenPlane.callSign; 
        newPlane.ident = chosenPlane.ident;
        
        newPlane._flightIndex = _planes.Count;
        newPlane.status = PlaneStatus.Terminal;
        newPlane.departure = true;
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

        //airplanes taking off should have a greater fuel level. Add 50%.
        newPlane.fuelLevel = Mathf.Min((chosenPlane.fuel + 50.0f), 100.0f);
        newPlane.planeClass = chosenPlane.planeSize;
        newPlane.planeType = chosenPlane.planeType;
        newPlane.passengersOnBoard = chosenPlane.maxPassengers;
        newPlane.planeAsset = chosenPlane.planeAsset; 
        newPlane.waitingTime = 0;
        newPlane.terminal = this;
        newPlane.atcRef = this.atc;
        _planes.Add(newPlane);
        //Debug.Log("Created new plane: " + newPlane._flightName + ". There are " + _planes.Count + " planes in the list.");
    }

    //creates a new airplane button that can be selected from the ui
    void createButton(){
        DepartureButton newbutton = Instantiate(button, new Vector3(0, 0, 0), Quaternion.identity);
        newbutton.transform.parent = GameObject.Find("DepScreen").transform;
        RectTransform rt = newbutton.GetComponent<RectTransform>();
        newbutton.terminal = this;
        newbutton.atc = atc;
        newbutton.buttonNumber = _planes.Count;
        //change the position of the button and add the onclick function
        rt.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        newbutton.transform.Rotate(new Vector3(25.0f, 180.0f, 0.0f));
        newbutton.GetComponent<Button>().onClick.AddListener(delegate {atc.selectDepartingAirplane(newbutton.buttonNumber); });
        _buttons.Add(newbutton);
    }
}
