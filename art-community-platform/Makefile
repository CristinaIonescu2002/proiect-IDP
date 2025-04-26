# Makefile pentru proiectul art-community-platform

STACK_NAME=art-platform
COMPOSE_FILE=docker-compose.swarm.yml

build:
	docker build -t art-community-platform-backend:latest ./backend
	docker build -t art-community-platform-frontend:latest ./frontend

deploy: build
	docker stack deploy -c $(COMPOSE_FILE) $(STACK_NAME)

down:
	docker stack rm $(STACK_NAME)

logs:
	docker service logs $(STACK_NAME)_backend
	docker service logs $(STACK_NAME)_frontend
	docker service logs $(STACK_NAME)_kong
	docker service logs $(STACK_NAME)_portainer
	docker service logs $(STACK_NAME)_postgres-db
	docker service logs $(STACK_NAME)_prometheus
	docker service logs $(STACK_NAME)_grafana

ps:
	docker stack ps $(STACK_NAME)

services:
	docker stack services $(STACK_NAME)

restart:
	make down
	make deploy
