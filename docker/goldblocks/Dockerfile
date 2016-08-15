FROM gridcoin-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.goldblocks/

COPY goldblocks.conf /root/.goldblocks/

VOLUME /root/.goldblocks

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["goldblocksd"]
