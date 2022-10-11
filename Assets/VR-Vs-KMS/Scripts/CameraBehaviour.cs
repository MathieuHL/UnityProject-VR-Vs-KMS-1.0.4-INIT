using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public enum RotationAxis
    {
        X = 1,
        Y = 2
    }
    public RotationAxis axis;
    private static float MAX_Y = 75.0f;
    private static float MIN_Y = -75.0f;

    public float SensVertical = 1.0f;
    public float SensHorizontal = 1.0f;
    private float rotationX = 0;
    private float rotationY = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;

        if (axis == RotationAxis.X)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * SensHorizontal, 0);
        }else if (axis == RotationAxis.Y)
        {
            rotationX -= Input.GetAxis("Mouse Y") * SensVertical;
            rotationX = Mathf.Clamp(rotationX, MIN_Y, MAX_Y);
            rotationY = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        }
    }
}
