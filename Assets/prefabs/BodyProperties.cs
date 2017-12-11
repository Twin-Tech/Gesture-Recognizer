using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;

public class BodyProperties : MonoBehaviour {

    public int noOfFrames = 0;
    public Boolean startGame = false;
    public Boolean InitializeObj = false;
    public int countinst = 0;
    int approximation = 0;
    public float leftarmLength = 0.0f;
    public float rightarmLength = 0.0f;
    int NoOfInitializationFrame = 100;
    public float handlengthdelta = 0.0f;
    public float shoulderdelta = 0.0f;
    public float heightdelta = 0.0f;
    public float angleadjust = 0.0f;
    public int NoOfCoins = 0;
    public bool createdagain = false;
    public float rightFootHeight = Single.PositiveInfinity;
    public float leftFootHeight = Single.PositiveInfinity;
    double[] AnglesOfHand = { 0.0, 30.0, 60.0, 90.0, 120.0, 150.0, 180.0 };
    public float Noofcollisions = 0;
    public float totalcollisions = 0;
    public string lastcolider = "";
    public bool started = false;
    //public Text acc;
    public GameObject acc;
    public float accuracy;
    public int totalInitial = 0;
    public int InitialCollisions = 0;
    DateTime start, stop;
    TimeSpan timeTaken;
    bool timestarted = false;
    int totalFrames = 0;
    int rate;
    public bool gestureTrue;
    // Use this for initialization
    void Start() {
       
        totalFrames = 55;
        gestureTrue = false;
    }

    // Update is called once per frame
    void Update() {
        if (startGame)
        {
            if (!InitializeObj)
            {
                float leftArm = FindHandLength("left");
                float rightArm = FindHandLength("right");
                if (leftArm != -1.0f && rightArm != -1.0f)
                {
                    FindLegHeightFromGround("left");
                    FindLegHeightFromGround("right");
                    leftarmLength += FindHandLength("left");
                    rightarmLength += FindHandLength("right");
                    approximation++;
                    if (approximation > NoOfInitializationFrame)
                    {
                        countinst++;
                        InitializeObj = true;
                        leftarmLength /= NoOfInitializationFrame;
                        rightarmLength /= NoOfInitializationFrame;
                        CreateCoins();
                        rate =Convert.ToInt32((0.75 * totalFrames) / totalInitial);
                    }
                }
            }
            else {
                 if(NoOfCoins == 0 && createdagain==false)
                {
                   // createAgain();
                }
            }
            if(InitialCollisions==totalInitial && timestarted==false)
            {
                timestarted = true;
                start = DateTime.Now;
            }
            noOfFrames++;
            checkGesture();
        }
    }
  
