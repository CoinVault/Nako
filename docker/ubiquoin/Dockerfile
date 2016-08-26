FROM ubiquoin-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.Ubiquoin/

COPY Ubiquoin.conf /root/.Ubiquoin/

VOLUME /root/.Ubiquoin

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["Ubiquoind"]
