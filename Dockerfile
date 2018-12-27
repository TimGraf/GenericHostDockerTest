FROM microsoft/dotnet:sdk

WORKDIR /vsdbg

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
            unzip \
    && rm -rf /var/lib/apt/lists/* \
    && curl -sSL https://aka.ms/getvsdbgsh \
        | bash /dev/stdin -v latest -l /vsdbg

RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -sL https://deb.nodesource.com/setup_8.x | bash - && apt-get install -yq nodejs build-essential
RUN npm install -g npm
RUN npm install

ENV DOTNET_USE_POLLING_FILE_WATCHER 1

WORKDIR /app

ENTRYPOINT dotnet watch run