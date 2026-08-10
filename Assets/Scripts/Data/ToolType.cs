namespace Farm.Data
{
    /// <summary>
    /// 工具功能类型。
    /// 为什么独立枚举而不是 ItemType.Tool 下的子分类：每个 ToolType 对应
    /// 一套动作（耕地/浇水/砍树/碎石/割草/收获），ToolsManager 用 switch 分发，
    /// 独立枚举让分发代码可读、可补，且 ToolData 需要一个类型判别字段。
    /// </summary>
    public enum ToolType
    {
        /// <summary>锄头 → 耕地。</summary>
        Hoe,
        /// <summary>水壶 → 浇水。</summary>
        WateringCan,
        /// <summary>斧头 → 砍树。</summary>
        Axe,
        /// <summary>镐 → 碎石/挖矿。</summary>
        Pickaxe,
        /// <summary>镰刀 → 割草/收割作物。</summary>
        Scythe,
        /// <summary>篮子 → 批量收获装入。</summary>
        Basket,
    }
}
