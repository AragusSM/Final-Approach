using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class that represents an airplane
public class Airplane : MonoBehaviour
{
    public string _flightName;  // flight identifier
    public int _flightIndex;
    public PlaneStatus status;  // status of this plane

    public bool departure; //  helps identify departing / arriving plane

    public float timeToAir = 15.0f;

    public float timeToTerminal = 20.0f;

    public Terminal terminal; 

    public Sky sky;    

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
        if (this.status == PlaneStatus.TakingOff) {
            timeToAir -= Time.deltaTime;
        } else if (this.status == PlaneStatus.Returning) {
            timeToTerminal -= Time.deltaTime;
        }
        //remove this plane along with the button associated with it
        if (this.status == PlaneStatus.TakingOff &&  timeToAir <= 0) {
            terminal._planes.Remove(this);
            //turn off the panel
            GameObject g = ATC.FindInActiveObjectByName("FlightDisplay");
            string displayname = ATC.FindInActiveObjectByName("FlightNumberText").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text;
            if(displayname.Equals(this._flightName)){
               g.SetActive(false); 
            }
            //delete the button and airplane
            DepartureButton d = terminal._buttons[_flightIndex];
            terminal._buttons.Remove(d);
            Destroy(d);
            Destroy(this);
            //update the indices of the other buttons and planes
            UpdatePlanes(terminal);

        } else if(this.status == PlaneStatus.Returning && timeToTerminal <= 0) {
            sky._planes.Remove(this);
            //turn off the panel
            GameObject g = ATC.FindInActiveObjectByName("FlightDisplay");
            string displayname = ATC.FindInActiveObjectByName("FlightNumberText").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text;
            if(displayname.Equals(this._flightName)){
               g.SetActive(false); 
            }
            //delete the button and airplane
            ArrivalButton a = sky._buttons[_flightIndex];
            sky._buttons.Remove(a);
            Destroy(a);
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