using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerChangeColor : NetworkBehaviour {

	[SyncVar(hook = "ApplyColor")] Color m_color = Color.red;

	[Command]
	public void CmdChangeColor()
	{
		if (!isServer)
			return;
		m_color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
		ApplyColor (m_color);
	}
	void ApplyColor (Color c)
	{
		foreach (var r in GetComponentsInChildren<Renderer>())
			r.material.color = c;
	}
}
