using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

[RequireComponent(typeof(ARRaycastManager))]
public class ARController : MonoBehaviour
{
    private ARRaycastManager _raycastManager;

    [SerializeField] private GameObject PlaceablePrefab;
    [SerializeField] private GameObject ARCamera;

    private static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if (_raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;
                PlaceablePrefab.transform.position = hitPose.position;
                PlaceablePrefab.transform.rotation = hitPose.rotation;

                //we want the portal to face the camera upon
                Vector3 cameraPosition = ARCamera.transform.position;

                // the portal shoul only rotate around the Y axis
                cameraPosition.y = hitPose.position.y;

                //Rotate the portal to face the camera
                PlaceablePrefab.transform.LookAt(cameraPosition,PlaceablePrefab.transform.up);
        }
    }
}