using UnityEngine;
using System.Collections.Generic;

public class ExtremitiesDetector : MonoBehaviour
{
    private string grabObjectTag;
    private List<GameObject> grabObjectsDetected = new List<GameObject>();
    private GameObject selectedGrab;

    private void Awake()
    {
        selectedGrab = null;
        grabObjectTag = "GrabObject";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(grabObjectTag))
        {
            if (!grabObjectsDetected.Contains(collision.gameObject))
            {
                grabObjectsDetected.Add(collision.gameObject);
                GetClosestGrab();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(grabObjectTag))
        {
            if (grabObjectsDetected.Contains(collision.gameObject))
            {
                grabObjectsDetected.Remove(collision.gameObject);
                GetClosestGrab();
            }
        }
    }

    private void GetClosestGrab()
    {
        if (grabObjectsDetected.Count <= 0) 
        {
            selectedGrab = null;
            return;
        }
        GameObject closestGrab = null;
        float closestDistance = 1000;
        foreach (GameObject grab in grabObjectsDetected)
        {
            if (Vector2.Distance(grab.transform.position, transform.position) < closestDistance)
            {
                closestDistance = Vector2.Distance(grab.transform.position, transform.position);
                closestGrab = grab;
            }
        }
        selectedGrab = closestGrab;
    }

    public bool IsHoldingGrab()
    {
        if (selectedGrab == null)
            return false;
        else
            return true;
    }
}
