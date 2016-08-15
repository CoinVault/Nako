FROM goldblocks-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.GoldBlocks/

COPY goldblocks.conf /root/.GoldBlocks/

VOLUME /root/.GoldBlocks

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["goldblocksd"]
