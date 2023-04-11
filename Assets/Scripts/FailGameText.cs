using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FailGameText : MonoBehaviour
{

    public TMP_Text points; 
    public TMP_Text failureReason;


    // Start is called before the first frame update
    void Start()
    {
        points.text =  InfoHolder.Points.ToString();
        failureReason.text = InfoHolder.FailureReason;
    }

    // Update is called once per frame
    void Update()
    {
        points.text =  InfoHolder.Points.ToString();
        failureReason.text = InfoHolder.FailureReason;
        //Debug.Log("Hello world: " + points.text);
        
    }
}
