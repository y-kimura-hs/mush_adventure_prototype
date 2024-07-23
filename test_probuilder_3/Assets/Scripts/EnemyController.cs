using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3.5f; // �ړ����x
    public float rotationSpeed = 720f; // ��]���x
    public float moveInterval = 2f; // �����_���Ɉړ�����Ԋu
    public float moveDistance = 5f; // �����_���Ɉړ����鋗��

    private NavMeshAgent agent;
    private float moveTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent������܂���I");
            return;
        }

        agent.speed = moveSpeed;
        moveTimer = moveInterval;
        //MoveToRandomPosition();

        SceneManager.sceneLoaded += OnSceneLoaded; // �V�[�����[�h��̃R�[���o�b�N��ǉ�
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
                collider.isTrigger = false; // `IsTrigger` �v���p�e�B�𖳌��ɂ���
            }
        }
    }

    void MoveToRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveDistance;
        randomDirection.y = 0; // Y���̃����_���l�𖳎�
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, moveDistance, NavMesh.AllAreas))
        {
            Vector3 finalPosition = hit.position;
            agent.SetDestination(finalPosition);
            Debug.Log("Moving to: " + finalPosition); // �f�o�b�O���b�Z�[�W
        }
        else
        {
            Debug.LogWarning("Failed to find NavMesh position.");
        }
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0; // Y���̉�]��h��
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
