FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# dependencies required to run the daemon
RUN apt-get update \
	&& apt-get install -y git \
	&& apt-get install -y build-essential libtool autotools-dev automake \ 
	&& apt-get install -y pkg-config libssl-dev libevent-dev bsdmainutils \
	&& apt-get install -y libboost-all-dev 

# get
RUN    apt-get update \
       && cd ~ \
       && git clone https://github.com/namecoin/namecoin-core.git namecoin

# build
RUN	cd ~/namecoin \
	&& ./autogen.sh \
	&& ./configure --disable-wallet --without-gui --without-miniupnpc \
	&& make

# install
RUN cd ~/namecoin/src \
    && strip namecoind \
    && install -m 755 namecoind /usr/local/bin  

RUN  mkdir /root/.namecoin/

COPY  namecoin.conf /root/.namecoin/

VOLUME /root/.namecoin

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["namecoind"]
