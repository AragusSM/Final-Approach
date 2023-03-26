using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class that handles the selection of a departing airplane
public class DepartureButton : MonoBehaviour
{
    public Terminal terminal;   // terminal object where all departing planes originate from
    public ATC atc; // class that handles the atc game logic
    public int buttonNumber;    // the button number of this button
    bool pressed;   // true if this button is selected, false otherwise

    // renders the button text and color, updates every frame
    void Update()   // there is probably a better way to do this than every frame
    {
        Debug.Log(terminal._planes.Count);
        if (terminal._planes.Count > buttonNumber) {    // only renders text if there is a plane's data to display
            this.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = terminal._planes[buttonNumber]._flightIndex + " " + terminal._planes[buttonNumber].status.ToString();
            if (buttonNumber == atc.selectedButton && atc.isDepartureButton) {
                this.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;   // color when a button is selected
            } else {
                this.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().color = Color.black;   // default text color
            }
            
        } else {
            this.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
        RectTransform rt = this.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0.0f, 135.0f - (18f)*buttonNumber); //change the value inside here if you change your buttons
    }

    
}
