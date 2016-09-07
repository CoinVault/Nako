FROM savetheoceancoin-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.SaveTheOcean/

COPY SaveTheOcean.conf /root/.SaveTheOcean/

VOLUME /root/.SaveTheOcean

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["SaveTheOceand"]
