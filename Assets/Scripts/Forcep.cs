using System;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("This class is deprecated and will be removed soon, please use ForcepEnhanced instead.")]
public class Forcep 
{
	internal Transform[] left = new Transform[2]; //left blades under this forcepsmaster
	internal Transform[] right = new Transform[2]; //right blades under this forcepsmaster
	internal GameObject[] devices = new GameObject[2]; //complete forceps models under this forcepsmaster
	public GameObject forcepMasterGameObject;
	internal int deviceIndex = 0; //CHANGE THIS TO CHANGE THE DEFAULT STARTING FORCEPS MODEL
	//SAMS VARIABLES
	public bool[] grasping = {false, false};//new bool[2];
	//public string[] objectNames = {"",""};//new string[2];
	public List<string>[] objectNames = new List<string>[2];
	public float releaseThreshold = 0;
	public List<GameObject> graspedObjects;// = new List<GameObject>();
	public Dictionary<GameObject, Transform> storedParents = new Dictionary<GameObject, Transform>(); //used to store original parents of grasped objects ONLY IF THE GRASPED OBJECT HAD A PARENT ORIGINALLY
	public List<GameObject> colliders;

    public static void UpdateForcepsHelper(Vector3 position, Quaternion rotation, float prcntClosed, Forcep forcep, int forcepIndex, ReadTrakStar readTrakStar)
	{
		//bool sameObjectGrasped = false;
		
		float angleVal;
		if (prcntClosed >= 100) 
		{ 
			//IF THE FORCEPS ARE COMPLETELY OPEN OR STRETCHING OUTWARD
			angleVal = 0;
		}

		//update the angle of the two closing parts based on the percent closed 
		else 
		{ 
			angleVal = 7 - 7 * prcntClosed / 100; //Has the fully/default open rotation as 7 Unity units. Change these values if a bigger distance is desired
			if (forcep.graspedObjects.Count > 0) 
			{ 
				//IF WE HAVE GRASPED ANYTHING IN A PREVIOUS FRAME AND WE ARE RELEASING IT
				if (prcntClosed < forcep.releaseThreshold) 
				{ 
					
					forcep.releaseThreshold = 0; //RESET RELEASE THRESHOLD
					//sameObjectGrasped = false;
					for (int i = 0; i < forcep.graspedObjects.Count; i++) { //RESET PHYSICS AND PARENTING PREFERENCES FOR EACH GRASPED OBJECT BEING RELEASED
						GameObject obj = GameObject.Find (forcep.graspedObjects [i].name);
						
						if (forcep.graspedObjects[i].GetComponent<Rigidbody>()) 
						{
							Rigidbody rb = forcep.graspedObjects [i].GetComponent<Rigidbody> ();
							rb.isKinematic = false;
							rb.useGravity = true;

							Debug.Log(obj.name + " Released");

							if (obj.GetComponent<CapsuleCollider>())
							{
								GameObject next = GameObject.Find("Node" + (Int32.Parse(obj.name.Substring(4)) + 1));
								GameObject prev = GameObject.Find("Node" + (Int32.Parse(obj.name.Substring(4)) - 1));
								if (next != null) 
								{
									next.GetComponent<CapsuleCollider>().enabled = true;
								}
								if (prev != null) 
								{
									prev.GetComponent<CapsuleCollider>().enabled = true;
								}
							}

							//ChangeCollisions(forcep, prev, next, false);
						}

						//if the object we are releasing originally had a parent and is not a compound object that we have grasped as a unit
						obj.transform.parent = forcep.storedParents.TryGetValue(obj, out var parent) 
							? parent 
							: null;
					}
					//CLEAR GRASPING DATA FROM FORCEPS
					forcep.graspedObjects.Clear();
					forcep.objectNames[0].Clear();
					forcep.objectNames[1].Clear();
				}
			}
			else 
			{ 
				foreach(string s in forcep.objectNames[0]) 
				{ //checks if any objects in objectNames[0] are also in objectNames[1]
					if (forcep.objectNames[1].Contains(s)) 
					{ 
						//if objectNames[1] has the current string
						bool flag = true;
						for (int i = 0; i < forcep.graspedObjects.Count; i++) 
						{
							if (forcep.graspedObjects[i].name == s) 
							{
								flag = false;
							}
						}
						if (flag) 
						{ 
							//THIS IS WHERE INITIAL GRASPING HAPPENS
							//Debug.Log ("Grabbing first");
							GameObject obj = GameObject.Find(s);
							forcep.graspedObjects.Add(obj);

							if (obj.GetComponent<Rigidbody>()) 
							{
								// TODO: Investigate the line below
								Debug.Log(obj.name + " Grabbed");
								//GameObject prev2 = GameObject.Find("Node" + (Int32.Parse(obj.name.Substring(4)) - 2));

								//ChangeCollisions(forcep, prev, next, true);
								obj.GetComponent<Rigidbody>().isKinematic = true;
								obj.GetComponent<Rigidbody>().useGravity = false;

								if (obj.GetComponent<CapsuleCollider>())
								{
									GameObject next = GameObject.Find("Node" + (Int32.Parse(obj.name.Substring(4)) + 1));
									GameObject prev = GameObject.Find("Node" + (Int32.Parse(obj.name.Substring(4)) - 1));
									if (next != null) 
									{
										next.GetComponent<CapsuleCollider>().enabled = false;
									}
									if (prev != null) 
									{
										prev.GetComponent<CapsuleCollider>().enabled = false;
									}
								}
								//prev2.GetComponent<CapsuleCollider>().enabled = false;
							}
							if (obj.transform.parent != null) 
							{ //if the object we are grasping originally had a parent
								if (obj.GetComponent<CompoundObjectFlag>() != null) 
								{ 
									//if the gameobject has the compound object flag component attached (gameobjects with multiple colliders should, but this is a failsafe)
									CompoundObjectFlag flag1 = obj.GetComponent<CompoundObjectFlag>();
									if (!flag1.isCompoundObject) 
									{ 
										//if the object we are grasping is not a compound object
										forcep.storedParents[obj] = obj.transform.parent; //store the parent for later recall
										obj.transform.parent = forcep.devices[forcep.deviceIndex].transform; //grasp the object
									}
									else 
									{ 
										//if the grasped object is a compound object
										//DO COMPOUND GRASPING HERE
										obj.transform.parent.gameObject.transform.parent = forcep.devices[forcep.deviceIndex].transform; //grasp the parent of the contacted gameobject
									}
								}
							}
							else 
							{ 
								//if the gameobject we are grasping is a lone gameobject
								obj.transform.parent = forcep.devices[forcep.deviceIndex].transform;
							}
							//GameObject.Find (s).transform.parent = forceps[forcepOn].devices[forceps[forcepOn].deviceIndex].transform;
							//maybe replace angleVal with prcntClosed and create conditionals based on storedAngleVal that change whether the raw prcntClosed or the storedAngleVal is used in subsequent angleVal calculations and other conditionals
						}
					}
				}

				if (forcep.graspedObjects.Count > 0) 
				{ 
					//IF WE HAVE GRASPED SOMETHING EARLIER IN THIS FRAME
					forcep.releaseThreshold = prcntClosed;
                    readTrakStar.UpdateGraspCounter(forcepIndex);
				}
				
			}
		}

		readTrakStar.UpdateMetrics(position, forcepIndex);

		//Update the percent open of the forcep
		forcep.left[forcep.deviceIndex].localRotation = Quaternion.Euler(0f,0f, -angleVal);
		forcep.right[forcep.deviceIndex].localRotation = Quaternion.Euler(0f,0f, angleVal);
		
		//update position and rotation of forceps
		forcep.forcepMasterGameObject.transform.position = position;
		forcep.forcepMasterGameObject.transform.localRotation = rotation;
	}

	public static void ChangeCollisions(Forcep forcep, GameObject node1, GameObject node2, bool state)
	{
		for (int i = 0; i < forcep.colliders.Count; i++)
		{
			Debug.Log(forcep.colliders[i].name);
			Physics.IgnoreCollision(forcep.colliders[i].GetComponent<BoxCollider>(), node1.GetComponent<CapsuleCollider>(), state);
			Physics.IgnoreCollision(forcep.colliders[i].GetComponent<BoxCollider>(), node2.GetComponent<CapsuleCollider>(), state);
		}
	}

}

