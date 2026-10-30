#!/bin/bash

set -u

DEPLOY_DIR="/home/Jude/Projects/Portfolio"

# Add or remove services here.
SERVICES=(
    "Identity"
    "Activity"
    "Audit"
    "Contact"
)

FAILED=()

cd "$(dirname "$0")/.." || exit 1

for name in "${SERVICES[@]}"; do
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

    # Stage the deployment without touching the running service.
    rm -rf "$STAGING" "$BACKUP"
    mkdir -p "$STAGING"

    if ! cp -a "$OUTPUT/." "$STAGING/"; then
        echo "STAGING FAILED: $name"
        rm -rf "$STAGING"
        FAILED+=("$name")
        continue
    fi

    # Swap the new deployment into place.
    if [ -d "$TARGET" ]; then
        mv "$TARGET" "$BACKUP" || {
            echo "BACKUP FAILED: $name"
            FAILED+=("$name")
            continue
        }
    fi

    if ! mv "$STAGING" "$TARGET"; then
        echo "DEPLOYMENT FAILED: $name"

        if [ -d "$BACKUP" ]; then
            mv "$BACKUP" "$TARGET"
        fi

        FAILED+=("$name")
        continue
    fi

    # Restart only this service.
    if ! sudo systemctl restart "${name,,}"; then
        echo "RESTART FAILED: $name"

        # Roll back to the previous deployment.
        rm -rf "$TARGET"
        if [ -d "$BACKUP" ]; then
            mv "$BACKUP" "$TARGET"
            sudo systemctl restart "${name,,}"
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