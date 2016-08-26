FROM terracoin-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.terracoin/

COPY terracoin.conf /root/.terracoin/

VOLUME /root/.terracoin

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["terracoind"]
