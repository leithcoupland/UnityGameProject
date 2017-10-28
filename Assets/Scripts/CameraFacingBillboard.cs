using UnityEngine;
using System.Collections;



public class CameraFacingBillboard : MonoBehaviour
{
    private Camera m_Camera;

    //Initialise main camera as the gameobject.
    void Start()
    {
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    //Transform canvas to the face the position of the mian camera game object.
    void Update()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);
    }
}