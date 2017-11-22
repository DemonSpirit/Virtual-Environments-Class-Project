using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour {
	[SerializeField] GameObject mainCamera;
	bool isCarrying;
	GameObject carriedObject;
	[SerializeField] float distance;
	[SerializeField] float smooth;
	[SerializeField] float pickUpDist;

	// Use this for initialization
	void Start () {
		if (mainCamera == null)
			mainCamera = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * distance, Color.green);

		if (isCarrying)
		{
			Carry(carriedObject);
			CheckDrop();
		}
		else {
			Pickup();
		}
	}

	void Carry(GameObject obj)
	{
		if (obj == null)
			return;
		obj.transform.position = Vector3.Lerp(obj.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smooth);
		obj.transform.rotation = Quaternion.identity;
	}

	void Pickup()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			print("yo");
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				Pickupable p = hit.collider.GetComponent<Pickupable>();
				print(p);
				if (p != null && Vector3.Distance(p.gameObject.transform.position, mainCamera.transform.position) < pickUpDist)
				{
					isCarrying = true;
					carriedObject = p.gameObject;
					p.gameObject.GetComponent<Rigidbody>().useGravity = false;
				}
			}
		}
	}

	void CheckDrop()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			DropObject();
		}
	}

	void DropObject()
	{
		isCarrying = false;
		carriedObject.GetComponent<Rigidbody>().useGravity = true;;
		carriedObject = null;
	}
}
