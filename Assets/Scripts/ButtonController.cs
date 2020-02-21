using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;

public class ButtonController : MonoBehaviour
{

    public Vector2d location;
    public int id;
    public List<Vector2d> restLocations;
    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClicked()
    {
        Debug.Log("Lol");
        Debug.Log(location);
        PlayerPrefs.SetFloat("rider_x", Convert.ToSingle(location.x));
        PlayerPrefs.SetFloat("rider_y", Convert.ToSingle(location.y));
        PlayerPrefs.SetInt("rest_number", restLocations.Count);
        int i = 1;
        foreach (var restLocation in restLocations)
        {
            PlayerPrefs.SetFloat("restaurant" + Convert.ToString(i) + "x", Convert.ToSingle(restLocation.x));
            PlayerPrefs.SetFloat("restaurant" + Convert.ToString(i) + "y", Convert.ToSingle(restLocation.y));
            i = i + 1;
        }
        Initiate.Fade("Zoom", Color.black, 2.5f);
    }

    public void RestarauntClicked()
    {
        if (panel.activeSelf == false) {
            panel.SetActive(true);
        } else {
            panel.SetActive(false);
        }
    }
}
