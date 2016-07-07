FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>


RUN set -ex \

	# get the binaries extract and delete the download file
	&& wget -O downloadfile.tar.gz $COIN_URL \
	&& tar -xzvf downloadfile.tar.gz -C /usr/local --strip-components=1 --exclude=*-qt \
	&& rm downloadfile.tar.gz \

	# remove build dependencies
	&& apt-get purge -y --auto-remove wget




WORKDIR /usr/local/bin

EXPOSE 5000

