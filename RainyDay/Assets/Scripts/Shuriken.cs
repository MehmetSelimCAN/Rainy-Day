using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour {

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    private Vector3 target;

    private float speed = 2f;
    private float angle = 0f;

    private void Awake() {
        target = pointA.position;
    }

    private void Update() {
        if (angle < 360) {
            angle += 2f;
        }
        else {
            angle = 0;
        }

        transform.rotation = Quaternion.Euler(0f,0f, angle);

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        if (Vector3.Distance(transform.position, target) < 0.001f) {
            ChangeTarget();
        }
    }

    private void ChangeTarget() {
        if (Vector3.Distance(transform.position, pointA.position) < 0.001f) {
            target = pointB.position;
        }
        else if (Vector3.Distance(transform.position, pointB.position) < 0.001f) {
            target = pointA.position;
        }
    }
}
