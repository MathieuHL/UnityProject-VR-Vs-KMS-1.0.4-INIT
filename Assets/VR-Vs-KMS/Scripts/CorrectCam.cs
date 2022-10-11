using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectCam : MonoBehaviour
{
    public Transform self;

    private void Awake()
    {
        self.SetParent(null, true);
    }
}