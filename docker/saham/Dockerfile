FROM saham-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.saham/

COPY saham.conf /root/.saham/

VOLUME /root/.saham

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["sahamd"]
