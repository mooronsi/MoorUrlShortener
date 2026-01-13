.PHONY: help build up down restart logs clean test migrate

DOCKER_COMPOSE := docker-compose -f .docker/docker-compose.yml

help: ## Show this help
	@echo "Available commands:"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-15s\033[0m %s\n", $$1, $$2}'

build: ## Build Docker images
	$(DOCKER_COMPOSE) build

up: ## Start the application
	$(DOCKER_COMPOSE) up

upd: ## Start the application in the background
	$(DOCKER_COMPOSE) up -d

down: ## Stop the application
	$(DOCKER_COMPOSE) down

.DEFAULT_GOAL := help
