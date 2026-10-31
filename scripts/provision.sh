#!/bin/bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/deployment-config.sh"

echo "Installing dependencies..."

pacman -S --needed --noconfirm curl # hopefully curl doesn't get breaking changes..

install_arch_package() {
    local name="$1"
    local version="$2"
    local architecture="${3:-x86_64}"
    local category="${name:0:1}"
    local filename="${name}-${version}-${architecture}.pkg.tar.zst"
    local url="https://archive.archlinux.org/packages/${category}/${name}/${filename}"

    local installed_version
    installed_version="$(pacman -Q "$name" 2>/dev/null | awk '{print $2}' || true)"

    if [[ "$installed_version" == "$version" ]]; then
        echo "$name $version already installed. Skipping."
        return 0
    fi

    echo "Installing $name $version (currently: ${installed_version:-not installed})..."

    curl -fsSL "$url" -o "/tmp/$filename"

    # Remove an existing version to avoid package conflicts.
    if [[ -n "$installed_version" ]]; then
        pacman -Rdd --noconfirm "$name"
    fi

    pacman -U --needed --noconfirm "/tmp/$filename"

    rm -f "/tmp/$filename"
}

for package in "${ARCH_PACKAGES[@]}"; do
    IFS='|' read -r name version architecture <<< "$package"
    install_arch_package "$name" "$version" "$architecture"
done

echo "Installing NATS Server ${NATS_SERVER_VERSION}..."

curl -fsSL \
    "https://binaries.nats.dev/nats-io/nats-server/v2@v${NATS_SERVER_VERSION}" \
    | sh

install -m 755 nats-server /usr/local/bin/nats-server

echo "Creating deployment directory..."
mkdir -p "$DEPLOY_DIR"

chown -R "$DEPLOY_USER:$DEPLOY_USER" "$DEPLOY_DIR"

mkdir -p "$DEPLOY_DIR/Frontend"

sudo chgrp -R caddy "$DEPLOY_DIR/Frontend"
sudo chmod -R g+rX "$DEPLOY_DIR/Frontend"

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
    redir https://www.judebradbury.com{uri} permanent
}

www.judebradbury.com {
    root * /deployments/Portfolio/Frontend
    encode zstd gzip

EOF

    for i in "${!DOTNET_SERVICES[@]}"; do
        name="${DOTNET_SERVICES[$i]}"
        port=$((5000 + i))

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

chgrp caddy /deployments
chmod 711 /deployments

chgrp caddy "$DEPLOY_DIR"
chmod 710 "$DEPLOY_DIR"

chgrp -R caddy "$DEPLOY_DIR/Frontend"
chmod -R g+rX "$DEPLOY_DIR/Frontend"

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

echo "Provisioning complete."
