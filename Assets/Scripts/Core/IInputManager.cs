using UnityEngine;

namespace Farm.Core
{
    public interface IInputManager : IGameService
    {
        /// <summary>
        /// 移动输入：WASD/方向键，四方向裁决（斜向按键只保留水平轴），
        /// 值为轴对齐单位向量（如 (1,0)/(0,-1)）或零向量。
        /// 为什么是属性而不是方法：输入是"每帧可读的当前状态"，
        /// 属性语义比方法调用更贴合"查询现在是什么"。
        /// </summary>
        Vector2 MoveInput { get; }

        /// <summary>Shift 点按切换跑步状态（点按取反，不是按住）。</summary>
        bool IsRunning { get; }

        /// <summary>ESC 切换暂停状态（供暂停菜单用，当前只记录状态）。</summary>
        bool IsPaused { get; }
    }
}
