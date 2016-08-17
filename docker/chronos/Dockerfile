FROM chronos-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.chronos/

COPY chronos.conf /root/.chronos/

VOLUME /root/.chronos

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["chronosd"]
