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
    curl \
    caddy

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

echo "Configuring Caddy..."

mkdir -p /etc/caddy

{
    cat <<EOF
judebradbury.com {
    root * /home/Jude/Projects/Portfolio/Frontend
    encode zstd gzip

EOF

    for i in "${!DOTNET_SERVICES[@]}"; do
        name="${DOTNET_SERVICES[$i]}"
        port=$((5001 + i))

        cat <<EOF
    handle /api/${name,,}/* {
        reverse_proxy 127.0.0.1:${port}
    }

EOF
    done

    cat <<EOF
    handle {
        try_files {path} /index.html
        file_server
    }
}
EOF
} > /etc/caddy/Caddyfile

caddy validate --config /etc/caddy/Caddyfile

echo "Configuring Caddy filesystem permissions..."

# Allow Caddy to traverse the parent directories, but not list their contents.
chgrp caddy /home/Jude /home/Jude/Projects
chmod 710 /home/Jude /home/Jude/Projects

# Give Caddy read access to the project.
chgrp -R caddy /home/Jude/Projects/Portfolio
chmod -R g+rX /home/Jude/Projects/Portfolio

echo "Creating NATS service..."

cat > /etc/systemd/system/nats.service <<EOF
[Unit]
Description=NATS Server
After=network.target

[Service]
Type=simple
User=${DEPLOY_USER}
ExecStart=/usr/local/bin/nats-server
Restart=always
RestartSec=5

[Install]
WantedBy=multi-user.target
EOF

SERVICE_NAMES=()

for name in "${DOTNET_SERVICES[@]}"; do
    SERVICE_NAMES+=("${name}.service")
done

systemctl enable "${SERVICE_NAMES[@]}"
systemctl enable --now nats.service caddy.service
systemctl enable --now caddy

echo "Provisioning complete."
