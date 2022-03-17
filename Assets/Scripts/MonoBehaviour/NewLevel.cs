using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewLevel : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] TextMeshProUGUI _txtCurrentLevel;
    [SerializeField] CreateObjects createObjects;

    void OnEnable() => GamePersist.loadFinished += GameLoadFinished;

    void GameLoadFinished()
    {
        _txtCurrentLevel.text = "LEVEL " + GamePersist.Instance.CurrentLevel.ToString();
        _animator.SetTrigger("FadeIn");
    }

    public void LoadNewLevel() => _animator.SetTrigger("FadeOut");

    public void OnFadeComplete()
    {
        GamePersist.Instance.CurrentLevel++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDisable() => GamePersist.loadFinished -= GameLoadFinished;

    public void ResetScene()
    {
        foreach (GameObject go in createObjects.GameObjects)
        {
            go.transform.SetParent(createObjects.GameObjectsTransform);
        }
    }
}
