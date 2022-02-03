using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(LineRenderer))]
public class OffRoadTeleporter : MonoBehaviour
{
    public SteamVR_Input_Sources hand;

    public float triggerSensitivity = 0.025f;
    public float maxDistance = 20.0f;
    public Material missMat;
    public Material hitMat;
    public GameObject player;

    private LineRenderer line;
    private bool padPressed = false;
    private bool doTeleport = false;
    private Vector3 destination;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.material = missMat;
    }

    void Update()
    {
        bool held = SteamVR_Input.GetState("offroadteleport", hand);
        if (held)
        {
            Ray raycast = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            bool bHit = Physics.Raycast(raycast, out hit);

            if (bHit && hit.distance <= maxDistance && hit.collider.name == "Terrain")
            {
                line.material = hitMat;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, hit.point);

                destination = hit.point;
                doTeleport = true;
            }
            else
            {
                line.material = missMat;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, transform.position + transform.forward * maxDistance);
                doTeleport = false;
            }

            line.enabled = true;
        }
        else
        {
            if (doTeleport)
            {
                player.transform.position = destination;
                doTeleport = false;
            }

            line.enabled = false;
        }


    }
}
