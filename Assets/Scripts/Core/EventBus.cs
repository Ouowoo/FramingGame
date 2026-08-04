using System;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Core
{
    /// <summary>
    /// 事件总线：模块间通信用 struct 事件按类型发布/订阅，彼此不直接引用。
    /// 性能定位：EventBus 面向低频、跨系统的广播（时间/背包/对话变化），
    /// 高频每帧数据（输入、移动）请用直连调用，不要走事件。
    /// </summary>
    public static class EventBus
    {
        // 订阅者列表：key=事件类型，value=回调列表。
        private static readonly Dictionary<Type, List<Delegate>> Handlers = new Dictionary<Type, List<Delegate>>();

        // 正在发布中的事件类型：回调内再发同型事件会被拦截，防止无限递归。
        private static readonly HashSet<Type> Publishing = new HashSet<Type>();

        // 与 ServiceLocator 同理，防止多线程并发读写
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// 订阅某类型事件。
        /// </summary>
        public static void Subscribe<T>(Action<T> handler) where T : struct
        {
            // 1.参数校验
            if (handler == null)
            {
                Debug.LogError($"[EventBus] 订阅失败：{typeof(T).Name} 的处理器为 null。");
                return;
            }

            // 2.锁定线程
            lock (SyncRoot)
            {
                // 3.字典取值
                Type type = typeof(T);
                if (!Handlers.TryGetValue(type, out List<Delegate> list))
                {
                    // 4.没有列表 则 创建列表
                    list = new List<Delegate>();
                    Handlers[type] = list;
                }
                // 5.列表添加（Contains 只对同一委托实例去重；每次新建 lambda 会产生新实例导致重复订阅）
                if (!list.Contains(handler))
                {
                    list.Add(handler);
                }
            }
        }

        /// <summary>
        /// 发布事件，通知所有订阅者。
        ///
        /// 为什么复制快照再遍历：回调里如果订阅/退订会修改正在遍历的列表
        ///
        /// 为什么 try-catch 逐个隔离：一个处理器抛异常不该拖垮其他订阅者
        ///
        /// </summary>
        public static void Publish<T>(T eventData) where T : struct
        {
            Type type = typeof(T);
            // 1.复制快照
            List<Delegate> snapshot;
            // 2.锁定线程
            lock (SyncRoot)
            {
                // 3.重入检查：回调里再发同型事件会无限递归，直接忽略本次
                if (Publishing.Contains(type))
                {
                    Debug.LogWarning($"[EventBus] 检测到 {type.Name} 事件重入发布，本次已忽略。");
                    return;
                }

                // 4.字典取值
                if (!Handlers.TryGetValue(type, out List<Delegate> list))
                {
                    return;
                }
                // 5.复制快照（避免回调中修改列表）
                snapshot = new List<Delegate>(list);
                // 6.标记发布中
                Publishing.Add(type);
            }

            try
            {
                // 锁外执行回调：回调里再 Subscribe/Unsubscribe 不会与持锁冲突，也减少持锁时间。
                for (int i = 0; i < snapshot.Count; i++)
                {
                    try
                    {
                        ((Action<T>)snapshot[i])(eventData);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[EventBus] 处理 {type.Name} 事件时订阅者抛异常：{e}");
                    }
                }
            }
            finally
            {
                // 7.解除发布标记：异常也要清除，否则该类型后续无法再发布
                lock (SyncRoot)
                {
                    Publishing.Remove(type);
                }
            }
        }

        /// <summary>
        /// 取消订阅。
        ///
        /// 为什么列表空时删键：避免字典里堆积大量空列表，保持结构干净。
        ///
        /// </summary>
        public static void Unsubscribe<T>(Action<T> handler) where T : struct
        {
            // 1.锁定线程
            lock (SyncRoot)
            {
                // 2.字典取值
                Type type = typeof(T);
                if (Handlers.TryGetValue(type, out List<Delegate> list))
                {
                    // 3.列表移除
                    list.Remove(handler);
                    // 4.列表空时删键
                    if (list.Count == 0)
                    {
                        Handlers.Remove(type);
                    }
                }
            }
        }

        /// <summary>清空所有订阅：场景切换/测试重置时使用，防止旧场景订阅残留。</summary>
        public static void Clear()
        {
            lock (SyncRoot)
            {
                Handlers.Clear();
            }
        }
    }
}
