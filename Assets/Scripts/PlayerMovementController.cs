using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{


    public Animator animator;
    public Rigidbody rb;
    public Transform cam;

    public float speed = 180f;
    public float turnSmoothTime = 0.25f;
    public float RideHeight = 1f;
    public float RideSpringStrength = 500f;
    public float RideSpringDamper = 3f;
    public float getUpStrength = 1f;
    public float uprightDrag = 0.05f;
    public float flippedDrag = 100;
    public float JumpForce = 5000f;
    public float _uprightJointSpringDamper = 1;
    public float _uprightJointSpringStrength = 30;



    bool canJump = true;
    float turnSmoothVel;
    float desiredVel;
    bool isUpright;
    float fallingthreshold = -3f;
    Quaternion _uprightJointTargetRot;



    Vector3 moveDir = Vector3.zero;
    Quaternion characterCurrent;



    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        this.gameObject.transform.GetChild(1).position -= new Vector3(0f, RideHeight - (RideHeight - 0.75f), 0f);
        // this.gameObject.transform.GetChild(1).position -= new Vector3(0f, RideHeight - (RideHeight - 0f), 0f);

    }



    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;



        animator.SetFloat("speed", direction.magnitude);



        if (direction.magnitude >= 0.1f)
        {
            float current_x = transform.eulerAngles.x;
            float current_z = transform.eulerAngles.z;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);
            rb.rotation = Quaternion.Euler(current_x, angle, current_z);
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


        
    }


   

    void FixedUpdate()
    {

        // Debug.Log(isUpright);

        if (moveDir.magnitude >= 0.1f)
        {
            _uprightJointTargetRot = Quaternion.LookRotation(moveDir);
            rb.AddForce(moveDir.normalized * desiredVel);
        }
        //else if (moveDir.magnitude >= 0.1f && !isUpright)
        //{
        //    transform.Rotate(0f, 0f, moveDir.z * 100f);
        //}



        bool springRayDidHit = UpdateRideSpringForce();
        UpdateUprightSpringForce();

        

        if (rb.velocity.y < fallingthreshold)
        {
            animator.SetBool("isRising", false);
            animator.SetBool("isFalling", true);
            canJump = false;
        }
        animator.SetBool("isLanding", springRayDidHit);


        

        if (characterCurrent.eulerAngles.z > 20 && characterCurrent.eulerAngles.z < 340)
        {
            isUpright = true;
        }
        else isUpright = false;

        // Debug.Log(characterCurrent.eulerAngles.z);
        // Debug.Log(isUpright);


        rb.velocity *= 0.95f;
    }

    public bool UpdateRideSpringForce()
    {
        RaycastHit hit;
        Ray springRay = new Ray(transform.position, Vector3.down);
        bool springRayDidHit = Physics.Raycast(springRay, out hit, RideHeight);

        // Debug.Log(springRayDidHit);
        if (springRayDidHit)
        {
            Vector3 vel = rb.velocity;
            Vector3 rayDir = transform.TransformDirection(Vector3.down);
            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = hit.rigidbody;


            
            animator.SetBool("isFalling", false);
            canJump = true;



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
            // Debug.Log(rayDir * springForce);

            rb.AddForce(rayDir * springForce);


            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(rayDir * -springForce * 2f, hit.point);
            }


        }

        return springRayDidHit;
    }


    public void UpdateUprightSpringForce()
    {
        // cache current transform rotation
        characterCurrent = transform.rotation;



        // calculate shortest rotation between desired rot and current rot
        Quaternion toGoal = ShortestRotation(_uprightJointTargetRot, characterCurrent);
        Vector3 rotAxis;
        float rotDegrees;



        toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
        rotAxis.Normalize();
        float rotRadians = rotDegrees * Mathf.Deg2Rad;



        rb.AddTorque((rotAxis * (rotRadians * _uprightJointSpringStrength)) - (rb.angularVelocity * _uprightJointSpringDamper));
    }


    public void RollForward()
    {
        rb.AddTorque(new Vector3(300f, 0f, 0f));
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
