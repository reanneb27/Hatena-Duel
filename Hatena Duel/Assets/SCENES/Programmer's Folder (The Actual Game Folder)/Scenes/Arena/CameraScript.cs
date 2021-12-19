using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // resources used:
    // https://answers.unity.com/questions/230190/how-to-get-the-width-and-height-of-a-orthographic.html
    // https://answers.unity.com/questions/215553/smoother-camera-movement.html

    Camera cam;
    public float EaseSpeed = 5;
    GameObject P1;
    GameObject P2;
    SpriteRenderer SR1;
    SpriteRenderer SR2;
    Vector3 P1Pos { get { return P1.transform.position; } }
    Vector3 P2Pos { get { return P2.transform.position; } }
    public float FixedYCam;
    public float MinXCam;
    public float MaxXCam;
    float CamWidth
    {
        get
        {
            return CamHeight * cam.aspect;
        }
        set
        {
            cam.orthographicSize = value / 2 / cam.aspect;
        }
    }
    float CamHeight
    {
        get
        {
            return cam.orthographicSize * 2f;
        }
        set
        {
            cam.orthographicSize = value / 2f;
        }
    }
    float MinCamWidth
    {
        get { return 5 * 2f * cam.aspect; }
    }
    float MinCamHeight
    {
        get { return 10f; }
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        P1 = GameObject.FindGameObjectWithTag("Player1");
        P2 = GameObject.FindGameObjectWithTag("Player2");

        SR1 = P1.GetComponent<SpriteRenderer>();
        SR2 = P2.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float PlayersHorizontalDistance = Vector3.Distance(new Vector3(P1Pos.x, 0), new Vector3(P2Pos.x, 0))
            + SR1.size.x + SR2.size.x;
        float PlayersVerticalDistance = Vector3.Distance(new Vector3(0, P1Pos.y), new Vector3(0, P2Pos.y))
            + SR1.size.y + SR2.size.y;

        float newCamWidth = CamWidth;
        float newCamHeight = CamHeight;
        if (PlayersHorizontalDistance > CamWidth)
        {
            newCamWidth = PlayersHorizontalDistance;

        }
        else if (PlayersVerticalDistance > CamHeight)
        {
            newCamHeight = PlayersVerticalDistance;
        }
        else
        {
            if (PlayersHorizontalDistance > MinCamWidth)
            {
                newCamWidth = PlayersHorizontalDistance;
                if (PlayersVerticalDistance > CamHeight)
                    newCamHeight = PlayersVerticalDistance;
            }
            else if (PlayersVerticalDistance > MinCamHeight)
            {
                newCamHeight = PlayersVerticalDistance;
                if (PlayersHorizontalDistance > CamWidth)
                    newCamWidth = PlayersHorizontalDistance;
            }
        }

        // resize camera
        CamWidth = Mathf.Lerp(CamWidth, newCamWidth, Time.deltaTime * EaseSpeed);
        CamHeight = Mathf.Lerp(CamHeight, newCamHeight, Time.deltaTime * EaseSpeed);

        // center camera between the 2 players
        Vector3 prevPos = cam.transform.position;
        cam.transform.position = Vector3.Lerp(cam.transform.position, (P1.transform.position + P2.transform.position) / 2, Time.deltaTime * EaseSpeed);
        cam.transform.position = new Vector3(cam.transform.position.x, prevPos.y, prevPos.z);

        // Snap the camera's min Y to the FixedYCam value
        cam.transform.position = new Vector3(cam.transform.position.x, FixedYCam + CamHeight / 2, cam.transform.position.z);

        // Snap the camera's min X to the FixedXCam value
        if (cam.transform.position.x - CamWidth / 2 < MinXCam)
            cam.transform.position = new Vector3(MinXCam + CamWidth / 2, cam.transform.position.y, cam.transform.position.z);
        else if (cam.transform.position.x + CamWidth / 2 > MaxXCam)
            cam.transform.position = new Vector3(MaxXCam - CamWidth / 2, cam.transform.position.y, cam.transform.position.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(Camera.main.transform.position, 2);
        Gizmos.DrawLine(Camera.main.transform.position - new Vector3(CamWidth / 2, 0), Camera.main.transform.position + new Vector3(CamWidth / 2, 0));

        Gizmos.DrawLine(new Vector3(MinXCam, -5), new Vector3(MinXCam, 5));
        Gizmos.DrawLine(new Vector3(MaxXCam, -5), new Vector3(MaxXCam, 5));
        Gizmos.DrawLine(new Vector3(5, FixedYCam), new Vector3(-5, FixedYCam));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(MinXCam, -5), new Vector3(MinXCam, 5));
        Gizmos.DrawLine(new Vector3(MaxXCam, -5), new Vector3(MaxXCam, 5));
        Gizmos.DrawLine(new Vector3(5, FixedYCam), new Vector3(-5, FixedYCam));
    }
}
