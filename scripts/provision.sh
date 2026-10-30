
#!/bin/bash
set -euo pipefail

DEPLOY_DIR="/home/Jude/Projects/Portfolio"
DEPLOY_USER="Jude"

echo "Installing dependencies..."
pacman -S --needed --noconfirm \
    dotnet-sdk \
    aspnet-runtime \
    nodejs \
    npm 

echo "Installing NATS Server..."

curl -sf https://binaries.nats.dev/nats-io/nats-server/v2@latest | sh
install -m 755 nats-server /usr/local/bin/nats-server

echo "Creating deployment directory..."
mkdir -p "$DEPLOY_DIR"

chown -R "$DEPLOY_USER:$DEPLOY_USER" "$DEPLOY_DIR"

echo "Creating systemd services..."

create_dotnet_service() {
    local name="$1"
    local dll="$2"

    cat > "/etc/systemd/system/${name}.service" <<EOF
[Unit]
Description=${name} API
After=network.target

[Service]
Type=simple
User=${DEPLOY_USER}
WorkingDirectory=${DEPLOY_DIR}/${name}
ExecStart=/usr/bin/dotnet ${DEPLOY_DIR}/${name}/${dll}.dll
Restart=always
RestartSec=5

[Install]
WantedBy=multi-user.target
EOF
}

create_dotnet_service Identity Identity
create_dotnet_service Activity Activity
create_dotnet_service Audit Audit
create_dotnet_service Contact Contact

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

systemctl daemon-reload
systemctl enable --now identity activity audit contact nats

echo "Provisioning complete."
