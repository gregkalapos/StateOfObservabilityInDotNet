#frontend
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-rc1-buster-slim AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-rc1-buster AS build
WORKDIR /src
#COPY ["ESMeetupSample/ESMeetupSample.csproj", "ESMeetupSample/"]
#RUN dotnet restore "ESMeetupSample/ESMeetupSample.csproj"
COPY /Currency.Frontend .
WORKDIR "/src"
ENTRYPOINT ["dotnet", "run", "--urls", "http://0.0.0.0:5000"]
# RUN dotnet build "ESMeetupSample.csproj" -c Release -o /app

# FROM build AS publish
# RUN dotnet publish "ESMeetupSample.csproj" -c Release -o /app

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app .
# ENTRYPOINT ["dotnet", "ESMeetupSample.dll"]