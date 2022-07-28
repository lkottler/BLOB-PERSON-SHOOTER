using UnityEngine.SceneManagement;
using UnityEngine;

public class RoomChanger : MonoBehaviour
{
  public Animator animator;
  private string levelToLoad;

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
