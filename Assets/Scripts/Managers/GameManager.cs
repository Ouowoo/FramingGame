using Farm.Core;
using UnityEngine;

namespace Farm.Managers
{
    /// <summary>
    /// 项目入口：顺序初始化所有 Manager 并注册到 ServiceLocator。
    /// 注册自身/跨场景存活由 ManagerBase 统一处理（base.Awake()），
    /// 这里只负责创建并初始化各业务 Manager。
    /// </summary>
    public class GameManager : ManagerBase<IGameManager>, IGameManager
    {
        protected override void Awake()
        {
            base.Awake();
            // 创建并初始化所有 Manager
            // 注意：这里创建挂载 Manager 时，在相应 Manager Awake() 中注册自身
            CreateManagerObject<InputManager>("InputManager");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        /// <summary>创建 Manager 的 GameObject 并挂载组件。</summary>
        private static T CreateManagerObject<T>(string managerName) where T : Component
        {
            GameObject managerGo = new GameObject(managerName);
            return managerGo.AddComponent<T>();
        }
    }
}
