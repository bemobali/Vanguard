using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Footsteps defines the main sound for a foot to step on the ground. Each footstepSound attaches to a collider on a foot.
//Used by both Vanguard and Zombie units.
//Terrain vegatation effect will be mixed in using an audio mixer. I suspect the rustling, or water puddle footstep sound can be omnidirectional, as long as the main footstep sound is still 3D
//Maybe now is a good time to toy with Udemy's suggestion on randomizing the footstep sound
public class Footstep : MonoBehaviour
{

    public AudioSource footstepSound;
    bool enableSound = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    //LOOVE the sound variation
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Terrain" && enableSound)
		{
            footstepSound.Play();
        }
    }

    public void jumping()
	{
        if (enableSound) enableSound = false;
	}

    public void walkOrRun()
	{
        if (!enableSound) enableSound = true;
	}
}
