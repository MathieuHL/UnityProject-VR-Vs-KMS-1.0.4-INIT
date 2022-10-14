﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRPlayerScript : MonoBehaviour
{
    public Transform leftHand, rightHand;
    public GameObject ballPrefab, shieldGO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions.default_GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            var shot = Instantiate(ballPrefab, rightHand, false);
            shot.transform.localPosition = new Vector3(0, 0, 0);

            var shotRb = shot.GetComponent<Rigidbody>();
            shotRb.velocity = (-rightHand.up+rightHand.forward)*25f;

        }

        if (SteamVR_Actions.default_GrabPinch.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            shieldGO.SetActive(true);
        }

        if (SteamVR_Actions.default_GrabPinch.GetStateUp(SteamVR_Input_Sources.LeftHand))
        {
            shieldGO.SetActive(false);
        }
    }
}
