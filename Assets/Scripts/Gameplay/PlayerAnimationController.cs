using UnityEngine;

namespace Farm.Gameplay
{
    /// <summary>
    /// 分层角色动画控制器：统一驱动 Player 所有部位的 Animator。
    /// 为什么需要它：角色是分层结构（Body/Hair/Hat/Arms/Tool/ToolEffect 各挂一个 Animator），
    /// 6 个部位必须同步切换动画，只驱动单个 Animator 会导致部位不同步。
    /// 为什么叫 Controller 而不是 Animator：它不继承 Animator，它是驱动 Animator 的控制器。
    /// </summary>
    public class PlayerAnimationController : MonoBehaviour
    {
        // 为什么分成两组：Body/Hair/Hat/Arms 共用移动参数体系（xInput/yInput/isWalking/isRunning/idle*），Tool/ToolEffect 是独立的工具动画参数体系，混在一起会导致 tool/toolEffect 被错误写入移动参数。
        [Header("移动动画（Body/Hair/Hat/Arms）")]
        [SerializeField]
        private Animator[] moveAnimators = new Animator[0];

        [Header("工具动画（Tool/ToolEffect）")]
        [SerializeField]
        private Animator[] toolAnimators = new Animator[0];

        // 为什么记录最后方向：停下时要触发对应方向的 idle Trigger（idleUp/Down/Left/Right）。
        private Vector2 lastDirection = Vector2.down;  // 默认朝下（星露谷风格初始朝向）

        /// <summary>设置移动方向：写入 xInput/yInput（Float），状态机按 ±0.01 阈值判定 4 方向。</summary>
        public void SetMoveDirection(Vector2 dir)
        {
            lastDirection = dir;
            for (int i = 0; i < moveAnimators.Length; i++)
            {
                moveAnimators[i].SetFloat("xInput", dir.x);
                moveAnimators[i].SetFloat("yInput", dir.y);
            }
        }

        /// <summary>走路：isWalking/isRunning 是 Bool（m_Type=4）而非 Trigger，用 SetBool 切换，跑步必须互斥置 false。</summary>
        public void SetWalking()
        {
            for (int i = 0; i < moveAnimators.Length; i++)
            {
                moveAnimators[i].SetBool("isWalking", true);
                moveAnimators[i].SetBool("isRunning", false);
            }
        }

        /// <summary>跑步：与走路互斥，避免两个 Bool 同时为 true 导致状态机条件冲突。</summary>
        public void SetRunning()
        {
            for (int i = 0; i < moveAnimators.Length; i++)
            {
                moveAnimators[i].SetBool("isWalking", false);
                moveAnimators[i].SetBool("isRunning", true);
            }
        }

        /// <summary>
        /// 待机：移动 Bool 全部置 false 回到 Idle 层，并触发最后方向的 idle Trigger——
        /// Idle 的 4 方向由 idleUp/Down/Left/Right（Trigger）决定，不读 xInput/yInput，
        /// 所以停下后必须补一次 Trigger 才能保持站立朝向。Trigger 单次消费，每帧重复触发无害。
        /// </summary>
        public void SetIdle()
        {
            for (int i = 0; i < moveAnimators.Length; i++)
            {
                moveAnimators[i].SetBool("isWalking", false);
                moveAnimators[i].SetBool("isRunning", false);
                moveAnimators[i].SetFloat("xInput", 0f);
                moveAnimators[i].SetFloat("yInput", 0f);

                if (lastDirection.y > 0.01f)       moveAnimators[i].SetTrigger("idleUp");
                else if (lastDirection.y < -0.01f) moveAnimators[i].SetTrigger("idleDown");
                else if (lastDirection.x > 0.01f)  moveAnimators[i].SetTrigger("idleRight");
                else if (lastDirection.x < -0.01f) moveAnimators[i].SetTrigger("idleLeft");
            }
        }
    }
}
