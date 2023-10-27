using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelPortal : MonoBehaviour
{
    public GameObject PressEText;

    [SerializeField]
    private LevelNames levelName;

    private void Start()
    {
        PressEText.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PressEText.activeSelf)
        {
            SceneManager.LoadScene((int)levelName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PressEText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PressEText.SetActive(false);
        }
    }
}
