using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3.5f; // 移動速度
    public float rotationSpeed = 720f; // 回転速度
    public float moveInterval = 2f; // ランダムに移動する間隔
    public float moveDistance = 5f; // ランダムに移動する距離

    private NavMeshAgent agent;
    private float moveTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgentがありません！");
            return;
        }

        agent.speed = moveSpeed;
        moveTimer = moveInterval;
        //MoveToRandomPosition();

        SceneManager.sceneLoaded += OnSceneLoaded; // シーンロード後のコールバックを追加
    }

    void Update()
    {
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            MoveToRandomPosition();
            moveTimer = moveInterval;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            RotateTowards(agent.steeringTarget);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleBattleScene")
        {
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = false; // `IsTrigger` プロパティを無効にする
            }
        }
    }

    void MoveToRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveDistance;
        randomDirection.y = 0; // Y軸のランダム値を無視
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, moveDistance, NavMesh.AllAreas))
        {
            Vector3 finalPosition = hit.position;
            agent.SetDestination(finalPosition);
            Debug.Log("Moving to: " + finalPosition); // デバッグメッセージ
        }
        else
        {
            Debug.LogWarning("Failed to find NavMesh position.");
        }
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0; // Y軸の回転を防ぐ
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
