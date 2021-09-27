# Acesso Fund-Transfer Test

Fund-Transfer test using microservices, MongoDb, RabbitMQ, ElasticSearch

==== ACESSO TEST =====

docker run -d --name fund-transfer-byAcesso -p 5000:80 baldini/testacesso

==== REDIS =====

docker pull redis

docker run -d -p 6379:6379 --name fund-transfer-redis redis

==== RABBIT MQ =====

docker pull rabbitmq

docker run -d --hostname my-rabbit --name fund-transfer-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management

==== MONGO DB =====

docker pull mongo

docker run -d -p 27017:27017 --name fund-transfer-mongo mongo

==== ELASTIC SEARCH =====

docker pull docker.elastic.co/elasticsearch/elasticsearch:7.5.2

docker run --name fund-transfer-elasticsearch -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" docker.elastic.co/elasticsearch/elasticsearch:7.5.2 
