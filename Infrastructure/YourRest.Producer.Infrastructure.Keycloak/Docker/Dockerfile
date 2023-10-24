FROM jboss/keycloak:latest

COPY Infrastructure/YourRest.Producer.Infrastructure.Keycloak/Docker/realm-export.json /opt/jboss/keycloak/

CMD ["-b", "0.0.0.0", "-Dkeycloak.migration.action=import", "-Dkeycloak.migration.provider=singleFile", "-Dkeycloak.migration.file=/opt/jboss/keycloak/realm-export.json", "-Dkeycloak.migration.strategy=OVERWRITE_EXISTING"]
