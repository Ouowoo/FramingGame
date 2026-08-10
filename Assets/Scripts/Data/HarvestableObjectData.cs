using UnityEngine;

namespace Farm.Data
{
    /// <summary>
    /// 场景交互物件：需特定工具才能处理（割草/敲石），处理后有掉落。
    /// 为什么单独建子类而不是在 ItemData 加字段：架构已有 ToolData 子类先例，
    /// "需要工具才能交互"是独立行为维度，子类模型一致——每个特殊行为类型
    /// 建独立子类，避免 ItemData 字段膨胀（种子带着 requiredTool=Hoe 没意义）。
    /// </summary>
    [CreateAssetMenu(fileName = "NewHarvestable", menuName = "Farm/Harvestable Object")]
    public class HarvestableObjectData : ItemData
    {
        [Header("交互属性")]
        [SerializeField, Tooltip("处理此物件需要的工具")]
        private ToolType requiredTool;

        [SerializeField, Tooltip("处理后的掉落物品（可为空）")]
        private ItemData dropItem;

        [SerializeField, Tooltip("掉落数量（敲碎/收割后掉几个）")]
        private int dropCount = 1;

        // —— 公共只读属性 ——
        public ToolType RequiredTool => requiredTool;
        public ItemData DropItem => dropItem;
        public int DropCount => dropCount;

        // Reset：自动归为 Other + 不可直接拾取。
        // 为什么用 SetCanPickUp(false) 而不是 override 属性：字段 private，
        // 子类覆写属性拿不到 backing field，走 protected 入口与 SetMaxStack 一致。
        private void Reset()
        {
            SetItemType(ItemType.Interact);
            SetCanPickUp(false);
        }
    }
}
