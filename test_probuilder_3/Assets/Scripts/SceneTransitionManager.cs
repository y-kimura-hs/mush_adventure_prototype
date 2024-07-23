using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[�����܂����ŃI�u�W�F�N�g��ێ�����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitionToBattleScene()
    {
        SceneManager.LoadScene("SampleBattleScene");
    }

    public void TransitionToMapScene()
    {
        SceneManager.LoadScene("SampleMapScene");
    }
}
