#dbbackend
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-rc1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 50001

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-rc1-buster AS build
WORKDIR /src
#COPY ["ESMeetupSample/ESMeetupSample.csproj", "ESMeetupSample/"]
#RUN dotnet restore "ESMeetupSample/ESMeetupSample.csproj"
COPY /Currency.DbService/ .
WORKDIR "/src"
ENTRYPOINT ["dotnet", "run", "--urls", "http://0.0.0.0:5001"]
# RUN dotnet build "ESMeetupSample.csproj" -c Release -o /app

# FROM build AS publish
# RUN dotnet publish "ESMeetupSample.csproj" -c Release -o /app

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app .
# ENTRYPOINT ["dotnet", "ESMeetupSample.dll"]