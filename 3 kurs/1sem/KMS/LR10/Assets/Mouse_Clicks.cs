using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mouse_Clicks : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
	{
        float red = Random.Range(.0f, 1.0f);
        float green = Random.Range(.0f, 1.0f);
        float blue = Random.Range(.0f, 1.0f);
        Color col1 = new Color(red, green, blue);
        GetComponent<Renderer>().material.color = col1;

        Vector3 target = eventData.pointerPressRaycast.worldPosition;
        Vector3 collid = Camera.main.transform.position;
        int forse = 300;

        Vector3 distance = target - collid;
        distance = distance.normalized;

        collid = distance * forse;

        gameObject.GetComponent<Rigidbody>().AddForceAtPosition(collid, target);
    }
}
