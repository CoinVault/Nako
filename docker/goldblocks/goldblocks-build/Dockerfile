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
	&& git clone https://github.com/goldblockscoin/goldblocks  

# build
RUN	cd ~/goldblocks/src \
	&& mkdir -p obj \
	&& chmod 755 leveldb/build_detect_platform \
	&& make -f makefile.unix USE_UPNP=- 

# install
RUN cd ~/goldblocks/src \
    && strip goldblocksd \
	&& install -m 755 goldblocksd /usr/local/bin  

# clean
RUN apt-get purge -y --auto-remove git \
  && rm -r ~/goldblocks