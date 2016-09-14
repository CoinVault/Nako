FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# dependencies required to run the daemon
RUN apt-get update && apt-get upgrade -y \
	&& apt-get install -y git \
	&& apt-get install -y ntp build-essential libssl-dev libdb-dev libdb++-dev libboost-all-dev libqrencode-dev 

# install upnp
#RUN cd ~ && wget http://miniupnp.free.fr/files/download.php?file=miniupnpc-1.8.tar.gz \ 
#	&& tar -zxf download.php\?file\=miniupnpc-1.8.tar.gz && cd miniupnpc-1.8/ && make && make install && cd .. \
#	&& rm -rf miniupnpc-1.8 download.php\?file\=miniupnpc-1.8.tar.gz

# get
RUN apt-get update \
    && cd ~ \
	&& git clone https://github.com/ZayedCoin/Zayedcoin.git  

# build
RUN	cd ~/Zayedcoin/src/leveldb \
	&& chmod 755 build_detect_platform \
    && make clean \
	&& cd ~/Zayedcoin/src \
	&& mkdir -p obj \
	&& make -f makefile.unix USE_UPNP=-

# install
RUN cd ~/Zayedcoin/src \
    && strip Zayedcoind \
	&& install -m 755 Zayedcoind /usr/local/bin  

# clean
RUN apt-get purge -y --auto-remove git \
  && rm -r ~/Zayedcoin