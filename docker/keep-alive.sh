while true
do 
	if [[ $(docker ps | grep $1-fullnode) ]]
       	then 
		echo `date` - Node is alive
	else 
		echo Node is dead, restarting
		docker-compose up -d --no-recreate 
	fi 

	if [[ $(docker ps | grep mongo) ]]
        then
                echo `date` - Mongo is alive
        else
		echo Mongo is dead, restarting
                docker-compose up -d --no-recreate
        fi

	if [[ $(docker ps | grep nako) ]]
        then
                echo `date` - Nako is alive
        else
		echo Nako is dead, restarting
                docker-compose up -d --no-recreate
        fi
	
	echo sleeping 60 seconds.
	sleep 60 
done
