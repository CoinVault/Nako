FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# dependencies required to run the daemon
RUN apt-get update \
	&& apt-get install -y git ntp \
	&& apt-get install -y build-essential libssl-dev libdb-dev libdb++-dev libboost-all-dev libqrencode-dev 

# get
RUN apt-get update \
    && cd ~ \
	&& git clone https://github.com/yavwa/Shilling.git  

# build
RUN	cd ~/Shilling/src \
	&& chmod 755 leveldb/build_detect_platform \
	&& mkdir -p obj \
	&& make -f makefile.unix USE_UPNP=-

# install
RUN cd ~/Shilling/src \
    && strip Shillingd \
	&& install -m 755 Shillingd /usr/local/bin  

# clean
RUN apt-get purge -y --auto-remove git \
  && rm -r ~/Shilling