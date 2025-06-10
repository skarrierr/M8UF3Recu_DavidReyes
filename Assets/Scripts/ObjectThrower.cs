using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectThrower : MonoBehaviour
{
    [System.Serializable]
    public struct ThrowableObject
    {
        public GameObject obj;
        public CanvasGroup selector;
    }
    [SerializeField]
    ThrowableObject[] objects;
    public int selected;
    public Image forceIndicator;
    public float force;
    public float forceMin;
    public float forceMax;
    public float forceSpeed;
    public float distance = 1;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            force += forceSpeed * Time.deltaTime;
            if (force < forceMin)
            {
                force = forceMin;
                forceSpeed *= -1;
            }
            if (force > forceMax)
            {
                force = forceMax;
                forceSpeed *= -1;
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            GameObject temp = Instantiate(objects[selected].obj, transform.position + transform.forward * distance, transform.rotation);
            temp.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.VelocityChange);
            force = forceMin;
        }
        else
        {
            selected += (int)Input.mouseScrollDelta.y;
            if(selected >= objects.Length)
            {
                selected = 0;
            }
            if(selected < 0)
            {
                selected = objects.Length - 1;
            }
            for (int i = 0; i < objects.Length; i++)
            {
                if(i != selected)
                {
                    objects[i].selector.alpha = 0;
                }
                else
                {
                    objects[i].selector.alpha = 1;
                }
            }
        }
    }
}
