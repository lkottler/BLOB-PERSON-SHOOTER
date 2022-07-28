using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LabelClick : MonoBehaviour
{

  public Animator animator;
  private string levelToLoad;

  private void OnMouseDown()
  {
    FadeToLevel(name);
  }

  public void FadeToLevel(string levelName)
  {
    levelToLoad = levelName;
    animator.SetTrigger("FadeOut");
  }

  public void OnFadeCOmplete()
  {
    SceneManager.LoadScene(levelToLoad);
  }
}
