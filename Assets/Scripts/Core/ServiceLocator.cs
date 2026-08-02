using System;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Core
{
    /// <summary>
    /// 服务注册中心：所有全局服务（Manager）在这里注册，其他模块按接口获取。
    /// 为什么需要它：把依赖从"代码里的硬引用"变成"运行时的查询"，
    /// 任何模块只认识接口、不认识具体实现，可替换、可测试。
    /// </summary>
    public static class ServiceLocator
    {
        // 为什么用 Type 作键：注册/获取都按接口类型定位，一处注册全项目可用；
        // 为什么值是 IGameService 而不用 object：工牌约束保证容器里只会有游戏服务。
        private static readonly Dictionary<Type, IGameService> Services = new Dictionary<Type, IGameService>();

        // 服务可能在非主线程被访问（后续资源加载等），统一加锁避免字典并发读写损坏。
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// 注册服务实例。
        /// 为什么重复注册用"覆盖 + 警告"而不是报错：热重载/场景重进时 Manager
        /// 可能被创建两次，覆盖能让游戏继续跑，警告则提醒开发者这里有问题。
        /// </summary>
        public static void Register<T>(T service) where T : IGameService
        {
            if (service == null)
            {
                Debug.LogError($"[ServiceLocator] 注册失败：服务 {typeof(T).Name} 为 null。");
                return;
            }

            lock (SyncRoot)
            {
                if (Services.ContainsKey(typeof(T)))
                {
                    Debug.LogWarning($"[ServiceLocator] 重复注册服务 {typeof(T).Name}，已用新实例覆盖。");
                }
                Services[typeof(T)] = service;
            }
        }

        /// <summary>
        /// 获取已注册的服务实例。
        /// 为什么未注册直接抛异常（fail fast）：服务没注册就有人要，是必现的逻辑错误，
        /// 早炸比后面到处 NullReferenceException 好定位得多。
        /// </summary>
        public static T Get<T>() where T : IGameService
        {
            lock (SyncRoot)
            {
                if (Services.TryGetValue(typeof(T), out IGameService service))
                {
                    return (T)service;
                }
                Debug.LogError($"[ServiceLocator] 未注册服务 {typeof(T).Name}，请检查 GameManager 的注册顺序。");
                throw new InvalidOperationException($"服务 {typeof(T).Name} 尚未注册。");
            }
        }

        /// <summary>
        /// 注销服务实例。
        /// 为什么成功移除要 Log 提示：注销通常发生在 Manager 销毁时，
        /// 日志能帮助确认生命周期确实执行到了。
        /// </summary>
        public static void Unregister<T>() where T : IGameService
        {
            lock (SyncRoot)
            {
                if (Services.Remove(typeof(T)))
                {
                    Debug.Log($"[ServiceLocator] 已注销服务 {typeof(T).Name}。");
                }
            }
        }

        /// <summary>清空所有注册：场景切换/编辑器热重载时防止旧服务悬挂。</summary>
        public static void Clear()
        {
            lock (SyncRoot)
            {
                Services.Clear();
            }
        }
    }
}
