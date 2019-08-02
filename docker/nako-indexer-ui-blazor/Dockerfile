FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview7-disco

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# install git and nano
RUN apt-get update && apt-get install -y nano git \
    && echo -e "\nexport TERM=xterm" >> ~/.bashrc

RUN git clone https://github.com/CoinVault/Nako.git /nako
WORKDIR /nako/core
RUN git checkout nako-ui-blazor
RUN git pull
WORKDIR /nako/core/nako.ui.blazor
RUN dotnet build

EXPOSE 9000

ENTRYPOINT ["dotnet", "run"]
