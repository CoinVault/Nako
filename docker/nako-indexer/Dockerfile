FROM mcr.microsoft.com/dotnet/core/sdk:2.2

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# install git and nano
RUN apt-get update && apt-get install -y nano git \
    && echo -e "\nexport TERM=xterm" >> ~/.bashrc

RUN git clone https://github.com/CoinVault/Nako.git /nako

WORKDIR /nako/core/nako.indexer
RUN dotnet build

EXPOSE 9000

ENTRYPOINT ["dotnet", "run"]

