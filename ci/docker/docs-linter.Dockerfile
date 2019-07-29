FROM microsoft/dotnet:2.2-sdk as build
ARG DOCS_PATH

# Copy everything and build
WORKDIR /app
COPY .shared-ci/tools ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:2.2
WORKDIR /app
COPY --from=build /app/*/out ./

# Copy docs folder
COPY "$DOCS_PATH/" /app/docs/

# Volume to output linter warnings/errors to
VOLUME /var/logs

ENTRYPOINT ["dotnet", "DocsLinter.dll"]
