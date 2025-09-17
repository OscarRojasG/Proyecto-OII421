#!/bin/bash

cp asd/Build/* ../biolab-escape-gh-pages/Build/

cd ../biolab-escape-gh-pages/Build

for f in *.br; do
    base="${f%.br}"      # remove .br extension
    rm "$base"              # remove original .br file
    brotli -d "$f" # decompress (unbrotli)
done

git add .
git commit -m "update $(date)"
git push
