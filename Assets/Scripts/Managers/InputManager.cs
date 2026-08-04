using Farm.Core;
using UnityEngine;

namespace Farm.Managers
{
    /// <summary>
    /// 输入服务：封装 Unity 旧版 Input，所有模块通过 IInputManager 读输入。
    /// 注册/注销/跨场景存活由 ManagerBase 统一处理，这里只放业务。
    /// 为什么输入全在 Update 读：离散事件（点按）和瞬时状态若放 FixedUpdate 可能漏帧。
    /// </summary>
    public class InputManager : ManagerBase<IInputManager>, IInputManager
    {
        // 私有 set：外部只读，状态只在 Update 里更新。
        public Vector2 MoveInput { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsPaused { get; private set; }

        private void Update()
        {
            // 为什么用 GetAxisRaw 而不是 GetAxis：不需要平滑/惯性，离散值（-1/0/1）手感更干脆。
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // 四方向裁决：禁止斜向移动，斜向按键时只保留一个轴。
            // 为什么水平优先：左右转向的操作频率高于上下，冲突时优先水平方向；
            // 若想改为垂直优先或最后按键优先，只需调整这里的取舍顺序。
            // 为什么不需要归一化：裁决后向量恒为轴对齐单位向量（如 (1,0)/(0,-1)）或零向量。
            Vector2 move = Vector2.zero;
            if (horizontal != 0f)
            {
                move.x = Mathf.Sign(horizontal);
            }
            else if (vertical != 0f)
            {
                move.y = Mathf.Sign(vertical);
            }
            MoveInput = move;

            // Shift 点按取反：Fire3 是 Unity InputManager 默认的左 Shift 绑定，无需额外配置。
            if (Input.GetButtonDown("Fire3"))
            {
                IsRunning = !IsRunning;
            }

            // ESC 切换暂停：Cancel 是 Unity InputManager 默认的 Escape 绑定，供后续暂停菜单使用。
            if (Input.GetButtonDown("Cancel"))
            {
                IsPaused = !IsPaused;
            }
        }
    }
}
