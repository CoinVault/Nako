FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

ENV COIN_URL https://www.dash.org/binaries/dashcore-0.12.1.3-linux64.tar.gz

RUN set -ex \

	# get the binaries extract and delete the download file
	&& wget -O downloadfile.tar.gz $COIN_URL \
	&& tar -xzvf downloadfile.tar.gz -C /usr/local --strip-components=1 --exclude=*-qt \
	&& rm downloadfile.tar.gz \

	# remove build dependencies
	&& apt-get purge -y --auto-remove wget

RUN  mkdir /root/.dashcore/

COPY dash.conf /root/.dashcore/

VOLUME /root/.dashcore

WORKDIR /usr/local/bin

EXPOSE 5000

# dash changed thier default fodler (why?)
CMD ["dashd"]