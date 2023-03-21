using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class that represents the sky where all arriving planes originate from (copy of terminal)
// In the future we may combine this into one class and have an arrival or departure variable
public class Sky : MonoBehaviour
{
    public Airplane airplane; // airplane prefab
    public List<Airplane> _planes = new List<Airplane>();   // list of all arriving airplanes

    private float nextActionTime = 0.0f;    // current time
    private float period = 15.0f;   // time in seconds between spawning airplanes
    void Update() {
        if (Time.time > nextActionTime) {
            nextActionTime += period;
            createPlane();
        }
    }

    // creates an airplane, initializes it, and adds to the list
    void createPlane() {
        Airplane newPlane = Instantiate(airplane, new Vector3(0, 1, 0), Quaternion.identity);
        newPlane._flightName = _planes.Count.ToString();    // currently the flight name is just the airplane number in the order it was spawned
        newPlane.status = PlaneStatus.Circling;
        newPlane.departure = false;
        newPlane.sky = this;
        _planes.Add(newPlane);
        Debug.Log("Created new plane: " + newPlane._flightName + ". There are " + _planes.Count + " planes in the list.");
    }
}
