# Version History

## 20260520-001

- **變更**：將 docs 內三頁籤人力資源表單（基本資料、工作經歷、自我簡介）重構為 Docker 化全端專案
- **原因**：取代舊版 ASP.NET WebForms 單頁 POST 架構，改為 Vue3 SPA + REST API + PostgreSQL，並導入 JWT 與 Session Table 驗證
- **相關檔案**：
  - `backend/` — ASP.NET Core API、EF Core 實體與 Migration
  - `frontend/` — Vue3 + Vite + Tailwind + shadcn 風格元件
  - `docker-compose.yml`
  - `docs/page1.md`, `docs/page2.md`, `docs/page3.md`（需求參考）

## 20260520-002

- **變更**：資料庫由 PostgreSQL 改為 SQLite；Docker 與 API 調整為 Cloud Run 友善（監聽 `PORT`、資料路徑 `DATABASE_PATH`）
- **原因**：單一容器即可運行，無需外部資料庫服務，便於封裝 Docker 部署至 GCP Cloud Run
- **相關檔案**：
  - `backend/Program.cs`、`backend/HumanResource.Api.csproj`
  - `backend/appsettings*.json`
  - `backend/Dockerfile`、`docker-compose.yml`
  - `deploy/cloudrun.md`
  - `README.md`

## 20260520-003

- **變更**：新增 `Debug.bat`，自動檢查 Docker 狀態；未運行時啟動 Docker Desktop 並部署，已運行時重建更新容器
- **原因**：簡化本機除錯流程，一鍵啟動開發環境
- **相關檔案**：`Debug.bat`、`README.md`

## 20260520-004

- **變更**：本機對外埠改為 API `5001`、前端 `5174`（各 +1），避免與其他 Docker 服務衝突
- **原因**：5000/5173 已被其他容器佔用
- **相關檔案**：`docker-compose.yml`、`Debug.bat`、`frontend/vite.config.ts`、`backend/appsettings.json`、`backend/Program.cs`、`frontend/nginx.conf`

## 20260520-005

- **變更**：修正 Docker 內 API 監聽埠為 8080（`PORT` 環境變數 + Production 預設），解決前端 nginx 代理 `/api` 回傳 502
- **原因**：對外埠改為 5001 後，容器內程式仍預設聽 5001，與 `5001:8080` 及 nginx `api:8080` 不一致
- **相關檔案**：`backend/Program.cs`、`docker-compose.yml`、`backend/Dockerfile`

## 20260520-006

- **變更**：移除 Production 下 `UseUrls(5001)` 覆寫；改以 `ASPNETCORE_URLS=http://0.0.0.0:8080` 讓容器內 API 正確監聽 8080，修復 nginx `Connection refused`
- **原因**：`Program.cs` 在 Production 仍綁定 5001，覆蓋 `HTTP_PORTS=8080`，導致 upstream `api:8080` 連線被拒
- **相關檔案**：`backend/Program.cs`、`docker-compose.yml`、`backend/Dockerfile`

## 20260520-007

- **變更**：修正 JWT `OnTokenValidated` 讀取 `sub` 造成 NullReferenceException（500）；前端新增儲存前必填驗證；表單資料自動暫存至 localStorage 並可還原
- **原因**：.NET 將 JWT `sub` 映射為 `NameIdentifier`，導致 session 驗證失敗；必填欄位僅 UI 標示未阻擋送出；關閉頁面會遺失未儲存輸入
- **相關檔案**：`backend/Program.cs`、`frontend/src/utils/formValidation.ts`、`frontend/src/utils/draftStorage.ts`、`frontend/src/composables/useFormDraft.ts`、`frontend/src/views/tabs/*.vue`、`frontend/src/stores/application.ts`

## 20260520-008

- **變更**：修正儲存基本資料/工作經歷時 EF Core `DbUpdateConcurrencyException`；改以 `ExecuteDelete` + `ChangeTracker.Clear` + 直接 `DbSet.Add` 取代導覽屬性 `Clear()`
- **原因**：含 Include 載入的子實體仍被追蹤，`SaveChanges` 對已刪除列發出 UPDATE
- **相關檔案**：`backend/Services/ApplicationService.cs`

## 20260520-009

- **變更**：示範種子改為隨機假資料（`DemoDataGenerator`）；示範帳號改為 `D123456789`；API 改為 `PUT /api/application` 一次儲存三頁籤；前端表單集中於 store、單一「儲存申請表」按鈕
- **原因**：避免使用真實個資；使用者可任意切換頁籤編輯後一次送出
- **相關檔案**：`backend/Data/DbSeeder.cs`、`backend/Data/DemoDataGenerator.cs`、`backend/Services/ApplicationService.cs`、`backend/Controllers/ApplicationController.cs`、`frontend/src/stores/application.ts`、`frontend/src/views/ApplicationView.vue`、`frontend/src/views/tabs/*.vue`、`README.md`

## 20260520-010

- **變更**：啟動時自動將舊示範帳號 `E123458858` 遷移為 `D123456789` 並重設密碼，修復既有 SQLite volume 登入 401
- **原因**：帳號變更後種子因已有使用者而跳過，新帳號查無資料
- **相關檔案**：`backend/Data/DbSeeder.cs`

## 20260520-011

- **變更**：示範帳號若仍含真實個資則啟動時重設為假資料；驗證失敗欄位紅框並捲動至第一個錯誤；待遇「依公司規定/面議」改為互斥單選；新增履歷審閱頁 `/review` 與彈窗檢視
- **原因**：舊 DB 仍保留個資；改善表單 UX；避免待遇選項同時勾選；HR 需瀏覽多份申請表
- **相關檔案**：`backend/Data/DemoApplicationFactory.cs`、`backend/Data/DbSeeder.cs`、`backend/Controllers/ReviewController.cs`、`frontend/src/utils/formValidation.ts`、`frontend/src/stores/application.ts`、`frontend/src/views/ReviewView.vue`、`frontend/src/views/tabs/SelfIntroTab.vue`
