using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  private int SceneNumber
  {
    get 
    {
      return SceneManager.GetActiveScene().buildIndex;
    }
  }
    
  public void PlayGame()
  {
    SceneManager.LoadScene(SceneNumber + 1);
  }

  public void QuitGame()
  {
    Application.Quit();
  }
}
