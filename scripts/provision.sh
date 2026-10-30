#!/bin/bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/deployment-config.sh"

echo "Installing dependencies..."
pacman -S --needed --noconfirm \
    dotnet-sdk \
    aspnet-runtime \
    nodejs \
    npm \
    curl

echo "Installing NATS Server..."

curl -sf https://binaries.nats.dev/nats-io/nats-server/v2@latest | sh
install -m 755 nats-server /usr/local/bin/nats-server

echo "Creating deployment directory..."
mkdir -p "$DEPLOY_DIR"

chown -R "$DEPLOY_USER:$DEPLOY_USER" "$DEPLOY_DIR"

echo "Creating systemd services..."

create_dotnet_service() {
    local name="$1"
    local directory="$2"
    local dll="$3"

    cat > "/etc/systemd/system/${name}.service" <<EOF
[Unit]
Description=${name} API
After=network.target

[Service]
Type=simple
User=${DEPLOY_USER}
WorkingDirectory=${DEPLOY_DIR}/${directory}
ExecStart=/usr/bin/dotnet ${DEPLOY_DIR}/${directory}/${dll}.dll
Restart=always
RestartSec=5

[Install]
WantedBy=multi-user.target
EOF
}

for name in "${DOTNET_SERVICES[@]}"; do
    create_dotnet_service "$name" "$name" "$name"
done

echo "Creating NATS service..."

cat > /etc/systemd/system/nats.service <<EOF
[Unit]
Description=NATS Server
After=network.target

[Service]
Type=simple
User=${DEPLOY_USER}
ExecStart=/usr/bin/nats-server
Restart=always
RestartSec=5

[Install]
WantedBy=multi-user.target
EOF

SERVICE_NAMES=()

for name in "${DOTNET_SERVICES[@]}"; do
    SERVICE_NAMES+=("${name}.service")
done

systemctl enable --now "${SERVICE_NAMES[@]}" nats.service

echo "Provisioning complete."
