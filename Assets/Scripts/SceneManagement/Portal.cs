using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
  public class Portal : MonoBehaviour
  {
    enum DistinationIdentifier
    {
      A, B, C, D, E
    }

    [SerializeField] int sceneToLoad = -1;
    [SerializeField] Transform spawnPoint;
    [SerializeField] DistinationIdentifier destination;
    [SerializeField] float fadeOutTime = 1f;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float fadeWaitTime = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
      if (other.tag == "Player")
      {
        StartCoroutine(Transition());
      }
    }

    private IEnumerator Transition()
    {
      if(sceneToLoad < 0)
      {
        Debug.LogError("Scene to load is not set.");
        yield break;
      }

      DontDestroyOnLoad(gameObject);

      Fader fader = FindObjectOfType<Fader>();

      yield return fader.FadeOut(fadeOutTime);
      yield return SceneManager.LoadSceneAsync(sceneToLoad);
      
      Portal otherPortal = GetOtherPortal();
      UpdatePlayer(otherPortal);

      yield return new WaitForSeconds(fadeWaitTime);
      yield return fader.FadeIn(fadeInTime);

      Destroy(gameObject);
    }

    private void UpdatePlayer(Portal otherPortal)
    {
      GameObject player = GameObject.FindWithTag("Player");
      // player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position); // use this if spawning in weird place after scene swap
      player.transform.position = otherPortal.spawnPoint.position; // remove this if using code above
      player.transform.rotation = otherPortal.spawnPoint.rotation;
    }

    private Portal GetOtherPortal()
    {
      foreach (Portal portal in FindObjectsOfType<Portal>())
      {
        if(portal == this) { continue; }
        if(portal.destination != destination) { continue; }

        return portal;
      }
      return null;
    }
  }
}
