using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Exit_button : MonoBehaviour
{
    TimeController Time;

    Text[] Food_Header;
    Canvas Stock;
    Text[] produce;
    Text[] dry;
    Text[] frozen;
    Text[] dairy;
    Text registerDebug;

    public void Exit_sim()
    {
        //reset the static day
        TimeController.Day = 1;
        //reset the static Cash
        Simulation.Cash = 10000;

        //clear all upcoming deliveries
        TimeController.deliveries.Clear();
        Object.Destroy(GameObject.Find("Simulation"));
        SceneManager.LoadScene("Main Menu");
    }

    public void Hide_Debug()
    {
        Food_Header = GameObject.Find("Foods").GetComponentsInChildren<Text>();
        Stock = GameObject.Find("stock_Pie").GetComponent<Canvas>();
        
        produce = GameObject.FindGameObjectWithTag("produceText").GetComponentsInChildren<Text>();
        dry = GameObject.FindGameObjectWithTag("DryGoodsText").GetComponentsInChildren<Text>();
        frozen = GameObject.FindGameObjectWithTag("FrozenText").GetComponentsInChildren<Text>();
        dairy = GameObject.FindGameObjectWithTag("DairyText").GetComponentsInChildren<Text>();
        registerDebug = GameObject.FindGameObjectWithTag("RegisterDebug").GetComponentInChildren<Text>();



        for (int i = 0; i < Food_Header.Length; i++)
        {
            Food_Header[i].enabled = !Food_Header[i].enabled;
        }

        for(int i = 0; i < 2; i++) 
        {
            produce[i].enabled = !produce[i].enabled;
            dry[i].enabled = !dry[i].enabled;
            frozen[i].enabled = !frozen[i].enabled;
            dairy[i].enabled = !dairy[i].enabled;
        }
        Stock.enabled = !Stock.enabled;

        registerDebug.enabled = !registerDebug.enabled;

    }

    public void skip_day() 
    {
        Time = GameObject.Find("UI Canvas").GetComponent<TimeController>();

        Time.Hour = 19;
        Time.Minute = 59;

    }

    public void Menu() 
    {
        if (transform.GetChild(1).gameObject.activeInHierarchy)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
        }

        else
        {
            StartCoroutine("Grow");
            
        }
    }

    IEnumerator Grow() 
    {
        for (int i = 1; i <= 3; i++)
        {
            transform.GetChild(i).localScale = new Vector3(0, 0, 0);
            transform.GetChild(i).gameObject.SetActive(true);

            float x = 0, y = 0, z = 0;

            for (int j = 0; j < 5; j++)
            {
                x += .2f;
                y += .2f;
                z += .2f;

                transform.GetChild(i).localScale = new Vector3(x,y,z);
                yield return new WaitForSeconds(.5f);
            }
        }
    }
}
