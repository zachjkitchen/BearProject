 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{


    public Animator animator;
    public Rigidbody rb;
    public Transform cam;

    public float speed = 9000f;
    public float RideHeight = 1f;
    public float RideSpringStrength = 500f;
    public float RideSpringDamper = 3f;
    public float JumpForce = 5000f;
    public float _uprightJointSpringDamper = 1;
    public float _uprightJointSpringStrength = 10;



    bool canJump = true;
    public float turnSmoothTime = 0.25f;
    float turnSmoothVel;
    float desiredVel;
    bool characterUpright;
    float fallingthreshold = -3f;
    public Quaternion _uprightJointTargetRot = new Quaternion(0f, 0f, 0f, 1f);

    Vector3 moveDir = Vector3.zero;





    // Start is called before the first frame update
    void Start()
    {

        // StartCoroutine(WaitForSeconds);

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        this.gameObject.transform.GetChild(1).position -= new Vector3(0f, RideHeight - (RideHeight - 0.75f), 0f);
        // this.gameObject.transform.GetChild(1).position -= new Vector3(0f, RideHeight - (RideHeight - 0f), 0f);

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray springRay = new Ray(transform.position, Vector3.down);
        bool springRayDidHit = Physics.Raycast(springRay, out hit, RideHeight);
        

        if (springRayDidHit)
        {
            Vector3 vel = rb.velocity;
            Vector3 rayDir = transform.TransformDirection(Vector3.down);

            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = hit.rigidbody;

            


            

            if (hitBody != null)
            {
                otherVel = hitBody.velocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = hit.distance - RideHeight;

            float springForce = (x * RideSpringStrength) - (relVel * RideSpringDamper);

     
            Debug.DrawLine(transform.position, transform.position + (rayDir * springForce), Color.red);
            Debug.DrawLine(transform.position, transform.position + rayDir, Color.red);

            rb.AddForce(rayDir * springForce);


            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(rayDir * -springForce, hit.point);
            }

        }



        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        

        

        animator.SetFloat("speed", direction.magnitude);


        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);
            rb.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            desiredVel = speed;
            
        }
        else
        {
            desiredVel = 0;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canJump)
            {
                canJump = false;
                rb.AddForce(new Vector3(0f, JumpForce, 0f));
                animator.SetBool("isRising", true);
                
            }
            
        }


        // Debug.Log(springRayDidHit);

        animator.SetBool("isLanding", false);



        if (rb.velocity.y < fallingthreshold)
        {
            animator.SetBool("isRising", false);
            animator.SetBool("isFalling", true);
            canJump = false;
        }

        if (springRayDidHit)
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("isLanding", true);
            canJump = true;
        }




    }


   

    void FixedUpdate()
    {

        if (moveDir.magnitude >= 0.1f)
            
        {
            rb.AddForce(moveDir.normalized * desiredVel * Time.fixedDeltaTime);
        }


        Quaternion characterCurrent = transform.rotation;
        _uprightJointTargetRot = Quaternion.LookRotation(moveDir);


        // calculate shortest rotation between desired rot and current rot
        Quaternion toGoal = ShortestRotation(_uprightJointTargetRot, characterCurrent);
        // Quaternion toGoal = new Quaternion(1f, 0f, 0f, 0f);
        Vector3 rotAxis;
        float rotDegrees;


        toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
        rotAxis.Normalize();
        float rotRadians = rotDegrees * Mathf.Deg2Rad;


        rb.AddTorque((rotAxis * (rotRadians * _uprightJointSpringStrength)) - (rb.angularVelocity * _uprightJointSpringDamper));



        // Debug.Log(rotRadians);

        rb.velocity *= 0.95f;

        
    }


    public static Quaternion ShortestRotation(Quaternion a, Quaternion b)

    {

        if (Quaternion.Dot(a, b) < 0)

        {

            return a * Quaternion.Inverse(Multiply(b, -1));

        }

        else return a * Quaternion.Inverse(b);

    }



    public static Quaternion Multiply(Quaternion input, float scalar)

    {

        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);

    }



}
