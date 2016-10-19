using UnityEngine;
using System.Collections;

public class FSM : MonoBehaviour {

    Vector3 Lray, RRay;
   
    public bool enemyFound;
	private bool enemyIsWeak;
	private bool fled;
	
    public bool Patrol;
    public bool Fight;
    public bool Flee;
    private bool rotated;

    private float RayRange;
    const float Y = 0.5f;

 	private Transform moveGameObject;
    public float step;
    // Use this for initialization
    private Vector3 whereToMove;
    private float a, b, c, d;
    //LookAt
    public float lookSpeed = 10;
    private Vector3 curLoc;
    private Vector3 prevLoc;
    void Start ()
    {
       
        a = 0.4f;
        b = 1;
        c = -0.4f;
        d = 1;
        RayRange = 10.0f;
        rotated = false;
        Patrol = true;
        enemyFound = false;
        enemyIsWeak = true;
        moveGameObject = GameObject.Find("Cube").transform;
        step = 0.05f;
        whereToMove = new Vector3(3.5f,0, 3.5f);
    

    }
	
	// Update is called once per frame
	void Update ()
    {
       

        Lray = moveGameObject.TransformDirection(a, 0, b);
        RRay = moveGameObject.TransformDirection(c, 0, d);
        // Debug.Log(whereToMove);
        // Visualises a ray 
        Debug.DrawRay(moveGameObject.position, Lray * RayRange, Color.red);
        Debug.DrawRay(moveGameObject.position, RRay * RayRange, Color.red);
        if (Patrol) 
		{
          
            if (rotated)
            {
                moveGameObject.transform.position = Vector3.MoveTowards(moveGameObject.transform.position, whereToMove, step);
            }

            // checks the angle between the gamobject and the point that it has to move to
            // for gameobject rotation towards the point
            if (Vector3.Angle(moveGameObject.forward, whereToMove) !=0)
            {
                Vector3 newDir = Vector3.RotateTowards(moveGameObject.forward, whereToMove, 0.05f, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
                rotated = true;
            }

            // Debug.Log(Vector3.Angle(moveGameObject.forward, whereToMove));

            if (transform.position == whereToMove)
            {
                whereToMove = new Vector3(Random.Range(-3.5f, 3.5f), 0, Random.Range(-3.5f, 3.5f));
                rotated = false;
            }

            RaycastHit hit;
            if(Physics.Raycast(transform.position,Lray, out hit) || Physics.Raycast(transform.position, RRay, out hit))
            {
                Debug.LogWarning("Hit something");
                enemyFound = true;
            }

        if ( enemyFound )
			{
                Fight = true;
                Patrol = false;
			}
		}
		else if (Fight) 
		{
            GetComponent<Renderer>().material.color = Color.red;
            // fight

            if (!enemyIsWeak)
			{
                Fight = false;
				Flee = true;
			}
		} 
		else if (Flee) 
		{
			// flee
			if(fled)
			{
				Patrol = true;
				Flee = false;
			}
		}


    }
    void OnCollisionEnter(Collision collision)
    {
       if(collision.collider.tag == "CanCollide")
        { 
            Patrol = false;
            Fight = true;
            Flee = false;
            Debug.LogError("Collided");
        }
    }
}
