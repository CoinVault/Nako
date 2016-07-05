FROM mono:4

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# install git and nano
RUN apt-get update && apt-get install -y nano git \
    && echo -e "\nexport TERM=xterm" >> ~/.bashrc

RUN mkdir -p /usr/src/app/source /usr/src/app/build

# clone the repository and build
RUN git clone https://github.com/CoinVault/Nako.git /usr/src/app/source
WORKDIR /usr/src/app/source/src
RUN nuget restore -NonInteractive
RUN xbuild /property:Configuration=Release /property:OutDir=/usr/src/app/build/

# remove git and the sourc
RUN apt-get purge -y --auto-remove git \
  && rm -r /usr/src/app/source

WORKDIR /usr/src/app/build

EXPOSE 9000

ENTRYPOINT [ "mono",  "./Nako.exe" ]

