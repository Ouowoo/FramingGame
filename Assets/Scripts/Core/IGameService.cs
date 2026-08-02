namespace Farm.Core
{
    /// <summary>
    /// 所有游戏服务的"工牌"接口。
    /// 为什么是空接口：ServiceLocator 用泛型约束 where T : IGameService
    /// 在编译期拦截非服务类型，保证容器里只会有游戏服务。
    /// 为什么不用抽象类：服务实现方可能是 MonoBehaviour 也可能是纯 C# 类，
    /// C# 单继承下抽象类会把实现方限制死，接口两者通吃。
    /// </summary>
    public interface IGameService
    {
    }
}
