FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

ENV COIN_URL https://download.litecoin.org/litecoin-0.13.2/linux/litecoin-0.13.2-x86_64-linux-gnu.tar.gz

RUN set -ex \

	# get the binaries extract and delete the download file
	&& wget -O downloadfile.tar.gz $COIN_URL \
	&& tar -xzvf downloadfile.tar.gz -C /usr/local --strip-components=1 --exclude=*-qt \
	&& rm downloadfile.tar.gz \

	# remove build dependencies
	&& apt-get purge -y --auto-remove wget

RUN  mkdir /root/.litecoin/

COPY litecoin.conf /root/.litecoin/

VOLUME /root/.litecoin

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["litecoind"]
