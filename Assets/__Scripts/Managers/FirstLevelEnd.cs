using UnityEngine;

public class FirstLevelEnd : MonoBehaviour
{
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameMenu.FirstLevelComplete();
    }
}
