#!/bin/bash
set -u

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_DIR="$(dirname "$SCRIPT_DIR")"

source "$SCRIPT_DIR/deployment-config.sh"

cd "$REPO_DIR" || exit 1

FAILED=()

for name in "${DOTNET_SERVICES[@]}"; do

    echo ""
    echo "=================================="
    echo "Deploying $name"
    echo "=================================="

    PROJECT="./$name/$name.csproj"
    OUTPUT="./publish/$name"
    TARGET="$DEPLOY_DIR/$name"
    STAGING="$DEPLOY_DIR/.${name}.staging"
    BACKUP="$DEPLOY_DIR/.${name}.backup"

    # Build and publish independently.
    rm -rf "$OUTPUT"

    if ! dotnet publish "$PROJECT" -c Release -o "$OUTPUT"; then
        echo "BUILD FAILED: $name"
        FAILED+=("$name")
        continue
    fi

    # Stage deployment.
    rm -rf "$STAGING" "$BACKUP"
    mkdir -p "$STAGING"

    cp "/deployments/Portfolio/config/${name}.env" "$STAGING/.env" # configure environment

    if ! cp -a "$OUTPUT/." "$STAGING/"; then
        echo "STAGING FAILED: $name"
        rm -rf "$STAGING"
        FAILED+=("$name")
        continue
    fi

    # Back up existing deployment.
    if [ -d "$TARGET" ]; then
        if ! mv "$TARGET" "$BACKUP"; then
            echo "BACKUP FAILED: $name"
            FAILED+=("$name")
            continue
        fi
    fi

    # Activate new deployment.
    if ! mv "$STAGING" "$TARGET"; then
        echo "DEPLOYMENT FAILED: $name"

        if [ -d "$BACKUP" ]; then
            mv "$BACKUP" "$TARGET"
        fi

        FAILED+=("$name")
        continue
    fi

    # Restart only this service.
    if ! sudo systemctl restart "${name}.service"; then
        echo "RESTART FAILED: $name"

        rm -rf "$TARGET"

        if [ -d "$BACKUP" ]; then
            mv "$BACKUP" "$TARGET"
            sudo systemctl restart "${name}.service"
        fi

        FAILED+=("$name")
        continue
    fi

    rm -rf "$BACKUP"

    echo "SUCCESS: $name"
done

echo ""
echo "=================================="
echo "Deployment summary"
echo "=================================="

if [ ${#FAILED[@]} -gt 0 ]; then
    echo "Failed services: ${FAILED[*]}"
    exit 1
fi

echo "All services deployed successfully."