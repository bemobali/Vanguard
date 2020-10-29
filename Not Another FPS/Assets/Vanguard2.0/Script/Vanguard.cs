using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    //Point in space where the head is looking at
    public GameObject headLookAt;
    //public GameObject[] floorRayCast = new GameObject[4];
    public GameObject activeWeapon;
    #endregion

    //Vanguard movement animator
    Animator animator;
    Rigidbody rigidBody;
    Camera fpsCamera;
    //Camera that is currently rendering. Can be fpsCameraObj, tpsCameraObj, and deathCamera if necessary
    [SerializeField]
    GameObject activeCamera;
    //overall health of the player
    Health health;
    //@note use uint for the shotgun shell for now. Convert to a shotgun shell class once I figure out how to swap sprites.
    //Carry only 4 boxes
    const uint m_maxShotgunShellToCarry = 100;
    [SerializeField, Range(1, m_maxShotgunShellToCarry)]
    uint m_shotgunShellInventory = 1;
   
    //At some point the player script needs to update the HUD. So the HUD here is an observer
    [SerializeField]
    HUD m_hud;

    //main body battle damage. 
    BattleDamage bodyDamage;
    //Weapon interface. @todo standardize the interface
    M4Shotgun m_shotgun;
    //Vanguard ragdoll. How is it that GameObject does not have a function to find a child by name, but can find all of its children's components?
    //public GameObject ragDoll;
    Dead deathSequence;
    //Target attach point to IK the left hand
    GameObject leftHandAttach;
    //Target attach point to IK the right hand
    GameObject rightHandAttach;
    //If true, hand IK will be active. False otherwise
    bool m_enableIK;
    public bool EnableIK
	{
		get { return m_enableIK; }
        set { m_enableIK = value; }
	}
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
        AssignWeapon();
        deathSequence = GetComponent<Dead>();
        m_enableIK = true;
        m_hud.SetAmmoInventory(m_shotgunShellInventory);
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
            if (m_shotgun.Fire()) animationContext.Firing();
		}

        if (controller.Esc())
		{
            if (Cursor.lockState != CursorLockMode.Locked) Cursor.lockState = CursorLockMode.Locked;
            else Cursor.lockState = CursorLockMode.None;
		}

        if (controller.PickUpWeapon())
		{
            PickUpWeapon();
		}

        if (controller.Reload())
		{
            ReloadWeapon();
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

        if (!m_enableIK) return;
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
        if (!fpsCameraObj.activeSelf)
        {
            fpsCameraObj.SetActive(true);
        }
        if (tpsCameraObj.activeSelf) tpsCameraObj.SetActive(false);
        activeCamera = fpsCameraObj;
        //This should reach m_shotgun
        SendMessage("SetActiveCamera", activeCamera);

    }

    public void SwitchToThirdPersonCamera()
	{
        if (fpsCameraObj.activeSelf) fpsCameraObj.SetActive(false);
        if (!tpsCameraObj.activeSelf)
        {
            tpsCameraObj.SetActive(true);
        }
        activeCamera = tpsCameraObj;
        SendMessage("SetActiveCamera", activeCamera);
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

    void AssignWeapon()
	{
        m_shotgun = activeWeapon.GetComponentInChildren<M4Shotgun>();
        m_shotgun.SetActiveCamera(activeCamera.GetComponent<Camera>());
        m_shotgun.SetEnableBallistics(true);
        m_shotgun.RefreshHUD();
        Weapon weaponScript = activeWeapon.GetComponent<Weapon>();
        rightHandAttach = weaponScript.RightHandAttach;
        leftHandAttach = weaponScript.LeftHandAttach;
    }

    void PickUpWeapon()
	{
        Camera currentCam = activeCamera.GetComponent<Camera>();
        Vector3 lineOrigin = currentCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hitTarget;
        //This is unreal, but maybe aesthetically OK?
        const float pickupRange = 2.5f;
        // Check if our raycast has hit anything
        bool hasPickup = Physics.Raycast(lineOrigin, activeCamera.transform.forward, out hitTarget, pickupRange);
        if (hasPickup && (hitTarget.collider.gameObject.tag == "Weapon"))
		{
            Debug.Log("Picking up weapon " + hitTarget.collider.gameObject.ToString());
            GameObject attachPoint = hitTarget.collider.gameObject.transform.parent.gameObject;            
            Transform dropSpot = attachPoint.transform;
            //@todo make this a function call somewhere in the weapon script
            GameObject weaponToDrop = activeWeapon.transform.gameObject;
            GameObject hand = weaponToDrop.transform.parent.gameObject;

            //swap weapon
            weaponToDrop.transform.parent = null;
            weaponToDrop.transform.position = dropSpot.position;
            weaponToDrop.transform.rotation = dropSpot.rotation;
            m_shotgun.SetEnableBallistics(false);

            attachPoint.transform.SetParent(hand.transform,false);
            attachPoint.transform.position = hand.transform.position;
            attachPoint.transform.rotation = hand.transform.rotation;
            attachPoint.transform.localPosition.Set(0f, 0f, 0f);
            attachPoint.transform.localRotation.eulerAngles.Set(0f, 0f, 0f);
            
            activeWeapon = attachPoint;
            AssignWeapon();
		}
    }

    //Begin the reload animation. There are quite a number of IK to be done during the sequence, and I will be using events in the reload animation to change the
    //firearm magazine using IK. So the start sequence is called here, and event handles will be defined elsewhere
    public void StartReloadAnimation()
	{
        //dont bother starting the reload sequence if there is nothing to reload 
        if (m_shotgunShellInventory == 0) return;
        //Disable inverse kinematics
        //@note still in progress
        m_enableIK = false;
        animationContext.Reload();
        //@todo set an event to re-enable IK. Although I think the IK should be on, but the left hand attach should go to the magazine
    }

    //Max the bullet in the current active weapon. In this case, currently just a m_shotgun
    public void ReloadWeapon()
	{
        //For now. Later this will be called as an animation event.
        if (m_shotgunShellInventory == 0) return;
        uint numBullets = System.Math.Min(m_shotgunShellInventory, m_shotgun.MaxCapacity - m_shotgun.RoundsRemaining);
        m_shotgun.Reload(numBullets);
        m_shotgunShellInventory -= numBullets;
        m_hud.SetAmmoInventory(m_shotgunShellInventory);
       
	}

    //GrabAmmo returns how much unconsumed ammo left. Definitely move this job over to an inventory system
    public uint GrabAmmo(uint available)
	{
        //This should bottom out to 0 without underflowing
        uint howManytoGrab = System.Math.Min(available, m_maxShotgunShellToCarry - m_shotgunShellInventory);
        Debug.Log("Player grabbing " + howManytoGrab.ToString() + " ammo");
        if (howManytoGrab == 0) return available;

        m_shotgunShellInventory += howManytoGrab;
        m_hud.SetAmmoInventory(m_shotgunShellInventory);
        return available - howManytoGrab;
    }
}

//Spine manipulation must be done in LateUpdate as an accumulation of a rotation angle. This accumulated value must be continuously applied. 
//Otherwise the bone animation will just overwrite the rotation angle in the next frame
//RigidBody attached to the Vanguard_Prefab with a capsule collider. This is a requirement. To stop the ragdoll colliders from pushing the Vanguard_prefab collider, make these colliders trigger colliders
//Good enough for inflicting battle damages for now.
//I think I will need a jump state manager. If user initiates the jump, then turn on parent collider and rigidbody to initiate the jump. On the ground, the Character Controller and NavMeshAgent takes over