using UnityEngine;

public class FollowerAI : MonoBehaviour
{
    public Transform player; // �v���C���[��Transform���C���X�y�N�^�[�Őݒ�
    public float followDistance = 2f; // �v���C���[�Ƃ̋���
    public float moveSpeed = 5f; // �C���X�y�N�^�[�Œ����\�Ȉړ����x
    public float rotationSpeed = 1080f; // ��]���x�i�C���X�y�N�^�[�Œ����\�j
    public float collisionAvoidanceDistance = 1f; // �Փˉ������
    public float avoidSpeedMultiplier = 2f; // �Փˉ�����̑��x�{��

    private bool isAvoiding = false; // ��𒆂��ǂ����̃t���O
    private Vector3 avoidPosition; // ����̈ʒu

    void Update()
    {
        if (isAvoiding)
        {
            AvoidingMovement();
        }
        else
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        // �v���C���[�̔w��̈ʒu���v�Z
        Vector3 followPosition = player.position - player.forward * followDistance;
        followPosition.y = transform.position.y; // Y���̍������Œ�

        // �q���L�m�R���v���C���[�̔w��̈ʒu�Ɉړ�
        transform.position = Vector3.MoveTowards(transform.position, followPosition, moveSpeed * Time.deltaTime);

        // �q���L�m�R���v���C���[���X���[�Y�Ɍ����悤�ɉ�]
        RotateTowards(player.position);
    }

    void AvoidingMovement()
    {
        // ����ʒu�Ɍ������Ĉړ�
        transform.position = Vector3.MoveTowards(transform.position, avoidPosition, moveSpeed * avoidSpeedMultiplier * Time.deltaTime);

        // �q���L�m�R������ʒu�������悤�ɉ�]
        RotateTowards(avoidPosition);

        // ����ʒu�ɓ��B�������m�F
        if (Vector3.Distance(transform.position, avoidPosition) < 0.1f)
        {
            isAvoiding = false; // ����I��
        }
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; // Y���̉�]��h��
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ����ʒu�̐ݒ�
            avoidPosition = player.position - player.forward * collisionAvoidanceDistance;
            avoidPosition.y = transform.position.y; // Y���̍������Œ�

            isAvoiding = true; // ����t���O�𗧂Ă�
        }
    }
}
