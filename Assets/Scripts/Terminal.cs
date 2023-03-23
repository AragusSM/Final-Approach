using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// class that represents the terminal where all departing planes originate from
public class Terminal : MonoBehaviour
{
    public Airplane airplane; // airplane prefab
    public DepartureButton button; //button prefab
    public List<Airplane> _planes = new List<Airplane>();   // list of all departing airplanes
    public List<DepartureButton> _buttons = new List<DepartureButton>();

    public ATC atc;

    private float nextActionTime = 0.0f;    // current time
    private float period = 15.0f;   // time in seconds between spawning airplanes
    void Update() {
        if (Time.time > nextActionTime) {
            nextActionTime += period;
            createButton();
            createPlane();
        }
    }

    // creates an airplane, initializes it, and adds to the list
    void createPlane() {
        Airplane newPlane = Instantiate(airplane, new Vector3(0, 1, 0), Quaternion.identity);
        newPlane._flightName = _planes.Count.ToString();    // currently the flight name is just the airplane number in the order it was spawned
        newPlane.status = PlaneStatus.Terminal;
        newPlane.departure = true;
        newPlane.terminal = this;
        _planes.Add(newPlane);
        Debug.Log("Created new plane: " + newPlane._flightName + ". There are " + _planes.Count + " planes in the list.");
    }

    //creates a new airplane button that can be selected from the ui
    void createButton(){
        DepartureButton newbutton = Instantiate(button, new Vector3(0, 0, 0), Quaternion.identity);
        newbutton.transform.parent = GameObject.Find("DepScreen").transform;
        RectTransform rt = newbutton.GetComponent<RectTransform>();
        newbutton.terminal = this;
        newbutton.atc = atc;
        newbutton.buttonNumber = _planes.Count;
        //change the position of the button and add the onclick function
        rt.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        rt.anchoredPosition = new Vector2(0.0f, 135.0f - (18f)*_buttons.Count); //change the value inside here if you change your buttons
        newbutton.transform.Rotate(new Vector3(25.0f, 180.0f, 0.0f));
        newbutton.GetComponent<Button>().onClick.AddListener(delegate {atc.selectDepartingAirplane(newbutton.buttonNumber); });
        _buttons.Add(newbutton);
    }
}
