#pragma strict
var Receivers : GameObject[];
var Cords : GameObject[];
var PhoneListObj : GameObject;
var HookSwitch : GameObject;
var woodTextures : Texture[];
var quackTextures : Texture[];
var normalTextures : Texture[];
var mainMaterial : Material;
private var hungup : boolean = true;
private var detachedList : boolean = false;
private var quackified : boolean = false;
private var woodified : boolean = false;

function Start () {
	mainMaterial.mainTexture = normalTextures[0];
	mainMaterial.SetTexture ("_BumpMap", normalTextures[1]);
}

function Update () {

}

function OnGUI () {
	GUILayout.BeginArea (Rect (20,20,550,100));
	GUILayout.BeginHorizontal ("box");

		if(hungup == true) {
			if(GUILayout.Button ("PICK\nUP")) {
				PickUp();
			}
		} else {
			if(GUILayout.Button ("HANG\nUP")) {
				PickUp();
			}
		}
		if(detachedList == false) {
			if(GUILayout.Button ("DETACH\nPHONE LIST")) {
				PhoneList();
			}
		} else {
			if(GUILayout.Button ("ATTACH\nPHONE LIST")) {
				PhoneList();
			}
		}
		if(woodified == false) {
			if(GUILayout.Button ("WOODIFY")) {
				Woodify();
			}
		} else {
			if(GUILayout.Button ("DE-WOODIFY")) {
				Woodify();
			}
		}
		if(quackified == false) {
			if(GUILayout.Button ("QUACKIFY")) {
				Quackify();
			}
		} else {
			if(GUILayout.Button ("DE-QUACKIFY")) {
				Quackify();
			}
		}
		GUILayout.FlexibleSpace();
		GUILayout.Label ("[Z] KEY\n= ZOOM");
		GUILayout.Space (20);
		GUILayout.Label ("[ARROW] KEYS\n= ROTATE");
		GUILayout.Space (20);
		
	GUILayout.EndHorizontal ();
	GUILayout.EndArea ();
}

function PickUp() {
	if(hungup == true) {
		hungup = false;
		Receivers[0].SetActive(false);
		Receivers[1].SetActive(true);
		Cords[0].SetActive(false);
		Cords[1].SetActive(true);
		HookSwitch.transform.localEulerAngles.x = -20;
	} else {
		hungup = true;
		Receivers[1].SetActive(false);
		Receivers[0].SetActive(true);
		Cords[1].SetActive(false);
		Cords[0].SetActive(true);
		HookSwitch.transform.localEulerAngles.x = 0;
	}
}
function PhoneList() {
	if(PhoneListObj.activeSelf == true) {
	detachedList = true;
	PhoneListObj.SetActive(false);
	} else {
	detachedList = false;
	PhoneListObj.SetActive(true);
	}
}
function Woodify() {
	if(woodified == true) {
	woodified = false;
	mainMaterial.mainTexture = normalTextures[0];
	mainMaterial.SetTexture ("_BumpMap", normalTextures[1]);
	} else {
	woodified = true;
	quackified = false;
	mainMaterial.mainTexture = woodTextures[0];
	mainMaterial.SetTexture ("_BumpMap", normalTextures[1]);
	}
}
function Quackify() {
	if(quackified == true) {
	quackified = false;
	mainMaterial.mainTexture = normalTextures[0];
	mainMaterial.SetTexture ("_BumpMap", normalTextures[1]);
	} else {
	quackified = true;
	woodified = false;
	mainMaterial.mainTexture = quackTextures[0];
	mainMaterial.SetTexture ("_BumpMap", quackTextures[1]);
	}
}