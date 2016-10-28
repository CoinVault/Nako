FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

ENV COIN_URL https://github.com/reddcoin-project/reddcoin/releases/download/v2.0.0.0/reddcoin-2.0.0.0-linux.tar.gz

RUN set -ex \

	# get the binaries extract and delete the download file
	&& wget -O downloadfile.tar.gz $COIN_URL \
	&& tar -xzvf downloadfile.tar.gz -C /usr/local --strip-components=1 --exclude=*-qt \
	&& rm downloadfile.tar.gz \

	# remove build dependencies
	&& apt-get purge -y --auto-remove wget

RUN  mkdir /root/.reddcoin/

COPY reddcoin.conf /root/.reddcoin/

VOLUME /root/.reddcoin

WORKDIR /usr/local/bin/64

EXPOSE 5000

CMD ["./reddcoind"]
