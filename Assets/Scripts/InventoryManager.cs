using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class InventoryManager : MonoBehaviour
{
    public RectTransform rect;
    public GameObject inventario;
    public bool isActive;
    public Slider sliderX;
    public Slider sliderY;

    private void Start()
    {
        sliderX.onValueChanged.AddListener(OnSliderXValueChanged);
        sliderY.onValueChanged.AddListener(OnSliderYValueChanged);

    }
    private void Update()
    {
        AbrirCerrarInv();
    }
    private void OnSliderXValueChanged(float value)
    {
        rect.sizeDelta = new Vector2(value, rect.sizeDelta.y);
    }
    private void OnSliderYValueChanged(float value)
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, value);
    }
    public void AbrirCerrarInv()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isActive)
            {
                inventario.SetActive(false);
                isActive = false;
            }
            else
            {
                inventario.SetActive(true);
                isActive = true;
            }
        }
    }
    public void cerrarINV()
    {
        inventario.SetActive(false);
        isActive = false;
    }





}
