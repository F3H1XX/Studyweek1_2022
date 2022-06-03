using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEnder : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(SceneEnd());
    }

    private IEnumerator SceneEnd()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainMenu");
    }
}
