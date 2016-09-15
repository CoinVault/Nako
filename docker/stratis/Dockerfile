FROM stratis-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.stratis/

COPY stratis.conf /root/.stratis/

VOLUME /root/.stratis

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["stratisd"]
