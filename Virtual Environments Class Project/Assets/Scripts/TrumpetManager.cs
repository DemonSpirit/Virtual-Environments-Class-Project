using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetManager : MonoBehaviour {
	
	[SerializeField] GameObject combTrump1x2;
	[SerializeField] GameObject combTrump1x3;
	[SerializeField] GameObject combTrump2x3;
	[SerializeField] GameObject combTrumpComplete;

	// Singleton
	[HideInInspector] public static TrumpetManager singleton;
	[HideInInspector] public bool isInstantiating;

	void Start()
	{
		if (singleton == null)
			singleton = this;
	}
		
	public void InstantiateCombinedObject(GameObject obj1, GameObject obj2, Vector3 position)
	{
		int obj1ID = obj1.GetComponent<TrumpetPiece>().id;
		int obj2ID = obj2.GetComponent<TrumpetPiece>().id;

		// 1x2
		if ( (obj1ID == 1 && obj2ID == 2) || (obj1ID == 2 && obj2ID == 1) )
		{
			Instantiate(combTrump1x2, position, Quaternion.identity);
		}

		// 1x3
		else if ( (obj1ID == 1 && obj2ID == 3) || (obj1ID == 3 && obj2ID == 1) )
		{
			Instantiate(combTrump1x3, position, Quaternion.identity);
		}

		// 2x3
		else if ( (obj1ID == 2 && obj2ID == 3) || (obj1ID == 3 && obj2ID == 2) )
		{
			Instantiate(combTrump2x3, position, Quaternion.identity);
		}

		// 1x2x3
		else 
		{
			Instantiate(combTrumpComplete, position, Quaternion.identity);
		}


		Destroy(obj1);
		Destroy(obj2);

	}

}
