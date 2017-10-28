

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerController : MonoBehaviour
{

    public XboxController controller;
    private static bool didQueryNumOfCtrlrs = false;
    public float maxStamina;
    private float stamina;
    public float rechargeDelay;
    public float attackCost;
    private float rechargeTimer = 0;
    private float stepDelay = 0.4f;
    private float stepTimer = 0f;

    [SerializeField]
    float m_MovingTurnSpeed = 360;
    [SerializeField]
    float m_StationaryTurnSpeed = 180;
    [SerializeField]
    float m_MoveSpeedMultiplier = 1f;
    [SerializeField]
    public float fireRate;

    private float lastShot = -10.0f;
    Rigidbody m_Rigidbody;
    Animator m_Animator;
    bool m_IsGrounded;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    bool m_Crouching;
    static Animator anim;
    public GameObject firingPoint;
    public Projectile projectile;
    private Transform m_Cam;
    private Vector3 m_CamForward;
    private Vector3 m_Move;
    private Vector3 m_Aim;
    private bool isAiming;
    private bool m_Jump;
    private int playerNum;


    //Initialise objects and controllers
    //Determine if controllers are plugged in
    void Start()
    {
        stamina = maxStamina;

        switch (controller)
        {
            case XboxController.First: playerNum = 1; break;
            case XboxController.Second: playerNum = 2; break;
            case XboxController.Third: playerNum = 3; break;
            case XboxController.Fourth: playerNum = 4; break;
        }

        if(playerNum>XCI.GetNumPluggedCtrlrs())
        {
            Destroy(this.gameObject);
        }

        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        anim = GetComponent<Animator>();

        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
        }

        if (!didQueryNumOfCtrlrs)
        {
            didQueryNumOfCtrlrs = true;

            int queriedNumberOfCtrlrs = XCI.GetNumPluggedCtrlrs();

            if (queriedNumberOfCtrlrs == 1)
            {
                Debug.Log("Only " + queriedNumberOfCtrlrs + " Xbox controller plugged in.");
            }
            else if (queriedNumberOfCtrlrs == 0)
            {
                Debug.Log("No Xbox controllers plugged in!");
            }
            else
            {
                Debug.Log(queriedNumberOfCtrlrs + " Xbox controllers plugged in.");
            }
            
        }

    }

    //Return player stamina
    public float getStamina()
    {
        return stamina;
    }

    //Player shooting controls and stamina/mana change
    void Update()
    {
        rechargeTimer += Time.deltaTime;
        if(stamina<maxStamina && rechargeTimer>rechargeDelay)
        {
            stamina += Time.deltaTime/4;
        }
        /*Use Later
        print(anim.name);
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isAttacking"))
        if (XCI.GetButtonDown(XboxButton.A, controller) && !anim.GetBool("isAttacking"+playerNum))
        {
            anim.SetTrigger("isJumping");
        }
        
        if (XCI.GetButtonDown(XboxButton.B, controller))
        {
            audMan.PlaySound2D("Fireball");
        }
        */
        if (XCI.GetButtonDown(XboxButton.RightBumper, controller))
            {
                /*Use Later
                if(playerNum == 1)
                {
                    anim.SetTrigger("p1isAttacking");
                }
                else if(playerNum == 2)
                {
                    anim.SetBool("p2atk", true);
                    anim.SetFloat("test", 20);
                }
                */
           

            if (Time.time > (fireRate*2 + lastShot) && stamina > 0)
            {
                if(stamina < attackCost)
                {
                    stamina = 0;
                    rechargeTimer = 0;
                }
                else
                {
                    stamina -= attackCost;
                    rechargeTimer = 0;
                }

                anim.SetBool("isCasting", true);
                new WaitForSeconds(0.1f);
                Projectile p = Instantiate(projectile, firingPoint.transform.position, transform.rotation);
                AudioManager.instance.PlaySound("spell", p.transform.position);
                p.setPlayerNo(playerNum);
                lastShot = Time.time;
            }
        }

        /*Use Later
        if (XCI.GetButtonDown(XboxButton.LeftBumper, controller))
        {
            //anim.SetTrigger("isAOE");
            //anim.SetBool("isCasting", true);          
            
        }
        */
    }

    //return player number which is their controller number
    public int getPlayerNum()
    {
        return playerNum;
    }

    //Return controller
    public XboxController getController()
    {
        return controller;
    }

    //Moving player and its animations
    public void Move(Vector3 move)
    {
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, new Vector3(0, 0, 0));
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;
        m_Animator.SetBool("isCasting", false);

        if (move.magnitude > 0)
        {
            m_Animator.SetBool("isRunning", true);
            m_Animator.SetBool("isIdle", false);
            
            if(stepTimer < stepDelay)
            {
                stepTimer += Time.deltaTime;
            }
            if(stepTimer > stepDelay)
            {
                stepTimer = 0;
                AudioManager.instance.PlaySound("footsteps", transform.position);
            }
        }
        else
        {
            m_Animator.SetBool("isRunning", false);
            m_Animator.SetBool("isIdle", true);
        }

        if (!(m_Animator.GetCurrentAnimatorStateInfo(0).IsName("CastPrimary") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("CastAOE")))
        {            
             ApplyExtraTurnRotation(move);                    
        }
    }

    //Aim player which will be properly implemented later
    public void Aim(Vector3 move)
    {
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, new Vector3(0,0,0));
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;
        m_Animator.SetBool("isCasting", false);
        

        if (!(m_Animator.GetCurrentAnimatorStateInfo(0).IsName("CastPrimary") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("CastAOE")))
        {
            float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
            transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0); 
        }
    }

    void ApplyExtraTurnRotation(Vector3 move)
    {
        if (!isAiming)
        {
            float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
            transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
        }
        
        transform.Translate(move * Time.deltaTime * m_MoveSpeedMultiplier);
    }

    private void FixedUpdate()
    {
        float h = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        float v = XCI.GetAxis(XboxAxis.LeftStickY, controller);
        float ah = XCI.GetAxis(XboxAxis.RightStickX, controller);
        float av = XCI.GetAxis(XboxAxis.RightStickY, controller);

        if (m_Cam != null)
        {
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
            m_Aim = av * m_CamForward + ah * m_Cam.right;

        }
        else
        {
            m_Move = v * Vector3.forward + h * Vector3.right;
            m_Aim = av * Vector3.forward + ah * Vector3.right;
        }

        if(m_Aim.magnitude>0)
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }

        Move(m_Move);
        Aim(m_Aim);
    }
    

}
