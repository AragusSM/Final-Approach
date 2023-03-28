using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunwayButton : MonoBehaviour
{
    public Runway runWay;
    public bool active;
    
    public AudioSource planeSound;
    public AudioSource planeSound2;

    public void runwayButtonPress() {
        if (active) {
            runWay.plane.finalStatus();
            if(runWay.plane.departure){
                runWay.plane.GetComponent<Animator>().Play("TakeOff");
                planeSound.Play();
            }else{
                runWay.plane.GetComponent<Animator>().Play("Returning");
                planeSound2.Play();
            }
            
        }
    }

    void Update() {
        active = runWay.plane && runWay.plane.status == PlaneStatus.Runway;
        if(runWay.plane && runWay.plane.status == PlaneStatus.Runway){
            if(runWay.plane.departure){
                this.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Clear For TakeOff";
            }else{
                this.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Clear to Terminal";

            }
        }else{
            this.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";

        }
    }
}
