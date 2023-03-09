using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class that represents an airplane
public class Airplane : MonoBehaviour
{
    public string _flightName;  // flight identifier
    public PlaneStatus status;  // status of this plane
    
    // airplane initialization
    /*void Start() {
        status = PlaneStatus.Terminal;
    }*/
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