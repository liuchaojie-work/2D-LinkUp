using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    LineRenderer line1, line2, line3;
    public void CreateLine()
    {
        GameObject line = new GameObject("line1");
        line1 = line.AddComponent<LineRenderer>();
        line1.startWidth = 0.1f;
        line1.endWidth = 0.1f; 
        line1.positionCount = 2;

        line = new GameObject("line2");
        line2 = line.AddComponent<LineRenderer>();
        line2.startWidth = 0.1f;
        line2.endWidth = 0.1f;
        line2.positionCount = 2;

        line = new GameObject("line3");
        line3 = line.AddComponent<LineRenderer>();
        line3.startWidth = 0.1f;
        line3.endWidth = 0.1f;
        line3.positionCount = 2;
    }

    public void DrawLinkLine(GameObject g1,GameObject g2, int linkType, Vector3 z1, Vector3 z2)
    {
        if(0 == linkType)
        {
            line1.SetPosition(0, g1.transform.position + new Vector3(0, 0, -1));
            line1.SetPosition(1, g2.transform.position + new Vector3(0, 0, -1));
        }

        if(1 == linkType)
        {
            line1.SetPosition(0, g1.transform.position);
            line1.SetPosition(1, z1);

            line2.SetPosition(0, z1);
            line2.SetPosition(1, g2.transform.position);

        }

        if(2 == linkType)
        {
            line1.SetPosition(0, g1.transform.position);
            line1.SetPosition(1, z2);

            line2.SetPosition(0, z2);
            line2.SetPosition(1, z1);

            line3.SetPosition(0, z1);
            line3.SetPosition(1, g2.transform.position);
        }
        StartCoroutine(DestoryLine());
    }


    IEnumerator DestoryLine()
    {
        yield return new WaitForSeconds(0.2f);
        line1.SetPosition(0, Vector3.zero);
        line1.SetPosition(1, Vector3.zero);

        line2.SetPosition(0, Vector3.zero);
        line2.SetPosition(1, Vector3.zero);

        line3.SetPosition(0, Vector3.zero);
        line3.SetPosition(1, Vector3.zero);
    }
}
