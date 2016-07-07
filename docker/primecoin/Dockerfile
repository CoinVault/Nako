FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

ENV COIN_URL https://github.com/dogecoin/dogecoin/releases/download/v1.10.0/dogecoin-1.10.0-linux64.tar.gz

COPY primecoin-0.1.2-linux.tar.gz .

RUN set -ex \

	# get the binaries extract and delete the download file
	# && wget -O downloadfile.tar.gz $COIN_URL \
	&& tar -xzvf primecoin-0.1.2-linux.tar.gz -C /usr/local --strip-components=1 --exclude=*-qt \
	&& rm primecoin-0.1.2-linux.tar.gz \

	# remove build dependencies
	&& apt-get purge -y --auto-remove wget

RUN  mkdir /root/.primecoin/

COPY primecoin.conf /root/.primecoin/

VOLUME /root/.primecoin

WORKDIR /usr/local/bin/64

EXPOSE 5000

CMD ["./primecoind"]
