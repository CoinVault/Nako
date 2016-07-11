FROM gridcoin-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.GridcoinResearch/

COPY gridcoinresearch.conf /root/.GridcoinResearch/

VOLUME /root/.GridcoinResearch

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["gridcoinresearchd"]
