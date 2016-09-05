FROM microsoft/dotnet:1.0.0-preview2-sdk

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# install git and nano
RUN apt-get update && apt-get install -y nano git \
    && echo -e "\nexport TERM=xterm" >> ~/.bashrc

RUN mkdir -p /dotnetapp
WORKDIR /dotnetapp

# clone the repository and build
RUN mkdir -p /usr/src/app/source 
RUN git clone https://github.com/CoinVault/Nako.git /usr/src/app/source 
RUN COPY /usr/src/app/source/core /dotnetapp

# remove git and the sourc
RUN apt-get purge -y --auto-remove git \
  && rm -r /usr/src/app/source

RUN dotnet restore

EXPOSE 9000

ENTRYPOINT ["dotnet", "run"]


