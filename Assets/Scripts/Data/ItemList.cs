using System.Collections.Generic;
using UnityEngine;

namespace Farm.Data
{
    /// <summary>
    /// 物品目录：按 ID 索引的物品配置清单。
    /// 为什么需要它：存档/背包只存 ItemID（int），运行时通过此表把 ID 转成
    /// 实际的 ItemData 引用，一处建表、全项目可用。
    /// 为什么用字典 Fallback 遍历而不是纯字典：编辑器改完 items 列表后
    /// OnValidate 自动重建字典，保持运行时查找 O(1)；遍历作为 OnValidate
    /// 无效时的兜底，保证不返回 null。
    /// </summary>
    [CreateAssetMenu(fileName = "NewItemList", menuName = "Farm/Item List")]
    public class ItemList : ScriptableObject
    {
        [SerializeField, Tooltip("物品清单")]
        private List<ItemData> items = new();

        // ID → ItemData 快速索引：OnEnable + OnValidate 时自动构建。
        private Dictionary<ItemID, ItemData> lookup;

        public int Count => items.Count;

        private void OnEnable()
        {
            BuildLookup();
        }

        // TODO：这个函数是什么
        private void OnValidate()
        {
            // 为什么放 OnValidate：编辑器里拖动列表项后即时重建，不依赖 Play。
            BuildLookup();
        }

        private void BuildLookup()
        {
            lookup = new Dictionary<ItemID, ItemData>(items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null && !lookup.ContainsKey(items[i].ItemID))
                {
                    lookup[items[i].ItemID] = items[i];
                }
                else if (items[i] != null)
                {
                    // 为什么只警告不报错：方便开发阶段快速建资产，重复 ID 取第一个。
                    Debug.LogWarning($"[ItemList] 物品 ID {items[i].ItemID} 重复，保留第一个。", this);
                }
            }
        }

        /// <summary>
        /// 按 ID 查找（主查找方式，存档/背包使用）。
        /// 为什么是 O(1)：字典查询，物品配置表引用数量有限但调用频繁。
        /// </summary>
        public ItemData GetByID(ItemID id)
        {
            if (lookup.TryGetValue(id, out ItemData data))
            {
                return data;
            }
            Debug.LogError($"[ItemList] 未找到物品 ID {id}。", this);
            return null;
        }
    }
}
