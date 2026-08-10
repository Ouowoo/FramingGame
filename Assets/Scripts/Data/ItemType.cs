namespace Farm.Data
{
    /// <summary>
    /// 物品大类。
    /// 为什么 Seed 和 Plant 分两类：种子种下后长成作物，产物需要独立的类型值；
    /// 背包按类型分页过滤时，种子和收获物不能混在同一标签下，否则购买/收获全堆一起。
    /// </summary>
    public enum ItemType
    {
        /// <summary>种子：撒在耕地上种出作物。</summary>
        Seed,
        /// <summary>作物收获物：从成熟作物上摘取的产物（非种子，不可种）。</summary>
        Plant,
        /// <summary>工具：锄头/水壶/斧/镐/镰刀/篮子。</summary>
        Tool,
        /// <summary>材料：木头/石头/矿石等建造/合成用。</summary>
        Material,
        /// <summary>交互物品：使用工具交互的物品。</summary>
        Interact,
        /// <summary>其他：任务道具/钥匙等不归类物品。</summary>
        Other,
    }
}
