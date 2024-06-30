FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
# Set timezone to UTC+7
RUN ln -snf /usr/share/zoneinfo/Asia/Ho_Chi_Minh /etc/localtime && echo "Asia/Ho_Chi_Minh" > /etc/timezone

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MinhTuan.API/MinhTuan.API.csproj", "MinhTuan.API/"]
COPY ["MinhTuan.Domain/MinhTuan.Domain.csproj", "MinhTuan.Domain/"]
COPY ["MinhTuan.Service/MinhTuan.Service.csproj", "MinhTuan.Service/"]
RUN dotnet restore "MinhTuan.API/MinhTuan.API.csproj"
COPY . .
WORKDIR "/src/MinhTuan.API"
RUN dotnet build "MinhTuan.API.csproj" -c ${BUILD_CONFIGURATION} -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MinhTuan.API.csproj" -c ${BUILD_CONFIGURATION} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinhTuan.API.dll"]