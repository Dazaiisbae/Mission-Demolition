using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour

{
    public GameObject LaunchPoint;
    public GameObject ProjectilePrefab;
    public float velocityMult = 10f;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    private void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        LaunchPoint = launchPointTrans.gameObject;
        LaunchPoint.SetActive(false);
        launchPos = LaunchPoint.transform.position;
    }
    void OnMouseEnter()
   {
     //print("SlingShot: OnMouseEnter");
     LaunchPoint.SetActive(true);
    }

    void OnMouseExit()
        {
       //print("SlingShot: OnMouseExit");
       LaunchPoint.SetActive(false);
    }


    void OnMouseDown()
    {
        aimingMode = true;
        //instantiate a projectile
        projectile = Instantiate(ProjectilePrefab) as GameObject;
        projectile.transform.position = launchPos;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Update()
    {
        if (!aimingMode) return;

        //get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;

        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        //move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            Rigidbody projRigidbody = projectile.GetComponent<Rigidbody>();
            projRigidbody.isKinematic = false;
            projRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRigidbody.velocity = -mouseDelta * velocityMult;
            projectile = null;
        }
    }
}
