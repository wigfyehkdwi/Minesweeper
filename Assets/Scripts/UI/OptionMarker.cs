using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMarker : MonoBehaviour
{
    private RectTransform RectTransform;
    public virtual RectTransform Target { get; }

    private void Awake()
    {
        this.RectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        this.RectTransform.position = new Vector3(Target.position.x - 45, Target.position.y);
    }
}
