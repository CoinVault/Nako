FROM node:10.3.0 as build-dependencies

# install git
RUN apt-get update && apt-get install -y git \
    && echo -e "\nexport TERM=xterm" >> ~/.bashrc

RUN git clone https://github.com/CoinVault/Nako.git /nako
WORKDIR /nako/core/blockexplorer

# Build the blockexplorer first using build.bat which outputs javascript into wwwroot
RUN build.bat


FROM microsoft/dotnet:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# install nano
RUN apt-get update && apt-get install -y nano \
    && echo -e "\nexport TERM=xterm" >> ~/.bashrc

RUN mkdir -p /nako

# copy the repository (with blockexplorer already built)
COPY --from=build-dependencies /nako /nako

WORKDIR /nako/core
RUN dotnet build

EXPOSE 9000

ENTRYPOINT ["dotnet", "run"]
