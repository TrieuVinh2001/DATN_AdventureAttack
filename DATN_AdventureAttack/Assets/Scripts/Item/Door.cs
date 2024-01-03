using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Vector3 pos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>().posLoadDoor = pos;
            collision.GetComponent<PlayerStats>().SaveDataPlayer();
            SceneManager.LoadScene(sceneName);
        }
    }
}
