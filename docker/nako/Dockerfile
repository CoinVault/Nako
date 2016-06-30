FROM mono:4

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN apt-get update && apt-get install nano /
    && echo -e "\nexport TERM=xterm" >> ~/.bashrc

RUN mkdir -p /usr/src/app/source /usr/src/app/build
#WORKDIR /usr/src/app/source

#COPY /Release /usr/src/app/source
COPY /Release /usr/src/app/build
#RUN nuget restore -NonInteractive
#RUN xbuild /property:Configuration=Release /property:OutDir=/usr/src/app/build/
WORKDIR /usr/src/app/build

EXPOSE 9000

ENTRYPOINT [ "mono",  "./Nako.exe" ]

