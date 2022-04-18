using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private Rigidbody2D rb;

    private Animator animator;
    private string currentAnimationState;
    private string PLAYER_IDLE = "Idle";
    private string PLAYER_RUN = "Run";
    private string PLAYER_JUMP = "Jump";
    private string PLAYER_FALL = "Fall";

    private void Awake() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            ChangeAnimationState(PLAYER_JUMP);
        }

        if (rb.velocity.y > 0) {
            ChangeAnimationState(PLAYER_JUMP);
        }

        if (rb.velocity.y < 0) {
            ChangeAnimationState(PLAYER_FALL);
        }

        if (rb.velocity.y == 0) {
            if (rb.velocity.x == 0) {
                ChangeAnimationState(PLAYER_IDLE);
            }
            else if (rb.velocity.x != 0) {
                ChangeAnimationState(PLAYER_RUN);
            }
        }
    }

    private void ChangeAnimationState(string newState) {
        if (currentAnimationState == newState)
            return;

        animator.Play(newState);
        currentAnimationState = newState;
    }
}
