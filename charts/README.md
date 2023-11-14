# Generating a new Helm Package, and repo index

_From [this](.) directory:_

```
helm package overdue-book-reporter/
helm repo index .
```

_or just the bash script here:_

```
./regenerate-helm-package-and-repo-index.sh
```