using Farm.Core;
using UnityEngine;

namespace Farm.Managers
{
    /// <summary>
    /// 所有 Manager 的公共基类：统一"注册/注销自身"和"跨场景存活"。
    /// </summary>
    public abstract class ManagerBase<T> : MonoBehaviour, IGameService where T : class, IGameService
    {
        protected virtual void Awake()
        {
            // this as T：基类无法在编译期确认子类实现了 T，用 as 安全转换，
            // 子类漏实现接口时得到 null，Register 的 null 检查会 LogError 暴露。
            ServiceLocator.Register<T>(this as T);
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnDestroy()
        {
            // 传自身实例：Unregister 校验"当前注册的正是我"才删，
            // 防止同类型残留实例互相误删注册。
            ServiceLocator.Unregister<T>(this as T);
        }
    }
}