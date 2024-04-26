using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.Barracuda;
using UnityEngine.UIElements;

public class PlayerAgent : Agent
{
    public Transform obstacle;
    public Transform reward;
    public float jumpForce = 10f;
    public float speed = 1f;
    public float obstacleProximityRadius = 6f;
    private Rigidbody rb;
    public GameObject rewardObject;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(0f, 1.5f, 0f);
        obstacle.position = new Vector3(0f, 0.54f, -19.81f);
        reward.GetComponent<Renderer>().enabled = true;
        reward.position = new Vector3(0f, 5.27f, -25.76f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.position);
        sensor.AddObservation(rb.velocity);
        sensor.AddObservation(obstacle.position);
        sensor.AddObservation(obstacle.position - transform.position);
        if (reward != null) {
            sensor.AddObservation(reward.position);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float distanceToClosestObstacle = float.MaxValue;
        if (obstacle != null)
        {
            float distance = Vector3.Distance(transform.position, obstacle.position);
            if (distance < distanceToClosestObstacle)
            {
                distanceToClosestObstacle = distance;
            }
        }

        // Acties, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.y = actionBuffers.ContinuousActions[0];
        transform.Translate(new Vector3(controlSignal.x * 0.1f, controlSignal.y * speed, controlSignal.z * 0.1f));


        // Jump
        /*if (action == 0 || distanceToClosestObstacle < obstacleProximityRadius)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }*/

        if (distanceToClosestObstacle < obstacleProximityRadius) {
            if (transform.position.y < 3)
            {
                Debug.Log("penalty has been given");
                AddReward(-1f);
                EndEpisode();
            }
        }
        else if (obstacle.position.z > 15)
        {
            Debug.Log("REWARD");
            AddReward(1f);
            EndEpisode();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == reward)
        {
            Debug.Log("COLLECT REWARD");
            AddReward(0.5f);
            reward.GetComponent<Renderer>().enabled = false;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        /*var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;*/
        var continiousActionsOut = actionsOut.ContinuousActions;
        continiousActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1f : 0;

    }
}