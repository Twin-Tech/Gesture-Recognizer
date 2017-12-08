using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class colloideDetect : MonoBehaviour {

    public bool started = false;
    public string lastcollided = " ";
    public GameObject start, mid, end;
    public float validangle;
    public bool anglevalid = true;
    bool initialised = false;
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name.Contains(this.name) && (other.name.CompareTo(lastcollided)>=0 || lastcollided.Contains("Initial")))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\Users\\rajan\\Desktop\\info.xml");
            foreach(XmlNode node in doc.DocumentElement)
            {
                if(node.Attributes[0].InnerText=="ValidAngle" && node.Attributes[1].InnerText==this.name)
                {
                    start = GameObject.Find(node.Attributes[2].InnerText);
                    mid = GameObject.Find(node.Attributes[3].InnerText);
                    end = GameObject.Find(node.Attributes[4].InnerText);
                    validangle = float.Parse(node.ChildNodes[0].InnerText);
                    Vector3 a = start.transform.position - mid.transform.position;
                    Vector3 b = end.transform.position - mid.transform.position;
                    if (Vector3.Angle(a, b) > validangle - 40 && Vector3.Angle(a, b) < validangle + 40)
                        anglevalid = true;
                    else
                    {
                        anglevalid = false;
                        break;
                    }
                }
            }
            BodyProperties BP = transform.parent.gameObject.GetComponent<BodyProperties>();
           if(lastcollided!=other.name && anglevalid)
            {
                if (other.name.Contains("Initial") && started == false)
                {
                    started = true;
                    BP.InitialCollisions++;
                }
                else if (other.name.Contains("Initial") && started == true)
                {
                    //if (lastcollided.Contains("Hand"))
                        Debug.Log(lastcollided);
                    started = false;
                    BP.Noofcollisions++;
                    BP.InitialCollisions++;
                    BP.ResetColliders();
                }
                if(started)
                {
                    BP.Noofcollisions++;
                    if (other.name.EndsWith("9"))
                        lastcollided = other.name.Replace("9", "0");
                    else
                        lastcollided = other.name;
                    //if(lastcollided.Contains("Hand"))
                        Debug.Log(lastcollided);
                }
             
            }
        }
    }
}
