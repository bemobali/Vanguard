using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using MyStuff = Assets.Vanguard.Script;

//Vanguard implements MonoBehaviour functions and interacts with the game engine.
//Collaborates with a Controller instance to move the Vanguard character in the game map
public class Vanguard : MonoBehaviour
{
    #region Public Members
    //Tweakable object properties
    //Express walking speed in m/s
    //[SerializeField] //not necessary for public member variables
    public float walkingSpeed;
    //runningSpeed is in m/s. Used by the 
    public float runningSpeed;
    //jump force vertical component
    public float jumpForce;
    //Left foot and right foot. Pass this to the MyStuff.Controller instance to toggle foot step
    public GameObject leftFoot, rightFoot;
    //Jumping and landing sound. Stays here because the collider belongs to the Vanguard unit
    public AudioSource jumpingSound, landingSound;
    public float tiltSensitivity;
    public float panSensitivity;
    public GameObject spine;
    //Jump direction uses Hip's XZ axis
    public GameObject hips;
    //First person camera object. Need this because during cam swaps, the AudioListener must deactivate also
    public GameObject fpsCameraObj;
    //Third person camera. Need this because during cam swaps, the AudioListener must deactivate also
    public GameObject tpsCameraObj;
    //I don't think I need the game object. Just the camera
    public GameObject deathCamera;
    public GameObject HUD;
    //Point in space where the head is looking at
    public GameObject headLookAt;
    //Target attach point to IK the left hand
    public GameObject leftHandAttach;
    //Target attach point to IK the right hand
    public GameObject rightHandAttach;
    //public GameObject[] floorRayCast = new GameObject[4];
    public GameObject activeWeapon;
    #endregion

    //Vanguard movement animator
    Animator animator;
    Rigidbody rigidBody;
    Camera fpsCamera;
    //overall health of the player
    Health health;
    //main body battle damage. 
    BattleDamage bodyDamage;
    //Weapon interface. @todo standardize the interface
    M4Shotgun shotgun;
    //Vanguard ragdoll. How is it that GameObject does not have a function to find a child by name, but can find all of its children's components?
    //public GameObject ragDoll;
    Dead deathSequence;

    //When initialing jump, the player moves in the direction of the lateralMovementVector. This way, the user cannot swirl the mouse around to change the jump direction.
    Vector3 lateralMovementVector;

    MyStuff.Controller controller;
    MyStuff.AnimationContext animationContext;
    MyStuff.MovementContext movementContext;

    //tilt angle accumulator;
    //float tiltAccum = 0f;
    bool groundContact;

    void AssignGameComponents()
    {
        animator = GetComponent<Animator>();
        //establish the observer relationship
        controller = new MyStuff.Controller(this);
        animationContext = new MyStuff.AnimationContext(animator);
        movementContext = new MyStuff.MovementContext(this);
        lateralMovementVector = new Vector3();
        rigidBody = GetComponent<Rigidbody>();
        groundContact = true;
        fpsCamera = fpsCameraObj.GetComponent<Camera>();
        health = GetComponent<Health>();
        bodyDamage = GetComponent<BattleDamage>();
        shotgun = activeWeapon.GetComponent<M4Shotgun>();
        deathSequence = GetComponent<Dead>();
    }

    #region BuiltIn Functions
    // Start is called before the first frame update
    void Start()
    {
        AssignGameComponents();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Vanguard is dead, so all bets are off
        if (health.HealthPoint < 1f)
        {
            Debug.Log("Killing the player");
            this.enabled = false;
            deathSequence.enabled = true;
            return;
        }
        movementContext.Update(Time.deltaTime);
        // Create a vector at the center of our camera's viewportes
        if (controller.Shoot())
		{
            if (shotgun.Fire()) animationContext.Firing();
		}

        if (controller.Esc())
		{
            if (Cursor.lockState != CursorLockMode.Locked) Cursor.lockState = CursorLockMode.Locked;
            else Cursor.lockState = CursorLockMode.None;
		}
    }

