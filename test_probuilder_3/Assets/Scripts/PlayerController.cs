
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    //[SerializeField] private float jumpPower = 3;
    //[SerializeField] private Animator animator;
    private CharacterController _characterController;
    private Transform _transform;
    private Vector3 _moveVelocity;
    private InputAction _move;
    //private InputAction _fire;
    //private InputAction _jump;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("init Player");
        _characterController = GetComponent<CharacterController>();
        _transform = transform;

        var input = GetComponent<PlayerInput>();
        input.currentActionMap.Enable();
        _move = input.currentActionMap.FindAction("Move");
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(_characterController.isGrounded ? "ínè„Ç…Ç¢Ç‹Ç∑" : "ãÛíÜÇ≈Ç∑");

        var moveValue = _move.ReadValue<Vector2>();
        _moveVelocity.x = moveValue.x * moveSpeed;
        _moveVelocity.z = moveValue.y * moveSpeed;

        _transform.LookAt(_transform.position + new Vector3(_moveVelocity.x, 0, _moveVelocity.z));

        _moveVelocity.y += Physics.gravity.y * Time.deltaTime;
        _characterController.Move(_moveVelocity * Time.deltaTime);

        //animator.SetFloat("MoveSpeed", new Vector3(_moveVelocity.x, 0, _moveVelocity.z).magnitude);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SceneTransitionManager.Instance.TransitionToBattleScene();
        }
    }
}
