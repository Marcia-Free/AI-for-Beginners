using UnityEngine;

public class Move : MonoBehaviour {

    public GameObject goal;
    Vector3 direction;
    public float speed = 2f;

    void Start() 
    {
        //will only add on the vector amount of the goal, it wont take into account the pig start position
        //this.transform.Translate(goal.transform.position);

        //direction = goal.transform.position - this.transform.position;


        //this.transform.position = this.transform.position + direction;
        //same as line above just a different way to write this line
        //this.transform.Translate(direction);


    }

    private void LateUpdate() 
    {
        direction = goal.transform.position - this.transform.position;
        this.transform.LookAt(goal.transform.position);
        if (direction.magnitude > 2)
        {
            Vector3 velocity = direction.normalized * speed * Time.deltaTime;
            this.transform.position = this.transform.position + velocity;
        }
    }
}
