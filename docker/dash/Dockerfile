FROM coinvault/client-base:latest

ENV COIN_URL https://www.dash.org/binaries/dash-0.12.0.58-linux64.tar.gz

RUN set -ex \

	# get the binaries extract and delete the download file
	&& wget -O downloadfile.tar.gz $COIN_URL \
	&& tar -xzvf downloadfile.tar.gz -C /usr/local --strip-components=1 --exclude=*-qt \
	&& rm downloadfile.tar.gz \

	# remove build dependencies
	&& apt-get purge -y --auto-remove wget

RUN  mkdir /root/.dash/

COPY dash.conf /root/.dash/

VOLUME /root/.dash

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["dashd"]