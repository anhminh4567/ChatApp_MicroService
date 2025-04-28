# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Modules/Chats/ThreadLike.Chat.Api/ThreadLike.Chat.Api.csproj", "src/Modules/Chats/ThreadLike.Chat.Api/"]
RUN dotnet restore "./src/Modules/Chats/ThreadLike.Chat.Api/ThreadLike.Chat.Api.csproj"
COPY . .
WORKDIR "/src/src/Modules/Chats/ThreadLike.Chat.Api"
RUN dotnet build "./ThreadLike.Chat.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build


# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ThreadLike.Chat.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
#USER root
#RUN apt-get update && apt-get install -y tzdata && \
    #ln -fs /usr/share/zoneinfo/Asia/Ho_Chi_Minh /etc/localtime && \
    #dpkg-reconfigure --frontend noninteractive tzdata
#USER $APP_UID
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ThreadLike.Chat.Api.dll","-e","WEBSITE_TIME_ZONE=Asia/Ho_Chi_Minh"]