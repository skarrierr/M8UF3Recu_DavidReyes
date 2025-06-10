using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickableObject : MonoBehaviour
{
    [HideInInspector][SerializeField]
    public Rigidbody rb;
    public enum TYPE { UNDEFINED, RED, GREEN, BLUE}
    public TYPE type;
    public bool collectable { get { return type != TYPE.UNDEFINED; } }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        ObjectPicker.pickObjects.Add(this);
    }
    private void OnDisable()
    {
        ObjectPicker.pickObjects.Remove(this);
    }
}
