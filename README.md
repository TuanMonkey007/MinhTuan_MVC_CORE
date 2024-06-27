# MinhTuan_MVC_CORE

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Tuandeptrai@123" ^
-p 1433:1433  --name SQL_Server_Container ^
-v E:\BE_STUDYING\DATN\Code\SQL_Docker_Volume/data:/var/opt/mssql/data ^
-v E:\BE_STUDYING\DATN\Code\SQL_Docker_Volume/log:/var/opt/mssql/log ^
-v E:\BE_STUDYING\DATN\Code\SQL_Docker_Volume/secrets:/var/opt/mssql/secrets ^
-d mcr.microsoft.com/mssql/server:2022-latest