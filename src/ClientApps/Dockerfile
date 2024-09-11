FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ClearHr.Blazor/ClearnHr.Blazor.csproj", "ClearnHr.Blazor/"]
RUN dotnet restore "JurayKV.Blazor/JurayKV.Blazor.csproj"
COPY . .
WORKDIR "/src/ClientApps/JurayKV.Blazor"
RUN dotnet build "JurayKV.Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JurayKV.Blazor.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf