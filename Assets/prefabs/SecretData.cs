using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
class SecretData
{
    public int noOfFrames;
    public float handlengthdelta;
    public float shoulderdelta;
    public float heightdelta;
    public float angleadjust;
    public int NoOfCoins;
    public float Noofcollisions;
    public float totalcollisions;
    public bool started;
    public GameObject acc;
    public int totalInitial;
    public int InitialCollisions;
    public DateTime start, stop;
    public TimeSpan timeTaken;
    public bool timestarted;
    public int totalFrames;
    public int rate;
    public string gestureType;
    public SecretData()
    {
        noOfFrames = 0;
        handlengthdelta = 0.0f;
        shoulderdelta = 0.0f;
        heightdelta = 0.0f;
        angleadjust = 0.0f;
        NoOfCoins = 0;
        Noofcollisions = 0;
        totalcollisions = 0;
        started = false;
        totalInitial = 0;
        InitialCollisions = 0;
        timestarted = false;
        totalFrames = 0;
        gestureType = " ";
    }
}
