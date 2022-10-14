using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonActionsScripts : MonoBehaviour
{
    public int tpLifePoint;
    public string tpColorShoot;

    private void Start()
    {
        tpLifePoint = GameManager.Instance.gameSetting.LifeNumber;
        tpColorShoot = GameManager.Instance.gameSetting.ColorShotKMS;
    }

}
