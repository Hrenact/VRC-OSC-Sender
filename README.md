<p align="center">
  <img src="! ReadMeAssets/IconForReadMe.png" alt="VRC OSC Sender Icon" width="128" height="128" />
</p>

<h1 align="center">VRC OSC Sender</h1>

<p align="center">
  由 Unity 制作的，向 <b>VRChat</b> 发送 <b>OSC</b> 数据的轻量工具。<br>
  支持 <b>Windows</b> 与 <b>Android</b> 双平台。
</p>

---

## 📡 数据类型
| 类型 | 说明 | 示例 |
|:--|:--|:--|
| **Bool** | 开 / 关 | true / false |
| **Int** | 整数值 | -50 ~ 120 |
| **Float** | 浮点值 | -1.0 ~ 1.0 |

---

## ✨ 特性
- 支持保存当前布局并加载他人分享的 **`.json`** 文件  
- Windows 与 Android 双端兼容  
- Android 要求系统版本 **≥ Android 10 (API 29)**  
- 简洁直观的 UI 设计，便于调试与演示  

---

## 🧩 二次开发指南
1. 下载本仓库的压缩包（`Download ZIP`），并解压到本地。  
2. 在 **Unity Hub** 中创建一个新项目，版本需 **≥ 2022.3.22f1**，模板选择 **2D (Built-In Render Pipeline)**。  
3. 打开新项目所在目录，进入其 `Assets` 文件夹。  
   示例路径：`E:\Unity\ProjectName\Assets`  
4. 将解压得到的文件内容移动至该 `Assets` 文件夹中。  
5. 打开 Unity，在 **Project** 窗口中展开 `Assets/Scenes` 目录：
   - 双击 **`OSC-Debugger.unity`** → 打开 Windows 平台场景  
   - 双击 **`OSC-Debugger Android.unity`** → 打开 Android 平台场景  

---

## 📜 协议
本项目基于 **MIT License** 授权。  
更多详情请参阅 [LICENSE 文件](https://github.com/Hrenact/VRC-OSC-Sender/blob/main/LICENSE)。

---

## 💬 反馈与贡献
欢迎通过 **Issues** 提交反馈或改进建议。  
如果你希望添加功能或修复问题，请提交 **Pull Request**，我会认真审阅每一份贡献。
