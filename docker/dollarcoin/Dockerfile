FROM dollarcoin-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.dollarcoin/

COPY dollarcoin.conf /root/.dollarcoin/

VOLUME /root/.dollarcoin

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["dollarcoind"]
