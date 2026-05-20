# 人力資源工作申請表系統

將既有 ASP.NET WebForms 三頁籤表單重構為現代化全端架構，並以 **SQLite + Docker** 封裝，可部署至 **GCP Cloud Run**。

## 技術棧

| 層級 | 技術 |
|------|------|
| 容器 | Docker + Docker Compose |
| 前端 | Vue 3 + Vite + TypeScript |
| UI | Tailwind CSS 4 + shadcn/ui 風格元件 + Motion-V + Lucide Vue |
| 後端 | ASP.NET Core 10 + EF Core |
| 資料庫 | **SQLite**（單檔 `humanresource.db`） |
| API | RESTful |
| 驗證 | JWT + Session Table |

## 三個頁籤

1. **基本資料** — 個人資料、學歷、外語、家庭成員、兵役
2. **工作經歷** — 工作經歷、證照、聯絡人推薦
3. **自我簡介** — 興趣、規劃、自傳、出差意願、待遇

## 快速開始（Docker）

**Windows 一鍵除錯：** 雙擊專案根目錄的 `Debug.bat`

- Docker 未運行 → 自動啟動 Docker Desktop 並部署
- Docker 已運行 → 重建映像並更新容器

或手動執行：

```bash
docker compose up --build
```

- 前端：http://localhost:5174
- API：http://localhost:5001（容器內監聽 **8080**，符合 Cloud Run 慣例）
- Swagger：http://localhost:5001/swagger
- SQLite 資料持久化於 Docker volume `sqlite_data`

### 示範帳號

- 身分證號：`D123456789`
- 密碼：`demo1234`
- 種子資料為**隨機產生的假資料**（非真實個資）。若曾用舊版種子，請執行 `docker compose down -v` 後重新 `up` 以重建資料庫。

## 本機開發

### 後端（無需額外安裝資料庫）

```bash
cd backend
dotnet run
```

SQLite 檔案會自動建立在 `backend/data/humanresource.db`。

### 前端

```bash
cd frontend
npm install
npm run dev
```

## 部署 GCP Cloud Run

詳見 [deploy/cloudrun.md](deploy/cloudrun.md)。

重點：

- API 映像讀取 `PORT` 環境變數（Cloud Run 預設 8080）
- 資料庫路徑由 `DATABASE_PATH` 設定（預設 `/app/data/humanresource.db`）
- 建議掛載 Volume 至 `/app/data` 以保留 SQLite 資料

## API 端點

| 方法 | 路徑 | 說明 |
|------|------|------|
| POST | `/api/auth/login` | 登入 |
| POST | `/api/auth/register` | 註冊 |
| POST | `/api/auth/logout` | 登出（需 JWT） |
| GET | `/api/application` | 取得完整表單 |
| PUT | `/api/application` | 一次儲存整份申請表（三個頁籤） |

## 專案結構

```
HumanResource/
├── backend/          # C# Web API + SQLite
├── frontend/         # Vue 3 SPA
├── deploy/           # Cloud Run 部署說明
├── docs/             # 原始 HTML 表單參考
├── docker-compose.yml
└── Version.md
```
