docker build -t mariadb-demo:2.0 .
docker tag mariadb-demo:2.0 cjiangyong/mariadb-demo:2.0
docker rmi -f $(docker images --filter dangling=true -q)