FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# dependencies required to run the daemon
RUN apt-get update \
	&& apt-get install -y git \
	&& apt-get install -y build-essential libtool autotools-dev automake \ 
	&& apt-get install -y pkg-config libssl-dev libevent-dev bsdmainutils \
	&& apt-get install -y libboost-all-dev 
	#&& apt-get install -y libcurl3-dev \
	#&& apt-get install -y libzip-dev

# get
RUN apt-get update \
    && cd ~ \
	&& git clone https://github.com/libertas/libertas.git

# build
RUN	cd ~/libertas \
	&& ./autogen.sh \
	&& ./configure --disable-wallet --without-gui --without-miniupnpc \
	&& make

# install
RUN cd ~/libertas/src \
    && strip libertas \
	&& install -m 755 libertas /usr/local/bin  

# clean
RUN apt-get purge -y --auto-remove git \
  && rm -r ~/libertas