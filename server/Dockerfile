FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проектов и восстанавливаем зависимости
COPY ["PL/clothingStoreWebAPI/clothingStoreWebAPI.csproj", "PL/clothingStoreWebAPI/"]
COPY ["BLL/ClothingStoreApplication/ClothingStoreApplication.csproj", "BLL/ClothingStoreApplication/"]
COPY ["BLL/ClothDTOs/ClothDTOs.csproj", "BLL/ClothDTOs/"]
COPY ["BLL/ClothesInterfacesBLL/ClothesInterfacesBLL.csproj", "BLL/ClothesInterfacesBLL/"]
COPY ["DAL/ClothesInterfacesDAL/ClothesInterfacesDAL.csproj", "DAL/ClothesInterfacesDAL/"]
COPY ["DAL/ClothingStorePersistence/ClothingStorePersistence.csproj", "DAL/ClothingStorePersistence/"]
COPY ["Core/ClothDomain/ClothDomain.csproj", "Core/ClothDomain/"]

RUN dotnet restore "PL/clothingStoreWebAPI/clothingStoreWebAPI.csproj"

# Копируем остальные файлы и компилируем проект
COPY . .
WORKDIR "/src/PL/clothingStoreWebAPI"
RUN dotnet build "clothingStoreWebAPI.csproj" -c Release -o /app/build

# Публикуем проект
FROM build AS publish
RUN dotnet publish "clothingStoreWebAPI.csproj" -c Release -o /app/publish

# Запускаем приложение
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "clothingStoreWebAPI.dll"]