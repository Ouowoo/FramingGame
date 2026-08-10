using UnityEngine;

namespace Farm.Data
{
    /// <summary>
    /// 工具配置：继承 ItemData，附加工具专用的功能参数。
    /// 为什么单独建子类而不是加 bool IsTool：每个工具需要 toolType/toolLevel
    /// /energyCost/range/useSound 五个额外字段，塞基类会让种子/食物也带着无意义的
    /// toolType=Hoe，浪费内存且语义混乱。子类是最干净的特化方式。
    /// </summary>
    [CreateAssetMenu(fileName = "NewTool", menuName = "Farm/Tool")]
    public class ToolData : ItemData
    {
        [Header("工具属性")] [SerializeField, Tooltip("工具功能类型")]
        private ToolType toolType;

        [SerializeField, Tooltip("使用范围（1=单格）")]
        private int range = 1;

        [Header("音效")] [SerializeField, Tooltip("挥舞/使用音效")]
        private AudioClip useSound;

        // —— 公共只读属性 ——
        public ToolType ToolType => toolType;
        public int Range => range;
        public AudioClip UseSound => useSound;

        // 为什么用 Reset()：Unity 编辑器在 CreateAssetMenu 创建 .asset 时自动调用，
        // 保证每个新建工具资产 maxStack=1（工具不可堆叠），不用手动改。
        private void Reset()
        {
            SetMaxStack(1);
            SetItemType(ItemType.Tool);
        }
    }
}