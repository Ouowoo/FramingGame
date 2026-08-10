using UnityEngine;

namespace Farm.Data
{
    /// <summary>
    /// 物品配置基类：所有物品共用的基础数据。
    /// 为什么用 ScriptableObject：一个物品一份 .asset 文件，改数值不用动代码；
    /// 不同存档加载同一份配置，内存中只有一份，不会每次读盘都 new。
    /// 为什么字段 private + 属性只读：外部只能拿值不能改，数据流向单向（SO → 使用方）。
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "NewItem", menuName = "Farm/Item")]
    public class ItemData : ScriptableObject
    {
        [Header("基础信息")]
        [SerializeField, Tooltip("物品唯一 ID")]
        private ItemID itemID;

        [SerializeField, Tooltip("物品名称")]
        private string itemName = "新物品";

        [SerializeField, Tooltip("物品大类")]
        private ItemType itemType;

        [SerializeField, Tooltip("图标")]
        private Sprite icon;

        [SerializeField, TextArea(2, 4), Tooltip("描述文本")]
        private string description;

        [Header("堆叠与价格")]
        [SerializeField, Tooltip("最大堆叠数（99=种子/材料，1=工具）")]
        private int maxStack = 99;

        [SerializeField, Tooltip("售价（0=不可卖）")]
        private int sellPrice;

        [SerializeField, Tooltip("购买价（0=不可购）")]
        private int buyPrice;

        // —— 公共只读属性 ——
        public ItemID ItemID => itemID;
        public string ItemName => itemName;
        public ItemType ItemType => itemType;
        public Sprite Icon => icon;
        public string Description => description;
        public int MaxStack => maxStack;
        public int SellPrice => sellPrice;
        public int BuyPrice => buyPrice;

        /// <summary>
        /// 获取显示名（子类可 override 加前缀，如"铁 斧头"）。
        /// </summary>
        public virtual string GetDisplayName()
        {
            return itemName;
        }

        /// <summary>
        /// 获取提示组合文本。
        /// 为什么是方法：调用时动态拼接——名字→描述→价格——每次结果可不同（子类 override）。
        /// </summary>
        public virtual string GetTooltip()
        {
            return $"{itemName}\n{description}\n" +
                   (sellPrice > 0 ? $"售价：{sellPrice}G\n" : "") +
                   (buyPrice > 0 ? $"购价：{buyPrice}G" : "");
        }

        /// <summary>
        /// 供子类 Reset 覆盖 maxStack（工具子类需设为 1）。
        /// 为什么是 protected 方法：maxStack 字段为 private，子类无法直接赋值，
        /// 暴露一个受控入口让 ToolData 在 Reset 时调。
        /// </summary>
        protected void SetMaxStack(int value)
        {
            maxStack = value;
        }
    }
}
