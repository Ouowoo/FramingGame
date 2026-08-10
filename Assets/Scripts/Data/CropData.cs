using UnityEngine;

namespace Farm.Data
{
    /// <summary>
    /// 作物配置：一种作物的完整生长链路（种子→生长→收获）。
    /// 为什么单独建子类：种子不只是"一种物品"，它关联了种下→阶段变化→
    /// 收获产物的完整生命周期，growTime/growthStages/growthSprites 在
    /// ItemData 里毫无意义，子类隔离是最干净的。
    /// </summary>
    [CreateAssetMenu(fileName = "NewCrop", menuName = "Farm/Crop")]
    public class CropData : ItemData
    {
        [Header("种植信息")]
        [SerializeField, Tooltip("对应的种子物品（种下去的就是这个 ItemData）")]
        private ItemData seedItem;

        [SerializeField, Tooltip("成熟后收获的产物")]
        private ItemData harvestItem;

        [SerializeField, Tooltip("总生长时间（天数）")]
        private float growTime = 5f;

        [SerializeField, Tooltip("生长阶段数（含种子期和成熟期）")]
        private int growthStages = 3;

        [Header("表现")]
        [SerializeField, Tooltip("每阶段对应图标（长度应等于 growthStages）")]
        private Sprite[] growthSprites;

        // —— 公共只读属性 ——
        public ItemData SeedItem => seedItem;
        public ItemData HarvestItem => harvestItem;
        public float GrowTime => growTime;
        public int GrowthStages => growthStages;
        public Sprite[] GrowthSprites => growthSprites;

        // 为什么 Reset 设 itemType=Seed：CropData 描述的是"种子的作物属性"，
        // 它本身就是种子的 SO 配置，建 .asset 时自动归为 Seed 大类。
        private void Reset()
        {
            SetItemType(ItemType.Seed);
            SetMaxStack(99);
        }
    }
}
