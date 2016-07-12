# prepare
apt-get update
apt-get -y install apt-transport-https ca-certificates
apt-key adv --keyserver hkp://p80.pool.sks-keyservers.net:80 --recv-keys 58118E89F3A912897C070ADBF76221572C52609D
echo "deb https://apt.dockerproject.org/repo ubuntu-trusty main" >> /etc/apt/sources.list.d/docker.list

# linux-image-extra
apt-get update
apt-get -y install linux-image-extra-$(uname -r)

# install
apt-get -y install docker-engine
#service docker start

# create group
#groupadd docker
usermod -aG docker $(whoami)

# docker compose (we use 1.8.0-rc1)
apt-get -y install curl
curl -L https://github.com/docker/compose/releases/download/1.8.0-rc1/docker-compose-`uname -s`-`uname -m` > /usr/local/bin/docker-compose
chmod +x /usr/local/bin/docker-compose

