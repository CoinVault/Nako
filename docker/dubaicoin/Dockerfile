FROM dubaicoin-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.DubaiCoin/

COPY DubaiCoin.conf /root/.DubaiCoin/

VOLUME /root/.DubaiCoin

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["DubaiCoind"]
