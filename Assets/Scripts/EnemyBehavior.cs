using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;

    [SerializeField]
    private float damage;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        agent.destination = player.position;
    }

    IEnumerator PauseEnemy()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(1f);
        agent.isStopped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatsManager playerStatsManager = other.GetComponent<PlayerStatsManager>();
            playerStatsManager.TakeDamage(damage);
            StartCoroutine(PauseEnemy());
        }
    }
}