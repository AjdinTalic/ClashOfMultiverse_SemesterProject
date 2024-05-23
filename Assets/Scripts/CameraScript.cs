using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float cameraSpeed = 2f;

    [SerializeField] private GameObject p1;
    [SerializeField] private GameObject p2;
    [SerializeField] private GameObject cameraEmpty;

    public static Vector3 cameraPos;

    private void Start()
    {
        gameObject.transform.position = cameraPos;
    }

    private void Update()
    {
        Vector3 middlePos = Vector3.Lerp(p1.transform.position, p2.transform.position, 0.5f);
        cameraEmpty.transform.position = middlePos;

        //Vector3 newPos = new Vector3(cameraEmpty.transform.position.x, cameraEmpty.transform.position.y, -10f);
        //transform.position = Vector3.Slerp(transform.position, newPos, cameraSpeed * Time.deltaTime);
    }
}
