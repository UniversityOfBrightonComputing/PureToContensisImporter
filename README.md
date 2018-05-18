Pure to Contensis Importer
===

Tested with Pure 5.11 and Contensis 11.0.

Integration between the Reseach Information System (RIS) Pure and the Content Management System (CMS) Contensis.

Imports Person entries in Pure into a ContentType in Contensis called Academic Staff. Deletes all existing entries and re-adds as new from Pure import.

Requirements
---

* .Net Core 2.0 Runtime
* Pure 5.11 installation
* Contensis >11.0 installation

RIS and CMS configuration
---

**Pure**

In the Administrator section create an API Key (in Security settings) and grant it access to the endpoints Person and Research output.

**Contensis**

Setup a ContentType in Contensis called "Academic Staff" with fields:
```
Number 'PureId'
Text 'Title'
Text 'First Name'
Text 'Last Name'
Text 'Email'
```

Create an API key with permissions to save and publish this Content type (see Contensis documentation on Zenhub).

Installation
---

1. Copy AppSecrets.config.example -> [SomeLocation]\AppSecrets.config
2. Edit App.config to refer to the file, e.g. `<appSettings file="C:\[SomeLocation]\AppSecrets.config">`
3. Edit your AppSecrets.config file with
    * CMS url
    * Contensis Client ID and Shared Secret (setup in Content-Types section).
    * Pure API url
    * Pure API key

