FROM microsoft/dotnet:2.1-sdk AS installer-env

COPY . /src/dotnet-function-app
RUN cd /src/dotnet-function-app && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot -c debug

#FROM microsoft/azure-functions-dotnet-core2.0
FROM mcr.microsoft.com/azure-functions/dotnet:2.0
ENV AzureWebJobsScriptRoot=/home/site/wwwroot
ENV WEBSITE_HOSTNAME=localhost:80
ENV APPINSIGHTS_INSTRUMENTATIONKEY="35487963-7b7f-432d-8a41-68625247bbcf"
ENV AzureWebJobsDashboard="DefaultEndpointsProtocol=https;AccountName=7celcp4caz2rcazfunctions;AccountKey=r7d+NdwgIdcISxfOn9Nzkg7G4xvHtVEPgI8R+oo3CVO7qkR5iJkIEI4wtxnCsbMpOv7U9YBWKKWMUPmr3+C3fQ=="
ENV AzureWebJobsStorage="DefaultEndpointsProtocol=https;AccountName=7celcp4caz2rcazfunctions;AccountKey=r7d+NdwgIdcISxfOn9Nzkg7G4xvHtVEPgI8R+oo3CVO7qkR5iJkIEI4wtxnCsbMpOv7U9YBWKKWMUPmr3+C3fQ=="
ENV FUNCTIONS_EXTENSION_VERSION="~2"
ENV WEBSITE_CONTENTAZUREFILECONNECTIONSTRING="DefaultEndpointsProtocol=https;AccountName=7celcp4caz2rcazfunctions;AccountKey=r7d+NdwgIdcISxfOn9Nzkg7G4xvHtVEPgI8R+oo3CVO7qk"
ENV WEBSITE_CONTENTSHARE="shoppingcartmicroservice-dev"
ENV WEBSITE_NODE_DEFAULT_VERSION="6.5.0"
ENV FUNCTIONS_WORKER_RUNTIME=dotnet
COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]