using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class that represents an airplane
public class Airplane : MonoBehaviour
{
    public string _flightName;  // flight identifier
    public PlaneStatus status;  // status of this plane
    public bool departure; //  helps identify departing / arriving plane
    public int baseValue; // base value based on size
    public int priorityMultiplier; // priority 1 (normal) to 3 (highest)
    public float fuelLevel; // 100 for full, 0 for empty
    public int waitingTime; // the time in secs that this plane has waited
    public char planeClass; // char that represents size / class of plane

    public Dictionary<char, float> fuelMap = new Dictionary<>{ // maps plane class to fuel decrement amt / second 
            { "a", 0.05 },
            { "b", 0.07 },
            { "c", 0.11},
            { "d", 0.14 },
            { "e", 0.17 },
            { "f", 0.20 },
        };
     
    // temp values
    public float timeToAir = 15.0f;
    public float timeToTerminal = 20.0f;

    // use terminal if departing and sky if arriving
    public Terminal terminal; 
    public Sky sky;
    const int LEEWAY = 5; 

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
        waitingTime += (int) Time.deltaTime;
        // update fuel value if in air and depending on fuel class
        if (this.status == PlaneStatus.Circling || this.status == PlaneStatus.Landing) {
            fuelLevel -= Time.deltaTime * fuelMap[this.planeClass];
        }
        if (this.status == PlaneStatus.TakingOff) {
            timeToAir -= Time.deltaTime;
        } else if (this.status == PlaneStatus.Returning) {
            timeToTerminal -= Time.deltaTime;
        }
        if (this.status == PlaneStatus.TakingOff &&  timeToAir <= 0) {
            terminal._planes.Remove(this);
            GameManager.score += baseValue - ((waitingTime - LEEWAY) * priorityMultiplier);
            Destroy(this);
        } else if(this.status == PlaneStatus.Returning && timeToTerminal <= 0) {
            sky._planes.Remove(this);
            GameManager.score += baseValue - ((waitingTime - LEEWAY) * priorityMultiplier);
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

