using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class GunShooting : MonoBehaviour
{
    public List<GameObject> pills;
    private Transform spawnPoint;
    private float speed = 25f;
    private float firingSpeed = 0.2f;
    private float TimeBetweenBullet = 0f;
    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = gameObject.transform.Find("BulletSpawnPoint");
        cam = GameObject.FindGameObjectWithTag("freeLookCam");
    }

    // Update is called once per frame
    void Update()
    {
        TimeBetweenBullet += Time.deltaTime;
        if (Input.GetMouseButton(0) && TimeBetweenBullet > firingSpeed)
        {
            var tempBullet = Instantiate(pills[Random.Range(0, pills.Count)], spawnPoint.position, cam.transform.rotation);
            tempBullet.GetComponent<Rigidbody>().velocity = cam.transform.forward * speed;
            tempBullet.GetComponent<Rigidbody>().angularVelocity = new Vector3( (Random.value - 0.5f)*10000, (Random.value - 0.5f) * 10000, (Random.value - 0.5f) * 10000);
            TimeBetweenBullet = 0;
        }
    }
}
