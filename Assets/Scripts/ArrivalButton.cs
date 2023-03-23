using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrivalButton : MonoBehaviour
{
    
    public Sky sky;   // terminal object where all departing planes originate from
    public ATC atc; // class that handles the atc game logic
    public int buttonNumber;    // the button number of this button
    bool pressed;   // true if this button is selected, false otherwise

    // renders the button text and color, updates every frame
    void Update()   // there is probably a better way to do this than every frame
    {
        if (sky._planes.Count > buttonNumber) {    // only renders text if there is a plane's data to display
            this.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = sky._planes[buttonNumber]._flightName + " " + sky._planes[buttonNumber].status.ToString();
            if (buttonNumber == atc.selectedButton && !atc.isDepartureButton) {
                this.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;   // color when a button is selected
            } else {
                this.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().color = Color.black;   // default text color
            }
            
        } else {
            this.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
    }

}
