#!/bin/bash
set -u

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_DIR="$(dirname "$SCRIPT_DIR")"

source "$SCRIPT_DIR/deployment-config.sh"

cd "$REPO_DIR" || exit 1

MARKER="/deployments/Portfolio/.provisioned"
BASE="${GITHUB_EVENT_BEFORE:-${1:-}}"
HEAD="${GITHUB_SHA:-HEAD}"

FULL=false
FRONTEND=false
NATS=false
CHANGED_SERVICES=()

# Fresh server: force complete deployment.
if [[ ! -f "$MARKER" ]]; then
    FULL=true

# Missing Git history: force complete deployment.
elif [[ -z "$BASE" ]] || [[ "$BASE" =~ ^0+$ ]] || ! git cat-file -e "$BASE^{commit}" 2>/dev/null; then
    FULL=true

else
    while IFS= read -r FILE; do
        case "$FILE" in
            scripts/*|Project/scripts/*|deployment-config.sh|.github/workflows/runner.yaml)
                FULL=true
                ;;

            Frontend/*|Project/Frontend/*)
                FRONTEND=true
                ;;

            nats/*|nats-server.conf|bin/nats-server)
                NATS=true
                ;;

            # Caddy configuration changes.
            Caddyfile|caddy/*)
                CADDY=true
                ;;
        esac

        # Dynamically detect .NET service changes.
        for name in "${DOTNET_SERVICES[@]}"; do
            case "$FILE" in
                "$name"/*|Project/"$name"/*)
                    # Avoid adding the same service twice.
                    if [[ ! " ${CHANGED_SERVICES[*]} " =~ " $name " ]]; then
                        CHANGED_SERVICES+=("$name")
                    fi
                    ;;
            esac
        done

    done < <(git diff --name-only "$BASE" "$HEAD")
fi

# Full deployment means every configured service.
if [[ "$FULL" == true ]]; then
    CHANGED_SERVICES=("${DOTNET_SERVICES[@]}")
fi

# Export outputs to GitHub Actions.
echo "all=$FULL" >> "$GITHUB_OUTPUT"
echo "frontend=$FRONTEND" >> "$GITHUB_OUTPUT"
echo "nats=$NATS" >> "$GITHUB_OUTPUT"
echo "caddy=${CADDY:-false}" >> "$GITHUB_OUTPUT"
echo "services=${CHANGED_SERVICES[*]}" >> "$GITHUB_OUTPUT"

echo "Full deployment: $FULL"
echo "Frontend: $FRONTEND"
echo "Caddy: ${CADDY:-false}"
echo "NATS: $NATS"
echo "Services: ${CHANGED_SERVICES[*]}"