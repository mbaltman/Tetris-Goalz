using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger: MonoBehaviour {
    public void ScenePlay() {
        SceneManager.LoadScene("ActiveGamePlay");
    }

    public void SceneHowTo() {
        SceneManager.LoadScene("HowTo");
    }

    public void SceneMenu() {
        SceneManager.LoadScene("Opening");
    }

    public void CreditMenu() {
        SceneManager.LoadScene("Credit");
    }

}
