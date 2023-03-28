using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class that handles the selection of a runway
public class Runway : MonoBehaviour
{
    public bool open; // true if this runway can be taxiied to (there is not another plane taxiing here or already here)
    public float timeToRunway = 10.0f;  // time it takes for an airplane to get here
    public float timeToLand = 15.0f; //time it takes to land

    public Airplane plane;  // the airplane that is arriving here or is already here
    
    void Update() {
        if (plane && (plane.status == PlaneStatus.TakingOff || plane.status == PlaneStatus.Returning) ) {
            timeToRunway = 10.0f;
            timeToLand = 15.0f;
            open = true;
        }
        if (!open && plane){
            if(timeToRunway <= 0.0f || timeToLand <= 0.0f){   // airplane has arrived at runway
                plane.status = PlaneStatus.Runway;
                //Debug.Log("Plane " + plane._flightName + " is at the Runway!");
            }else if (plane.status == PlaneStatus.Taxiing){
                timeToRunway -= Time.deltaTime; // counting down
            }else if (plane.status == PlaneStatus.Landing){
                timeToLand -= Time.deltaTime; //count down
            }
        }
    }    
}
