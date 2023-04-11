using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monitors : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "10:00";
    }

    // Update is called once per frame
    void Update()
    {
        
        if(GameManager.state == GameState.AirTrafficControl && GameManager.time_left <= 0)
        {
            InfoHolder.Points = GameManager.score;
            InfoHolder.FailureReason = "Time's up! Well Done!";
            SceneManager.LoadScene(2);
        }else if(GameManager.state == GameState.AirTrafficControl){
            GameManager.time_left -= Time.deltaTime;
        }
    
        string minutes = (((int) GameManager.time_left) / 60).ToString();
        string seconds = (((int) GameManager.time_left) % 60).ToString();
        this.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = minutes + ":" + seconds;
    }
}
