FROM zayedcoin-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.Zayedcoin/

COPY Zayedcoin.conf /root/.Zayedcoin/

VOLUME /root/.Zayedcoin

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["Zayedcoind"]
