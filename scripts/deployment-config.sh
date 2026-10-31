#!/bin/bash

DEPLOY_DIR="/deployments/Portfolio"
DEPLOY_USER="Jude"

# Add new services, comment out stale services or remove old services from the deployment pipeline here.
# Services should be .Net web servers with csproj in root/ServiceName.
DOTNET_SERVICES=(
    "Identity"
    "Audit"
    "Activity"
    "Contact"
)

# Arch Linux repository snapshot (when I last updated my server, don't judge if I don't update for a few months at a time and also please don't hack me).
ARCH_REPO_DATE="2026/10/31"

# Current package versions. Adding these here so I can just change the version number here, push to prod and it will automatically update on the server.
# NOTE FOR LATER: TO UPGRADE, JUST CHANGE THE VERSION HERE
DOTNET_SDK_VERSION="10.0.13.sdk113-1"
ASPNET_RUNTIME_VERSION="10.0.13.sdk113-1"
NODEJS_VERSION="26.11.1-1"
NPM_VERSION="12.2.1-1"
CADDY_VERSION="2.11.7-1"

NATS_SERVER_VERSION="2.15.2"