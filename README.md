# SceneEffectsPresetsPatch

Koikatsu Sunshine / CharaStudio 向けの BepInEx 5 プラグインです。  
[Scene Effects Presets](https://github.com/ShalltyB/SceneEffectsPresets)（`KKS_SceneEffectsPresets.dll`）を改変せず、Harmony パッチでウィンドウ位置・サイズの記憶を追加します。

## 機能

- Scene Effects Presets UI ウィンドウの位置・サイズをセッション間で記憶
- Mix Presets パネルの表示状態も保存（幅の倍率変更に対応）
- 解像度変更時は画面内にクランプして復元

## 前提

`KKS_SceneEffectsPresets.dll` を `BepInEx/plugins/` に配置してください。  
本プラグインは元 DLL の上にパッチを当てる補助プラグインです。元 DLL が無い場合は何もしません。

## Build

```bash
make dll
```

または

```bash
dotnet build -c Release
```

ゲームのインストール先は `KKSPath` または環境変数 `KKS_PATH` で上書きできます。

## Output

```text
bin/Release/SceneEffectsPresetsPatch.dll
```

## Install

```text
KKS_SceneEffectsPresets.dll
SceneEffectsPresetsPatch.dll
↓
Koikatsu Sunshine/BepInEx/plugins/
```

## Config

`BepInEx/config/local.kks.sceneeffectspresets.windowmemory.cfg`

```ini
[Window Memory]

## Remember Scene Effects Presets window position and size between Studio sessions.
# Setting type: Boolean
# Default value: true
Enabled = true
```

`Enabled = false` にすると保存・復元を行いません。
