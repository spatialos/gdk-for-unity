function delete_secret() {
    rm -rf "${SPATIAL_OAUTH_DIR}"
}

function fetch_service_account() {
    local ENV="${1}"

    export SPATIAL_OAUTH_DIR=$(mktemp -d)
    local SPATIAL_OAUTH_FILE="${SPATIAL_OAUTH_DIR}/oauth2_refresh_token"

    # Fetch a SpatialOS service account with [r,w]int/docs perms.
    imp-ci secrets read \
        --environment=production \
        --buildkite-org=improbable \
        --secret-type=spatialos-service-account \
        --secret-name="improbadoc/token-${ENV}" \
        --field=token \
        --write-to="${SPATIAL_OAUTH_FILE}"

    trap delete_secret INT TERM EXIT
}

function setup_improbadoc() {
    local ENV="${1}"

    fetch_service_account "${ENV}"

    echo "Adding imp-tool subcriptions to PATH."
    export PATH="$HOME/.improbable/imp-tool/subscriptions:$PATH"

    echo "Subscribing to imp-tool and improbadoc."
    imp-tool-bootstrap subscribe \
        --gcs_credentials_type=auto \
        --tools=imp-tool,improbadoc \
        --log_level debug
}

function setup_docs_linter() {
    docker build \
        --tag local:gdk-docs-linter \
        --file ./ci/docker/docs-linter.Dockerfile \
        .
}
