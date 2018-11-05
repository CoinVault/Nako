FROM node:10.3.0 as build-dependencies

# install git
RUN apt-get update && apt-get install -y git \
    && echo -e "\nexport TERM=xterm" >> ~/.bashrc

# clone the repository and build  
RUN git clone https://github.com/majikandy/Nako.git /nako
WORKDIR /nako/core/wwwroot
RUN git checkout addSphere
RUN yarn
RUN yarn build


FROM microsoft/dotnet:2.1-sdk

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# install git and nano
RUN apt-get update && apt-get install -y nano git \
    && echo -e "\nexport TERM=xterm" >> ~/.bashrc

RUN mkdir -p /nako

# clone the repository and build  
COPY --from=build-dependencies /nako /nako

# remove git and the sourc
RUN apt-get purge -y --auto-remove git 

WORKDIR /nako/core
#RUN dotnet restore
RUN dotnet build Nako.Core.sln

EXPOSE 9000

ENTRYPOINT ["dotnet", "run"]


