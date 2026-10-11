@echo off
REM BTH4 - Build toan bo solution (cap nhat cho layout 3-project/service + Gateway + Blazor)
REM Chay file nay bang quyen Admin. Yeu cau: .NET SDK 8+ da cai.
cd /d %~dp0
echo === Restore + Build InsuranceAgency.sln ===
dotnet build InsuranceAgency.sln --nologo
if %errorlevel% neq 0 (
  echo BUILD FAILED
  exit /b 1
)
echo.
echo === Test ===
dotnet test InsuranceAgency.sln --no-build --nologo
echo.
echo === XONG: 0 Error la dat (Warning ve Steeltoe/Blazor co san, khong phai loi) ===
pause
