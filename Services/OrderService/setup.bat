@echo off
echo Đang quét các project trong thư mục src và test để thêm vào Solution...

:: Quét tất cả file .csproj trong toàn bộ thư mục service vừa tạo
for /r %%i in (*.csproj) do (
    echo Đang thêm: %%i
    dotnet sln ../../MyECommerce.sln add "%%i"
)

echo Hoàn tất cấu trúc Microservice!