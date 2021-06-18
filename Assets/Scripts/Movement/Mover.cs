using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
  public class Mover : MonoBehaviour, IAction
  {
    [SerializeField] Transform target;

    NavMeshAgent navMeshAgent;
    Health health;

    private void Start()
    {
      navMeshAgent = GetComponent<NavMeshAgent>();
      health = GetComponent<Health>();
    }

    void Update()
    {
      navMeshAgent.enabled = !health.IsDead();

      UpdateAnimator();
    }

    public void StartMoveAction(Vector3 destination)
    {
      GetComponent<ActionScheduler>().StartAction(this);
      MoveTo(destination);
    }

    public void MoveTo(Vector3 destination)
    {
      navMeshAgent.destination = destination;
      navMeshAgent.isStopped = false;
    }

    public void Cancel()
    {
      navMeshAgent.isStopped = true;
    }

    private void UpdateAnimator()
    {
      // Get velocity from players NavMeshAgent
      Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
      // Convert into a local value relative to the Player
      Vector3 localVelocity = transform.InverseTransformDirection(velocity);
      float speed = localVelocity.z;
      GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }
  }
}
