using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PlayerAgent : Agent
{
    public float jumpForce = 5f;
    public float checkRadius = 1f;
    public Vector3 detectionsize = new Vector3(5f, 5f, 5f);
    private bool isJumping = false;

    public override void OnEpisodeBegin()
    {
        Debug.ClearDeveloperConsole();
        transform.localPosition = new Vector3(0f, 1.5f, 0f);
        isJumping = false;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition); // Observation for agent position
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        // Check if an enemy is nearby
        bool enemyNearby = CheckEnemyNearby();

        // Jump only if there is an enemy nearby and the agent is not already jumping
        if (actions.DiscreteActions[0] == 1 && !isJumping)
        {
            Jump();
            if (enemyNearby)
            {
                SetReward(1f);
                Debug.Log("SetReward");
                isJumping = true; // Set jumping flag
            }
        }
    }

    private void Jump()
    {
        // Apply jump force upwards
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJumping = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collide");

        if (collision.gameObject.tag == "enemy")
        {
            SetReward(-1f); // Punish the agent for colliding with an enemy
            Debug.Log("punishment has been given");
            isJumping = false; // Reset jumping flag when landing
            EndEpisode();
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continiousActionsOut = actionsOut.DiscreteActions;
        continiousActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0; // Jump action (Space key)
        Debug.Log("space");

    }
    private bool CheckEnemyNearby()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, detectionsize / 2, Quaternion.identity, LayerMask.GetMask("Default"));

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("enemy"))
            {
                return true; // Return true if an enemy is nearby
            }
        }

        return false; // Return false if no enemies are nearby
    }
}
