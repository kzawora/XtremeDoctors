FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 51795
EXPOSE 44302

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["XtremeDoctors/XtremeDoctors.csproj", "XtremeDoctors/"]
RUN dotnet restore "XtremeDoctors/XtremeDoctors.csproj"
COPY . .
WORKDIR "/src/XtremeDoctors"
RUN dotnet build "XtremeDoctors.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "XtremeDoctors.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "XtremeDoctors.dll"]