    public void checkGesture()
    {
        if (noOfFrames <= (rate * Noofcollisions))
            gestureTrue = true;
        else
            gestureTrue = false;
        GameObject statusBox = GameObject.Find("Text 1");
        Text status = statusBox.GetComponent<Text>();
        status.text = gestureTrue.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("StartCircle")) {
            startGame = true;
            other.enabled = false;
        }
    }

    public double CalculateAngle(GameObject start, GameObject mid, GameObject end)
    {
        Vector3 a = start.transform.position - mid.transform.position;
        Vector3 b = end.transform.position - mid.transform.position;
        return Vector3.Angle(a, b);
    }

    public float CalculateDistance(GameObject g1, GameObject g2)
    {
        return Vector3.Distance(g2.transform.position, g1.transform.position);
    }

    public float CalculateDistance(Windows.Kinect.Joint g1, Windows.Kinect.Joint g2)
    {
        return Vector3.Distance(GetVector3FromJoint(g1), GetVector3FromJoint(g2));
    }

    public void ResetColliders()
    {
        if(InitialCollisions>=(2*totalInitial))
        {
            CalculateAccuracy();
            Noofcollisions = 0;
            InitialCollisions = 0;
            stop = DateTime.Now;
            timeTaken = stop.Subtract(start);
            start = DateTime.Now;
            //Debug.Log(timeTaken.TotalSeconds.ToString());
            //Debug.Log(noOfFrames.ToString());
            noOfFrames = 0;
        }
    }
    public double CalculateDistanceByFormula(Windows.Kinect.Joint g1, Windows.Kinect.Joint g2)
    {
        Vector3 s = GetVector3FromJoint(g1);
        Vector3 t = GetVector3FromJoint(g2);

        return Math.Sqrt(Math.Pow(s.x - t.x, 2) + Math.Pow(s.y - t.y, 2) + Math.Pow(s.z - t.z, 2));

    }
    private static Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z);
    }

    public void CalculateAccuracy()
    {
       
            acc = GameObject.Find("Text 2");
            Text abc = acc.GetComponent<Text>();
            accuracy = (Noofcollisions/totalcollisions) *100;
        abc.text = accuracy.ToString();
            Debug.Log(accuracy.ToString());
    }
    public float FindHandLength(string hand)
    {
        switch (hand)
        {
            case "left":
            case "Left":
            case "LEFT":
                GameObject leftShoulder = transform.Find("ShoulderLeft").gameObject;
                GameObject leftElbow = transform.Find("ElbowLeft").gameObject;
                GameObject leftWrist = transform.Find("WristLeft").gameObject;
                return Vector3.Distance(leftShoulder.transform.position, leftElbow.transform.position) + Vector3.Distance(leftElbow.transform.position, leftWrist.transform.position);
                break;
            case "right":
            case "Right":
            case "RIGHT":
                GameObject rightShoulder = transform.Find("ShoulderRight").gameObject;
                GameObject rightElbow = transform.Find("ElbowRight").gameObject;
                GameObject rightWrist = transform.Find("WristRight").gameObject;
                return Vector3.Distance(rightShoulder.transform.position, rightElbow.transform.position) + Vector3.Distance(rightElbow.transform.position, rightWrist.transform.position);
                break;
            default:
                return -1.0f;
        }
        //return 0.0f;
    }

    public void FindLegHeightFromGround(string leg)
    {
        switch (leg)
        {
            case "left":
            case "Left":
            case "LEFT":
                GameObject leftFoot = transform.Find("FootLeft").gameObject;
                JointsProperties LFJP = leftFoot.GetComponent<JointsProperties>();
                leftFootHeight = leftFootHeight > (float)(LFJP.distanceFromGround) ? (float)LFJP.distanceFromGround : leftFootHeight;
                break;
            case "right":
            case "Right":
            case "RIGHT":
                GameObject rightFoot = transform.Find("FootRight").gameObject;
                JointsProperties RFJP = rightFoot.GetComponent<JointsProperties>();
                rightFootHeight = rightFootHeight > (float)(RFJP.distanceFromGround) ? (float)RFJP.distanceFromGround : rightFootHeight;
                break;
        }
    }

    /*  public void CreateCoins()
      {
          GameObject leftShoulder = transform.Find("ShoulderLeft").gameObject;
          GameObject rightShoulder = transform.Find("ShoulderRight").gameObject;

          foreach (double angle in AnglesOfHand)
          {
              GameObject lcoin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
              lcoin.transform.position = PlaceCoinsIn2D(angle, leftShoulder.transform.position, leftarmLength + 0.2f, -90);
              lcoin.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
              lcoin.name = "lcoin" + angle.ToString();
              SphereCollider lsc = lcoin.AddComponent<SphereCollider>();
              lsc.isTrigger = true;
              Rigidbody lrb = lcoin.AddComponent<Rigidbody>();
              lrb.useGravity = false;
              lcoin.tag = "coins";
              NoOfCoins++;


              GameObject rcoin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
              rcoin.transform.position = PlaceCoinsIn2D(angle, rightShoulder.transform.position, rightarmLength + 0.15f, 90);
              rcoin.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
              rcoin.name = "rcoin" + angle.ToString();
              SphereCollider rsc = rcoin.AddComponent<SphereCollider>();
              rsc.isTrigger = true;
              Rigidbody rrb = rcoin.AddComponent<Rigidbody>();
              rrb.useGravity = false;
              rcoin.tag = "coins";
              NoOfCoins++;
          }

      }*/

    public void CreateCoins()
    {
        float realhandlength = 0.0f;
        float realheight = 0.0f;
        float realshoulder = 0.0f;
        float realfootlevel = 0.0f;
        double angle = 0;
        GameObject head = transform.Find("Head").gameObject;
        GameObject shoulderLeft = transform.Find("ShoulderLeft").gameObject;
        GameObject shoulderRight = transform.Find("ShoulderRight").gameObject;
        GameObject foot = transform.Find("FootLeft").gameObject;
        JointsProperties jphead = head.GetComponent<JointsProperties>();
        JointsProperties jpshoulderl = shoulderLeft.GetComponent<JointsProperties>();
        JointsProperties jpshoulderr = shoulderRight.GetComponent<JointsProperties>();
        JointsProperties jpfoot = foot.GetComponent<JointsProperties>();
        realheight = jphead.position.y;
        realshoulder = Vector3.Distance(jpshoulderl.position, jpshoulderr.position);
        realhandlength = (rightarmLength + leftarmLength) / 2;
        realfootlevel = jpfoot.position.y;
        XmlDocument doc = new XmlDocument();
        doc.Load("C:\\Users\\rajan\\Desktop\\info.xml");
        foreach(XmlNode node in doc.DocumentElement)
        {
            string jointname = node.Attributes[0].InnerText;
            if (jointname == "HandLength")
            {
                handlengthdelta =  realhandlength - float.Parse(node.ChildNodes[0].InnerText);
            }
            else if (jointname == "HeadPos")
            {
                heightdelta = realheight - float.Parse(node.ChildNodes[0].InnerText);
            }
            else if (jointname == "ShoulderWidth")
            {
                shoulderdelta = ( realshoulder - float.Parse(node.FirstChild.InnerText)) / 2;
            }
            else if(jointname == "AngleAdjust")
            {
                angleadjust = float.Parse(node.FirstChild.InnerText) - realfootlevel;
            }
            else if (jointname == "TotalCollisions")
            {
                totalcollisions = float.Parse(node.FirstChild.InnerText);
            }
            else if(jointname!="ValidAngle" && jointname!="GestureType" && !jointname.Contains("Head"))
            {
                if (jointname.Contains("Initial"))
                    totalInitial++;
                float x, y, z;
                x = float.Parse(node.ChildNodes[0].InnerText);
                y = float.Parse(node.ChildNodes[1].InnerText);
                z = float.Parse(node.ChildNodes[2].InnerText);
                if (jointname.Contains("Hand"))
                {
                    angle = Convert.ToDouble(node.ChildNodes[3].InnerText);
                    if (jointname.Contains("Right"))
                    {
                        if (angle < 90)
                            y = y - angleadjust + heightdelta + (handlengthdelta * Math.Abs(Convert.ToSingle(Math.Cos(ConvertDegreeToRadian(angle)))));
                        else
                            y = y - angleadjust + heightdelta - (handlengthdelta * Math.Abs(Convert.ToSingle(Math.Cos(ConvertDegreeToRadian(angle)))));
                        x += shoulderdelta + (handlengthdelta * Math.Abs(Convert.ToSingle(Math.Sin(ConvertDegreeToRadian(angle)))));
                    }
                    else if (jointname.Contains("Left"))
                    {
                        if (angle < 90)
                            y = y - angleadjust + heightdelta + (handlengthdelta * Math.Abs(Convert.ToSingle(Math.Cos(ConvertDegreeToRadian(angle)))));
                        else
                            y = y - angleadjust + heightdelta - (handlengthdelta * Math.Abs(Convert.ToSingle(Math.Cos(ConvertDegreeToRadian(angle)))));
                        x = x - shoulderdelta - (handlengthdelta * Math.Abs(Convert.ToSingle(Math.Sin(ConvertDegreeToRadian(angle)))));
                    }
                }
                else if(jointname.Contains("Head"))
                {
                    y = y - angleadjust + heightdelta;
                }
                else if(jointname.Contains("Foot"))
                {
                    y = y - angleadjust;
                    if (jointname.Contains("Right"))
                        x += shoulderdelta;
                    else if (jointname.Contains("Left"))
                        x -= shoulderdelta;
                }
                GameObject coin = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                coin.transform.position = new Vector3(x, y, z);
                coin.transform.localScale = new Vector3(0.06f, 0.15f, 0.06f);
                coin.transform.rotation = Quaternion.AngleAxis(90, Vector3.right);
                coin.name = jointname;
                CapsuleCollider sc = coin.AddComponent<CapsuleCollider>();
                sc.isTrigger = true;
                sc.height = 2f;
                /*Rigidbody rb = coin.AddComponent<Rigidbody>();
                rb.useGravity = false;*/
                //coin.tag = jointname;
                NoOfCoins++;    
            }
        }
    }
    public Vector3 PlaceCoinsIn2D(double angle, Vector3 origin, float radius, int angleDeviation)
    {
        float rad = ConvertDegreeToRadian(angle - angleDeviation);
        float x = (float)((origin.x + radius * Math.Cos(rad)));
        float y = (float)(origin.y + radius * Math.Sin(rad));
        float z = origin.z;

        return new Vector3(x, y, z);
    }

    public float ConvertDegreeToRadian(double degree)
    {
        return (float)(Math.PI * degree / 180.0);
    }

    public bool CheckLegAboveGround()
    {
        GameObject leftFoot = transform.Find("FootLeft").gameObject;
        JointsProperties LFJP = leftFoot.GetComponent<JointsProperties>();
        float leftLegHeight = (float)LFJP.distanceFromGround;

        GameObject rightFoot = transform.Find("FootRight").gameObject;
        JointsProperties RFJP = rightFoot.GetComponent<JointsProperties>();
        float rightLegHeight = (float)RFJP.distanceFromGround;

        float rightLegDiff = rightLegHeight - rightFootHeight;
        float leftLegDiff = leftLegHeight - leftFootHeight;
        
        if(rightLegDiff >= 0.02f && leftLegDiff >= 0.02f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
   /* public double getangle()
    {
        GameObject rightElbow = transform.Find("ElbowRight").gameObject;
        GameObject rightShoulder = transform.Find("ShoulderRight").gameObject;
        GameObject rightWrist = transform.Find("WristRight").gameObject;
        double armangle = CalculateAngle(rightShoulder, rightElbow, rightWrist);
        return armangle;
    }*/
    public float findDistanceBtwnFoots()
    {
        GameObject leftFoot = transform.Find("FootLeft").gameObject;
        GameObject rightFoot = transform.Find("FootRight").gameObject;

        return CalculateDistance(leftFoot, rightFoot);
    }

    public bool findAngleOfHand()
    {
        GameObject spineShoulder = transform.Find("SpineShoulder").gameObject;

        GameObject rightShoulder = transform.Find("ShoulderRight").gameObject;
        GameObject rightElbow = transform.Find("ElbowRight").gameObject;
        GameObject rightWrist = transform.Find("WristRight").gameObject;

        GameObject leftShoulder = transform.Find("ShoulderLeft").gameObject;
        GameObject leftElbow = transform.Find("ElbowLeft").gameObject;
        GameObject leftWrist = transform.Find("WristLeft").gameObject;

        double leftarmangle = CalculateAngle(leftShoulder, leftElbow, leftWrist);
        double leftarmshoulderangle = CalculateAngle(spineShoulder, leftShoulder, leftWrist);

        double rightarmangle = CalculateAngle(rightShoulder, rightElbow, rightWrist);
        double rightarmshoulderangle = CalculateAngle(spineShoulder, rightShoulder, rightWrist);

        //Debug.Log(leftarmangle);
        //Debug.Log(leftarmshoulderangle);

        bool lefthand = false;
        bool righthand = false;
        if ((leftarmangle > 150 && leftarmangle < 200) && (leftarmshoulderangle > 240 && leftarmshoulderangle < 300))
        {
             lefthand = true;
        }

        if ((rightarmangle > 150 && rightarmangle < 200) && (rightarmshoulderangle > 240 && rightarmshoulderangle < 300))
        {
             righthand = true;
        }

        return lefthand && righthand;

    } 

}