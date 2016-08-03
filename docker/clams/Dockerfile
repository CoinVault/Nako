FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

ENV COIN_URL https://github.com/nochowderforyou/clams/releases/download/v1.4.17/clam-1.4.17-linux64.tar.gz

RUN set -ex \

	# get the binaries extract and delete the download file
	&& wget -O downloadfile.tar.gz $COIN_URL \
	&& tar -xzvf downloadfile.tar.gz -C /usr/local --strip-components=1 --exclude=*-qt \
	&& rm downloadfile.tar.gz \

    # clam hack
	&&  mv /usr/local/clam-1.4.17/* /usr/local/ \
	
	# remove build dependencies
	&& apt-get purge -y --auto-remove wget

RUN  mkdir /root/.clam/

COPY clam.conf /root/.clam/

VOLUME /root/.clam

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["./clamd"]
