using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// class that represents an airplane
public class Airplane : MonoBehaviour
{

    public const int MED_WAIT_TIME = 30; // med amt of waiting time as a const
    public const int XTR_WAIT_TIME = 60; // large amt of waiting time as a const
    public const float DEPART_MULT_MED = 1.25f; // multiplier for terminal planes waiting more than 30 seconds
    public const float DEPART_MULT_XTR = 1.5f; // multiplier for terminal planes waiting more than 60 seconds
    public string _flightName;  // flight identifier
    public string ident;
    public int _flightIndex;
    public PlaneStatus status;  // status of this plane

    public bool departure; //  helps identify departing / arriving plane
    public int baseValue; // base value based on size
    public int priorityMultiplier; // priority 1 (normal) to 3 (highest)
    public float fuelLevel; // 100 for full, 0 for empty
    public float waitingTime;
    public int waitingTimeSeconds; // the time in secs that this plane has waited
    public char planeClass; // char that represents size / class of plane
    public string planeType; // string that represents the type of plane (i.e Boeing 747)
    public string planeAsset; // string that represents the asset name of the plane
    public int passengersOnBoard; // number of passengers on board
    public string priority; // priority of the plane as a string

    public ATC atcRef; // reference to the ATC

    public Dictionary<char, float> fuelMap = new Dictionary<char, float>{ // maps plane class to fuel decrement amt / second 
            { 'A', 0.05f },
            { 'B', 0.07f },
            { 'C', 0.11f},
            { 'D', 0.14f },
            { 'E', 0.17f },
            { 'F', 0.20f },
    };
    
    public Dictionary<char, int> basePointMap = new Dictionary<char, int>{ // maps plane class to base point value 
            { 'A', 100 },
            { 'B', 150 },
            { 'C', 200},
            { 'D', 250 },
            { 'E', 300 },
            { 'F', 350 },
    };


    // temp values
    public float timeToAir = 15.0f;
    public float timeToTerminal = 10.0f;

    public Terminal terminal; 
    public Sky sky;    
    const int LEEWAY = 30;

    // update final status depending on plane type
    public void finalStatus() {
        if (departure) {
            this.status = PlaneStatus.TakingOff;
        }
        else {
            this.status = PlaneStatus.Returning;
        }
    }
    
    // if we are in the last leg of depart of arrive
    // update plane status accordingly before destruction
    void Update() {
        waitingTime += Time.deltaTime;
        waitingTimeSeconds = (int) waitingTime;
        // update fuel value if in air and depending on fuel class. Also note that the airplane consumes fuel if it is moving! (not at terminal)
        if (this.status != PlaneStatus.Terminal) {
            fuelLevel -= Time.deltaTime * fuelMap[this.planeClass];
            // if the fuel level is 0 for any plane, the player automatically loses the game and is directed to the lose game screen
            if (fuelLevel <= 0) {
                InfoHolder.Points = GameManager.score;
                InfoHolder.FailureReason = "Ran out of fuel!";
                SceneManager.LoadScene(2);
            }
        }
        if (this.status == PlaneStatus.TakingOff) {
            timeToAir -= Time.deltaTime;
        } else if (this.status == PlaneStatus.Returning) {
            timeToTerminal -= Time.deltaTime;
        }
        //remove this plane along with the button associated with it
        if (this.status == PlaneStatus.TakingOff &&  timeToAir <= 0) {
            terminal._planes.Remove(this);
            if(waitingTimeSeconds <= MED_WAIT_TIME) {
                GameManager.score += Mathf.Max(baseValue - (Mathf.Max((waitingTimeSeconds - LEEWAY), 0) * priorityMultiplier), 0);
            }
            else if(waitingTime <= XTR_WAIT_TIME) {
                GameManager.score += Mathf.Max(baseValue - (Mathf.Max(((int)(waitingTimeSeconds * DEPART_MULT_MED) - LEEWAY), 0) * priorityMultiplier), 0);
            }
            else {
                GameManager.score += Mathf.Max(baseValue - (Mathf.Max(((int)(waitingTimeSeconds * DEPART_MULT_XTR) - LEEWAY), 0) * priorityMultiplier), 0);
            }
            //turn off the panel
            GameObject g = ATC.FindInActiveObjectByName("FlightDisplay");
            GameObject g2 = ATC.FindInActiveObjectByName("ProjectorLightLeft");
            GameObject g3 = ATC.FindInActiveObjectByName("ProjectorLightRight");

            string displayname = ATC.FindInActiveObjectByName("FlightNumberText").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text;
            if(displayname.Equals(this._flightName)){
               g.SetActive(false); 
               g2.SetActive(false); 
               g3.SetActive(false); 
            }
            //delete the button and airplane
            DepartureButton d = terminal._buttons[_flightIndex];
            terminal._buttons.Remove(d);
            this.atcRef.selectedButton = -1;
            Destroy(d);
            Destroy(this.gameObject);
            Destroy(this);
            //update the indices of the other buttons and planes
            UpdatePlanes(terminal);

        } else if(this.status == PlaneStatus.Returning && timeToTerminal <= 0) {
            sky._planes.Remove(this);
            GameManager.score += Mathf.Max(baseValue - ((waitingTimeSeconds - LEEWAY) * priorityMultiplier), 0);
            //turn off the panel
            GameObject g = ATC.FindInActiveObjectByName("FlightDisplay");
            GameObject g2 = ATC.FindInActiveObjectByName("ProjectorLightLeft");
            GameObject g3 = ATC.FindInActiveObjectByName("ProjectorLightRight");

            string displayname = ATC.FindInActiveObjectByName("FlightNumberText").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text;
            if(displayname.Equals(this._flightName)){
               g.SetActive(false); 
               g2.SetActive(false); 
               g3.SetActive(false); 
            }
            //delete the button and airplane
            ArrivalButton a = sky._buttons[_flightIndex];
            sky._buttons.Remove(a);
            this.atcRef.selectedButton = -1;
            Destroy(a);
            Destroy(this.gameObject);
            Destroy(this);
            //update the indices of the other buttons and planes
            UpdatePlanes(sky);
        }
    }

    void UpdatePlanes(Terminal t){
        for(int i = 0; i < t._planes.Count; i++){
            t._planes[i]._flightIndex = i;
        }
        for(int i = 0; i < t._buttons.Count; i++){
            t._buttons[i].buttonNumber = i;
        }
    }

    void UpdatePlanes(Sky s){
        for(int i = 0; i < s._planes.Count; i++){
            s._planes[i]._flightIndex = i;
        }
        for(int i = 0; i < s._buttons.Count; i++){
            s._buttons[i].buttonNumber = i;
        }
    }
}

// enum of airplane statuses
public enum PlaneStatus {
    Terminal, //at the terminal, generated from terminal
    Taxiing, //going from the terminal to the runway
    Runway, //on the runway
    TakingOff, //leaving the runway into the air
    Circling, //in the air ready to land
    Landing, // landing from air to runway
    Returning // going back to terminal from runway after landing
}