FROM debian:jessie

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN set -ex \
	&& apt-get update \
	&& apt-get install -y --no-install-recommends ca-certificates wget nano \
	&& rm -rf /var/lib/apt/lists/* \
    && echo -e "\nexport TERM=xterm" >> ~/.bashrc
