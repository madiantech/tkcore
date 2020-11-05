FROM microsoft/dotnet:latest
COPY ../_lib /app/_lib
COPY . /app/Toolkit5.5

WORKDIR /app/Toolkit5.5/CoreWebTest/TestWeb
RUN dotnet restore
RUN dotnet build

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker

ENTRYPOINT dotnet run