# a cointainer cointaining the build (follow instruction from the github quarckcoin project)
FROM quark-build  

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>


#RUN  mkdir /root/.quarkcoin/

COPY quarkcoin.conf /root/.quarkcoin/

VOLUME /root/.quarkcoin

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["quarkd"]
