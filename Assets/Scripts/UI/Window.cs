using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public bool DestroyOnClose = true;

    public virtual void Close()
    {
        if (DestroyOnClose) Destroy(gameObject);
        else gameObject.SetActive(false);
    }
}
