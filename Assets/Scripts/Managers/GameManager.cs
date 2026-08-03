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
            CreateManagerObject<InputManager>("InputManager");
        }

        /// <summary>创建 Manager 的 GameObject 并挂载组件。</summary>
        private static T CreateManagerObject<T>(string managerName) where T : Component
        {
            GameObject managerGo = new GameObject(managerName);
            return managerGo.AddComponent<T>();
        }
    }
}
