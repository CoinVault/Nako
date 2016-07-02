FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# dependencies required to run the daemon
RUN set -ex \
	&& apt-get update \
	&& apt-get install -y libssl-dev libdb-dev libdb++-dev libboost-all-dev libqrencode-dev \
	&& apt-get install -y qt-sdk qt4-default \
	&& apt-get install -y libcurl3-dev \
	&& apt-get install -y libzip-dev


# require a comppiled version of gridcoinresearchd in place 
COPY gridcoinresearchd /usr/local/bin

RUN  mkdir /root/.GridcoinResearch/

COPY gridcoinresearch.conf /root/.GridcoinResearch/

VOLUME /root/.GridcoinResearch

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["./gridcoinresearchd"]
