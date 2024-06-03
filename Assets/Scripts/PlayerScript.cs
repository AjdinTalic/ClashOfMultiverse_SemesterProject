using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private float p1Horizontal;
    private float p2Horizontal;
    
    [SerializeField] private float moveSpeed;
    private float currentMoveSpeed;

    private float attackPeriod = 1f;

    [SerializeField] private float jump = 3f;
    [SerializeField] private float fallAccellaration = 1.5f;
    [SerializeField] public float maxVitality = 10000;
    [SerializeField] private float maxParryMeter = 4000;
    [SerializeField] private float maxBurnoutTime = 500;
    [SerializeField] public float currentVitality;
    [SerializeField] private float currentParryMeter;
    [SerializeField] private float currentBurnoutTime;
    [SerializeField] private float parryDrain;
    [SerializeField] private float timeAdd;

    [SerializeField] public Image vitalityBar;
    [SerializeField] private Image parryMeter;
    [SerializeField] private Image timeBar;

    [SerializeField] private GameObject midPos;

    private Vector2 gravityVector;

    private int motionInputIndex = 0;

    private bool standBlocking;
    private bool crouchBlocking;
    private bool isParrying;
    private bool isBurnout;

    private float scaleX;

    private readonly AttacksDataScript _attackStats = new AttacksDataScript();
    // Start is called before the first frame update
    void Start()
    {
        scaleX = transform.localScale.x;
        gravityVector = new Vector2(0, -Physics2D.gravity.y);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
        currentVitality = maxVitality;
        currentParryMeter = maxParryMeter;
        currentBurnoutTime = 0;
        vitalityBar.fillAmount = currentVitality / maxVitality;
        parryMeter.fillAmount = currentParryMeter / maxParryMeter;
        timeBar.fillAmount = currentBurnoutTime / maxBurnoutTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!anim.GetBool("isCrouching") &&
            (!anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_Standing_light") &&
             !anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_Standing_medium") &&
             !anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_Standing_heavy") &&
             !anim.GetCurrentAnimatorStateInfo(0).IsName("lightAttackHit") &&
             !anim.GetCurrentAnimatorStateInfo(0).IsName("mediumAttackHit") &&
             !anim.GetCurrentAnimatorStateInfo(0).IsName("heavyAttackHit") && 
             !anim.GetCurrentAnimatorStateInfo(0).IsName("lightBlockHit") &&
             !anim.GetCurrentAnimatorStateInfo(0).IsName("mediumBlockHit") &&
             !anim.GetCurrentAnimatorStateInfo(0).IsName("heavyBlockHit") &&
             !anim.GetCurrentAnimatorStateInfo(0).IsName("lightCrouchBlockHit") &&
             !anim.GetCurrentAnimatorStateInfo(0).IsName("mediumCrouchBlockHit") &&
             !anim.GetCurrentAnimatorStateInfo(0).IsName("heavyCrouchBlockHit")) &&
            gameObject.CompareTag("Player"))
        {
            currentMoveSpeed = moveSpeed;
            p1Horizontal = Input.GetAxis("Horizontal");
        }
        else if (!anim.GetBool("isCrouching") &&
                 (!anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_Standing_light") &&
                  !anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_Standing_medium") &&
                  !anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_Standing_heavy") &&
                  !anim.GetCurrentAnimatorStateInfo(0).IsName("lightAttackHit") &&
                  !anim.GetCurrentAnimatorStateInfo(0).IsName("mediumAttackHit") &&
                  !anim.GetCurrentAnimatorStateInfo(0).IsName("heavyAttackHit") &&
                  !anim.GetCurrentAnimatorStateInfo(0).IsName("lightBlockHit") &&
                  !anim.GetCurrentAnimatorStateInfo(0).IsName("mediumBlockHit") &&
                  !anim.GetCurrentAnimatorStateInfo(0).IsName("heavyBlockHit") &&
                  !anim.GetCurrentAnimatorStateInfo(0).IsName("lightCrouchBlockHit") &&
                  !anim.GetCurrentAnimatorStateInfo(0).IsName("mediumCrouchBlockHit") &&
                  !anim.GetCurrentAnimatorStateInfo(0).IsName("heavyCrouchBlockHit")) &&
                 gameObject.CompareTag("Player2"))
        {
            currentMoveSpeed = moveSpeed;
            p2Horizontal = Input.GetAxis("Horizontal2");
        }
        else
        {
            currentMoveSpeed = 0;
        }

        if (midPos.transform.position.x > gameObject.transform.position.x &&
            scaleX < 0)
        {
            FLipPlayer();
        }
        else if (midPos.transform.position.x <= gameObject.transform.position.x &&
                 scaleX > 0)
        {
            FLipPlayer();
        }
        
        if ((gameObject.CompareTag("Player") && Input.GetKey(KeyCode.S)) ||
            (gameObject.CompareTag("Player2") && Input.GetKey(KeyCode.DownArrow)) && IsGrounded())
        {
            anim.SetBool("isCrouching", true);
        }
        else
        {
            anim.SetBool("isCrouching", false);
        }

        if ((((Input.GetKey(KeyCode.A) && gameObject.CompareTag("Player")  &&
               anim.GetBool("isCrouching")) ||
              (Input.GetKey(KeyCode.LeftArrow) && gameObject.CompareTag("Player2")  &&
               anim.GetBool("isCrouching"))) && scaleX > 0) ||
            (((Input.GetKey(KeyCode.D) && gameObject.CompareTag("Player")  &&
               anim.GetBool("isCrouching")) ||
              (Input.GetKey(KeyCode.RightArrow) && gameObject.CompareTag("Player2")  &&
               anim.GetBool("isCrouching"))) && scaleX < 0) && IsGrounded())
        {
            standBlocking = false;
            crouchBlocking = true;
        }
        else if ((((Input.GetKey(KeyCode.A) && gameObject.CompareTag("Player")) ||
                   (Input.GetKey(KeyCode.LeftArrow) && gameObject.CompareTag("Player2"))) && scaleX > 0) ||
                 (((Input.GetKey(KeyCode.D) && gameObject.CompareTag("Player")) ||
                   (Input.GetKey(KeyCode.RightArrow) && gameObject.CompareTag("Player2"))) && scaleX < 0) &&
                 IsGrounded())
            
        {
            standBlocking = true;
            crouchBlocking = false;
        }
        else
        {
            standBlocking = false;
            crouchBlocking = false;
            
        }
            
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_Standing_light") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_Standing_medium") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_Standing_heavy") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_CrouchToStanding") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_StandingToCrouch") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_Crouch_light") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_Crouch_medium") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_Crouch_heavy") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("lightAttackHit") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("mediumAttackHit") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("heavyAttackHit"))
        {
            standBlocking = false;
            crouchBlocking = false;
        }

        if ((gameObject.CompareTag("Player") && Input.GetKey(KeyCode.I) && currentParryMeter > 0 &&
             currentParryMeter < maxParryMeter && IsGrounded()) || gameObject.CompareTag("Player2") &&
            Input.GetKey(KeyCode.Keypad5) && currentParryMeter > 0 && currentParryMeter < maxParryMeter && IsGrounded())
        {
            isParrying = true;
            gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            isParrying = false;

            if (gameObject.CompareTag("Player"))
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
            }
            else if (gameObject.CompareTag("Player2"))
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        if (currentParryMeter <= 0)
        {
            isBurnout = true;
        }

        if ((gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.J)) ||
            (gameObject.CompareTag("Player2") && Input.GetKeyDown(KeyCode.Keypad1)))
        {
            anim.SetTrigger("lightAttack");
        }
        
        if ((gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.K)) ||
            (gameObject.CompareTag("Player2") && Input.GetKeyDown(KeyCode.Keypad2)))
        {
            anim.SetTrigger("mediumAttack");
        }
        
        if ((gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.L)) ||
            (gameObject.CompareTag("Player2") && Input.GetKeyDown(KeyCode.Keypad3)))
        {
            anim.SetTrigger("heavyAttack");
        }

        if (((Input.GetKey(KeyCode.W) && gameObject.CompareTag("Player")) ||
             (Input.GetKey(KeyCode.UpArrow) && gameObject.CompareTag("Player2"))) && IsGrounded() &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_Standing_light") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_Standing_medium") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("TestPlayer_Standing_heavy") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("lightAttackHit") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("mediumAttackHit") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("heavyAttackHit") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("lightBlockHit") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("mediumBlockHit") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("heavyBlockHit") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("lightCrouchBlockHit") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("mediumCrouchBlockHit") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("heavyCrouchBlockHit"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
        }
        
        if (rb.velocity.y < 0)
        {
            rb.velocity -= gravityVector * fallAccellaration * Time.deltaTime;
        }

        if (currentVitality <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        CheckForAttack(col, "KayoLightAttack");
        CheckForAttack(col, "KayoMediumAttack");
        CheckForAttack(col, "KayoHeavyAttack");
        CheckForAttack(col, "KayoCrouchLightAttack");
        CheckForAttack(col, "KayoCrouchMediumAttack");
        CheckForAttack(col, "KayoCrouchHeavyAttack");
    }

    private void FixedUpdate()
    {
        if (gameObject.CompareTag("Player"))
        {
            rb.velocity = new Vector2(p1Horizontal * currentMoveSpeed * Time.deltaTime, rb.velocity.y );
        }
        else if (gameObject.CompareTag("Player2"))
        {
            rb.velocity = new Vector2(p2Horizontal * currentMoveSpeed * Time.deltaTime, rb.velocity.y );
        }
        
        if (isParrying)
        {
            currentParryMeter -= parryDrain;
            parryMeter.fillAmount = currentParryMeter / maxParryMeter;
        }
        else
        {
            currentParryMeter -= parryDrain / 5f;
            parryMeter.fillAmount = currentParryMeter / maxParryMeter;
        }

        if (isBurnout)
        {
            currentBurnoutTime += timeAdd;
            timeBar.fillAmount = currentBurnoutTime / maxBurnoutTime;

            if (currentBurnoutTime == maxBurnoutTime)
            {
                currentParryMeter += maxParryMeter * 0.75f;
                isBurnout = false;
                currentBurnoutTime = 0;
                timeBar.fillAmount = currentBurnoutTime / maxBurnoutTime;
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.05f, groundLayer);
    }

    void CheckForAttack(Collider2D col, string attackName)
    {
        if (col.gameObject.CompareTag(attackName))
        {
            if (!_attackStats.attacks[attackName].crouchAttack)
            {
                if (!standBlocking && !isParrying)
                {
                    DamagePlayer(attackName);
                    anim.SetTrigger(_attackStats.attacks[attackName].attackRecovery);
                }
                else if (crouchBlocking && !isParrying)
                {
                    DamagePlayer(attackName);
                    anim.SetTrigger(_attackStats.attacks[attackName].attackRecovery);
                }
                else if (isParrying)
                {
                    DrainParryMeter(attackName);
                }
                else
                {
                    anim.SetTrigger(_attackStats.attacks[attackName].blockRecovery);
                }
            }
            else if (_attackStats.attacks[attackName].crouchAttack)
            {
                if (!crouchBlocking && !isParrying)
                {
                    DamagePlayer(attackName);
                    anim.SetTrigger(_attackStats.attacks[attackName].attackRecovery);
                }
                else if (standBlocking && !isParrying)
                {
                    DamagePlayer(attackName);
                    anim.SetTrigger(_attackStats.attacks[attackName].attackRecovery);
                }
                else if (isParrying)
                {
                    DrainParryMeter(attackName);
                }
                else
                {
                    anim.SetTrigger(_attackStats.attacks[attackName].blockRecovery);
                }
            }
        }
    }

    private void DamagePlayer(string attackName)
    {
        currentVitality -= _attackStats.attacks[attackName].damage;
        vitalityBar.fillAmount = currentVitality / maxVitality;
    }

    private void DrainParryMeter(string attackName)
    {
        currentParryMeter += _attackStats.attacks[attackName].damage;
        parryMeter.fillAmount = currentParryMeter / maxParryMeter;
    }

    void FLipPlayer()
    {
        scaleX *= -1;
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
    }
}
