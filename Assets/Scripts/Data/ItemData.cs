using System;
using UnityEngine;

namespace Farm.Data
{
    /// <summary>
    /// 物品配置基类：所有物品共用的基础数据。
    /// 为什么用 ScriptableObject：一个物品一份 .asset 文件，改数值不用动代码；
    /// 不同存档加载同一份配置，内存中只有一份，不会每次读盘都 new。
    /// 为什么字段 private + 属性只读：外部只能拿值不能改，数据流向单向（SO → 使用方）。
    /// </summary>
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

        [Header("堆叠与价格")]
        [SerializeField, Tooltip("最大堆叠数（99=种子/材料，1=工具）")]
        private int maxStack = 99;

        [SerializeField, Tooltip("售价（0=不可卖）")]
        private int sellPrice;

        [SerializeField, Tooltip("购买价（0=不可购）")]
        private int buyPrice;

        [Header("交互")]
        [SerializeField, Tooltip("是否可直接拾取（场景物件需工具处理→false）")]
        private bool canBePickedUp = true;

        // —— 公共只读属性 ——
        public ItemID ItemID => itemID;
        public string ItemName => itemName;
        public ItemType ItemType => itemType;
        public Sprite Icon => icon;
        public int MaxStack => maxStack;
        public int SellPrice => sellPrice;
        public int BuyPrice => buyPrice;
        public bool CanBePickedUp => canBePickedUp;

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
            return $"{itemName}\n" +
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

        protected void SetItemType(ItemType value)
        {
            itemType = value;
        }

        /// <summary>
        /// 供子类强制关闭拾取（场景物件不可直接拾取，必须工具处理）。
        /// 为什么用 SetXXX 模式：与 SetMaxStack/SetItemType 一致，字段 private
        /// 无法被子类直接赋值，受控入口是最小暴露面。
        /// 为什么默认 true：绝大多数物品（种子/材料/收获物）都能拾取，
        /// 默认开启让 Item 菜单建资产时不用手动设。
        /// </summary>
        protected void SetCanPickUp(bool value)
        {
            canBePickedUp = value;
        }
    }
}
