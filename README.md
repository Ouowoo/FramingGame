# 🌾 FarmingGame — 2D 像素农场经营 RPG

> 使用 Unity 自建轻量框架（ServiceLocator + EventBus）开发的 2D 农场经营游戏 Demo，参考《星露谷物语》《牧场物语》玩法。

![Unity](https://img.shields.io/badge/Unity-2021.3.45f2c1-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-9.0-green)
![License](https://img.shields.io/badge/License-MIT-yellow)

---

## 🎮 项目简介

一个 2D 像素风格的农场经营游戏 Demo，核心玩法是 **种田—收获—成长** 的循环：

```
移动 → 耕地 → 播种 → 浇水 → 作物随时间生长 → 收获 → 背包
→ （规划中：卖作物换金币 → 买种子/工具 → 扩大农田）
```

**项目定位**：求职作品集 Demo，展示自建游戏框架的架构设计能力。

## ✨ 当前功能（开发中）

| 状态 | 功能 |
|:---:|------|
| ✅ | 自建轻量框架（ServiceLocator + EventBus + Manager 分层） |
| ✅ | 2D 瓦片地图（Tilemap + Rule Tile 规则瓦片自动过渡） |
| ✅ | 基础农场场景（草地/水区域） |
| 🔨 | 玩家移动（WASD + 跑步切换） |
| 📋 | 种田系统（耕地/播种/浇水/收获）— 规划中 |
| 📋 | 背包/物品系统 — 规划中 |
| 📋 | 时间/季节系统 — 规划中 |
| 📋 | NPC 对话 — 规划中 |
| 📋 | 存档系统 — 规划中 |

## 🏗️ 技术架构

```
┌─────────────────────────────────────────────┐
│                 GameManager                 │  ← 应用入口
├─────────────────────────────────────────────┤
│  InputManager │ FarmingManager │ Inventory  │  ← 业务服务（各实现专属接口）
│  TimeManager  │ DialogueManager │ SaveMgr   │
├─────────────────────────────────────────────┤
│         ServiceLocator（服务注册中心）        │  ← Type 键 + 接口解耦
├─────────────────────────────────────────────┤
│           EventBus（事件总线）                │  ← struct 事件广播
└─────────────────────────────────────────────┘
```

**架构亮点：**

- 🧩 **ServiceLocator**：服务通过专属接口注册/获取（Type 键一致性），模块完全解耦
- 📡 **EventBus**：struct 事件发布/订阅（快照复制 + try-catch 隔离），跨系统广播
- 🧬 **ManagerBase\<T\>**：泛型基类统一"注册/注销/跨场景存活"
- 💾 **数据驱动**：作物/物品用 ScriptableObject 配置（规划中）
- 🔥 **热更新预留**：接口隔离 + 程序集划分设计

## 📁 目录结构

```
Assets/
├── Art/                    # 美术资源
│   ├── Sprites/            # 精灵（角色/作物/工具/UI，13 类）
│   ├── Animation/          # 动画（玩家/NPC/作物）
│   └── Tilemap/            # 瓦片资源 + 调色板
├── Audio/Sounds/           # 音频（环境/音效/音乐）
├── Fonts/                  # 字体
├── Scenes/                 # 场景
├── Scripts/
│   ├── Core/               # 框架核心（ServiceLocator/EventBus/接口）
│   ├── Managers/           # 业务服务（GameManager/InputManager...）
│   ├── Gameplay/           # 玩法（PlayerController...）
│   ├── UI/                 # UI 层
│   └── Editor/             # 编辑器工具
├── Configs/                # ScriptableObject 配置（规划中）
└── Resources/              # 运行时加载资源（规划中）
```

## 🚀 运行方式

1. 使用 **Unity 2021.3.45f2c1** 打开本项目
2. 打开 `Assets/Scenes/SampleScene.unity`
3. 点击 Play

## 📝 开发日志

开发日志记录在项目文档中，按日期整理：

- [2026-08-02：项目启动，框架移植](docs/日志/2026-08-02.md)
- [2026-08-05：基础地图，规则瓦片](docs/日志/2026-08-05.md)

> 日志同步自 Obsidian 笔记，包含每日目标、技术笔记、踩坑记录、下一步计划。

## 🛠️ 技术栈

| 项目 | 选型 |
|------|------|
| 引擎 | Unity 2021.3.45f2c1（内置渲染管线，2D） |
| 语言 | C# 9.0 |
| IDE | Rider |
| 版本控制 | Git + SourceTree |
| 素材 | 课程教程素材包（仅美术/音频，代码全部自写） |

## 📅 路线图

- [ ] P1：玩家移动 + 动画 + 摄像机跟随
- [ ] P2：种田核心（耕地/播种/浇水/收获）
- [ ] P3：背包 + 工具栏 + UI
- [ ] P4：时间/季节 + 存档
- [ ] P5：NPC 对话 + 商店 + 打磨

---

## ⚠️ 说明

- 本项目为**学习/求职作品**，素材来自课程教程配套资源包
- 游戏逻辑代码（框架/玩法）全部为自写实现，未使用教程脚本
