FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

ENV COIN_URL https://github.com/dogecoin/dogecoin/releases/download/v1.10.0/dogecoin-1.10.0-linux64.tar.gz

COPY ppcoin-0.5.4ppc-linux.tar.gz .

RUN set -ex \

	# get the binaries extract and delete the download file
	# && wget -O downloadfile.tar.gz $COIN_URL \
	&& tar -xzvf ppcoin-0.5.4ppc-linux.tar.gz -C /usr/local --strip-components=1 --exclude=*-qt \
	&& rm ppcoin-0.5.4ppc-linux.tar.gz \

	# remove build dependencies
	&& apt-get purge -y --auto-remove wget

RUN  mkdir /root/.ppcoin/

COPY ppcoin.conf /root/.ppcoin/

VOLUME /root/.ppcoin

WORKDIR /usr/local/bin/64

EXPOSE 5000

CMD ["./ppcoind"]
