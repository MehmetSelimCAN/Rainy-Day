using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D rb;
    private float speed = 30f;
    private float horizontalMove;

    [SerializeField] private LayerMask obstacleLayer;
    private bool isGrounded;
    private Vector2 feetPosition;
    private float jumpForce = 8f;
    private float fallMultiplier = 4f;

    private float fJumpPressedRemember = 0;
    private float fJumpPressedRememberTime = 0.2f;

    private float fGroundedRemember = 0;
    private float fGroundedRememberTime = 0.025f;

    private bool canChangeGravity = true;

    private ParticleSystem.MainModule particleSystemMain;
    private Transform waterSplashEffect;
    private Transform changeGravityEffect;
    private Transform deathEffect;
    private Transform trapCollisionEffect;
    private bool waitingForDeathAnimation = false;
    private int numberOfKey = 0;
    private int numberOfKeysRequired;

    private Transform door;
    private Transform fade;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        waterSplashEffect = GameObject.Find("waterSplashEffect").transform;
        changeGravityEffect = GameObject.Find("changeGravityEffect").transform;
        deathEffect = GameObject.Find("deathEffect").transform;
        trapCollisionEffect = GameObject.Find("trapCollisionEffect").transform;
        fade = GameObject.Find("Fade").transform;

        if (GameObject.Find("pfDoor")) {
            door = GameObject.Find("pfDoor").transform;

            numberOfKeysRequired = GameObject.FindGameObjectsWithTag("Key").Length;
            if (numberOfKeysRequired == 0) {
                door.GetComponent<SpriteRenderer>().color = new Color32(0, 129, 204, 255);
            }
        }
    }

    private void Update() {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        #region Jump
        fGroundedRemember -= Time.deltaTime;
        if (isGrounded) {
            fGroundedRemember = fGroundedRememberTime;
        }

        fJumpPressedRemember -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.W)) {
            fJumpPressedRemember = fJumpPressedRememberTime;
        }

        if ((fJumpPressedRemember > 0) && (fGroundedRemember > 0)) {
            fJumpPressedRemember = 0;
            fGroundedRemember = 0;

            if (rb.gravityScale < 0) {
                rb.velocity = Vector2.down * jumpForce;
            }
            else if (rb.gravityScale > 0) {
                rb.velocity = Vector2.up * jumpForce;
            }
        }

        if (rb.velocity.y < 0 && rb.gravityScale > 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        else if (rb.velocity.y > 0 && rb.gravityScale < 0) {
            rb.velocity += Vector2.down * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        #endregion

        #region Gravity
        if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene().buildIndex > 3) {
            ChangeGravity();
        }
        #endregion


        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void FixedUpdate() {
        if (rb.gravityScale > 0) {
            feetPosition = (Vector2)transform.position + new Vector2(0, -0.1f);
        }
        else if (rb.gravityScale < 0) {
            feetPosition = (Vector2)transform.position + new Vector2(0, +0.1f);
        }
        isGrounded = Physics2D.OverlapBox(feetPosition, transform.localScale, 0, obstacleLayer);

        rb.velocity = new Vector2(horizontalMove * Time.fixedDeltaTime * 10f, rb.velocity.y);
    }

    private void ChangeGravity() {
        if (canChangeGravity) {
            rb.gravityScale *= -1;
            canChangeGravity = false;
            GetComponent<SpriteRenderer>().flipY = !GetComponent<SpriteRenderer>().flipY;

            waterSplashEffect.localPosition = new Vector3(waterSplashEffect.localPosition.x, waterSplashEffect.localPosition.y * -1, 0f);
            particleSystemMain = waterSplashEffect.GetComponent<ParticleSystem>().main;
            particleSystemMain.gravityModifierMultiplier *= -1;
            changeGravityEffect.GetComponent<ParticleSystem>().Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Obstacle") {
            canChangeGravity = true;
            waterSplashEffect.GetComponent<ParticleSystem>().Play();
        }

        if (collision.collider.tag == "Key") {
            numberOfKey++;
            Destroy(collision.collider.gameObject);
            if (numberOfKey == numberOfKeysRequired) {
                door.GetComponent<SpriteRenderer>().color = new Color32(0, 129, 204, 255);
            }
        }

        if (collision.collider.tag == "Door") {
            if (numberOfKey == numberOfKeysRequired) {
                StartCoroutine(WaitForFade());
            }
        }

        if (collision.collider.tag == "MapBorder") {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            this.enabled = false;
            deathEffect.GetComponent<ParticleSystem>().Play();
            StartCoroutine(WaitForDeathEffect());
        }

        if ((collision.collider.tag == "Trap" || collision.collider.tag == "Shuriken") && !waitingForDeathAnimation) {
            //death animation
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            this.enabled = false;
            deathEffect.GetComponent<ParticleSystem>().Play();
            trapCollisionEffect.GetComponent<ParticleSystem>().Play();
            StartCoroutine(WaitForDeathEffect());
        }
    }

    private IEnumerator WaitForFade() {
        fade.GetComponent<Animator>().Play("FadeOut");

        if (PlayerPrefs.GetInt("LastLevel") < SceneManager.GetActiveScene().buildIndex + 1) {
            PlayerPrefs.SetInt("LastLevel", SceneManager.GetActiveScene().buildIndex + 1);
        }

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator WaitForDeathEffect() {
        waitingForDeathAnimation = true;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
