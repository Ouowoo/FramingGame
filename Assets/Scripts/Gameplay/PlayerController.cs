using Farm.Core;
using UnityEngine;

namespace Farm.Gameplay
{
    /// <summary>
    /// 玩家移动控制器：通过 ServiceLocator 获取输入，避免直接依赖 Input 类。
    /// 为什么 velocity 放 FixedUpdate：物理驱动移动，速度稳定、不受帧率波动影响；
    /// 为什么翻转/动画放 Update：它们是视觉表现，跟渲染帧走更顺滑。
    /// </summary>

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Tooltip("走路速度")]
        private float walkSpeed = 5f;

        [SerializeField, Tooltip("跑步速度")]
        private float runSpeed = 8f;

        [SerializeField]
        private Rigidbody2D rb;

        [SerializeField]
        private Animator anim;
        private IInputManager input;

        // 为什么记录初始缩放：翻转时只改符号，避免抹掉 prefab 设定的非 1 缩放。
        private Vector3 originalScale;

        // 为什么单独记录朝向：松开方向键后仍需保持最后移动方向，不能直接取 MoveInput.x（会归零）。
        private float lastFacingDirection = 1f;

        private void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();   // 没拖就自动补
            if (anim == null) anim = GetComponent<Animator>();  // 没拖就自动补
        }

        private void Start()
        {
            // Start 保证在所有 Awake 之后执行：此时 GameManager 已创建并注册 InputManager。
            input = ServiceLocator.Get<IInputManager>();
            originalScale = transform.localScale;
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

            // 有水平输入时更新朝向（1=右，-1=左）。
            if (Mathf.Abs(move.x) > 0.01f)
            {
                lastFacingDirection = Mathf.Sign(move.x);
            }

            // 用记录的初始缩放乘符号：翻转不丢失 prefab 设定的非 1 缩放。
            Vector3 scale = originalScale;
            scale.x = originalScale.x * lastFacingDirection;
            transform.localScale = scale;

            // 动画参数 Speed：走路 0~1，跑步翻倍到 0~2，驱动 Blend Tree（Idle 0 / Walk 1 / Run 2）。
            if (anim != null)
            {
                float speedValue = move.magnitude * (input.IsRunning ? 2f : 1f);
                anim.SetFloat("Speed", speedValue);
            }
        }
    }
}
