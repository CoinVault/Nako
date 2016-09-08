FROM argentum-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.argentum/

COPY argentum.conf /root/.argentum/

VOLUME /root/.argentum

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["argentumd"]
