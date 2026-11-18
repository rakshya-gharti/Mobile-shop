#!/bin/bash
# Runs GlowUp on Linux using Mono + SQLite.  Usage: ./run.sh [port]
cd "$(dirname "$0")"
export PATH=/home/linuxbrew/.linuxbrew/bin:$PATH
mkdir -p bin /tmp/GlowUpMail
mcs -nowarn:0618 -out:bin/ShopHost.exe -r:System.Web.dll Host/ShopHost.cs || exit 1
exec mono --debug bin/ShopHost.exe "$PWD" "${1:-8080}"
