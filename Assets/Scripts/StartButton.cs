using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void StartGame() {
        GameManager.UpdateGameState(GameState.AirTrafficControl);
    }

    public void ShowInfo(){
        GameObject g = ATC.FindInActiveObjectByName("InfoPanel");
        GameObject g2 = ATC.FindInActiveObjectByName("InfoProj1");
        GameObject g3 = ATC.FindInActiveObjectByName("InfoProj2");
        GameObject g4 = ATC.FindInActiveObjectByName("InfoProj3");
        GameObject g5 = ATC.FindInActiveObjectByName("InfoProj4");
        if(g.activeSelf){
            g.SetActive(false);
            g2.transform.GetChild(0).gameObject.SetActive(false);
            g3.transform.GetChild(0).gameObject.SetActive(false);
            g4.transform.GetChild(0).gameObject.SetActive(false);
            g5.transform.GetChild(0).gameObject.SetActive(false);
        }else{
            g.SetActive(true);
            g2.transform.GetChild(0).gameObject.SetActive(true);
            g3.transform.GetChild(0).gameObject.SetActive(true);
            g4.transform.GetChild(0).gameObject.SetActive(true);
            g5.transform.GetChild(0).gameObject.SetActive(true);
        }
        
        GameObject gInst = ATC.FindInActiveObjectByName("InstructionsText");
        if(gInst.activeSelf){
            gInst.SetActive(false);
        }
    }

    public void ShowInfo2(){
        GameObject g = ATC.FindInActiveObjectByName("InstructionsText");
        GameObject g2 = ATC.FindInActiveObjectByName("InfoProj1");
        GameObject g3 = ATC.FindInActiveObjectByName("InfoProj2");
        GameObject g4 = ATC.FindInActiveObjectByName("InfoProj3");
        GameObject g5 = ATC.FindInActiveObjectByName("InfoProj4");
        if(g.activeSelf){
            g.SetActive(false);
            g2.transform.GetChild(0).gameObject.SetActive(false);
            g3.transform.GetChild(0).gameObject.SetActive(false);
            g4.transform.GetChild(0).gameObject.SetActive(false);
            g5.transform.GetChild(0).gameObject.SetActive(false);
        }else{
            g.SetActive(true);
            g2.transform.GetChild(0).gameObject.SetActive(true);
            g3.transform.GetChild(0).gameObject.SetActive(true);
            g4.transform.GetChild(0).gameObject.SetActive(true);
            g5.transform.GetChild(0).gameObject.SetActive(true);
        }
        
        GameObject gPanel = ATC.FindInActiveObjectByName("InfoPanel");
        if(gPanel.activeSelf){
            gPanel.SetActive(false);
        }
    }

    public void ShowInfo3(){
        GameObject g = ATC.FindInActiveObjectByName("InstructionsText");
        if(g.activeSelf){
            g.SetActive(false);
        }else{
            g.SetActive(true);
        }
        
    }
}
