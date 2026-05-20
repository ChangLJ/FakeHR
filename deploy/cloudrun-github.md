# 從 GitHub 自動部署至 GCP Cloud Run

儲存庫：https://github.com/ChangLJ/FakeHR

推送 `main` 分支後，由 **Cloud Build Trigger** 自動建置並部署至 Cloud Run（無需本機 `docker build`）。

## 架構

```
GitHub (FakeHR)  push main
       ↓
Cloud Build Trigger  →  cloudbuild.yaml
       ↓
Artifact Registry  →  Cloud Run (API + Web)
```

| 服務 | Cloud Run 名稱 | 說明 |
|------|----------------|------|
| API | `humanresource-api` | ASP.NET Core，port 8080 |
| 前端 | `humanresource-web` | Nginx 靜態，直連 API（CORS） |

---

## 一、一次性設定（GCP 主控台 + gcloud）

### 1. 設定專案

```powershell
gcloud config set project 你的專案ID
```

### 2. 啟用 API

```powershell
gcloud services enable run.googleapis.com artifactregistry.googleapis.com cloudbuild.googleapis.com sourcerepo.googleapis.com
```

### 3. 建立 Artifact Registry

```powershell
gcloud artifacts repositories create humanresource `
  --repository-format=docker `
  --location=asia-east1
```

### 4. 授權 Cloud Build 部署 Cloud Run

```powershell
$projectId = gcloud config get-value project
$projectNumber = gcloud projects describe $projectId --format="value(projectNumber)"
$cbSa = "$projectNumber@cloudbuild.gserviceaccount.com"

gcloud projects add-iam-policy-binding $projectId `
  --member="serviceAccount:$cbSa" `
  --role="roles/run.admin"

gcloud projects add-iam-policy-binding $projectId `
  --member="serviceAccount:$cbSa" `
  --role="roles/iam.serviceAccountUser"

gcloud projects add-iam-policy-binding $projectId `
  --member="serviceAccount:$cbSa" `
  --role="roles/artifactregistry.writer"
```

---

## 二、連結 GitHub 儲存庫

### 方式 A：GCP 主控台（建議）

1. 開啟 [Cloud Build → 儲存庫](https://console.cloud.google.com/cloud-build/repositories)
2. **建立存放區** → 選 **GitHub (Cloud Build GitHub App)**
3. 授權並選擇 **ChangLJ/FakeHR**
4. 建立 **觸發條件 (Trigger)**：
   - 名稱：`deploy-fakehr-main`
   - 事件：**推送至分支**
   - 分支：`^main$`
   - 設定檔：`cloudbuild.yaml`（或第一次先用 `cloudbuild.api.yaml`）
   - 位置：儲存庫根目錄

### 方式 B：gcloud（需先完成 GitHub App 連線）

```powershell
gcloud builds triggers create github `
  --name="deploy-fakehr-main" `
  --repo-name="FakeHR" `
  --repo-owner="ChangLJ" `
  --branch-pattern="^main$" `
  --build-config="cloudbuild.yaml" `
  --substitutions="_JWT_SECRET=請換成至少32字元的密鑰"
```

---

## 三、Trigger 必填 Substitution

在 Trigger 的 **Substitution variables** 新增：

| 變數 | 範例 | 說明 |
|------|------|------|
| `_JWT_SECRET` | `Your-Production-Jwt-Secret-32chars!!` | **必填**，JWT 簽章 |
| `_REGION` | `asia-east1` | 選填，預設已是 asia-east1 |
| `_SERVICE_API` | `humanresource-api` | 選填 |
| `_SERVICE_WEB` | `humanresource-web` | 選填 |

> 勿將 `_JWT_SECRET` 寫進 GitHub 程式碼；僅放在 Trigger 或 [Secret Manager](https://cloud.google.com/build/docs/securing-builds/use-secrets)。

### 使用 Secret Manager（建議正式環境）

```powershell
echo -n "Your-Production-Jwt-Secret-32chars!!" | gcloud secrets create hr-jwt-secret --data-file=-
```

在 Trigger 的「可用密碼」綁定 `hr-jwt-secret`，並在 `cloudbuild.yaml` 改為 `--set-secrets`（進階，可後續再調整）。

---

## 四、部署流程建議

### 第一次（僅 API）

1. Trigger 的設定檔改為：`cloudbuild.api.yaml`
2. 手動執行 Trigger 或 `git push`
3. 查看 API 網址：

```powershell
gcloud run services describe humanresource-api --region asia-east1 --format="value(status.url)"
```

### 之後（API + 前端）

1. Trigger 設定檔改為：`cloudbuild.yaml`
2. 每次 push `main` 會自動：
   - 建置並部署 API
   - 取得 API URL → 建置前端（`VITE_API_URL`）
   - 部署前端
   - 更新 API 的 CORS 為前端網址

---

## 五、手動觸發建置（不 push 也可）

```powershell
gcloud builds triggers run deploy-fakehr-main --branch=main
```

或主控台：Cloud Build → 觸發條件 → **執行**

---

## 六、查看結果

```powershell
gcloud run services list --region asia-east1
```

- 前端 URL → 瀏覽器開啟登入頁
- API URL + `/swagger`（Production 預設可能關閉 Swagger）

建置紀錄：

```powershell
gcloud builds list --limit=5
```

---

## 七、注意事項

1. **SQLite**：Cloud Run 未掛載 Volume 時，重啟後資料會清空；正式環境請規劃持久化或改用 Cloud SQL。
2. **CORS**：`cloudbuild.yaml` 會在部署後自動把前端 URL 寫入 API 的 `Cors__Origins__0`。
3. **示範帳號**：`D123456789` / `demo1234` 僅供測試。
4. **費用**：Cloud Build + Cloud Run 依使用量計費，請留意 GCP 帳單。

---

## 八、常見錯誤

| 現象 | 處理 |
|------|------|
| Permission denied (run.deploy) | 執行上方 IAM 授權給 Cloud Build SA |
| `_JWT_SECRET` 未設定 | 在 Trigger 補上 Substitution |
| 前端無法呼叫 API | 確認 API CORS 含前端 URL；重新跑完整 `cloudbuild.yaml` |
| GitHub 連不上 | 重新安裝 Cloud Build GitHub App 並授權 FakeHR |
