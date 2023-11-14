#!/bin/bash

helm package overdue-book-reporter/
helm repo index .

echo
echo ===========================================================
echo "Don't forget to commit these changes, and push to upstream!"
echo ===========================================================