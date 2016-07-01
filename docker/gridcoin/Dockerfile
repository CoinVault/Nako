FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

ENV COIN_URL https://github.com/reddcoin-project/reddcoin/releases/download/v1.4.1.0/reddcoin-1.4.1.0-linux.tar.gz

RUN set -ex \

	# get the binaries extract and delete the download file
	&& wget -O downloadfile.tar.gz $COIN_URL \
	&& tar -xzvf downloadfile.tar.gz -C /usr/local --strip-components=1 --exclude=*-qt \
	&& rm downloadfile.tar.gz \

	# remove build dependencies
	&& apt-get purge -y --auto-remove wget

RUN  mkdir /root/.gridcoin/

COPY gridcoin.conf /root/.gridcoin/

VOLUME /root/.gridcoin

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["gridcoind"]
