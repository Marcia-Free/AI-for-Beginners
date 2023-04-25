using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// A very simplistic car driving on the x-z plane.

public class DriveTank : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    float autoSpeed = 2f;
    float autoRotationSpeed = 0.05f;
    public GameObject fuel;
    bool autopilot = false;

    void AutoPilot()
    {
        CalculateAngle();
        transform.position += transform.up * autoSpeed * Time.deltaTime;
    }

    void CalculateAngle()
    {
        //Dot product
        Vector3 tankForward = transform.up;
        Vector3 fuelDirection = fuel.transform.position - transform.position;

        Debug.DrawRay(transform.position, tankForward * 10, Color.green, 5);
        Debug.DrawRay(transform.position, fuelDirection, Color.red, 5);

        float dot = (tankForward.x * fuelDirection.x) + (tankForward.y * fuelDirection.y);
        float angle = Mathf.Acos(dot / (tankForward.magnitude * fuelDirection.magnitude));

        //Debug.Log("Angle: " + angle * Mathf.Rad2Deg);
        //Unity built in way of handling angles
        //Debug.Log("Unity Angle: " + Vector3.Angle(tankForward, fuelDirection));

        int clockwise = 1;
        if(Cross(tankForward, fuelDirection).z < 0)
        {
            clockwise = -1;
        }

        if((angle * Mathf.Rad2Deg) > 10)
        {
            transform.Rotate(0, 0, angle * Mathf.Rad2Deg * clockwise * autoRotationSpeed);
        }
    }

    Vector3 Cross(Vector3 v, Vector3 w)
    {
        //Cross product calculation
        //Unity has a method that handled this but this is how you would calculate if that was not available
        float xMult = (v.y * w.z) - (v.z * w.y);
        float yMult = (v.x * w.z) - (v.z * w.x);
        float zMult = (v.x * w.y) - (v.y * w.x);

        return (new Vector3(xMult, yMult, zMult));
    }

    float CalculateDistance()
    {
        //Pythagorus theorem to calculate distance
        float x = Mathf.Pow((transform.position.x - fuel.transform.position.x),2);
        float y = Mathf.Pow((transform.position.y - fuel.transform.position.y),2);
        float distance = Mathf.Sqrt(x + y);

        Vector3 tankPos = new Vector3(transform.position.x, transform.position.y, 0);
        Vector3 fuelPos = new Vector3(fuel.transform.position.x, fuel.transform.position.y, 0);
        //Unity built in method to calculate distance
        float uDistance = Vector3.Distance(tankPos, fuelPos);

        Vector3 tankToFuel = tankPos - fuelPos;

        //Debug.Log("Distance: " + distance);
        //Debug.Log("Unity Distance: " + uDistance);
        //Debug.Log("V Magnitude : " + tankToFuel.magnitude);
        //Faster operation then magnitude
        //Debug.Log("V SqMagnitude : " + tankToFuel.sqrMagnitude);
        return distance;
    }

    void LateUpdate()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, translation, 0);

        // Rotate around our y-axis
        transform.Rotate(0, 0, -rotation);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            CalculateDistance();
            CalculateAngle();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            autopilot = !autopilot;
        }

        if(CalculateDistance() < 3)
        {
            autopilot = false;
        }

        if (autopilot)
        {
            AutoPilot();
        }

    }
}