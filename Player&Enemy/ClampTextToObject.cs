using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClampTextToObject : MonoBehaviour
{
    public Text label;
    
    void Update()
    {
        Vector3 labelPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        label.transform.position = labelPosition;
    }
}
