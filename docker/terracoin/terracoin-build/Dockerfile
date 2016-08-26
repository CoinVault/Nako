FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# dependencies required to run the daemon
RUN apt-get update \
	&& apt-get install -y git \
	&& apt-get install -y libssl-dev libdb-dev libdb++-dev libboost-all-dev libqrencode-dev \
	&& apt-get install -y qt-sdk qt4-default \
	&& apt-get install -y libcurl3-dev \
	&& apt-get install -y libzip-dev

# get
RUN apt-get update \
    && cd ~ \
    && git clone https://github.com/clockuniverse/terracoin.git  

# build
RUN	cd ~/terracoin/src \
	&& ./autogen.sh \
	&& ./configure --disable-wallet --without-gui --without-miniupnpc \
	&& make

#       && mkdir -p obj \
#	&& chmod 755 leveldb/build_detect_platform \
#	&& make -f makefile.unix USE_UPNP=- 

# install
RUN cd ~/terracoin/src \
    && strip terracoind \
	&& install -m 755 terracoind /usr/local/bin  

# clean
RUN apt-get purge -y --auto-remove git \
  && rm -r ~/terracoin
