using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ShotgunShell checks for collision with player, and attaches itself, if possible, to the player
//If attachment succeeded, game object will delete itself
//To use this, add a layer called Resuplly, make it collide only with the Player layer, and attach to a player script (Vanguard in our case)
public class ShotgunShell : MonoBehaviour
{
    //How many shell to add to the player. Yes, this types has only 1. A ShotgunShellBox will have 25
    [SerializeField]
    uint m_numShell = 1;
    float m_selfDestructTimer = 0;
    [SerializeField, Range(1, 10)]
    //Seconds before object self-destruct
    float m_timeToDie = 1f;
    //If true, the shell will self destruct on a timer. Otherwise it will self-destruct after m_numShell reaches 0;
    //Timer takes precedence over m_numShell. Ideally I would have 2 self-destruct instances using a common interface. Meh. Only 2 cases to consider
    bool m_useTimerSelfDestruct = false;
    [SerializeField]
    AudioSource m_pickUp;

    public bool UserTimerSelfDestruct
    {
        get { return m_useTimerSelfDestruct; }
        set { m_useTimerSelfDestruct = value; }
    }

    void Start()
    {
    }

    void Update()
    {
        if (m_useTimerSelfDestruct)
        {
            m_selfDestructTimer += Time.deltaTime;
            if (m_selfDestructTimer > m_timeToDie)
            {
                Destroy(gameObject);
            }
            return;
        }

        if (m_numShell == 0 && !m_pickUp.isPlaying)
        {
            Destroy(gameObject);
            return;
        }
    }

    //Player needs a standardized way to reduce the number of shell. Destroy object when m_numShell drops to 0
    uint NumShellRemaining
    {
        get { return m_numShell; }
        set { m_numShell = value; }
    }

    public void GrabAmmo(Vanguard player)
	{
        //This is the result of waiting for the sound to stop playing
        if (m_numShell == 0) return;
        //I won't bother checking tags here. The Resupply layer collides a player and radar layer. I don't care if the zombies can overrun the resupply stuff.
        //Besides, if they occupy the space, the resupply object is practically unusable anyway.
        //@todo Maybe I want to come up with an inventory script. Use Vanguard fornow
        if (player == null)
        {
            return;
        }

        m_numShell = player.GrabAmmo(m_numShell);
        if (m_pickUp) m_pickUp.Play();
    }
}
