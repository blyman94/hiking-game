using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncToMainCameraRotation : MonoBehaviour
{
    private Transform _cameratransform;

    private void Start()
    {
        _cameratransform = Camera.main.transform;
    }
    
    void Update()
    {
        transform.rotation = _cameratransform.rotation;
    }
}