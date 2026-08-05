using Farm.Core;
using UnityEngine;

namespace Farm.Gameplay
{
    /// <summary>
    /// 玩家移动控制器：通过 ServiceLocator 获取输入，避免直接依赖 Input 类。
    /// 为什么 velocity 放 FixedUpdate：物理驱动移动，速度稳定、不受帧率波动影响；
    /// 为什么翻转/动画放 Update：它们是视觉表现，跟渲染帧走更顺滑。
    /// 为什么动画交给 PlayerAnimationController：角色是分层结构，必须由它统一驱动 6 个部位。
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Tooltip("走路速度")]
        private float walkSpeed = 5f;

        [SerializeField, Tooltip("跑步速度")]
        private float runSpeed = 8f;

        [SerializeField]
        private Rigidbody2D rb;

        [SerializeField]
        private PlayerAnimationController playerAnimationController;  // 手动拖入，挂 Player 根节点
        private IInputManager input;


        private void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();                              // 没拖就自动补
        }

        private void Start()
        {
            // Start 保证在所有 Awake 之后执行：此时 GameManager 已创建并注册 InputManager。
            input = ServiceLocator.Get<IInputManager>();
        }

        private void FixedUpdate()
        {
            // 物理步可能先于 Start 触发（激活对象的首个物理帧），此时 input 尚未赋值，跳过首帧。
            if (input == null)
            {
                return;
            }

            // IsRunning 为 true 时切到跑步速度；MoveInput 已裁决为轴对齐单位向量，斜向不会更快。
            float speed = input.IsRunning ? runSpeed : walkSpeed;
            rb.velocity = input.MoveInput * speed;
        }

        private void Update()
        {
            Vector2 move = input.MoveInput;


            // 动画驱动：方向参数必须先设置（Transition 需要 xInput/yInput 路由到对应方向的
            // Walk/Run 子状态），之后再按移动状态切换走路/跑步/待机，由 PlayerAnimationController 统一广播。
            if (playerAnimationController != null)
            {
                playerAnimationController.SetMoveDirection(move);

                if (move.magnitude > 0.01f)
                {
                    if (input.IsRunning)
                    {
                        playerAnimationController.SetRunning();
                    }
                    else
                    {
                        playerAnimationController.SetWalking();
                    }
                }
                else
                {
                    playerAnimationController.SetIdle();
                }
            }
        }
    }
}
