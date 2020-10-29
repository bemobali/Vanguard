using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EjectShell : MonoBehaviour
{
	[SerializeField]
	GameObject m_ejectionPort = null;
	[SerializeField]
	GameObject m_shell = null;
	GameObject m_shellImpulsePoint = null;
	[SerializeField]
	Vector3 m_impulse;

	public void Eject()
	{
		GameObject clone = Instantiate(m_shell, m_ejectionPort.transform.position, m_shell.transform.rotation);
		ShotgunShell shell = clone.GetComponent<ShotgunShell>();
		if (shell) shell.UserTimerSelfDestruct = true;
		Rigidbody rb = clone.GetComponent<Rigidbody>();
		m_shellImpulsePoint = clone.transform.GetChild(0).gameObject;
		if (rb)
		{
			rb.isKinematic = false;
			Vector3 impulse = (m_ejectionPort.transform.right.normalized * m_impulse.x)			//x
							+ (m_ejectionPort.transform.up.normalized * m_impulse.y)				//y
							+ (m_ejectionPort.transform.forward.normalized * m_impulse.z);  //z

			rb.AddForceAtPosition(impulse, m_shellImpulsePoint.transform.position, ForceMode.Impulse);
		}
	}

	void FixedUpdate()
	{ }

}
