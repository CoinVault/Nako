FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# dependencies required to run the daemon
RUN apt-get update \
	&& apt-get install -y git \
	&& apt-get install -y libssl-dev libdb-dev libdb++-dev libboost-all-dev libqrencode-dev \
	&& apt-get install -y qt-sdk qt4-default \
	&& apt-get install -y libcurl3-dev \
	&& apt-get install -y libzip-dev

# get (use my forked repo as the rwp raw trx is configured to return the trx hex)
RUN apt-get update \
    && cd ~ \
	&& git clone https://github.com/dangershony/mastertrader  

# build
RUN	cd ~/mastertrader/src \
	&& make -f makefile.unix USE_UPNP=- 
       
# install
RUN cd ~/mastertrader/src \
    && strip mastertraderd \
	&& install -m 755 mastertraderd /usr/local/bin 

# clean
RUN apt-get purge -y --auto-remove git \
  && rm -r ~/mastertrader