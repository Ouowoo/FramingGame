using Farm.Core;
using UnityEngine.UIElements;

namespace Farm.Managers
{
    /// <summary>
    /// 输入服务：封装 Unity 旧版 Input，所有模块通过 IInputManager 读输入。
    /// 注册/注销/跨场景存活由 ManagerBase 统一处理，这里只放业务。
    /// </summary>
    public class InputManager : ManagerBase<IInputManager>, IInputManager
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
