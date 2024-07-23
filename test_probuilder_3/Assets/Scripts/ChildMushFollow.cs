using UnityEngine;

public class FollowerAI : MonoBehaviour
{
    public Transform player; // プレイヤーのTransformをインスペクターで設定
    public float followDistance = 2f; // プレイヤーとの距離
    public float moveSpeed = 5f; // インスペクターで調整可能な移動速度
    public float rotationSpeed = 1080f; // 回転速度（インスペクターで調整可能）
    public float collisionAvoidanceDistance = 1f; // 衝突回避距離
    public float avoidSpeedMultiplier = 2f; // 衝突回避時の速度倍率

    private bool isAvoiding = false; // 回避中かどうかのフラグ
    private Vector3 avoidPosition; // 回避先の位置

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
        // プレイヤーの背後の位置を計算
        Vector3 followPosition = player.position - player.forward * followDistance;
        followPosition.y = transform.position.y; // Y軸の高さを固定

        // 子分キノコをプレイヤーの背後の位置に移動
        transform.position = Vector3.MoveTowards(transform.position, followPosition, moveSpeed * Time.deltaTime);

        // 子分キノコがプレイヤーをスムーズに向くように回転
        RotateTowards(player.position);
    }

    void AvoidingMovement()
    {
        // 回避位置に向かって移動
        transform.position = Vector3.MoveTowards(transform.position, avoidPosition, moveSpeed * avoidSpeedMultiplier * Time.deltaTime);

        // 子分キノコが回避位置を向くように回転
        RotateTowards(avoidPosition);

        // 回避位置に到達したか確認
        if (Vector3.Distance(transform.position, avoidPosition) < 0.1f)
        {
            isAvoiding = false; // 回避終了
        }
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; // Y軸の回転を防ぐ
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
            // 回避位置の設定
            avoidPosition = player.position - player.forward * collisionAvoidanceDistance;
            avoidPosition.y = transform.position.y; // Y軸の高さを固定

            isAvoiding = true; // 回避フラグを立てる
        }
    }
}
