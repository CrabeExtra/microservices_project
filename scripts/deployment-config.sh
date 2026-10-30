#!/bin/bash

DEPLOY_DIR="/home/Jude/Projects/Portfolio"
DEPLOY_USER="Jude"

# Add new services, comment out stale services or remove old services from the deployment pipeline here.
# Services should be .Net web servers with csproj in root/ServiceName.
DOTNET_SERVICES=(
    "Identity"
    "Activity"
    "Audit"
    "Contact"
)