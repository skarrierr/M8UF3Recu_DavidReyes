using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    public static List<PickableObject> pickObjects = new List<PickableObject>();

    [Header("Picking")]
    public Camera cam;
    public float minDistance;

    [NonSerialized]
    public PickableObject pickableObject;
    float distance;
    Vector3 viewport;
    public enum PICKSTATE { NOTVISIBLE, FAR, BLOCKED, AVAILABLE }
    public PICKSTATE state;
    public float pickingIconSpeed = 10;
    public CanvasGroup pickingIcon;
    Vector3 pickingIconCurrentPos;
    [Range(0f, 1f)]
    public float pickingIconFar;
    [Range(0f, 1f)]
    public float pickingIconBlocked;
    [Range(0f, 1f)]
    public float pickingIconAvailable;

    [SerializeField]
    LayerMask mask;

    [Header("Picked")]
    [NonSerialized]
    public PickableObject pickedObject = null;
    [SerializeField]
    private LineRenderer line;
    Vector3 pickedPosition;
    public float forceScale = 1;
    public float scrollSensitivity = 1;
    public float distanceMax = 10;
    public float distanceMin = 2;
    public float pickedRbDragLinear = 2;
    float pickedRbDragLinearOriginal;
    public float pickedRbDragAngular = 100;
    float pickedRbDragAngularOriginal;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedObject == null)
        {
            NearestObject();
        }

        HandlePickingUI();

        StartPickup();
        HandlePickup();
        EndPickup();
    }

    void NearestObject()
    {
        pickableObject = null;
        distance = float.MaxValue;
        viewport = Vector3.one * float.MaxValue;
        pickedPosition = Vector3.zero;
        state = PICKSTATE.NOTVISIBLE;
        foreach (PickableObject pickableObject in pickObjects)
        {
            Vector3 tempviewport = cam.WorldToViewportPoint(pickableObject.transform.position);
            if (
                tempviewport.x < 0 || tempviewport.x > 1 ||
                tempviewport.y < 0 || tempviewport.y > 1 ||
                tempviewport.z < 0
                )
            {
                continue;
            }
            if (tempviewport.z < minDistance)
            {
                if (new Vector2(viewport.x - 0.5f, viewport.y - 0.5f).sqrMagnitude > new Vector2(tempviewport.x - 0.5f, tempviewport.y - 0.5f).sqrMagnitude)
                {
                    viewport = tempviewport;
                    distance = tempviewport.z;
                    this.pickableObject = pickableObject;
                }
            }
            else
            {
                if (distance > tempviewport.z)
                {
                    viewport = tempviewport;
                    distance = tempviewport.z;
                    this.pickableObject = pickableObject;
                }
            }
        }

        if (pickableObject != null)
        {
            if (distance > minDistance)
            {
                state = PICKSTATE.FAR;
            }
            else
            {
                RaycastHit hit;
                Ray ray = new Ray(cam.transform.position, cam.transform.forward);
                Debug.DrawRay(ray.origin, ray.direction * minDistance, Color.red);
                if(Physics.Raycast(ray,out hit, minDistance, mask))
                {
                    PickableObject obj = hit.collider.GetComponent<PickableObject>();
                    if (obj || hit.collider.transform.IsChildOf(pickableObject.transform))
                    {
                        state = PICKSTATE.AVAILABLE;
                        pickedPosition = pickableObject.transform.InverseTransformPoint(hit.point);
                        if (obj)
                            pickableObject = obj;
                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                        distance = hit.distance;
                    }
                    else
                    {
                        state = PICKSTATE.BLOCKED;
                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
                    }
                }
                else
                {
                    state = PICKSTATE.BLOCKED;
                }
            }
        }
    }
    void HandlePickingUI()
    {

    }

    void StartPickup()
    {
        if (pickedObject != null || pickableObject == null || state != PICKSTATE.AVAILABLE) return;
        if (Input.GetMouseButtonDown(0))
        {
            pickedObject = pickableObject;
            pickableObject = null;
            pickedRbDragLinearOriginal = pickedObject.rb.drag;
            pickedObject.rb.drag = pickedRbDragLinear;
            pickedRbDragAngularOriginal = pickedObject.rb.angularDrag;
            pickedObject.rb.angularDrag = pickedRbDragAngular;
            state = PICKSTATE.NOTVISIBLE;
            line.enabled = true;
        }
    }
    void HandlePickup()
    {
        if (pickedObject == null) return;

        distance += Input.mouseScrollDelta.y * scrollSensitivity;
        distance = Mathf.Clamp(distance, distanceMin, distanceMax);
        Vector3 forcepos = pickedObject.transform.TransformPoint(pickedPosition);
        Vector3 targetPos = cam.transform.position + cam.transform.forward * distance;
        Debug.DrawLine(forcepos, targetPos);
        line.SetPosition(0, forcepos);
        line.SetPosition(1, targetPos);
        pickedObject.rb.AddForceAtPosition((targetPos - forcepos) * forceScale, forcepos, ForceMode.VelocityChange);
    }
    void EndPickup()
    {
        if (pickedObject == null) return;
        if (Input.GetMouseButtonUp(0))
        {
            pickedObject.rb.drag = pickedRbDragLinearOriginal;
            pickedObject.rb.angularDrag = pickedRbDragAngularOriginal;
            pickedObject = null;
            pickableObject = null;
            state = PICKSTATE.NOTVISIBLE;
            line.enabled = false;
        }
    }
}
