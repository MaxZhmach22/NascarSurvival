using Obi;
using UnityEngine;


public class TestCursorLength : MonoBehaviour
{
   public float speed = 1;
   	ObiRopeCursor cursor;
   	ObiRope rope;
   
   	void Start () {
   		cursor = GetComponentInChildren<ObiRopeCursor>();
   		rope = cursor.GetComponent<ObiRope>();
   	}
   
   	void Update () {
   		if (Input.GetKey(KeyCode.W))
   			cursor.ChangeLength(rope.restLength - speed * Time.deltaTime);
   
   		if (Input.GetKey(KeyCode.S))
   			cursor.ChangeLength(rope.restLength + speed * Time.deltaTime);
   	}
}
