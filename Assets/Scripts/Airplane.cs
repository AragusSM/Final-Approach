using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class that represents an airplane
public class Airplane : MonoBehaviour
{
    public string _flightName;  // flight identifier
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
        if (this.status == PlaneStatus.TakingOff &&  timeToAir <= 0) {
            terminal._planes.Remove(this);
            Destroy(this);
        } else if(this.status == PlaneStatus.Returning && timeToTerminal <= 0) {
            sky._planes.Remove(this);
            Destroy(this);
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