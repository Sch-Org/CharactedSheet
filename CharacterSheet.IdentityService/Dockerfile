FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env
WORKDIR /src
COPY CharacterSheet.IdentityService.Api/*.csproj .
RUN dotnet restore
COPY CharacterSheet.IdentityService.Api .
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /publish
COPY --from=build-env /publish .
EXPOSE 5001
ENTRYPOINT ["dotnet", "CharacterSheet.IdentityService.Api.dll"]