using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByScreen : MonoBehaviour {

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
