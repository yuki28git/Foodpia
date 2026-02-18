# Foodpia

## 概要

本プロジェクトは Unity を使用したガチャアプリ開発プロジェクトです。  

---

## 開発環境

- Unityバージョン: 6000.0.66f1

---

## プロジェクト共通設定

### 1. Asset Serialization（Force Text）

#### 説明
アセット（Scene・Prefabなど）をテキスト形式で保存します。  
Gitで差分管理を可能にするため、必須設定です。

#### 設定方法
Edit > Project Settings > Editor > Asset Serialization : Force Text

---

### 2. Visible Meta Files

#### 説明
.metaファイルを表示・管理します。  
アセット参照切れを防ぐため、必須設定です。

#### 設定方法
Edit > Project Settings > Editor > Version Control > Mode : Visible Meta Files

---

### 3. Active Input Handling（Input System Package）

#### 説明
入力処理方式を設定します。  
本プロジェクトでは新Input Systemを使用します。

#### 設定方法
Edit > Project Settings > Player > Active Input Handling : Input System Package (New)

---

### 4. Color Space

#### 説明
光の計算方式を設定します。  
3Dモデルを綺麗に表示するため Linear を使用します。

#### 設定方法
Edit > Project Settings > Player > Other Settings > Color Space : Linear

---

### 5. API Compatibility Level

#### 説明
使用する.NET APIレベルを設定します。  
Unity推奨の .NET Standard 2.1 を使用します。

#### 設定方法
Edit > Project Settings > Player > Other Settings > API Compatibility Level : .NET Standard 2.1

---

### 6. Render Pipeline（URP / Built-in）

#### 説明
レンダリング方式を設定します。  
要件等

#### 確認方法
Edit > Project Settings > Graphics

- Scriptable Render Pipeline Settings が未設定 → Built-in
- URP Asset が設定されている → URP

---
