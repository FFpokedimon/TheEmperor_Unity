using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MiniMap : MonoBehaviourPunCallbacks
{
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private float minValue = 10f;
    [SerializeField] private float maxValue = 60f;
    private float currentValue;

    void Start()
    {
        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta > 0)
        {
            currentValue += scrollSpeed;
        }
        else if (scrollDelta < 0)
        {
            currentValue -= scrollSpeed;
        }
        currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
        gameObject.GetComponent<Camera>().orthographicSize = currentValue;
    }
}
