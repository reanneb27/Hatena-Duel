using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // resources used:
    // https://answers.unity.com/questions/230190/how-to-get-the-width-and-height-of-a-orthographic.html
    // https://answers.unity.com/questions/215553/smoother-camera-movement.html

    Camera cam = Camera.main;
    public float EaseSpeed = 5;
    GameObject P1;
    GameObject P2;
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
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); // getting main camera method 1
        cam = Camera.main; // getting main camera method 2

        P1 = GameObject.FindGameObjectWithTag("Player1");
        P2 = GameObject.FindGameObjectWithTag("Player2");
    }

    // Update is called once per frame
    void Update()
    {
        //string name = "Camera";
        //Debug.Log("Camera Width: " + CamWidth + " Camera Height: " + CamHeight + "\n"
        //    + name + " Viewport Position: " + transform.position + name + " World Position: " + cam.ViewportToWorldPoint(transform.position));

        //name = "Player 1";
        //Debug.Log(name + " Width: " + P1.GetComponent<SpriteRenderer>().bounds.size.x + name + " Height: " + P1.GetComponent<SpriteRenderer>().bounds.size.y + "\n"
        //    + name + " Viewport Position: " + P1.transform.position + name + " World Position: " + cam.ViewportToWorldPoint(P1.transform.position));

        //name = "Player2";
        //Debug.Log(name + " Width: " + P2.GetComponent<SpriteRenderer>().bounds.size.x + name + " Height: " + P2.GetComponent<SpriteRenderer>().bounds.size.y + "\n"
        //    + name + " Viewport Position: " + P2.transform.position + name + " World Position: " + cam.ViewportToWorldPoint(P2.transform.position));

        // to get width and height of object use SpriteRenderer.bounds.size.x and ...size.y
        // there's viewport and world coordinate system.
        // i also need to add the width/height of the players to fit inside the horizontal distance and vertical distance of the camera-
        // because the position is at the center of the player
        float PlayersHorizontalDistance = Vector3.Distance(new Vector3(P1.transform.position.x, 0), new Vector3(P2.transform.position.x, 0))
            + P1.GetComponent<SpriteRenderer>().bounds.size.x + P2.GetComponent<SpriteRenderer>().bounds.size.x;
        float PlayersVerticalDistance = Vector3.Distance(new Vector3(0, P1.transform.position.y), new Vector3(0, P2.transform.position.y))
            + P1.GetComponent<SpriteRenderer>().bounds.size.y + P2.GetComponent<SpriteRenderer>().bounds.size.y;

        if (PlayersHorizontalDistance > CamWidth)
        {
            CamWidth = PlayersHorizontalDistance;

        }
        else if (PlayersVerticalDistance > CamHeight)
        {
            CamHeight = PlayersVerticalDistance;
        }
        else
        {
            if (PlayersHorizontalDistance > MinCamWidth)
            {
                CamWidth = PlayersHorizontalDistance;
                if (PlayersVerticalDistance > CamHeight)
                    CamHeight = PlayersVerticalDistance;
            }
            else if (PlayersVerticalDistance > MinCamHeight)
            {
                CamHeight = PlayersVerticalDistance;
                if (PlayersHorizontalDistance > CamWidth)
                    CamWidth = PlayersHorizontalDistance;
            }
        }

        // cemter camera between the 2 players
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
