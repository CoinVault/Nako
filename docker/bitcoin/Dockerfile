FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

ENV COIN_URL https://bitcoin.org/bin/bitcoin-core-0.12.1/bitcoin-0.12.1-linux64.tar.gz

RUN set -ex \

	# get the binaries extract and delete the download file
	&& wget -O downloadfile $COIN_URL \
	&& tar -xzvf downloadfile -C /usr/local --strip-components=1 --exclude=*-qt \
	&& rm downloadfile \

	# remove build dependencies
	&& apt-get purge -y --auto-remove wget

RUN  mkdir /root/.bitcoin/

COPY bitcoin.conf /root/.bitcoin/

VOLUME /root/.bitcoin

WORKDIR /usr/local/bin

EXPOSE 8332 8333

CMD ["bitcoind"]