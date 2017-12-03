#pragma strict
var cameraObject : Transform;
var turntable : boolean;
var turnSpeed : int;
private var zoomed : boolean = false;

function Update () {
	//Input
	var horizontal : float = Input.GetAxis ("Horizontal") * 5;
	var vertical : float = Input.GetAxis ("Vertical") * 3;
	
	transform.Rotate (0, -horizontal, 0, Space.World);
	transform.Rotate (vertical, 0, 0);
	
	if(turntable == true) {
	transform.Rotate(Vector3.down * Time.deltaTime*turnSpeed, Space.World);
	}
	
	if (Input.GetKeyDown (KeyCode.Z)) {
		if(zoomed == false) {
			zoomed = true;
			cameraObject.transform.localPosition.z = -0.3;
		} else {
			zoomed = false;
			cameraObject.transform.localPosition.z = -0.5;
		}
	}
}