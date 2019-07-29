function delete_secret() {
    echo "--- Deleting service account :wastebasket:"
    rm -rf "${SPATIAL_OAUTH_DIR}"
}

function fetch_service_account() {
    echo "--- Fetching service account :face_with_finger_covering_closed_lips:"

    export SPATIAL_OAUTH_DIR=$(mktemp -d)
    local SPATIAL_OAUTH_FILE="${SPATIAL_OAUTH_DIR}/oauth2_refresh_token"

    # Fetch a SpatialOS service account with [r,w]int/docs perms.
    imp-ci secrets read \
        --environment=production \
        --buildkite-org=improbable \
        --secret-type=spatialos-service-account \
        --secret-name="improbadoc/token-production" \
        --field=token \
        --write-to="${SPATIAL_OAUTH_FILE}"

    trap delete_secret INT TERM EXIT
}

function setup_improbadoc() {
    fetch_service_account

    echo "--- Setting up Improbadoc :page_facing_up:"

    echo "Adding imp-tool subcriptions to PATH."
    export PATH="$HOME/.improbable/imp-tool/subscriptions:$PATH"

    echo "Subscribing to imp-tool and improbadoc."
    imp-tool-bootstrap subscribe \
        --gcs_credentials_type=auto \
        --tools=imp-tool,improbadoc \
        --log_level debug
}

function setup_docs_linter() {
    echo "--- Setting up DocsLinter :docker:"

    local DOCS_BUILD_PATH="${1}"

    docker build \
        --tag local:gdk-docs-linter \
        --file ./ci/docker/docs-linter.Dockerfile \
        --build-arg DOCS_BUILD_PATH="${DOCS_BUILD_PATH}" \
        .
}
