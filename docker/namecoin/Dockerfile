FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN set -ex \
	&& echo 'deb http://download.opensuse.org/repositories/home:/p_conrad:/coins/Debian_8.0/ /' >> /etc/apt/sources.list.d/namecoin.list \ 
	&& apt-get update \
	&& apt-get install -y --force-yes namecoin \
	&& apt-get purge -y --auto-remove wget

RUN  mkdir /root/.namecoin/

COPY namecoin.conf /root/.namecoin/

VOLUME /root/.namecoin

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["namecoind"]