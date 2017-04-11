FROM libertas-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.libertas/

COPY libertas.conf /root/.libertas/

VOLUME /root/.libertas

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["libertasd"]
