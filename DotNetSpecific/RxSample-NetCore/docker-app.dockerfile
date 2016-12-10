FROM microsoft/dotnet:1.1.0-sdk-projectjson

COPY ./src/RxSample-NetCore/project.json /app/RxSample-NetCore/
COPY ./NuGet.Config /app/
WORKDIR /app/
RUN dotnet restore
ADD ./src/ /app/

# run the app
WORKDIR /app/RxSample-NetCore/
RUN dotnet publish -c Debug -o out
ENTRYPOINT ["dotnet", "out/RxSample-NetCore.dll"]