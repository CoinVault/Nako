FROM microsoft/dotnet:1.0.0-preview2-sdk

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# install git and nano
RUN apt-get update && apt-get install -y nano git \
    && echo -e "\nexport TERM=xterm" >> ~/.bashrc

RUN mkdir -p /nako

# clone the repository and build  
RUN git clone https://github.com/CoinVault/Nako.git /nako

# remove git and the sourc
RUN apt-get purge -y --auto-remove git 

WORKDIR /nako/core
RUN dotnet restore

EXPOSE 9000

ENTRYPOINT ["dotnet", "run"]


