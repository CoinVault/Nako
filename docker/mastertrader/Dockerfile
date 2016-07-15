# a cointainer cointaining the build (follow instruction from the github quarckcoin project)
FROM mastertrader-build  

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>


#RUN  mkdir /root/.quarkcoin/

COPY MasterTrader.conf /root/.MasterTrader/

VOLUME /root/.MasterTrader

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["mastertraderd"]