    void FixedUpdate()
    {
        movementContext.FixedUpdate(Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        //override rig animation transform here.
        movementContext.LateUpdate();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Terrain")
        {
            //Debug.Log("Contact with terrain");
            groundContact = true;
            movementContext.OnGroundCollisionEnter();
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.name == "Terrain")
        {
            //Debug.Log("No contact with terrain");
            groundContact = false;
            movementContext.OnGroundCollisionExit();
        }
    }

    //Constraints gets resolved AFTER IK resolution, or so it seems
    void OnAnimatorIK(int layerIndex)
    {
        // Set the look target position, if one has been assigned
        if (headLookAt != null)
        {
            animator.SetLookAtWeight(1);
            //This turns the head
            animator.SetLookAtPosition(headLookAt.transform.position);
        }
        if (rightHandAttach != null)
        {
            //The IKGoal is limited to feet and hand. So how do I IK turn the whole spine?
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandAttach.transform.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandAttach.transform.rotation);
            //animator.MatchTarget(rightHandAttach.transform.position, rightHandAttach.transform.rotation, AvatarTarget.RightHand, new MatchTargetWeightMask(Vector3.one, 1f), 0f, 1f);
        }
        //Do this only on animations that require the left hand on the weapon. Do not do this while reloading
        if (leftHandAttach != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandAttach.transform.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandAttach.transform.rotation);
            //animator.MatchTarget(leftHandAttach.transform.position, leftHandAttach.transform.rotation, AvatarTarget.LeftHand, new MatchTargetWeightMask(Vector3.one, 1f), 0f, 1f);
        }
    }
    #endregion
    /*
    void EnableRagdollPhysics()
	{
        Rigidbody [] rb = gameObject.GetComponentsInChildren<Rigidbody>();
        Collider[] col = gameObject.GetComponentsInChildren<Collider>();
        foreach(Rigidbody rigid in rb)
		{
            rigid.isKinematic = false;
		}

        foreach (Collider collide in col)
		{
            collide.isTrigger = false;
		}
	}
    void Dead()
	{
        LookAtConstraint constraint = fpsCamera.GetComponent<LookAtConstraint>();
        if (constraint != null)
		{
            constraint.enabled = false;
		}

        LookAtConstraint chestLookAt = spine.GetComponent<LookAtConstraint>();
        if (chestLookAt != null)
		{
            chestLookAt.enabled = false;
		}
        
        animationContext.Dead();
    }

    //Call this from inside the Dead animation state
    public void switchToDeathCamera()
	{
        HUD.SetActive(false);
        fpsCameraObj.SetActive(false);
        deathCamera.SetActive(true);
        //Enable physics
        EnableRagdollPhysics();
    }
    */
    private void LateralMove(float sideways, float forward, float movementSpeed, float deltaT)
    {
        //These approaches do not work
        //lateralMovementVector = (this.transform.forward * forward + this.transform.right * sideways) * movementSpeed * deltaT;
        //lateralMovementVector = (hips.transform.forward * forward + hips.transform.right * sideways) * movementSpeed * deltaT;
        lateralMovementVector = (fpsCamera.transform.forward * forward + fpsCamera.transform.right * sideways) * movementSpeed * deltaT;

        //position transform is taken care of by the root animation, so dont do this
        //this.transform.position += lateralMovementVector;
    }

    public Vector3 LateralMovementVector()
    {
        return lateralMovementVector;
    }

    //Need unit test
    public void Run(float sideways, float forward, float deltaT)
    {
        //We speed up the animation
        LateralMove(sideways, forward, runningSpeed, deltaT);
        animationContext.running(sideways, forward);
        ActivateFootstepSound();
    }

    public void Walk(float sideways, float forward, float deltaT)
    {
        //float z = 0.75f;    //based on experimental observation
        //float x = 0.75f;    //based on experimental observation
        //@note using the root animation motion
        //@todo Fix jump. Learn the proper way to jump
        LateralMove(sideways, forward, walkingSpeed, deltaT);
        animationContext.walking(sideways, forward);
        ActivateFootstepSound();
    }

    public void Stay()
    {
        animationContext.Stay();

    }

    public void Pan(float angle)
    {
        this.transform.localRotation *= Quaternion.Euler(0, angle, 0);
    }

    /*//This was the tilt to rotate the spine around its x-axis. Too difficult right now for me to get right. Then I saw the Full Body FPS tutorial with the head stabilizer topic.
    public void Tilt(float angle)
    {
        tiltAccum += angle;
        spine.transform.localRotation *= Quaternion.Euler(-tiltAccum, 0, 0);
    }
    */
    public void Tilt(float angle)
    {
        //Adjust the height of HeadLookAt based on the mouse input
        headLookAt.transform.position += new Vector3(0, angle, 0);
    }

    //Jump action by Vanguard unit
    public void Jump()
    {
        rigidBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        //Challenge here is to sync the animation with jumping
        animationContext.Jumping();
        ActivateJumpSound();
    }

    //Landing action from Jumping
    public void LandingFromJumping()
    {
        //So far this is the only action I need to do.
        landingSound.Play();
    }

    public bool IsGrounded()
    {
        return groundContact;
    }

    public void SwitchToFirstPersonCamera()
	{
        if (!fpsCameraObj.activeSelf) fpsCameraObj.SetActive(true);
        if (tpsCameraObj.activeSelf) tpsCameraObj.SetActive(false);
	}

    public void SwitchToThirdPersonCamera()
	{
        if (fpsCameraObj.activeSelf) fpsCameraObj.SetActive(false);
        if (!tpsCameraObj.activeSelf) tpsCameraObj.SetActive(true);
    }

    public MyStuff.Controller Controller()
    {
        return controller;
    }

    //Activate footstep sound. Deactivate the footsteps when jumping
    void ActivateFootstepSound()
    {
        leftFoot.GetComponent<Footstep>().walkOrRun();
        rightFoot.GetComponent<Footstep>().walkOrRun();
    }

    void ActivateJumpSound()
    {
        leftFoot.GetComponent<Footstep>().jumping();
        rightFoot.GetComponent<Footstep>().jumping();
        if (!jumpingSound.isPlaying) jumpingSound.Play();
    }
}

//Spine manipulation must be done in LateUpdate as an accumulation of a rotation angle. This accumulated value must be continuously applied. 
//Otherwise the bone animation will just overwrite the rotation angle in the next frame
//RigidBody attached to the Vanguard_Prefab with a capsule collider. This is a requirement. To stop the ragdoll colliders from pushing the Vanguard_prefab collider, make these colliders trigger colliders
//Good enough for inflicting battle damages for now.
//I think I will need a jump state manager. If user initiates the jump, then turn on parent collider and rigidbody to initiate the jump. On the ground, the Character Controller and NavMeshAgent takes over