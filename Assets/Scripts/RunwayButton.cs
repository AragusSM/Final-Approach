using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunwayButton : MonoBehaviour
{
    public Runway runWay;
    public bool active;
    

    public void runwayButtonPress() {
        if (active) {
            runWay.plane.finalStatus();
        }
    }

    void Update() {
        active = runWay.plane && runWay.plane.status == PlaneStatus.Runway;
    }
}
