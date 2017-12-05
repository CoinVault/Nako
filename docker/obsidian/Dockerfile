FROM obsidian-build

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

RUN  mkdir /root/.obsidian/

COPY obsidian.conf /root/.obsidian/

VOLUME /root/.obsidian

WORKDIR /usr/local/bin

EXPOSE 5000

CMD ["obsidiand"]
