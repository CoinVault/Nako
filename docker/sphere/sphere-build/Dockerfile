FROM microsoft/dotnet:2.1-sdk

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# install git and nano
RUN apt-get update && apt-get install -y nano git \
    && echo -e "\nexport TERM=xterm" >> ~/.bashrc

RUN mkdir -p /usr/src/app/source /usr/src/app/build

# clone the repository and build
RUN git clone https://github.com/majikandy/StratisBitcoinFullNode.git /usr/src/app/source
WORKDIR /usr/src/app/source/src
RUN git checkout sphere
WORKDIR /usr/src/app/source/src/SphereD
#RUN dotnet build -o /usr/src/app/build
RUN dotnet build


COPY sphere.conf /root/.stratis/
#WORKDIR /usr/src/app/build

#COPY sphere.conf.docker /root/.stratisnode/Sphere/SphereMain/sphere.conf

#RUN nuget restore -NonInteractive

#RUN xbuild /property:Configuration=Release /property:OutDir=/usr/src/app/build/

# remove git and the source
RUN apt-get purge -y --auto-remove git

RUN apt-get install -y curl

#RUN rm -r /usr/src/app/source


#ENV ASPNETCORE_URLS=http://+:5000;http://+:39221

EXPOSE 5000 39221

ENTRYPOINT ["dotnet", "run", "-apiport=39221", "-server=1", "-rpcallowip=*", "-rpcuser=rpcuser", "-rpcpassword=rpcpassword", "-rpcport=5000", "-rpcthreads=300", "-txindex=1", "-addnode=86.10.43.9"]
#"-rpcallowip=*", "-rpcbind=sphere-client"