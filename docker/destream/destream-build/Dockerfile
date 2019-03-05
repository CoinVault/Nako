FROM coinvault/client-base:latest

# dependencies required to run the daemon
RUN apt-get update \
	&& apt-get install -y git wget libicu-dev apt-transport-https \
	&& wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg \
	&& mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ \
	&& wget -q https://packages.microsoft.com/config/debian/8/prod.list \
	&& mv prod.list /etc/apt/sources.list.d/microsoft-prod.list \
	&& chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg \
	&& chown root:root /etc/apt/sources.list.d/microsoft-prod.list \
	&& apt-get update \
	&& apt-get install -y dotnet-sdk-2.1 
	
# get
RUN apt-get update \
	&& cd ~ \
	&& git clone https://git.poka.website/destream-public/destream-blockchain.git
	
# install
RUN cd ~/destream-blockchain/Sources/DeStream.DeStreamD \
	&& dotnet publish -c Release -f netcoreapp2.1 -r debian.9-x64 -o /usr/local/bin/destream 

# clean
RUN apt-get purge -y --auto-remove git wget apt-transport-https dotnet-sdk-2.1 \
	&& rm -r ~/destream-blockchain
