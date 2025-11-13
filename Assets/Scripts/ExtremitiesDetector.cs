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
        GameObject closestStone = null;
        float closestDistance = 1000;
        foreach (GameObject stone in grabObjectsDetected)
        {
            if (Vector2.Distance(stone.transform.position, transform.position) < closestDistance)
            {
                closestDistance = Vector2.Distance(stone.transform.position, transform.position);
                closestStone = stone;
            }
        }
        selectedGrab = closestStone;
    }

    public bool IsHoldingGrab()
    {
        if (selectedGrab == null)
            return false;
        else
            return true;
    }
}
