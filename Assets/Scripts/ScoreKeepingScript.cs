using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeepingScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (GameManager.state == GameState.AirTrafficControl) {
            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + GameManager.score;
        }
    }
}
