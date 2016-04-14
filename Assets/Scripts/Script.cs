using UnityEngine;
using System.Collections;

public class Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.back * (Time.deltaTime + .05f));
		transform.Translate(Vector3.down * Time.deltaTime, Space.World);
	}
}
