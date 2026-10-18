@echo off
echo ================================================================
echo TAO FILE PDF CHUNG MINH EUREKA (Task #2)
echo ================================================================
echo.
echo CACH 1: Dung Markdown (khuyen nghi)
echo ------------------------------------------------------------
echo 1. Mo file Eureka_Evidence.md bang VS Code
echo 2. Ctrl+Shift+V de mo Preview
echo 3. Ctrl+Shift+P -> "Markdown: Print to PDF"
echo 4. Luu ten: Eureka_Evidence.pdf
echo.
echo CACH 2: Dung Word
echo ------------------------------------------------------------
echo 1. Mo Word -> File -> Open -> Chon Eureka_Evidence.md
echo 2. File -> Save As -> PDF
echo 3. Luu ten: Eureka_Evidence.pdf
echo.
echo CACH 3: Chup man hinh thu cong (backup)
echo ------------------------------------------------------------
echo 1. Mo Terminal -> chay: docker ps
echo 2. Mo Browser -> http://localhost:8761
echo 3. Mo Terminal -> chay: docker logs eureka-server
echo 4. Chup 3 anh (Alt+PrtSc) -> Dan vao Word -> Save as PDF
echo.
echo FILE CAN NOP: Eureka_Evidence.pdf
echo ================================================================
pause