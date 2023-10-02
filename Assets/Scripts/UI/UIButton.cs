using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip highlightSound;


    public void Pressed()
    {
        AudioManager.Instance.PlayOnce(clickSound);
    }

    public void Highlighted()
    {
        AudioManager.Instance.PlayOnce(highlightSound);
    }
}