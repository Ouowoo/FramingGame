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

        // —— 种子（20001～20999）——
        ParsnipSeeds = 20001,
        Stone1Seed = 20002,
        Stone2Seed = 20003,

        // —— 作物（21001～21999）——
        Parsnip = 21001,
        Sweetcorn = 21002,
        Pumpkin = 21003,

        // —— 材料（30001～39999）——
        Wood = 30001,
        Stone = 30002,
        Acorn = 30003,
        PineCone = 30004,

        // —— 预留：食物 ——
        // —— 预留：其他 ——
    }
}
