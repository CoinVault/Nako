FROM stratis-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.Stratis/

COPY Stratis.conf /root/.Stratis/

VOLUME /root/.Stratis

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["stratisd"]
