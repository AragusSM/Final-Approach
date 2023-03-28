using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrivalButton : MonoBehaviour
{
    
    public Sky sky;   // terminal object where all departing planes originate from
    public ATC atc; // class that handles the atc game logic
    public int buttonNumber;    // the button number of this button
    bool pressed;   // true if this button is selected, false otherwise



    // renders the button text and color, updates every frame
    void Update()   // there is probably a better way to do this than every frame
    {
        if(sky != null && atc != null){
            if (sky._planes.Count > buttonNumber) {    // only renders text if there is a plane's data to display
                this.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = sky._planes[buttonNumber]._flightName.Substring(sky._planes[buttonNumber]._flightName.Length - 4) 
                 + " " + sky._planes[buttonNumber].status.ToString();
            if (buttonNumber == atc.selectedButton && !atc.isDepartureButton) {
                this.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;   // color when a button is selected
            } else {
                this.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().color = Color.black;   // default text color
            }
                
                //transform the battery bar just like in the departure button
                float size = (sky._planes[buttonNumber].fuelLevel/100.0f) * 20.0f;
                this.transform.GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
                Vector2 movement = new Vector2(10-(20 - size)/2,0);
                this.transform.GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().anchoredPosition = movement;
                
                //change color of the component
                if(sky._planes[buttonNumber].fuelLevel < 20){
                    this.transform.GetChild(1).GetChild(1).GetComponent<RawImage>().color = Color.red;
                }else if(sky._planes[buttonNumber].fuelLevel < 40){
                    Color orange = new Color(1.0f, 0.64f, 0.0f);
                    this.transform.GetChild(1).GetChild(1).GetComponent<RawImage>().color = orange;
                }else if(sky._planes[buttonNumber].fuelLevel < 75){
                    this.transform.GetChild(1).GetChild(1).GetComponent<RawImage>().color = Color.yellow;
                }else{
                    this.transform.GetChild(1).GetChild(1).GetComponent<RawImage>().color = Color.green;
                }
            } else {
                this.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            }
            RectTransform rt = this.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0.0f, 135.0f - (18f)*buttonNumber); //change the value inside here if you change your buttons
        }
        
    }

}
