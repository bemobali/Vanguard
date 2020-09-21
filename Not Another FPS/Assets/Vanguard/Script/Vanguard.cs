using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyStuff = Assets.Vanguard.Script;

//Vanguard implements MonoBehaviour functions and interacts with the game engine.
//Collaborates with a Controller instance to move the Vanguard character in the game map
public class Vanguard : MonoBehaviour
{

    //Tweakable object properties
    //Express walking speed in m/s
    public float walkingSpeed;
    //runningSpeed is in m/s. Used by the 
    public float runningSpeed;
    //jump force vertical component
    public float jumpForce;

    Vector3 lateralMovementVector;

    //@todo Constrain from 0 to 1
    public float tiltSensitivity;
    //@todo Constrain from 0 to 1
    public float panSensitivity;

    //Vanguard movement animator
    Animator animator;
    Rigidbody rigidBody;
    //Vanguard ragdoll. How is it that GameObject does not have a function to find a child by name, but can find all of its children's components?
    //public GameObject ragDoll;
    //Tilt rotation is to be done on the spine.
    GameObject spine;

    MyStuff.Controller controller;
    MyStuff.AnimationContext animationContext;
    MyStuff.MovementContext movementContext;

    //tilt angle accumulator;
    float tiltAccum = 0f;
    bool groundContact;

    void assignGameComponents()
	{
        animator = GetComponent<Animator>();
        //establish the observer relationship
        controller = new MyStuff.Controller(this);
        animationContext = new MyStuff.AnimationContext(animator);
        movementContext = new MyStuff.MovementContext(this);
        lateralMovementVector = new Vector3();
        //@note this is a hack! Find the spine from the ragDoll object.
        spine = GameObject.Find("mixamorig:Spine1");
        rigidBody = GetComponent<Rigidbody>();
        groundContact = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        assignGameComponents();
    }

    // Update is called once per frame
    void Update()
    {
        //controller.Update(Time.deltaTime);
        movementContext.Update(Time.deltaTime);
    }

    void FixedUpdate()
	{
        movementContext.FixedUpdate(Time.fixedDeltaTime);
        //if (controller.jump())
        //{
        //    jump();
        //}
    }

    void LateUpdate()
	{
        //override rig animation transform here.
        //controller.updateTilt();
        movementContext.LateUpdate();
    }

    void OnCollisionEnter(Collision col)
	{
        if (col.gameObject.name == "Terrain")
		{
            //Debug.Log("Contact with terrain");
            groundContact = true;
		}
	}

    void OnCollisionExit(Collision col)
	{
        if (col.gameObject.name == "Terrain")
		{
            //Debug.Log("No contact with terrain");
            groundContact = false;
		}
	}

    private void LateralMove(float x, float z, float movementSpeed)
	{
        lateralMovementVector = (this.transform.forward * z + this.transform.right * x) * movementSpeed;
        this.transform.position += lateralMovementVector;
    }

    public Vector3 LateralMovementVector()
	{
        return lateralMovementVector;
	}

    //Need unit test
    public void Run(float x, float z)
    {
        LateralMove(x, z, runningSpeed);
        animationContext.running();
    }

    public void Walk(float x, float z)
    {
        LateralMove(x, z, walkingSpeed);
        animationContext.walking();
    }

    public void Stay()
	{
        animationContext.stay();
        
	}

    public void Pan(float angle)
	{
        this.transform.localRotation *= Quaternion.Euler(0, angle, 0);
    }

    public void Tilt(float angle)
	{
        tiltAccum += angle;
        spine.transform.localRotation *= Quaternion.Euler(-tiltAccum, 0, 0);
    }

    public void Jump()
	{
        Debug.Log("Vanguard.Jump");
        rigidBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        //Challenge here is to sync the animation with jumping
        animationContext.Jumping();
	}

    public bool IsGrounded()
	{
        return groundContact;
	}

    public MyStuff.Controller Controller()
	{
        return controller;
	}
}

//Spine manipulation must be done in LateUpdate as an accumulation of a rotation angle. This accumulated value must be continuously applied. 
//Otherwise the bone animation will just overwrite the rotation angle in the next frame
//RigidBody attached to the Vanguard_Prefab with a capsule collider. This is a requirement. To stop the ragdoll colliders from pushing the Vanguard_prefab collider, make these colliders trigger colliders
//Good enough for inflicting battle damages for now.
//I think I will need a jump state manager. If user initiates the jump, then turn on parent collider and rigidbody to initiate the jump. On the ground, the Character Controller and NavMeshAgent takes over