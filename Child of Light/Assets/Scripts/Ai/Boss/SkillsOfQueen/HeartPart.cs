using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPart : MonoBehaviour {
    private void Update() {
        Destroy(gameObject, 1.5f);
    }
}
