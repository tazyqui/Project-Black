using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    //This Script Should be placed on the parent object of your player IE the object named "Player"
    // Start is called before the first frame update

    [Header("Player References")]
    [Tooltip("The animator of the player, should be on the sprite")]
    public Animator PlayerAnimator;
    [Tooltip("The audio script (Should already be on this object)")]
    public PlayerAudio playerAudio;
    [Tooltip("The attack script (Should be placed on this object if you want the player to be able to attack)")]
    public PlayerAttack playerAttack;
    [Tooltip("This is the GameManager, you only need this if the player can die")]
    public GameManager gm;
    [Tooltip("This is the SceneManager, you only need this if the player can die")]
    public GameSceneManager gsm;

    [Header("Properties")]
    [Tooltip("The speed at which the player moves")]
    public float PlayerMovementSpeed = 3.5f;

    [Tooltip("This means that you're 1D character is facing left by default (1D means you only face left or right)")]
    public bool isFlipped = false;

    [Tooltip("This should be checked if your character has Multi-Directional movement (IE up, down, left, right) and your animator is set up accordingly")]
    public bool isMultiDirectional = false;

    [Tooltip("If you want audio to be used alongside this object")]
    public bool isAudioEnabled = true;

    [Tooltip("This is whether or not the player can actually move")]
    public bool disabled = false;

    [Header("Side-Scroller Properties")]
    [Tooltip("If you're player can jump, it's automatically a side-scroller, it isn't compatible with Top-Down movement")]
    public bool canJump = false;

    [Tooltip("The force of your jump (Be sure to have your gravity set to 1 for side-scroller)")]
    public float jumpForce = 1;

    [Tooltip("The multiplier at which you fall down (used for smooth movement) and it can't be below 1")]
    public float fallMultiplier = 2.5f;

    [Tooltip("The multiplier at which you fall down if you hold the jump button (used for smooth movement) and it can't be below 1")]
    public float lowJumpMultiplier = 2f;
    private Rigidbody2D rb;
    private bool isJumping = false;
    private bool canRayCastJump = false;

    [Header("Raycast Jumping")]
    [Tooltip("If you're using raycast ")]
    public bool useRayCastJumping = false;
    public LayerMask groundLayer;
    public float rayLength = 0;

    // Update is called once per frame
    void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
        playerAttack = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float HorizontalMovement = 0;
        float VerticleMovement = 0;
        float JumpMovement = 0;
        if (!disabled)
        {
            HorizontalMovement = Input.GetAxisRaw("Horizontal");
            VerticleMovement = Input.GetAxisRaw("Vertical");
            if (canJump)
            {
                HandleJump();
            }
            if (Input.GetButtonDown("Jump") && canJump)
            {
                HandleJump();
                //Debug.Log(canRayCastJump);
                if (useRayCastJumping && canRayCastJump)
                {
                    rb.velocity = Vector2.up * jumpForce;
                    isJumping = true;
                    canRayCastJump = false;
                    HandleJumpAudio();
                   // Debug.Log("Managed to Jump");
                }
                else if(!useRayCastJumping && !isJumping)
                {
                    rb.velocity = Vector2.up * jumpForce;
                    isJumping = true;
                    HandleJumpAudio();
                }
            }
        }

        if (PlayerAnimator != null)
        {
            HandleAnimations(HorizontalMovement, VerticleMovement);
        }
        //HandleAnimations(HorizontalMovement, VerticleMovement);
        HandleMovement(HorizontalMovement, VerticleMovement);
        if (isAudioEnabled)
        {
            HandleAudio(HorizontalMovement, VerticleMovement);
        }
    }

    #region Animations

    void HandleAnimations(float HorizontalMovement, float VerticleMovement)
    {
        if (isMultiDirectional)
        {
            HandleAnimationsMulti(HorizontalMovement, VerticleMovement);
        }
        else
        {
            HandleAnimations1D(HorizontalMovement, VerticleMovement);
            HandlePlayerOrientation(HorizontalMovement);
        }
    }

    private void HandleAnimations1D(float HorizontalMovement, float VerticleMovement)
    {
        bool MovementInputDetected = (HorizontalMovement != 0 || VerticleMovement != 0);
        if (MovementInputDetected)
        {
            PlayerAnimator.SetBool("isMoving", true);
        }
        else
        {
            PlayerAnimator.SetBool("isMoving", false);
        }

        HandleAttackAnimation(HorizontalMovement, VerticleMovement);
    }

    private void HandleAnimationsMulti(float HorizontalMovement, float VerticleMovement)
    {
        HandleAnimations1D(HorizontalMovement, VerticleMovement);
        PlayerAnimator.SetFloat("MoveHorizontal", HorizontalMovement);
        PlayerAnimator.SetFloat("MoveVertical", VerticleMovement);
        PlayerAnimator.SetFloat("MoveMagnitude", (float)Math.Sqrt((Math.Pow(HorizontalMovement,2)) + Math.Pow(VerticleMovement,2)));
    }

    void HandleAttackAnimation(float HorizontalMovement, float VerticleMovement)
    {
        if (Input.GetKey(KeyCode.Mouse0) && !disabled)
        {
            if (!PlayerAnimator.GetBool("isAttacking"))
            {
                PlayerAnimator.SetBool("isAttacking", true);
                if (playerAttack != null)
                {
                    playerAttack.Attack(HorizontalMovement, VerticleMovement);
                }
            }
            else
            {
                if (playerAttack != null)
                {
                    playerAttack.Attack(HorizontalMovement, VerticleMovement);
                }
            }
        }
        else
        {
            PlayerAnimator.SetBool("isAttacking", false);
            if (playerAttack != null)
            {
                playerAttack.StopAttack();
            }

        }

        if (isAudioEnabled && playerAudio != null)
        {
            HandleAttackAudio();
        }
    }

    void HandleJumpAnimation()
    {
        if (canJump)
        {
            if (isJumping)
            {
                PlayerAnimator.SetBool("isJumping", true);
            }
            else
            {
                PlayerAnimator.SetBool("isJumping", false);
            }
        }
    }

    #endregion

    #region Movement

    void HandleMovement(float HorizontalMovement, float VerticleMovement)
    {
        if (canJump)
        {
            VerticleMovement = 0;
        }
        Vector3 inputVector = new Vector3(HorizontalMovement, VerticleMovement, 0).normalized;
        float ScaledSpeed = PlayerMovementSpeed * Time.deltaTime;//Make this bad boi frame independent
        if (!canJump)
        {
            transform.Translate(inputVector * ScaledSpeed);
        }
        else
        {
            rb.velocity = new Vector2(HorizontalMovement * PlayerMovementSpeed, rb.velocity.y);
        }
    }

    void HandleJump()
    {
        if (useRayCastJumping && TryGetComponent(out Collider2D p_collider))
        {
            Vector3 temp = p_collider.bounds.center;
            //temp.y += p_collider.bounds.extents.y;
            RaycastHit2D hit = Physics2D.BoxCast(temp, p_collider.bounds.size, 0.0f, Vector2.down, rayLength, groundLayer);
            //Debug.DrawRay(temp, Vector2.down * rayLength, Color.red);
            //RaycastHit2D hit = Physics2D.Raycast(p_collider.bounds.center, Vector2.down, rayLength, groundLayer);
            if (hit.collider != null )
            {
                //Debug.Log("Hit the floor!");
                canRayCastJump = true;
                isJumping = false;
            }
        }
        else
        {
            if (rb.velocity.y < 0)
            {
                isJumping = true;
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                isJumping = true;
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

            if (rb.velocity.y == 0)
            {
                isJumping = false;
            }
        }
        if (PlayerAnimator != null)
        {
            HandleJumpAnimation();
        }
    }

    void HandlePlayerOrientation(float HorizontalVelocity)
    {
        if (HorizontalVelocity < 0)
        {
            SpriteRenderer PlayerVector = PlayerAnimator.gameObject.GetComponent<SpriteRenderer>();
            PlayerVector.flipX = !isFlipped;
        }
        else if (HorizontalVelocity > 0)
        {
            SpriteRenderer PlayerVector = PlayerAnimator.gameObject.GetComponent<SpriteRenderer>();
            PlayerVector.flipX = isFlipped;
        }
    }

    #endregion

    #region Audio

    void HandleAudio(float HorizontalMovement, float VerticleMovement)
    {
        if (isAudioEnabled && playerAudio!=null)
        {
            HandlePlayerMovementAudio(HorizontalMovement, VerticleMovement);
            //HandleAttackAudio();
            HandleDeathAudio();
        }
    }

    void HandlePlayerMovementAudio(float HorizontalMovement, float VerticleMovement)
    {
        if (PlayerAnimator.GetBool("isMoving"))
        {
            if (!playerAudio.WalkSource.isPlaying && playerAudio.WalkSource.clip != null)
            {
                playerAudio.WalkSource.Play();
            }
        }
        else
        {
            if (playerAudio.WalkSource.isPlaying && playerAudio.WalkSource.clip != null)
            {
                playerAudio.WalkSource.Stop();
            }
        }
    }

    void HandleAttackAudio()
    {
        if (PlayerAnimator.GetBool("isAttacking"))
        {
            if (!playerAudio.AttackSource.isPlaying && playerAudio.AttackSource.clip!=null)
            {
                playerAudio.AttackSource.Play();
            }
        }
        else
        {
            if (playerAudio.AttackSource.isPlaying && playerAudio.AttackSource.clip != null)
            {
                if (!PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
                {
                    //playerAudio.AttackSource.Stop();//Uncomment this if you have an audioclip that you want to cut short
                }
            }
        }
    }

    void HandleDeathAudio()
    {
        if (PlayerAnimator.GetBool("isDead"))
        {
            if (!playerAudio.DeathSource.isPlaying && playerAudio.DeathSource.clip != null)
            {
                playerAudio.DeathSource.Play();
            }
        }
        else
        {
            if (playerAudio.DeathSource.isPlaying && playerAudio.DeathSource.clip != null)
            {
                if (!PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("isDead"))
                {
                    playerAudio.DeathSource.Stop();
                }
            }
        }
    }

    void HandleJumpAudio()
    {
        if (isAudioEnabled && playerAudio != null)
        {
            playerAudio.JumpSource.Play();
        }
    }


    #endregion

    public void TimeToDie() //This isn't used in this script, but rather called elsewhere
    {
        StartCoroutine(deathOccurance());
    }

    IEnumerator deathOccurance()
    {
        if (gm != null && gsm != null)
        {
            PlayerAnimator.SetBool("isDead", true);
            disabled = true;
            yield return new WaitForSeconds(1f);
            StartCoroutine(gsm.FadeOut());

            yield return new WaitForSeconds(1f);
            gm.Respawn(gameObject);
            StartCoroutine(gsm.FadeIn());
            yield return new WaitForSeconds(1f);
            GetComponent<PlayerHealth>().ResetHealth();
            disabled = false;
            PlayerAnimator.SetBool("isDead", false);
        }
        else
        {
            Debug.Log("Game Manager or Game Scene Manager not assigned on player!");
        }
    }
}
