namespace Farm.Data
{
    /// <summary>
    /// 物品唯一 ID。
    /// 为什么 None=0：default(ItemID) 隐含 0 值，若 0 对应实际物品会导致
    /// "未赋值"和"第一个物品"语义冲突，None 占 0 位可明确区分。
    /// 为什么用枚举不用字符串：编译期拼写检查、存档存 int 稳定（不因改名断档）、
    /// switch 穷举可读。
    /// </summary>
    public enum ItemID
    {
        /// <summary>空槽位。</summary>
        None = 0,

        // —— 工具 ——
        Hoe = 10001,
        WateringCan = 10002,
        Axe = 10003,
        Pickaxe = 10004,
        Scythe = 10005,
        Basket = 10006,

        // —— 预留：种子（CarrotSeed / CabbageSeed / ...）——
        // —— 预留：作物（Carrot / Cabbage / ...）——
        // —— 预留：材料（Wood / Stone / ...）——
        // —— 预留：食物 ——
        // —— 预留：其他 ——
    }
}
