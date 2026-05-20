# 部署至 GCP Cloud Run

> **從 GitHub 自動部署**：請見 [cloudrun-github.md](./cloudrun-github.md)（連結 FakeHR → Cloud Build Trigger → Cloud Run）。

本專案使用 **SQLite 單檔資料庫**，適合容器化部署。API 映像已支援 Cloud Run 的 `PORT` 環境變數。

## 架構建議

| 元件 | 建議部署方式 |
|------|----------------|
| 後端 API | Cloud Run（本文件） |
| 前端 SPA | Cloud Run（靜態）或 Firebase Hosting / Cloud Storage + CDN |
| SQLite 資料 | Cloud Run **Volume** 掛載至 `/app/data`（需啟用） |

> **注意**：若未掛載 Volume，容器重啟後 SQLite 資料會重置。正式環境請掛載持久化 Volume，或定期備份 `humanresource.db`。

## 1. 建置並推送映像

```bash
# 設定專案 ID
export PROJECT_ID=your-gcp-project
export REGION=asia-east1
export SERVICE_NAME=humanresource-api

# 啟用 API
gcloud services enable run.googleapis.com artifactregistry.googleapis.com

# 建立 Artifact Registry（若尚未建立）
gcloud artifacts repositories create humanresource \
  --repository-format=docker \
  --location=$REGION

# 建置並推送（在專案根目錄）
gcloud builds submit ./backend \
  --tag $REGION-docker.pkg.dev/$PROJECT_ID/humanresource/api:latest
```

## 2. 部署 API 至 Cloud Run

```bash
gcloud run deploy $SERVICE_NAME \
  --image $REGION-docker.pkg.dev/$PROJECT_ID/humanresource/api:latest \
  --region $REGION \
  --platform managed \
  --allow-unauthenticated \
  --port 8080 \
  --memory 512Mi \
  --set-env-vars "ASPNETCORE_ENVIRONMENT=Production,DATABASE_PATH=/app/data/humanresource.db,Jwt__Secret=請替換為正式環境密鑰" \
  --set-env-vars "Cors__Origins__0=https://你的前端網域"
```

### 掛載 SQLite Volume（建議）

```bash
gcloud run deploy $SERVICE_NAME \
  --image $REGION-docker.pkg.dev/$PROJECT_ID/humanresource/api:latest \
  --region $REGION \
  --add-volume name=sqlite-vol,type=cloud-storage,bucket=YOUR_BACKUP_BUCKET \
  --add-volume-mount volume=sqlite-vol,mount-path=/app/data
```

或使用 Cloud Run 本機檔案系統 Volume（依 GCP 文件調整參數）。

## 3. 部署前端（選用）

```bash
gcloud builds submit ./frontend \
  --tag $REGION-docker.pkg.dev/$PROJECT_ID/humanresource/frontend:latest

gcloud run deploy humanresource-web \
  --image $REGION-docker.pkg.dev/$PROJECT_ID/humanresource/frontend:latest \
  --region $REGION \
  --allow-unauthenticated \
  --port 80
```

部署後請將前端的 API 請求指向 Cloud Run API 網址，或於 `frontend/nginx.conf` 調整 `proxy_pass`。

## 4. 本機 Docker 測試（模擬 Cloud Run）

```bash
docker compose up --build
```

- API：http://localhost:5001（對應容器 8080）
- 前端：http://localhost:5174
- SQLite 檔案持久化於 Docker volume `sqlite_data`

## 環境變數

| 變數 | 說明 | 預設 |
|------|------|------|
| `PORT` | Cloud Run 注入的監聽埠 | `8080` |
| `DATABASE_PATH` | SQLite 檔案路徑 | `/app/data/humanresource.db` |
| `Jwt__Secret` | JWT 簽章密鑰 | （必須在正式環境設定） |
| `Cors__Origins__0` | 允許的前端來源 | |
