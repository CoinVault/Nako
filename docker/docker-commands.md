## useful docker command

list containers by name
```
docker stats $(docker ps | awk '{if(NR>1) print $NF}')
```

stop / remove all containers
```
docker stop $(docker ps -a -q)
docker rm $(docker ps -a -q)
```

remove all images
```
docker rmi $(docker images -q)
```

delete all volumes (active volumes will be ignored)
```
docker volume rm $(docker volume ls -q)
```
