FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

COPY libertasd /usr/local/

RUN  mkdir /root/.libertas/

COPY libertas.conf /root/.libertas/

VOLUME /root/.libertas

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["libertasd"]
