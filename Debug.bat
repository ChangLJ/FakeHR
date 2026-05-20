@echo off
chcp 65001 >nul
setlocal EnableExtensions

REM 切換至專案根目錄（與本 bat 同層）
cd /d "%~dp0"
title HumanResource Debug

echo ========================================
echo   人力資源表單 - Debug 環境
echo ========================================
echo.

call :CheckDockerRunning
if "%DOCKER_OK%"=="1" goto DockerRunning

echo [資訊] Docker 未運行，正在啟動 Docker Desktop...
call :StartDockerDesktop
call :WaitForDocker 90
if errorlevel 1 (
    echo [錯誤] Docker 啟動逾時，請手動開啟 Docker Desktop 後再執行本腳本。
    pause
    exit /b 1
)

echo [資訊] Docker 已就緒，開始首次部署...
goto Deploy

:DockerRunning
echo [資訊] Docker 已運行，正在重建並更新容器...
goto Deploy

:Deploy
echo.
echo [資訊] 執行 docker compose up --build ...
echo [資訊] 前端: http://localhost:5174
echo [資訊] API:  http://localhost:5001
echo [資訊] 示範帳號: D123456789 / demo1234 （假資料）
echo [資訊] 按 Ctrl+C 可停止服務
echo.

docker compose up --build
set EXIT_CODE=%ERRORLEVEL%

echo.
if %EXIT_CODE% neq 0 (
    echo [錯誤] docker compose 結束，代碼: %EXIT_CODE%
) else (
    echo [完成] 服務已停止。
)
pause
exit /b %EXIT_CODE%

REM ---------- 子程序 ----------

:CheckDockerRunning
set "DOCKER_OK=0"
docker info >nul 2>&1
if not errorlevel 1 set "DOCKER_OK=1"
exit /b 0

:StartDockerDesktop
set "DOCKER_DESKTOP="

if exist "%ProgramFiles%\Docker\Docker\Docker Desktop.exe" (
    set "DOCKER_DESKTOP=%ProgramFiles%\Docker\Docker\Docker Desktop.exe"
)
if not defined DOCKER_DESKTOP if exist "%LocalAppData%\Docker\Docker Desktop.exe" (
    set "DOCKER_DESKTOP=%LocalAppData%\Docker\Docker Desktop.exe"
)
if not defined DOCKER_DESKTOP if exist "%ProgramFiles(x86)%\Docker\Docker\Docker Desktop.exe" (
    set "DOCKER_DESKTOP=%ProgramFiles(x86)%\Docker\Docker\Docker Desktop.exe"
)

if defined DOCKER_DESKTOP (
    echo [資訊] 啟動: %DOCKER_DESKTOP%
    start "" "%DOCKER_DESKTOP%"
) else (
    echo [警告] 找不到 Docker Desktop，請手動啟動後重試。
)
exit /b 0

:WaitForDocker
REM 參數 %1 = 最多等待秒數
set /a "MAX_WAIT=%~1"
if not defined MAX_WAIT set /a "MAX_WAIT=90"
set /a "ELAPSED=0"

:WaitLoop
call :CheckDockerRunning
if "%DOCKER_OK%"=="1" exit /b 0

if %ELAPSED% geq %MAX_WAIT% exit /b 1

echo [等待] Docker 啟動中... (%ELAPSED%/%MAX_WAIT% 秒)
timeout /t 3 /nobreak >nul
set /a "ELAPSED+=3"
goto WaitLoop
