FROM coinvault/client-base:latest

MAINTAINER Dan Gershony - CoinVault <dan@coinvault.io>

# dependencies required to run the daemon
RUN apt-get update \
        && apt-get install -y git ntp \
        && apt-get install -y build-essential libssl-dev libdb-dev libdb++-dev libboost-all-dev libqrencode-dev libsodium-dev

# get
RUN apt-get update \
    && cd ~ \
        && git clone https://github.com/obsidianproject/Obsidian-Qt.git

# build
RUN     cd ~/Obsidian-Qt/src \
        && chmod 755 leveldb/build_detect_platform \
#       && mkdir -p obj \
#       && mkdir -p obj/x13hash \
        && make -f makefile.unix USE_UPNP=-

# install
RUN cd ~/Obsidian-Qt/src \
    && strip obsidiand \
        && install -m 755 obsidiand /usr/local/bin

# clean
RUN apt-get purge -y --auto-remove git \
  && rm -r ~/Obsidian-Qt
