using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunwayButton : MonoBehaviour
{
    public Runway runWay;
    public bool active;
    
    public AudioSource planeSound;

    public void runwayButtonPress() {
        if (active) {
            runWay.plane.finalStatus();
            if(runWay.plane.departure){
                if(runWay.index == 1){
                    runWay.plane.GetComponent<Animator>().Play("TakeOff");
                }else{
                    runWay.plane.GetComponent<Animator>().Play("TakeOff2");
                }
                planeSound.Play();
            }else{
                if(runWay.index == 1){
                    runWay.plane.GetComponent<Animator>().Play("Returning2");
                }else{
                    runWay.plane.GetComponent<Animator>().Play("Returning");
                }
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
