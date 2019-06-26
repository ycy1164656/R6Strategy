using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotHelper : MonoBehaviour {

	// Use this for initialization
    public Transform DeletePivot()
    {
        Transform t = GetComponentsInChildren<Transform>()[1];
        t.parent = null;
        Destroy(this.gameObject, 0.1f); // delay self destroy to make sure return runs

        return t;
    }
}
