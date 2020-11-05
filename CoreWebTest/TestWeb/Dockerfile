FROM microsoft/dotnet:runtime

RUN apt-get update
RUN apt-get -qqy --no-install-recommends install wget gnupg \
    git \
    unzip 

RUN curl -sL https://deb.nodesource.com/setup_13.x | bash -
RUN apt-get install -y nodejs vim

COPY bin/Debug/linux-x64/publish /app
WORKDIR /app

#RUN dotnet restore
#RUN dotnet build

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker

ENTRYPOINT dotnet ToolkitWeb.dll