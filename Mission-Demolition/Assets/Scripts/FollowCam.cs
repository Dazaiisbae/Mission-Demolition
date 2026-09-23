using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // The static point of interest
    public float easing = 0.05f; // The easing for the camera movement
    public Vector2 minXY = Vector2.zero; // The minimum x and y values for the camera position
    public float camZ; // the desired z pos of the camera


    void Awake()
    {
        camZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        if (POI == null) return; // return if there is no POI

        // get the position of the POI
        Vector3 destination = POI.transform.position;
        if (POI != null)
        {
            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();
            if (poiRigid != null)
            {
                if (poiRigid.IsSleeping())
                {
                    POI = null;
                }
            }
        }
        if (POI != null)
        {
            destination.x = Mathf.Max(minXY.x, destination.x); // clamp the x pos to the min x
            destination.y = Mathf.Max(minXY.y, destination.y); // clamp the y pos to the min y
            destination = Vector3.Lerp(transform.position, destination, easing); // interpolate between the current position and the POI position
            destination.z = camZ; // set the desired z pos
            transform.position = destination;
            Camera.main.orthographicSize = destination.y + 10; // set the orthographic size to the y pos of the camera plus 10
        }
        else
        {
            destination = new Vector3(0, 0, camZ); // set the destination to the origin with the desired z pos
           // transform.position = Vector3.Lerp(transform.position, destination, easing); // interpolate between the current position and the origin
            transform.position = destination; // set the position to the origin
        }
    }
}
