FROM shilling-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.Shilling/

COPY Shilling.conf /root/.Shilling/

VOLUME /root/.Shilling

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["Shillingd"]
